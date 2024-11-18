using eShop.Catalog.Contracts;
using eShop.Customer.Contracts;
using eShop.Identity.Contracts;
using eShop.MasterData.Contracts;
using eShop.Ordering.Contracts;
using eShop.ServiceDefaults;
using eShop.ServiceInvocation.CatalogApiClient;
using eShop.ServiceInvocation.CustomerApiClient;
using eShop.ServiceInvocation.IdentityApiClient;
using eShop.ServiceInvocation.MasterDataApiClient;
using eShop.ServiceInvocation.OrderingApiClient;
using eShop.Shared.Auth;
using eShop.Shared.Features;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.JsonWebTokens;
using Refit;

namespace eShop.AdminApp.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();

        builder.Services.AddRazorPages()
            .AddMicrosoftIdentityUI();

        FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();

        if (features?.ServiceInvocation.ServiceInvocationType == ServiceInvocationType.Dapr)
        {
            builder.AddDaprServices();
        }
        else
        {
            builder.AddRefitServices();
        }

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();
        });
    }

    private static void AddDaprServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDaprClient();        
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<AccessTokenAccessor>();

        builder.Services.AddScoped<ICatalogApiClient, ServiceInvocation.CatalogApiClient.Dapr.CatalogApiClient>();
        builder.Services.AddScoped<ICustomerApiClient, ServiceInvocation.CustomerApiClient.Dapr.CustomerApiClient>();
        builder.Services.AddScoped<IIdentityApiClient, ServiceInvocation.IdentityApiClient.Dapr.IdentityApiClient>();
        builder.Services.AddScoped<IMasterDataApiClient, ServiceInvocation.MasterDataApiClient.Dapr.MasterDataApiClient>();
        builder.Services.AddScoped<IOrderingApiClient, ServiceInvocation.OrderingApiClient.Dapr.OrderingApiClient>();
    }

    private static void AddRefitServices(this IHostApplicationBuilder builder)
    {
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
            .AddRefitClient<IIdentityApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri($"{builder.Configuration["IdentityUrl"]}"))
            .AddAuthToken();

        builder.Services
            .AddRefitClient<IMasterDataApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://masterdata-api"))
            .AddAuthToken();

        builder.Services
            .AddRefitClient<IOrderingApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://ordering-api"))
            .AddAuthToken();

        builder.Services.AddScoped<ICatalogApiClient, ServiceInvocation.CatalogApiClient.Refit.CatalogApiClient>();
        builder.Services.AddScoped<ICustomerApiClient, ServiceInvocation.CustomerApiClient.Refit.CustomerApiClient >();
        builder.Services.AddScoped<IIdentityApiClient, ServiceInvocation.IdentityApiClient.Refit.IdentityApiClient>();
        builder.Services.AddScoped<IMasterDataApiClient, ServiceInvocation.MasterDataApiClient.Refit.MasterDataApiClient>();
        builder.Services.AddScoped<IOrderingApiClient, ServiceInvocation.OrderingApiClient.Refit.OrderingApiClient>();
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
            options.ClientId = "adminApp";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.RequireHttpsMetadata = false;
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("IdentityServerApi");
            options.Scope.Add("orders");
            options.Scope.Add("basket");
        });

        // Blazor auth services
        builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        builder.Services.AddCascadingAuthenticationState();
    }
}
