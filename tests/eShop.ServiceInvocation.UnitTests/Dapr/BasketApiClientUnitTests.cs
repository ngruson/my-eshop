using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Basket.Contracts.Grpc;
using Grpc.Core;
using NSubstitute;
using static eShop.Basket.Contracts.Grpc.Basket;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class BasketApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task delete_basket(
        [Substitute, Frozen] BasketClient basketClient,
        BasketApiClient.Dapr.BasketApiClient sut)
    {
        // Arrange        

        DeleteBasketResponse response = new();
        AsyncUnaryCall<DeleteBasketResponse> asyncUnaryCall = new(Task.FromResult(response), Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => [], () => { });
        basketClient.DeleteBasketAsync(Arg.Any<DeleteBasketRequest>(), Arg.Any<Metadata>()).Returns(asyncUnaryCall);

        // Act

        await sut.DeleteBasketAsync();

        // Assert

        _ = basketClient.Received().DeleteBasketAsync(Arg.Any<DeleteBasketRequest>(), Arg.Any<Metadata>());
    }

    [Theory, AutoNSubstituteData]
    public async Task get_basket(
        [Substitute, Frozen] BasketClient basketClient,
        BasketApiClient.Dapr.BasketApiClient sut,
        CustomerBasketResponse response)
    {
        // Arrange        
        
        AsyncUnaryCall<CustomerBasketResponse> asyncUnaryCall = new(Task.FromResult(response), Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => [], () => { });
        basketClient.GetBasketAsync(Arg.Any<GetBasketRequest>(), Arg.Any<Metadata>()).Returns(asyncUnaryCall);

        // Act

        CustomerBasketResponse result = await sut.GetBasketAsync();

        // Assert

        Assert.Equal(response, result);
        _ = basketClient.Received().GetBasketAsync(Arg.Any<GetBasketRequest>(), Arg.Any<Metadata>());
    }

    [Theory, AutoNSubstituteData]
    public async Task update_basket(
        [Substitute, Frozen] BasketClient basketClient,
        BasketApiClient.Dapr.BasketApiClient sut,
        UpdateBasketRequest updateBasketRequest,
        CustomerBasketResponse customerBasketResponse)
    {
        // Arrange        

        AsyncUnaryCall<CustomerBasketResponse> asyncUnaryCall = new(Task.FromResult(customerBasketResponse), Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => [], () => { });
        basketClient.UpdateBasketAsync(updateBasketRequest, Arg.Any<Metadata>()).Returns(asyncUnaryCall);

        // Act

        await sut.UpdateBasketAsync(updateBasketRequest);

        // Assert
        
        _ = basketClient.Received().UpdateBasketAsync(Arg.Any<UpdateBasketRequest>(), Arg.Any<Metadata>());
    }
}
