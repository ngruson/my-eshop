using System.Net.Http;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Client;
using eShop.MasterData.Contracts;
using eShop.Shared.Auth;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class MasterDataApiClientUnitTests
{
    public class GetCountries
    {
        [Theory, AutoNSubstituteData]
        public async Task return_countries(
            [Substitute, Frozen] AccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] DaprClient daprClient,
            MasterDataApiClient.Dapr.MasterDataApiClient sut,
            CountryDto[] countries,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessTokenAsync()
                .Returns(accessToken);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<CountryDto[]>(httpRequestMessage)
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
            [Substitute, Frozen] AccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] DaprClient daprClient,
            MasterDataApiClient.Dapr.MasterDataApiClient sut,
            StateDto[] states,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessTokenAsync()
                .Returns(accessToken);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<StateDto[]>(httpRequestMessage)
                .Returns(states);

            // Act

            StateDto[] actual = await sut.GetStates();

            // Assert

            Assert.Equal(actual, states);
        }
    }
}
