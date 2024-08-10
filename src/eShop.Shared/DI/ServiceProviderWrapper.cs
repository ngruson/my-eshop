using Microsoft.Extensions.DependencyInjection;

namespace eShop.Shared.DI;

public class ServiceProviderWrapper(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public virtual T GetRequiredService<T>() where T : class
    {
        return this._serviceProvider.GetRequiredService<T>();
    }

    public object GetRequiredService(Type type)
    {
        return this._serviceProvider.GetRequiredService(type);
    }
}
