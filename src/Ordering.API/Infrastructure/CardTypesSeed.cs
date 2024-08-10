namespace eShop.Ordering.API.Infrastructure;

using System.Reflection;
using eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;
using eShop.Shared.Data;
using eShop.Shared.DI;

public class CardTypesSeed: IDbSeeder
{
    public async Task SeedAsync(ServiceProviderWrapper serviceProviderWrapper)
    {
        IRepository<CardType> cardTypeRepository = serviceProviderWrapper.GetRequiredService<IRepository<CardType>>();

        if (!await cardTypeRepository.AnyAsync())
        {
            await cardTypeRepository.AddRangeAsync(GetPredefinedCardTypes());
        }
    }

    private static IEnumerable<CardType> GetPredefinedCardTypes()
    {
        Type baseType = typeof(CardType);

        return [.. (from t in (from t in Assembly.GetAssembly(baseType)!.GetTypes()
                            where baseType.IsAssignableFrom(t)
                            select t).SelectMany(GetFieldsOfType<CardType>)
                select t)];
    }

    private static TFieldType[] GetFieldsOfType<TFieldType>(Type type)
    {
        return (from p in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                where type.IsAssignableFrom(p.FieldType)
                select p into pi
                select (TFieldType)pi.GetValue(null)!).ToArray();
    }
}
