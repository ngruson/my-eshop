using eShop.Ordering.Contracts.GetOrders;

namespace eShop.Ordering.API.Application.Queries.GetOrders;

internal static class MapperExtensions
{
    internal static List<OrderDto> MapToOrderDtoList(this List<Order> orders)
    {
        return orders
            .Select(o => new OrderDto(
                o.ObjectId,
                o.Id.ToString(),
                o.OrderDate,
                o.Buyer?.Name!,
                o.OrderStatus.ToString(),
                o.Total))
            .ToList();
    }
}
