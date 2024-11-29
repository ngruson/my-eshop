using eShop.ServiceInvocation.Auth;
using eShop.ServiceInvocation.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eShop.ServiceInvocation;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddServiceInvocation(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<AccessTokenAccessorFactory>();

        builder.Services.Configure<ServiceInvocationOptions>(builder.Configuration.GetSection("Identity"));

        return builder.Services;
    }
}
