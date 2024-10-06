using eShop.Shared.Data;
using eShop.Shared.Data.Seed;
using eShop.Shared.DI;

namespace eShop.Ordering.API.Infrastructure;
public class CardTypesSeed: IDbSeeder
{
    public async Task SeedAsync(ServiceProviderWrapper serviceProviderWrapper)
    {
        IRepository<CardType> cardTypeRepository = serviceProviderWrapper.GetRequiredService<IRepository<CardType>>();

        if (!await cardTypeRepository.AnyAsync())
        {
            await cardTypeRepository.AddRangeAsync(
                [new CardType("Amex"), new CardType("Visa"), new CardType("MasterCard")]);
        }
    }
}
