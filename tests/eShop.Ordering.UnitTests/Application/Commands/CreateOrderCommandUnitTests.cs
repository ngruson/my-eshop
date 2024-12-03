using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Ordering.Domain.AggregatesModel.SalesTaxRateAggregate;
using eShop.Shared.Data;

namespace eShop.Ordering.UnitTests.Application.Commands;
public class CreateOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task return_success_when_order_created(
        [Substitute, Frozen] IRepository<Order> orderRepositoryMock,
        [Substitute, Frozen] IRepository<CardType> cardTypeRepositoryMock,
        CreateOrderCommandHandler sut,
        CreateOrderCommand command,
        CardType cardType)
    {
        // Arrange

        cardTypeRepositoryMock.SingleOrDefaultAsync(Arg.Any<CardTypeSpecification>(), default)
            .Returns(cardType);

        OrderItemDto[] orderItems = command.Items.Select(
            x => new OrderItemDto(x.ProductId, x.ProductName, Math.Abs(x.UnitPrice), 0, x.Units, x.PictureUrl)).ToArray();

        //Act

        await sut.Handle(command with { CardType = cardType.ObjectId, Items = orderItems }, default);

        //Assert

        await orderRepositoryMock.Received().AddAsync(Arg.Any<Order>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task return_success_given_sales_tax_when_order_created(
        [Substitute, Frozen] IRepository<Order> orderRepositoryMock,
        [Substitute, Frozen] IRepository<CardType> cardTypeRepositoryMock,
        [Substitute, Frozen] IRepository<SalesTaxRate> salesTaxRateRepositoryMock,
        CreateOrderCommandHandler sut,
        CreateOrderCommand command,
        CardType cardType,
        SalesTaxRate salesTaxRate)
    {
        // Arrange

        cardTypeRepositoryMock.SingleOrDefaultAsync(Arg.Any<CardTypeSpecification>(), default)
            .Returns(cardType);

        salesTaxRateRepositoryMock.SingleOrDefaultAsync(Arg.Any<SalesTaxRateSpecification>(), default)
            .Returns(salesTaxRate);

        OrderItemDto[] orderItems = command.Items.Select(
            x => new OrderItemDto(x.ProductId, x.ProductName, Math.Abs(x.UnitPrice), 0, x.Units, x.PictureUrl)).ToArray();

        //Act

        await sut.Handle(command with { CardType = cardType.ObjectId, Items = orderItems }, default);

        //Assert

        await orderRepositoryMock.Received().AddAsync(Arg.Any<Order>(), default);
    }
}
