using eShop.Ordering.Contracts.GetOrders;

namespace eShop.Ordering.API.Application.Queries.GetOrders;

internal static class MapperExtensions
{
    internal static List<OrderDto> MapToOrderDtoList(this List<Domain.AggregatesModel.OrderAggregate.Order> orders)
    {
        return orders
            .Select(o => new OrderDto(
                o.Id.ToString(),
                o.OrderDate,
                o.Buyer?.Name!,
                o.OrderStatus.ToString(),
                o.GetTotal()))
            .ToList();
    }
}
