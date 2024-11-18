using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Basket.Contracts.Grpc;
using Grpc.Core;
using NSubstitute;
using static eShop.Basket.Contracts.Grpc.Basket;

namespace eShop.ServiceInvocation.UnitTests.Refit;

public class BasketApiClientUnitTests
{
    public class DeleteBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task delete_basket(
            [Substitute, Frozen] BasketClient basketClient,
            BasketApiClient.Refit.BasketApiClient sut)
        {
            // Arrange

            basketClient.DeleteBasketAsync(Arg.Any<DeleteBasketRequest>())
                .Returns(new AsyncUnaryCall<DeleteBasketResponse>(
                    Task.FromResult(new DeleteBasketResponse()),
                    Task.FromResult(new Metadata()),
                    () => Status.DefaultSuccess,
                    () => [],
                    () => { }));

            // Act

            await sut.DeleteBasketAsync();

            // Assert

            //await basketClient.Received().DeleteBasketAsync(
            //    Arg.Any<DeleteBasketRequest>());

            //await basketClient.Received().DeleteBasketAsync(
            //    Arg.Any<DeleteBasketRequest>(),
            //    Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>());
        }
    }

    public class GetBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task return_basket(
            [Substitute, Frozen] BasketClient basketClient,
            BasketApiClient.Refit.BasketApiClient sut,
            CustomerBasketResponse customerBasketResponse)
        {
            // Arrange

            basketClient.GetBasketAsync(Arg.Any<GetBasketRequest>())
                .Returns(new AsyncUnaryCall<CustomerBasketResponse>(
                    Task.FromResult(customerBasketResponse),
                    Task.FromResult(new Metadata()),
                    () => Status.DefaultSuccess,
                    () => [],
                    () => { }));

            // Act

            CustomerBasketResponse actual = await sut.GetBasketAsync();

            // Assert
            Assert.Equivalent(actual, customerBasketResponse);
        }
    }

    public class UpdateBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task update_basket(
            [Substitute, Frozen] BasketClient basketClient,
            BasketApiClient.Refit.BasketApiClient sut,
            UpdateBasketRequest updateBasketRequest,
            CustomerBasketResponse customerBasketResponse)
        {
            // Arrange

            basketClient.UpdateBasketAsync(Arg.Any<UpdateBasketRequest>())
                .Returns(new AsyncUnaryCall<CustomerBasketResponse>(
                    Task.FromResult(customerBasketResponse),
                    Task.FromResult(new Metadata()),
                    () => Status.DefaultSuccess,
                    () => [],
                    () => { }));

            // Act

            await sut.UpdateBasketAsync(updateBasketRequest);

            // Assert

            //await basketClient.Received().UpdateBasketAsync(
            //    Arg.Any<UpdateBasketRequest>());
        }
    }
}
