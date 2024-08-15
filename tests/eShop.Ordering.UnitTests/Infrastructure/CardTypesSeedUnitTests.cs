using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Infrastructure;
using eShop.Shared.Data;
using eShop.Shared.DI;

namespace eShop.Ordering.UnitTests.Infrastructure;
public class CardTypesSeedUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task when_no_existing_data_seed_data(
        [Substitute, Frozen] ServiceProviderWrapper services,
        [Substitute, Frozen] IRepository<CardType> cardTypeRepository,
        CardTypesSeed sut)
    {
        // Arrange

        services.GetRequiredService<IRepository<CardType>>()
            .Returns(cardTypeRepository);

        // Act

        await sut.SeedAsync(services);

        //Assert

        await cardTypeRepository.Received().AddRangeAsync(Arg.Any<IEnumerable<CardType>>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task when_existing_data_no_seed(
        [Substitute, Frozen] ServiceProviderWrapper services,
        [Substitute, Frozen] IRepository<CardType> cardTypeRepository,
        CardTypesSeed sut)
    {
        // Arrange

        cardTypeRepository.AnyAsync()
            .Returns(true);

        services.GetRequiredService<IRepository<CardType>>()
            .Returns(cardTypeRepository);

        // Act

        await sut.SeedAsync(services);

        //Assert

        await cardTypeRepository.DidNotReceive().AddRangeAsync(Arg.Any<IEnumerable<CardType>>());
    }
}
