using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace Ordering.UnitTests.Application.Commands;
public class CreateOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task WhenValidOrder_CreateOrder(
        [Substitute, Frozen] IRepository<Order> orderRepositoryMock,
        CreateOrderCommandHandler sut,
        CreateOrderCommand command)
    {
        // Arrange

        //Act

        await sut.Handle(command, default);

        //Assert

        await orderRepositoryMock.Received().UnitOfWork.SaveChangesAsync(default);
    }
}
