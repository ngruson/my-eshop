using System.Security.Claims;
using eShop.Catalog.Contracts;
using eShop.Customer.Contracts;
using eShop.EventBus.Dapr;
using eShop.EventBus.Extensions;
using eShop.EventBusRabbitMQ;
using eShop.Ordering.Contracts;
using eShop.ServiceInvocation;
using eShop.ServiceInvocation.BasketApiClient;
using eShop.ServiceInvocation.CatalogApiClient;
using eShop.ServiceInvocation.CustomerApiClient;
using eShop.ServiceInvocation.OrderingApiClient;
using eShop.Shared.DI;
using eShop.Shared.Features;
using eShop.WebApp.Services.OrderStatus;
using eShop.WebApp.Services.OrderStatus.IntegrationEvents;
using eShop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;
using eShop.WebApp.Services.OrderStatus.IntegrationEvents.Events;
using eShop.WebAppComponents.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.SemanticKernel;
using Refit;
using static eShop.Basket.Contracts.Grpc.Basket;

namespace eShop.WebApp.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();

        builder.Services.Configure<FeaturesConfiguration>(builder.Configuration.GetSection("Features"));

        FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();
        if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
        {
            builder.AddDaprEventBus()
                .AddEventBusSubscriptions()
                .ConfigureJsonOptions(options => options.PropertyNameCaseInsensitive = true);
        }
        else
        {
            builder.AddRabbitMqEventBus("eventBus")
                .AddEventBusSubscriptions()
                .ConfigureJsonOptions(options => options.PropertyNameCaseInsensitive = true);
        }

        builder.AddServiceInvocation();
        if (features?.ServiceInvocation.ServiceInvocationType == ServiceInvocationType.Dapr)
        {
            builder.AddDaprServices();
        }
        else
        {
            builder.AddRefitServices();
        }

        builder.Services.AddSingleton<ServiceProviderWrapper>();
        builder.Services.AddHttpForwarderWithServiceDiscovery();

        // Application services
        builder.Services.AddScoped<BasketState>();
        builder.Services.AddScoped<LogOutService>();
        builder.Services.AddSingleton<OrderStatusNotificationService>();
        builder.Services.AddSingleton<IProductImageUrlProvider, ProductImageUrlProvider>();
        builder.AddAIServices();
    }

    private static void AddDaprServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddGrpc();
        builder.Services.AddDaprClient();

        string? grpcEndpoint = builder.Configuration["DAPR_GRPC_ENDPOINT"];
        builder.Services.AddGrpcClient<BasketClient>(o => o.Address = new(grpcEndpoint!))
            .AddAuthToken();

        builder.Services.AddScoped<IBasketApiClient, ServiceInvocation.BasketApiClient.Dapr.BasketApiClient>();
        builder.Services.AddScoped<ICatalogApiClient, ServiceInvocation.CatalogApiClient.Dapr.CatalogApiClient>();
        builder.Services.AddScoped<ICustomerApiClient, ServiceInvocation.CustomerApiClient.Dapr.CustomerApiClient>();
        builder.Services.AddScoped<IOrderingApiClient, ServiceInvocation.OrderingApiClient.Dapr.OrderingApiClient>();
    }

    private static void AddRefitServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddGrpcClient<BasketClient>(o => o.Address = new("http://basket-api"))
            .AddAuthToken();

        builder.Services
                .AddRefitClient<ICatalogApi>()
                .ConfigureHttpClient(c =>
                    c.BaseAddress = new Uri("http://catalog-api"))
                .AddAuthToken();

        builder.Services
            .AddRefitClient<ICustomerApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://customer-api"))
            .AddAuthToken();

        builder.Services
            .AddRefitClient<IOrderingApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://ordering-api"))
            .AddAuthToken();

        builder.Services.AddScoped<IBasketApiClient, ServiceInvocation.BasketApiClient.Refit.BasketApiClient>();
        builder.Services.AddScoped<ICatalogApiClient, ServiceInvocation.CatalogApiClient.Refit.CatalogApiClient>();
        builder.Services.AddScoped<ICustomerApiClient, ServiceInvocation.CustomerApiClient.Refit.CustomerApiClient>();
        builder.Services.AddScoped<IOrderingApiClient, ServiceInvocation.OrderingApiClient.Refit.OrderingApiClient>();
    }

    public static IEventBusBuilder AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        eventBus.AddSubscription<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToStockConfirmedIntegrationEvent, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToShippedIntegrationEvent, OrderStatusChangedToShippedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToCancelledIntegrationEvent, OrderStatusChangedToCancelledIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToSubmittedIntegrationEvent, OrderStatusChangedToSubmittedIntegrationEventHandler>();

        return eventBus;
    }

    public static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        string identityUrl = builder.Configuration.GetRequiredValue("IdentityUrl");
        string callBackUrl = builder.Configuration.GetRequiredValue("CallBackUrl");
        int sessionCookieLifetime = builder.Configuration.GetValue("SessionCookieLifetimeMinutes", 60);

        // Add Authentication services
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(options => options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
        .AddOpenIdConnect(options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = identityUrl;
            options.SignedOutRedirectUri = callBackUrl;
            options.ClientId = "webapp";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.RequireHttpsMetadata = false;
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("orders");
            options.Scope.Add("basket");
        });

        // Blazor auth services
        builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        builder.Services.AddCascadingAuthenticationState();
    }

    private static void AddAIServices(this IHostApplicationBuilder builder)
    {
        OpenAIOptions? openAIOptions = builder.Configuration.GetSection("AI").Get<AIOptions>()?.OpenAI;
        string? deploymentName = openAIOptions?.ChatModel;

        if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("openai")) && !string.IsNullOrWhiteSpace(deploymentName))
        {
            builder.Services.AddKernel();
            builder.AddAzureOpenAIClient("openai");
            builder.Services.AddAzureOpenAIChatCompletion(deploymentName);
        }
    }

    public static async Task<Guid?> GetBuyerIdAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        AuthenticationState authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        ClaimsPrincipal user = authState.User;
        string? subValue = user.FindFirst("sub")?.Value;

        if (Guid.TryParse(subValue, out Guid buyerId))
        {
            return buyerId;
        }

        return null;
    }

    public static async Task<string?> GetUserNameAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        AuthenticationState authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        ClaimsPrincipal user = authState.User;
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static async Task<string?> GetBuyerNameAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        AuthenticationState authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        ClaimsPrincipal user = authState.User;
        string? firstName = user.FindFirst(ClaimTypes.GivenName)?.Value;
        string? lastName = user.FindFirst(ClaimTypes.Surname)?.Value;

        return $"{firstName} {lastName}";
    }
}
