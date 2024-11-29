using Microsoft.Extensions.DependencyInjection;

namespace eShop.Shared.DI;

public class ServiceProviderWrapper(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    public virtual T GetRequiredService<T>() where T : class
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public object GetRequiredService(Type type)
    {
        return this.serviceProvider.GetRequiredService(type);
    }
}
