using Ardalis.Result;
using eShop.Ordering.Contracts.CreateOrder;

namespace eShop.Ordering.API.Application.Commands.CreateOrderDraft;

public record CreateOrderDraftCommand(string BuyerId, OrderItemDto[] Items) : IRequest<Result<OrderDraftDTO>>;
