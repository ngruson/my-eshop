using eShop.Ordering.Contracts.GetOrders;

namespace eShop.AdminApp.Application.Queries.Order.GetOrders;

internal static class MapperExtensions
{
    internal static List<OrderViewModel> MapToOrderViewModelList(this List<OrderDto> orders)
    {
        return orders
            .Select(o => new OrderViewModel(o.ObjectId, o.OrderNumber, o.OrderDate, o.BuyerName, o.OrderStatus, o.Total))
            .ToList();
    }
}
