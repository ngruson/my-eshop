using eShop.Ordering.Contracts.GetCardTypes;

namespace eShop.Ordering.API.Application.Queries;

public interface IOrderQueries
{
    Task<Order> GetOrderAsync(int id);

    Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(string userId);

    Task<IEnumerable<CardTypeDto>> GetCardTypesAsync();
}
