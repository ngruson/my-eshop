using eShop.Ordering.Contracts.CreateOrder;

namespace eShop.Ordering.API.Application.Commands;

public record CreateOrderDraftCommand(string BuyerId, IEnumerable<OrderItemDto> Items) : IRequest<OrderDraftDTO>;
