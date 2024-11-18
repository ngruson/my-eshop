namespace eShop.Shared.Features;
public class FeaturesConfiguration
{
    public PublishSubscribeConfiguration PublishSubscribe { get; set; } = new PublishSubscribeConfiguration();
    public ServiceInvocationConfiguration ServiceInvocation { get; set; } = new ServiceInvocationConfiguration();
}
public class PublishSubscribeConfiguration
{
    public EventBusType EventBus { get; set; } = EventBusType.RabbitMq;
}
public class ServiceInvocationConfiguration
{
    public ServiceInvocationType ServiceInvocationType { get; set; }
}
public enum EventBusType
{
    RabbitMq,
    Dapr
}
public enum ServiceInvocationType
{
    Refit,
    Dapr
}
