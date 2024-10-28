using eShop.WebAppComponents.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.SemanticKernel;
using eShop.WebApp.Services.OrderStatus.IntegrationEvents;
using eShop.WebApp.Services.OrderStatus.IntegrationEvents.Events;
using eShop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;
using Refit;
using System.Security.Claims;
using eShop.Customer.Contracts;
using eShop.Ordering.Contracts;
using eShop.EventBus.Extensions;
using eShop.EventBusRabbitMQ;
using eShop.Shared.Features;
using eShop.Catalog.Contracts;
using eShop.ServiceInvocation.CatalogService;
using eShop.ServiceInvocation.CatalogService.Refit;
using eShop.ServiceInvocation.OrderingService;
using eShop.ServiceInvocation.OrderingService.Refit;
using eShop.ServiceInvocation.CustomerService;
using eShop.ServiceInvocation.CustomerService.Refit;

namespace eShop.WebApp.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();

        builder.AddRabbitMqEventBus("EventBus")
               .AddEventBusSubscriptions();

        builder.Services.AddHttpForwarderWithServiceDiscovery();

        // Application services
        builder.Services.AddScoped<BasketState>();
        builder.Services.AddScoped<LogOutService>();
        builder.Services.AddSingleton<BasketService>();
        builder.Services.AddSingleton<OrderStatusNotificationService>();
        builder.Services.AddSingleton<IProductImageUrlProvider, ProductImageUrlProvider>();
        builder.AddAIServices();

        // HTTP and GRPC client registrations
        builder.Services.AddGrpcClient<Basket.API.Grpc.Basket.BasketClient>(o => o.Address = new("http://basket-api"))
            .AddAuthToken();

        FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();

        if (features?.ServiceInvocation.ServiceInvocationType == ServiceInvocationType.Refit)
        {
            builder.Services.AddRefitServices();
        }
    }

    private static void AddRefitServices(this IServiceCollection services)
    {
        services
                .AddRefitClient<ICatalogApi>()
                .ConfigureHttpClient(c =>
                    c.BaseAddress = new Uri("http://catalog-api"))
                .AddAuthToken();

        services
            .AddRefitClient<ICustomerApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://customer-api"))
            .AddAuthToken();

        services
            .AddRefitClient<IOrderingApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://ordering-api"))
            .AddAuthToken();

        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderingService, OrderingService>();
    }

    public static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        eventBus.AddSubscription<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToStockConfirmedIntegrationEvent, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToShippedIntegrationEvent, OrderStatusChangedToShippedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToCancelledIntegrationEvent, OrderStatusChangedToCancelledIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToSubmittedIntegrationEvent, OrderStatusChangedToSubmittedIntegrationEventHandler>();
    }

    public static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;

        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        var identityUrl = configuration.GetRequiredValue("IdentityUrl");
        var callBackUrl = configuration.GetRequiredValue("CallBackUrl");
        var sessionCookieLifetime = configuration.GetValue("SessionCookieLifetimeMinutes", 60);

        // Add Authentication services
        services.AddAuthorization();
        services.AddAuthentication(options =>
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
        services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        services.AddCascadingAuthenticationState();
    }

    private static void AddAIServices(this IHostApplicationBuilder builder)
    {
        var openAIOptions = builder.Configuration.GetSection("AI").Get<AIOptions>()?.OpenAI;
        var deploymentName = openAIOptions?.ChatModel;

        if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("openai")) && !string.IsNullOrWhiteSpace(deploymentName))
        {
            builder.Services.AddKernel();
            builder.AddAzureOpenAIClient("openai");
            builder.Services.AddAzureOpenAIChatCompletion(deploymentName);
        }
    }

    public static async Task<string?> GetBuyerIdAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst("sub")?.Value;
    }

    public static async Task<string?> GetUserNameAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }
}
