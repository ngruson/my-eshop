using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;

namespace eShop.ServiceDefaults;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddDefaultAuthentication(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;
        IConfigurationManager configuration = builder.Configuration;

        // {
        //   "Identity": {
        //     "Url": "http://identity",
        //     "Audience": "basket"
        //    }
        // }

        IConfigurationSection identitySection = configuration.GetSection("Identity");

        if (!identitySection.Exists())
        {
            // No identity section, so no authentication
            return services;
        }

        // prevent from mapping "sub" claim to nameidentifier.
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        services.AddAuthentication().AddJwtBearer(options =>
        {
            string identityUrl = identitySection.GetRequiredValue("Url");
            string audience = identitySection.GetRequiredValue("Audience");

            options.Authority = identityUrl;
            options.RequireHttpsMetadata = false;
            options.Audience = audience;
            
#if DEBUG
            //Needed if using Android Emulator Locally. See https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/local-web-services?view=net-maui-8.0#android
            options.TokenValidationParameters.ValidIssuers = [identityUrl, "https://10.0.2.2:5243"];
#else
            options.TokenValidationParameters.ValidIssuers = [identityUrl];
#endif
            
            options.TokenValidationParameters.ValidateAudience = false;
        });

        services.AddAuthorization();

        return services;
    }
}
