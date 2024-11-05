using eShop.Ordering.Contracts.GetCardTypes;

namespace eShop.Ordering.API.Application.Queries;

public interface IOrderQueries
{
    Task<Order> GetOrderAsync(Guid objectId);

    Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(Guid userId);

    Task<IEnumerable<CardTypeDto>> GetCardTypesAsync();
}
