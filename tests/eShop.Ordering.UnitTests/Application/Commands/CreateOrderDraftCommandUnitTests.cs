using eShop.Ordering.API.Application.Commands.CreateOrderDraft;
using eShop.Ordering.Contracts.CreateOrder;

namespace eShop.Ordering.UnitTests.Application.Commands;
public class CreateOrderDraftCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task CreateDraftOrder(
        CreateOrderDraftCommandHandler sut,
        CreateOrderDraftCommand command)
    {
        // Arrange

        OrderItemDto[] items = command.Items.Select(
            x => new OrderItemDto(x.ProductId, x.ProductName, Math.Abs(x.UnitPrice), 0, x.Units, x.PictureUrl)).ToArray();

        //Act

        OrderDraftDTO result = await sut.Handle(command with { Items = items }, default);

        //Assert

        Assert.NotNull(result);
    }
}
