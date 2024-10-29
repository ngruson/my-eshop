using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace eShop.Ordering.UnitTests.Application.Commands;
public class CreateOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task WhenValidOrder_CreateOrder(
        [Substitute, Frozen] IRepository<Order> orderRepositoryMock,
        [Substitute, Frozen] IRepository<CardType> cardTypeRepositoryMock,
        CreateOrderCommandHandler sut,
        CreateOrderCommand command,
        CardType cardType)
    {
        // Arrange

        cardTypeRepositoryMock.SingleOrDefaultAsync(Arg.Any<CardTypeSpecification>(), default)
            .Returns(cardType);

        OrderItemDto[] orderItems = command.OrderItems.Select(
            x => new OrderItemDto(x.ProductId, x.ProductName, Math.Abs(x.UnitPrice), 0, x.Units, x.PictureUrl)).ToArray();

        //Act

        await sut.Handle(command with { CardType = cardType.ObjectId, OrderItems = orderItems }, default);

        //Assert

        await orderRepositoryMock.Received().AddAsync(Arg.Any<Order>(), default);
    }
}
