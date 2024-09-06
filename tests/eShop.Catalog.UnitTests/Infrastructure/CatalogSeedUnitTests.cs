using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Infrastructure;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Services;
using eShop.Shared.Data;
using eShop.Shared.DI;
using NSubstitute;
using Pgvector;
using static eShop.Catalog.API.Infrastructure.CatalogSeed;

namespace eShop.Catalog.API.UnitTests.Infrastructure;
public class CatalogSeedUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task when_no_existing_data_seed_data(
        [Substitute, Frozen] ServiceProviderWrapper services,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] ICatalogAI catalogAI,
        CatalogSeed sut,
        List<CatalogBrand> catalogBrands,
        List<CatalogType> catalogTypes,
        Vector vector)
    {
        // Arrange

        Random random = new();

        foreach (CatalogSourceEntry source in sut.SourceItems)
        {
            source.Brand = catalogBrands[random.Next(0, catalogBrands.Count)].Brand;
            source.Type = catalogTypes[random.Next(0, catalogTypes.Count)].Type;
        }

        catalogBrandRepository.ListAsync(default)
            .Returns(catalogBrands);

        catalogTypeRepository.ListAsync()
            .Returns(catalogTypes);

        services.GetRequiredService<IRepository<CatalogBrand>>()
            .Returns(catalogBrandRepository);

        services.GetRequiredService<IRepository<CatalogType>>()
            .Returns(catalogTypeRepository);

        catalogAI.IsEnabled
            .Returns(true);

        List<Vector> vectors = sut.SourceItems.Select(_ => vector).ToList();

        catalogAI.GetEmbeddingsAsync(Arg.Any<IEnumerable<CatalogItem>>())
            .Returns(vectors);

        // Act

        await sut.SeedAsync(services);

        //Assert

        await catalogBrandRepository.DeleteRangeAsync(Arg.Any<IEnumerable<CatalogBrand>>());
        await catalogBrandRepository.AddRangeAsync(Arg.Any<IEnumerable<CatalogBrand>>());

        await catalogTypeRepository.DeleteRangeAsync(Arg.Any<IEnumerable<CatalogType>>());
        await catalogTypeRepository.AddRangeAsync(Arg.Any<IEnumerable<CatalogType>>());

        await catalogItemRepository.AddRangeAsync(Arg.Any<IEnumerable<CatalogItem>>());
    }
}
