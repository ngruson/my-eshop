using eShop.Shared.DI;

namespace eShop.Shared.Data.Seed;

public interface IDbSeeder
{
    Task SeedAsync(ServiceProviderWrapper serviceProviderWrapper);
}
