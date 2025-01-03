using Dapr.Workflow;
using eShop.Catalog.Contracts;
using eShop.Ordering.Contracts;
using eShop.ServiceDefaults;
using eShop.ServiceInvocation;
using eShop.ServiceInvocation.BasketApiClient;
using eShop.ServiceInvocation.CatalogApiClient;
using eShop.ServiceInvocation.OrderingApiClient;
using eShop.ServiceInvocation.PaymentProcessorApiClient;
using eShop.ServiceInvocation.WebAppApiClient;
using eShop.Shared.DI;
using eShop.Shared.Features;
using eShop.Workflow.API.Activities;
using eShop.Workflow.API.Workflows;
using Refit;
using static eShop.Basket.Contracts.Grpc.Basket;

namespace eShop.Workflow.API;

internal static class AppExtensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();
        builder.AddClientCredentials(builder.Configuration, "orders");

        builder.Services.AddDaprWorkflow(options =>
        {
            options.RegisterWorkflow<OrderProcessingWorkflow>();
            options.RegisterActivity<CreateOrderActivity>();
            options.RegisterActivity<ConfirmGracePeriodActivity>();
            options.RegisterActivity<AssessStockItemsActivity>();
            options.RegisterActivity<ConfirmStockActivity>();
            options.RegisterActivity<PaymentActivity>();
        });

        FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();        

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
    }

    private static void AddDaprServices(this IHostApplicationBuilder builder)
    {
        string? gRpcEndpoint = builder.Configuration["DAPR_GRPC_ENDPOINT"];
        builder.Services.AddGrpcClient<BasketClient>(o => o.Address = new(gRpcEndpoint!))
            .AddAuthToken();

        builder.Services.AddScoped<IBasketApiClient, ServiceInvocation.BasketApiClient.Dapr.BasketApiClient>();
        builder.Services.AddScoped<ICatalogApiClient, ServiceInvocation.CatalogApiClient.Dapr.CatalogApiClient>();
        builder.Services.AddScoped<IOrderingApiClient, ServiceInvocation.OrderingApiClient.Dapr.OrderingApiClient>();
        builder.Services.AddScoped<IPaymentProcessorApiClient, ServiceInvocation.PaymentProcessorApiClient.Dapr.PaymentProcessorApiClient>();
        builder.Services.AddScoped<IWebAppApiClient, ServiceInvocation.WebAppApiClient.Dapr.WebAppApiClient>();
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
            .AddRefitClient<IOrderingApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://ordering-api"))
            .AddAuthToken();

        builder.Services
            .AddRefitClient<IWebAppApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://webapp"))
            .AddAuthToken();

        builder.Services.AddScoped<IBasketApiClient, ServiceInvocation.BasketApiClient.Refit.BasketApiClient>();
        builder.Services.AddScoped<ICatalogApiClient, ServiceInvocation.CatalogApiClient.Refit.CatalogApiClient>();
        builder.Services.AddScoped<IOrderingApiClient, ServiceInvocation.OrderingApiClient.Refit.OrderingApiClient>();
        builder.Services.AddScoped<IPaymentProcessorApiClient, ServiceInvocation.PaymentProcessorApiClient.Refit.PaymentProcessorApiClient>();
        builder.Services.AddScoped<IWebAppApiClient, ServiceInvocation.WebAppApiClient.Refit.WebAppApiClient>();
    }
}
