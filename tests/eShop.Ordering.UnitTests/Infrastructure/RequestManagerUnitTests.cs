using eShop.Ordering.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace eShop.Ordering.UnitTests.Infrastructure;

public class RequestManagerUnitTests
{
    public class Exists
    {
        [Theory, AutoNSubstituteData]
        internal async Task return_true_when_client_request_exists(
            DbContextOptionsBuilder<OrderingContext> optionsBuilder,
            IMediator mediator,
            ClientRequest clientRequest)
        {
            // Arrange

            optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
            OrderingContext context = new(optionsBuilder.Options, mediator);
            context.Database.EnsureCreated();

            context.Set<ClientRequest>().Add(clientRequest);

            RequestManager sut = new(context);

            // Act
            var result = await sut.ExistAsync(clientRequest.Id);

            // Assert
            Assert.True(result);
        }

        [Theory, AutoNSubstituteData]
        internal async Task return_false_when_client_request_does_not_exist(
            DbContextOptionsBuilder<OrderingContext> optionsBuilder,
            IMediator mediator,
            ClientRequest clientRequest)
        {
            // Arrange

            optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
            OrderingContext context = new(optionsBuilder.Options, mediator);
            context.Database.EnsureCreated();

            RequestManager sut = new(context);

            // Act
            var result = await sut.ExistAsync(clientRequest.Id);

            // Assert
            Assert.False(result);
        }
    }

    public class CreateRequestForCommand
    {
        [Theory, AutoNSubstituteData]
        internal async Task throw_exception_when_client_request_exists(
            DbContextOptionsBuilder<OrderingContext> optionsBuilder,
            IMediator mediator,
            ClientRequest clientRequest)
        {
            // Arrange
            optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
            OrderingContext context = new(optionsBuilder.Options, mediator);
            context.Database.EnsureCreated();
            context.Set<ClientRequest>().Add(clientRequest);
            RequestManager sut = new(context);

            // Act

            async Task act() => await sut.CreateRequestForCommandAsync<RequestManagerUnitTests>(clientRequest.Id);

            // Assert

            await Assert.ThrowsAsync<OrderingDomainException>(act);
        }

        [Theory, AutoNSubstituteData]
        internal async Task create_client_request_when_client_request_does_not_exist(
            DbContextOptionsBuilder<OrderingContext> optionsBuilder,
            IMediator mediator,
            ClientRequest clientRequest)
        {
            // Arrange

            optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
            OrderingContext context = new(optionsBuilder.Options, mediator);
            context.Database.EnsureCreated();
            RequestManager sut = new(context);

            // Act

            await sut.CreateRequestForCommandAsync<RequestManagerUnitTests>(clientRequest.Id);

            // Assert

            var result = await context.Set<ClientRequest>().FindAsync(clientRequest.Id);
            Assert.NotNull(result);
        }
    }
}
