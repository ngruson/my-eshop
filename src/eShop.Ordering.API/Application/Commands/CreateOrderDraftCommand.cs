using eShop.Ordering.Contracts.CreateOrder;

namespace eShop.Ordering.API.Application.Commands;

public record CreateOrderDraftCommand(string BuyerId, OrderItemDto[] Items) : IRequest<OrderDraftDTO>;
