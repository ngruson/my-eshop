using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.MasterData.Contracts;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests;

public class MasterDataServiceUnitTests
{
    public class GetCountries
    {
        [Theory, AutoNSubstituteData]
        public async Task return_countries(
            [Substitute, Frozen] IMasterDataApi masterDataApi,
            MasterDataService.Refit.MasterDataService sut,
            CountryDto[] countries
        )
        {
            // Arrange
            masterDataApi.GetCountries()
                .Returns(countries);

            // Act

            CountryDto[] actual = await sut.GetCountries();

            // Assert

            Assert.Equal(actual, countries);
        }
    }

    public class GetStates
    {
        [Theory, AutoNSubstituteData]
        public async Task return_states(
            [Substitute, Frozen] IMasterDataApi masterDataApi,
            MasterDataService.Refit.MasterDataService sut,
            StateDto[] states
        )
        {
            // Arrange
            masterDataApi.GetStates()
                .Returns(states);

            // Act

            StateDto[] actual = await sut.GetStates();

            // Assert

            Assert.Equal(actual, states);
        }
    }
}
