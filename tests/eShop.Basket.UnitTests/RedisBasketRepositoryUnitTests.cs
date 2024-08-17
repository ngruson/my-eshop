using System.Text.Json;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Basket.API.Model;
using eShop.Basket.API.Repositories;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Xunit;

namespace eShop.Basket.UnitTests;

public class RedisBasketRepositoryUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task delete_basket(
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

        bool result = await sut.DeleteBasketAsync(id);

        // Assert

        Assert.True(result);

        await database.Received().KeyDeleteAsync(Arg.Any<RedisKey>(), default);
    }

    public class GetBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task return_basket(
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

            CustomerBasket result = await sut.GetBasketAsync(id);

            // Assert

            Assert.Equivalent(basket, result);

            await database.Received().StringGetAsync(Arg.Any<RedisKey>());
        }

        [Theory, AutoNSubstituteData]
        public async Task return_null(
        [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
        [Substitute, Frozen] IDatabase database,
        ILogger<RedisBasketRepository> logger,
        string id)
        {
            // Arrange

            connectionMultiplexer.GetDatabase().Returns(database);

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            CustomerBasket result = await sut.GetBasketAsync(id);

            // Assert

            Assert.Null(result);

            await database.Received().StringGetAsync(Arg.Any<RedisKey>());
        }
    }

    public class UpdateBasket
    {
        [Theory, AutoNSubstituteData]
        public async Task when_update_ok_return_basket(
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

            CustomerBasket result = await sut.UpdateBasketAsync(basket);

            // Assert

            Assert.Equivalent(basket, result);
        }

        [Theory, AutoNSubstituteData]
        public async Task when_update_failed_return_null(
            [Substitute, Frozen] IConnectionMultiplexer connectionMultiplexer,
            [Substitute, Frozen] IDatabase database,
            ILogger<RedisBasketRepository> logger,
            CustomerBasket basket)
        {
            // Arrange

            connectionMultiplexer.GetDatabase().Returns(database);

            RedisBasketRepository sut = new(logger, connectionMultiplexer);

            // Act

            CustomerBasket result = await sut.UpdateBasketAsync(basket);

            // Assert

            Assert.Null(result);
        }
    }
}
