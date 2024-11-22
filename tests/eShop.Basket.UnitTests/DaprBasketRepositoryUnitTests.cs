using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Client;
using eShop.Basket.API.Model;
using eShop.Basket.API.Repositories;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace eShop.Basket.UnitTests;

public class DaprBasketRepositoryUnitTests
{
    public class DeleteBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task return_success_when_basket_was_deleted(
        [Substitute, Frozen] DaprClient daprClient,
        DaprBasketRepository sut,
        string id)
        {
            // Arrange

            // Act

            Result result = await sut.DeleteBasketAsync(id);

            // Assert

            Assert.True(result.IsSuccess);

            await daprClient.Received().DeleteStateAsync(Arg.Any<string>(), id);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_error_when_exception_was_thrown(
        [Substitute, Frozen] DaprClient daprClient,
        DaprBasketRepository sut,
        string id)
        {
            // Arrange

            daprClient.DeleteStateAsync(Arg.Any<string>(), id)
                .ThrowsAsync<Exception>();

            // Act

            Result result = await sut.DeleteBasketAsync(id);

            // Assert

            Assert.True(result.IsError());

            await daprClient.Received().DeleteStateAsync(Arg.Any<string>(), id);
        }
    }

    public class GetBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task return_success_when_basket_exists(
            [Substitute, Frozen] DaprClient daprClient,
            DaprBasketRepository sut,
            CustomerBasket basket,
            string id)
        {
            // Arrange

            daprClient.GetStateAsync<CustomerBasket>(Arg.Any<string>(), id)
                .Returns(basket);

            // Act

            Result<CustomerBasket> result = await sut.GetBasketAsync(id);

            // Assert

            Assert.True(result.IsSuccess);
            Assert.Equivalent(basket, result.Value);

            await daprClient.Received().GetStateAsync<CustomerBasket>(Arg.Any<string>(), id);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_error_when_exception_was_thrown(
            [Substitute, Frozen] DaprClient daprClient,
            DaprBasketRepository sut,
            string id)
        {
            // Arrange

            daprClient.GetStateAsync<CustomerBasket>(Arg.Any<string>(), id)
                .ThrowsAsync<Exception>();

            // Act

            Result<CustomerBasket> result = await sut.GetBasketAsync(id);

            // Assert

            Assert.True(result.IsError());
            await daprClient.Received().GetStateAsync<CustomerBasket>(Arg.Any<string>(), id);
        }
    }

    public class UpdateBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task return_success_when_basket_was_updated(
            [Substitute, Frozen] DaprClient daprClient,
            DaprBasketRepository sut,
            CustomerBasket basket)
        {
            // Arrange

            // Act

            Result<CustomerBasket> result = await sut.UpdateBasketAsync(basket);

            // Assert

            Assert.True(result.IsSuccess);
            Assert.Equivalent(basket, result.Value);
            await daprClient.Received().SaveStateAsync(Arg.Any<string>(), basket.BuyerId, basket);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_error_when_exception_was_thrown(
            [Substitute, Frozen] DaprClient daprClient,
            DaprBasketRepository sut,
            CustomerBasket basket)
        {
            // Arrange

            daprClient.SaveStateAsync(Arg.Any<string>(), basket.BuyerId, basket)
                .ThrowsAsync<Exception>();

            // Act

            Result<CustomerBasket> result = await sut.UpdateBasketAsync(basket);

            // Assert

            Assert.True(result.IsError());
            await daprClient.Received().SaveStateAsync(Arg.Any<string>(), basket.BuyerId, basket);
        }
    }
}
