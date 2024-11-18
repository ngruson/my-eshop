namespace eShop.Ordering.API.Application.Commands.CreateOrderDraft;

using Ardalis.Result;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

// Regular CommandHandler
public class CreateOrderDraftCommandHandler
    : IRequestHandler<CreateOrderDraftCommand, Result<OrderDraftDTO>>
{
    public Task<Result<OrderDraftDTO>> Handle(CreateOrderDraftCommand message, CancellationToken cancellationToken)
    {
        Order order = Order.NewDraft();

        foreach (OrderItemDto item in message.Items)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        return Task.FromResult(Result.Success(OrderDraftDTO.FromOrder(order)));
    }
}

public record OrderDraftDTO
{
    public required IEnumerable<OrderItemDto> OrderItems { get; init; }
    public decimal Total { get; init; }

    public static OrderDraftDTO FromOrder(Order order)
    {
        return new OrderDraftDTO()
        {
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto(
                oi.ProductId,
                oi.ProductName!,
                oi.UnitPrice,
                oi.Discount,
                oi.Units,
                oi.PictureUrl!)),
            Total = order.GetTotal()
        };
    }
}
