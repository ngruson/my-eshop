using System.Security.Claims;
using eShop.Basket.API.Repositories;
using eShop.Basket.API.Grpc;
using eShop.Basket.API.Model;
using eShop.Basket.UnitTests.Helpers;
using Xunit;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using eShop.Basket.Contracts.Grpc;
using Ardalis.Result;

namespace eShop.Basket.UnitTests;

public class BasketServiceTests
{
    public class GetBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task when_no_user_return_empty_basket(
            [Substitute, Frozen] IBasketRepository repository,
            BasketService sut,
            string userId)
        {
            // Arrange

            TestServerCallContext serverCallContext = TestServerCallContext.Create();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", userId)]))
            };
            serverCallContext.SetUserState("__HttpContext", httpContext);

            repository.GetBasketAsync(userId).Returns(Result.NotFound());

            // Act

            CustomerBasketResponse response = await sut.GetBasket(new GetBasketRequest(), serverCallContext);

            // Assert

            Assert.IsType<CustomerBasketResponse>(response);
            Assert.Empty(response.Items);
        }

        [Theory, AutoNSubstituteData]
        public async Task when_valid_user_id_return_basket(
            [Substitute, Frozen] IBasketRepository repository,
            [Substitute, Frozen] ILogger<BasketService> logger,
            BasketService sut,
            string userId,
            CustomerBasket basket)
        {
            // Arrange

            repository.GetBasketAsync(userId).Returns(basket);

            logger.IsEnabled(LogLevel.Debug).Returns(true);

            TestServerCallContext serverCallContext = TestServerCallContext.Create();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", userId)]))
            };
            serverCallContext.SetUserState("__HttpContext", httpContext);

            // Act

            CustomerBasketResponse response = await sut.GetBasket(new GetBasketRequest(), serverCallContext);

            // Assert

            Assert.IsType<CustomerBasketResponse>(response);
            Assert.Equal(basket.Items.Count, response.Items.Count);
        }

        [Theory, AutoNSubstituteData]
        public async Task when_invalid_user_id_return_empty_basket(
            [Substitute, Frozen] IBasketRepository repository,
            BasketService sut,
            CustomerBasket basket)
        {
            // Arrange

            repository.GetBasketAsync("1").Returns(basket);
            TestServerCallContext serverCallContext = TestServerCallContext.Create();
            DefaultHttpContext httpContext = new();
            serverCallContext.SetUserState("__HttpContext", httpContext);

            // Act

            CustomerBasketResponse response = await sut.GetBasket(new GetBasketRequest(), serverCallContext);

            Assert.IsType<CustomerBasketResponse>(response);
            Assert.Empty(response.Items);
        }
    }

    public class UpdateBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task when_no_basket_throw_not_found(
            [Substitute, Frozen] IBasketRepository repository,
            BasketService sut,
            string userId)
        {
            // Arrange

            TestServerCallContext serverCallContext = TestServerCallContext.Create();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", userId)]))
            };
            serverCallContext.SetUserState("__HttpContext", httpContext);

            repository.UpdateBasketAsync(Arg.Any<CustomerBasket>())
                .Returns(Result.NotFound());

            // Act

            RpcException exception = await Assert.ThrowsAsync<RpcException>(() => sut.UpdateBasket(new UpdateBasketRequest(), serverCallContext));

            // Assert

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
        }

        [Theory, AutoNSubstituteData]
        public async Task when_valid_user_id_update_and_return_basket(
            [Substitute, Frozen] IBasketRepository repository,
            [Substitute, Frozen] ILogger<BasketService> logger,
            BasketService sut,
            string userId,
            CustomerBasket basket)
        {
            // Arrange

            repository.UpdateBasketAsync(Arg.Any<CustomerBasket>()).Returns(basket);
            logger.IsEnabled(LogLevel.Debug).Returns(true);

            TestServerCallContext serverCallContext = TestServerCallContext.Create();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", userId)]))
            };
            serverCallContext.SetUserState("__HttpContext", httpContext);

            // Act

            CustomerBasketResponse response = await sut.UpdateBasket(new UpdateBasketRequest(), serverCallContext);

            // Assert

            await repository.Received().UpdateBasketAsync(Arg.Any<CustomerBasket>());
            Assert.IsType<CustomerBasketResponse>(response);
            Assert.Equal(basket.Items.Count, response.Items.Count);
        }

        [Theory, AutoNSubstituteData]
        public async Task when_invalid_user_id_throw_not_authenticated(
            BasketService sut)
        {
            // Arrange

            TestServerCallContext serverCallContext = TestServerCallContext.Create();
            DefaultHttpContext httpContext = new();
            serverCallContext.SetUserState("__HttpContext", httpContext);

            // Act

            RpcException exception = await Assert.ThrowsAsync<RpcException>(() => sut.UpdateBasket(new UpdateBasketRequest(), serverCallContext));

            // Assert

            Assert.Equal(StatusCode.Unauthenticated, exception.StatusCode);
        }
    }

    public class DeleteBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task when_invalid_user_id_throw_not_authenticated(
            [Substitute, Frozen] IBasketRepository repository,
            BasketService sut,
            DeleteBasketRequest request)
        {
            // Arrange

            TestServerCallContext serverCallContext = TestServerCallContext.Create();
            DefaultHttpContext httpContext = new();
            serverCallContext.SetUserState("__HttpContext", httpContext);

            // Act

            RpcException exception = await Assert.ThrowsAsync<RpcException>(() => sut.DeleteBasket(request, serverCallContext));

            // Assert

            await repository.DidNotReceive().DeleteBasketAsync(Arg.Any<string>());
            Assert.Equal(StatusCode.Unauthenticated, exception.StatusCode);
        }

        [Theory, AutoNSubstituteData]
        public async Task when_valid_user_id_delete_basket(
            [Substitute, Frozen] IBasketRepository repository,
            BasketService sut,
            string userId,
            DeleteBasketRequest request)
        {
            // Arrange

            TestServerCallContext serverCallContext = TestServerCallContext.Create();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", userId)]))
            };
            serverCallContext.SetUserState("__HttpContext", httpContext);

            // Act

            DeleteBasketResponse result =  await sut.DeleteBasket(request, serverCallContext);

            // Assert

            await repository.Received().DeleteBasketAsync(Arg.Any<string>());
            Assert.NotNull(result);
        }
    }
}
