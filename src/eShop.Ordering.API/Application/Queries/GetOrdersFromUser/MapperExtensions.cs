using eShop.Ordering.Contracts.GetOrdersFromUser;

namespace eShop.Ordering.API.Application.Queries.GetOrdersFromUser;

internal static class MapperExtensions
{
    internal static OrderDto[] Map(this List<Domain.AggregatesModel.OrderAggregate.Order> orders)
    {
        return orders
            .Select(o => new OrderDto(
                o.ObjectId,
                o.Id.ToString(),
                o.OrderDate,
                o.Buyer?.Name!,
                o.OrderStatus.ToString(),
                o.GetTotal()))
            .ToArray();
    }
}
