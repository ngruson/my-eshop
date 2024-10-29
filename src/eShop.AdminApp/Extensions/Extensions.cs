using eShop.Catalog.Contracts;
using eShop.Customer.Contracts;
using eShop.Identity.Contracts;
using eShop.MasterData.Contracts;
using eShop.Ordering.Contracts;
using eShop.ServiceDefaults;
using eShop.ServiceInvocation.CatalogService;
using eShop.ServiceInvocation.CatalogService.Refit;
using eShop.ServiceInvocation.CustomerService;
using eShop.ServiceInvocation.CustomerService.Refit;
using eShop.ServiceInvocation.IdentityService;
using eShop.ServiceInvocation.IdentityService.Refit;
using eShop.ServiceInvocation.MasterDataService;
using eShop.ServiceInvocation.MasterDataService.Refit;
using eShop.ServiceInvocation.OrderingService;
using eShop.ServiceInvocation.OrderingService.Refit;
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

        if (features?.ServiceInvocation.ServiceInvocationType == ServiceInvocationType.Refit)
        {
            builder.AddRefitServices();
        }        

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();
        });
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

        builder.Services.AddScoped<ICatalogService, CatalogService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();
        builder.Services.AddScoped<IMasterDataService, MasterDataService>();
        builder.Services.AddScoped<IOrderingService, OrderingService>();
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
        services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        services.AddCascadingAuthenticationState();
    }
}
