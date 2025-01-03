namespace eShop.EventBusRabbitMQ;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Ardalis.Result;
using CloudNative.CloudEvents;
using eShop.EventBus.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly.Retry;

public sealed class RabbitMQEventBus(
    ILogger<RabbitMQEventBus> logger,
    IServiceProvider serviceProvider,
    IOptions<EventBusOptions> options,
    IOptions<EventBusSubscriptionInfo> subscriptionOptions,
    RabbitMQTelemetry rabbitMQTelemetry) : IEventBus, IDisposable, IHostedService
{
    private const string ExchangeName = "eShop_event_bus";

    private readonly ResiliencePipeline _pipeline = CreateResiliencePipeline(options.Value.RetryCount);
    private readonly TextMapPropagator _propagator = rabbitMQTelemetry.Propagator;
    private readonly ActivitySource _activitySource = rabbitMQTelemetry.ActivitySource;
    private readonly string _queueName = options.Value.SubscriptionClientName;
    private readonly EventBusSubscriptionInfo _subscriptionInfo = subscriptionOptions.Value;
    private IConnection? _rabbitMQConnection;

    private IModel? _consumerChannel;

    public Task<Result> PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken)
    {
        string routingKey = @event.GetType().Name;

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, routingKey);
        }

        using IModel channel = this._rabbitMQConnection?.CreateModel() ?? throw new InvalidOperationException("RabbitMQ connection is not open");

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);
        }

        channel.ExchangeDeclare(exchange: ExchangeName, type: "direct");

        byte[] body = this.SerializeMessage(@event);

        // Start an activity with a name following the semantic convention of the OpenTelemetry messaging specification.
        // https://github.com/open-telemetry/semantic-conventions/blob/main/docs/messaging/messaging-spans.md
        string activityName = $"{routingKey} publish";

        return this._pipeline.Execute(() =>
        {
            using Activity? activity = this._activitySource.StartActivity(activityName, ActivityKind.Client);

            // Depending on Sampling (and whether a listener is registered or not), the activity above may not be created.
            // If it is created, then propagate its context. If it is not created, the propagate the Current context, if any.

            ActivityContext contextToInject = default;

            if (activity != null)
            {
                contextToInject = activity.Context;
            }
            else if (Activity.Current != null)
            {
                contextToInject = Activity.Current.Context;
            }

            IBasicProperties properties = channel.CreateBasicProperties();
            // persistent
            properties.DeliveryMode = 2;

            static void InjectTraceContextIntoBasicProperties(IBasicProperties props, string key, string value)
            {
                props.Headers ??= new Dictionary<string, object>();
                props.Headers[key] = value;
            }

            this._propagator.Inject(new PropagationContext(contextToInject, Baggage.Current), properties, InjectTraceContextIntoBasicProperties);

            SetActivityContext(activity!, routingKey, "publish");

            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);
            }

            try
            {
                channel.BasicPublish(
                    exchange: ExchangeName,
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);

                return Task.FromResult(Result.Success());
            }
            catch (Exception ex)
            {
                activity!.SetExceptionTags(ex);

                throw;
            }
        });
    }

    private static void SetActivityContext(Activity activity, string routingKey, string operation)
    {
        if (activity is not null)
        {
            // These tags are added demonstrating the semantic conventions of the OpenTelemetry messaging specification
            // https://github.com/open-telemetry/semantic-conventions/blob/main/docs/messaging/messaging-spans.md
            activity.SetTag("messaging.system", "rabbit-mq");
            activity.SetTag("messaging.destination_kind", "queue");
            activity.SetTag("messaging.operation", operation);
            activity.SetTag("messaging.destination.name", routingKey);
            activity.SetTag("messaging.rabbit-mq.routing_key", routingKey);
        }
    }

    public void Dispose()
    {
        this._consumerChannel?.Dispose();
    }

    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        static IEnumerable<string> ExtractTraceContextFromBasicProperties(IBasicProperties props, string key)
        {
            if (props.Headers is not null)
            {
                if (props.Headers.TryGetValue(key, out object? value))
                {
                    byte[] bytes = (byte[])value;
                    return [Encoding.UTF8.GetString(bytes)];
                }
            }
            
            return [];
        }

        // Extract the PropagationContext of the upstream parent from the message headers.
        PropagationContext parentContext = this._propagator.Extract(default, eventArgs.BasicProperties, ExtractTraceContextFromBasicProperties);
        Baggage.Current = parentContext.Baggage;

        // Start an activity with a name following the semantic convention of the OpenTelemetry messaging specification.
        // https://github.com/open-telemetry/semantic-conventions/blob/main/docs/messaging/messaging-spans.md
        string activityName = $"{eventArgs.RoutingKey} receive";

        using Activity? activity = this._activitySource.StartActivity(activityName, ActivityKind.Client, parentContext.ActivityContext);

        SetActivityContext(activity!, eventArgs.RoutingKey, "receive");

        string eventName = eventArgs.RoutingKey;
        string message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            activity?.SetTag("message", message);

            if (message.Contains("throw-fake-exception", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
            }

            await this.ProcessEvent(eventName, message);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error Processing message \"{Message}\"", message);

            activity!.SetExceptionTags(ex);
        }

        // Even on exception we take the message off the queue.
        // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
        // For more information see: https://www.rabbitmq.com/dlx.html
        this._consumerChannel!.BasicAck(eventArgs.DeliveryTag, multiple: false);
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);
        }

        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

        if (!this._subscriptionInfo.EventTypes.TryGetValue(eventName, out Type? eventType))
        {
            logger.LogWarning("Unable to resolve event type for event name {EventName}", eventName);
            return;
        }

        // Deserialize the event
        IntegrationEvent? integrationEvent = this.DeserializeMessage(message, eventType);
        
        // REVIEW: This could be done in parallel

        // Get all the handlers using the event type as the key
        foreach (IIntegrationEventHandler handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
        {
            await handler.Handle(integrationEvent!, default);
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026:RequiresUnreferencedCode",
        Justification = "The 'JsonSerializer.IsReflectionEnabledByDefault' feature switch, which is set to false by default for trimmed .NET apps, ensures the JsonSerializer doesn't use Reflection.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode", Justification = "See above.")]
    private IntegrationEvent? DeserializeMessage(string message, Type eventType)
    {
        if (IsCloudEvent(message, out CloudEvent? cloudEvent))
        {
            return JsonSerializer.Deserialize(cloudEvent!.Data!.ToString()!, eventType, this._subscriptionInfo.JsonSerializerOptions) as IntegrationEvent;
        }

        return JsonSerializer.Deserialize(message, eventType, this._subscriptionInfo.JsonSerializerOptions) as IntegrationEvent;
    }

    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
    [RequiresUnreferencedCode("The 'JsonSerializer.IsReflectionEnabledByDefault' feature switch, which is set to false by default for trimmed .NET apps, ensures the JsonSerializer doesn't use Reflection.")]
    public static bool IsCloudEvent(string input, out CloudEvent? cloudEvent)
    {
        if (string.IsNullOrEmpty(input))
        {
            cloudEvent = null;
            return false;
        }

        try
        {
            JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
            JsonSerializerOptions options = jsonSerializerOptions;
            cloudEvent = JsonSerializer.Deserialize<CloudEvent>(input, options);
            return cloudEvent != null;
        }
        catch
        {
            cloudEvent = null;
            return false;
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026:RequiresUnreferencedCode",
        Justification = "The 'JsonSerializer.IsReflectionEnabledByDefault' feature switch, which is set to false by default for trimmed .NET apps, ensures the JsonSerializer doesn't use Reflection.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode", Justification = "See above.")]
    private byte[] SerializeMessage(IntegrationEvent @event)
    {
        return JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), this._subscriptionInfo.JsonSerializerOptions);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Messaging is async so we don't need to wait for it to complete. On top of this
        // the APIs are blocking, so we need to run this on a background thread.
        Task.Factory.StartNew(() =>
        {
            try
            {
                logger.LogInformation("Starting RabbitMQ connection on a background thread");

                this._rabbitMQConnection = serviceProvider.GetRequiredService<IConnection>();
                if (!this._rabbitMQConnection.IsOpen)
                {
                    return;
                }

                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace("Creating RabbitMQ consumer channel");
                }

                this._consumerChannel = this._rabbitMQConnection.CreateModel();

                this._consumerChannel.CallbackException += (sender, ea) =>
                {
                    logger.LogWarning(ea.Exception, "Error with RabbitMQ consumer channel");
                };

                this._consumerChannel.ExchangeDeclare(exchange: ExchangeName,
                                        type: "direct");

                this._consumerChannel.QueueDeclare(queue: this._queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace("Starting RabbitMQ basic consume");
                }

                AsyncEventingBasicConsumer consumer = new(this._consumerChannel);

                consumer.Received += this.OnMessageReceived;

                this._consumerChannel.BasicConsume(
                    queue: this._queueName,
                    autoAck: false,
                    consumer: consumer);

                foreach ((string eventName, Type _) in this._subscriptionInfo.EventTypes)
                {
                    this._consumerChannel.QueueBind(
                        queue: this._queueName,
                        exchange: ExchangeName,
                        routingKey: eventName);
                }

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error starting RabbitMQ connection");
            }
        },
        TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        // See https://www.pollydocs.org/strategies/retry.html
        RetryStrategyOptions retryOptions = new()
        {
            ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
            MaxRetryAttempts = retryCount,
            DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();

        static TimeSpan? GenerateDelay(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }
}
