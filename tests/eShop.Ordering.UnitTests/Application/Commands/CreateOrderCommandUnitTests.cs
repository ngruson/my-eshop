using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace eShop.Ordering.UnitTests.Application.Commands;
public class CreateOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task WhenValidOrder_CreateOrder(
        [Substitute, Frozen] IRepository<Order> orderRepositoryMock,
        CreateOrderCommandHandler sut,
        CreateOrderCommand command)
    {
        // Arrange

        OrderItemDto[] orderItems = command.OrderItems.Select(
            x => new OrderItemDto(x.ProductId, x.ProductName, x.UnitPrice, 0, x.Units, x.PictureUrl)).ToArray();

        CreateOrderCommand command2 = new(
            orderItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            command.CardNumber,
            command.CardHolderName,
            command.CardExpiration,
            command.CardSecurityNumber,
            command.CardTypeId);

        //Act

        await sut.Handle(command, default);

        //Assert

        await orderRepositoryMock.Received().AddAsync(Arg.Any<Order>(), default);
    }
}
