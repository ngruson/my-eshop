using System.Text.Json;
using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Basket.API.Model;
using eShop.Basket.API.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using StackExchange.Redis;
using Xunit;

namespace eShop.Basket.UnitTests;

public class RedisBasketRepositoryUnitTests
{
    public class DeleteBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task return_success_when_basket_was_deleted(
            [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
            [Substitute, Frozen] IDatabase database,
            ILogger<RedisBasketRepository> logger,
            string id)
        {
            // Arrange

            database.KeyDeleteAsync(Arg.Any<RedisKey>(), default).Returns(true);

            connectionMultiplexer.GetDatabase().Returns(database);

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            Result result = await sut.DeleteBasketAsync(id);

            // Assert

            Assert.True(result.IsSuccess);

            await database.Received().KeyDeleteAsync(Arg.Any<RedisKey>(), default);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_error_when_exception_was_thrown(
            [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
            [Substitute, Frozen] IDatabase database,
            ILogger<RedisBasketRepository> logger,
            string id)
        {
            // Arrange

            database.KeyDeleteAsync(Arg.Any<RedisKey>(), default)
                .ThrowsAsync<Exception>();

            connectionMultiplexer.GetDatabase().Returns(database);

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            Result result = await sut.DeleteBasketAsync(id);

            // Assert

            Assert.True(result.IsError());
            await database.Received().KeyDeleteAsync(Arg.Any<RedisKey>(), default);
        }
    }    

    public class GetBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task return_success_when_basket_was_retrieved(
            [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
            [Substitute, Frozen] IDatabase database,
            ILogger<RedisBasketRepository> logger,
            CustomerBasket basket,
            string id)
        {
            // Arrange

            connectionMultiplexer.GetDatabase().Returns(database);
            
            database.StringGetAsync(Arg.Any<RedisKey>())
                .Returns(Task.FromResult(new RedisValue(JsonSerializer.Serialize(basket))));

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            Result<CustomerBasket> result = await sut.GetBasketAsync(id);

            // Assert

            Assert.True(result.IsSuccess);
            Assert.Equivalent(basket, result.Value);
            await database.Received().StringGetAsync(Arg.Any<RedisKey>());
        }

        [Theory, AutoNSubstituteData]
        public async Task return_not_found_when_basket_does_not_exist(
        [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
        [Substitute, Frozen] IDatabase database,
        ILogger<RedisBasketRepository> logger,
        string id)
        {
            // Arrange

            connectionMultiplexer.GetDatabase().Returns(database);

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            Result<CustomerBasket> result = await sut.GetBasketAsync(id);

            // Assert

            Assert.True(result.IsNotFound());
            await database.Received().StringGetAsync(Arg.Any<RedisKey>());
        }

        [Theory, AutoNSubstituteData]
        public async Task return_error_when_exception_was_thrown(
            [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
            [Substitute, Frozen] IDatabase database,
            ILogger<RedisBasketRepository> logger,
            string id)
        {
            // Arrange

            connectionMultiplexer.GetDatabase().Returns(database);

            database.StringGetAsync(Arg.Any<RedisKey>())
                .ThrowsAsync<Exception>();

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            Result<CustomerBasket> result = await sut.GetBasketAsync(id);

            // Assert

            Assert.True(result.IsError());            
            await database.Received().StringGetAsync(Arg.Any<RedisKey>());
        }
    }

    public class UpdateBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task return_success_when_basket_was_updated(
            [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
            [Substitute, Frozen] IDatabase database,
            ILogger<RedisBasketRepository> logger,
            CustomerBasket basket)
        {
            // Arrange

            connectionMultiplexer.GetDatabase().Returns(database);

            string json = JsonSerializer.Serialize(basket);

            database.StringGetAsync(Arg.Any<RedisKey>())
                .Returns(Task.FromResult(new RedisValue(json)));

            database.StringSetAsync(Arg.Any<RedisKey>(), json)
                .Returns(true);

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            Result<CustomerBasket> result = await sut.UpdateBasketAsync(basket);

            // Assert

            Assert.True(result.IsSuccess);
            Assert.Equivalent(basket, result.Value);
            await database.Received().StringSetAsync(Arg.Any<RedisKey>(), json);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_error_when_update_failed(
            [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
            [Substitute, Frozen] IDatabase database,
            ILogger<RedisBasketRepository> logger,
            CustomerBasket basket)
        {
            // Arrange

            connectionMultiplexer.GetDatabase().Returns(database);

            string json = JsonSerializer.Serialize(basket);

            database.StringGetAsync(Arg.Any<RedisKey>())
                .Returns(Task.FromResult(new RedisValue(json)));

            database.StringSetAsync(Arg.Any<RedisKey>(), json)
                .Returns(false);

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            Result<CustomerBasket> result = await sut.UpdateBasketAsync(basket);

            // Assert

            Assert.True(result.IsError());
            await database.Received().StringSetAsync(Arg.Any<RedisKey>(), json);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_error_when_exception_was_thrown(
            [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
            [Substitute, Frozen] IDatabase database,
            ILogger<RedisBasketRepository> logger,
            CustomerBasket basket)
        {
            // Arrange

            connectionMultiplexer.GetDatabase().Returns(database);

            string json = JsonSerializer.Serialize(basket);

            database.StringGetAsync(Arg.Any<RedisKey>())
                .Returns(Task.FromResult(new RedisValue(json)));

            database.StringSetAsync(Arg.Any<RedisKey>(), json)
                .ThrowsAsync<Exception>();

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            Result<CustomerBasket> result = await sut.UpdateBasketAsync(basket);

            // Assert

            Assert.True(result.IsError());
            await database.Received().StringSetAsync(Arg.Any<RedisKey>(), json);
        }
    }
}
