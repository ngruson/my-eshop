using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace eShop.WebhookClient.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();

        // Application services
        builder.Services.AddOptions<WebhookClientOptions>().BindConfiguration(nameof(WebhookClientOptions));
        builder.Services.AddSingleton<HooksRepository>();

        // HTTP client registrations
        builder.Services.AddHttpClient<WebhooksClient>(o => o.BaseAddress = new("http://webhooks-api"))
            .AddApiVersion(1.0)
            .AddAuthToken();
    }

    public static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
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
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime);

            // Must be distinct from WebApp's cookie name, otherwise the two sites will interfere
            // with each other when both are on localhost (yes, even when they are on different ports)
            options.Cookie.Name = ".AspNetCore.WebHooksClientIdentity";
        })
        .AddOpenIdConnect(options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = identityUrl.ToString();
            options.SignedOutRedirectUri = callBackUrl.ToString();
            options.ClientId = "webhooksclient";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.RequireHttpsMetadata = false;
            options.Scope.Add("openid");
            options.Scope.Add("webhooks");
        });

        builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        builder.Services.AddCascadingAuthenticationState();
    }
}
