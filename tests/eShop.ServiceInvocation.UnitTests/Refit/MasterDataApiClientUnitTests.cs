using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.MasterData.Contracts;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Refit;

public class MasterDataApiClientUnitTests
{
    public class GetCountries
    {
        [Theory, AutoNSubstituteData]
        public async Task return_countries(
            [Substitute, Frozen] IMasterDataApi masterDataApi,
            MasterDataApiClient.Refit.MasterDataApiClient sut,
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
            MasterDataApiClient.Refit.MasterDataApiClient sut,
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
