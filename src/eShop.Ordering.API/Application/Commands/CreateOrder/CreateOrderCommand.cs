namespace eShop.Ordering.API.Application.Commands.CreateOrder;

using Ardalis.Result;
using eShop.Ordering.Contracts.CreateOrder;

public record CreateOrderCommand(
    string WorkflowInstanceId,
    OrderItemDto[] Items,
    Guid UserId,
    string UserName,
    string BuyerName,
    string City,
    string Street,
    string State,
    string Country,
    string ZipCode,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    Guid CardType) : IRequest<Result<Guid>>;
