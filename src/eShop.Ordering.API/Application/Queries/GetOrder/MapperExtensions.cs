using eShop.Ordering.Contracts.GetOrder;

namespace eShop.Ordering.API.Application.Queries.GetOrder;

internal static class MapperExtensions
{
    internal static OrderDto Map(this Order order)
    {
        return new OrderDto(
            order.ObjectId,
            order.Id,
            order.OrderDate,
            order.Buyer?.Name,
            order.OrderStatus.ToString(),
            new AddressDto(
                order.Address!.City,
                order.Address.Country,
                order.Address.State,
                order.Address.Street,
                order.Address.ZipCode
            ),
            order.NetTotal,
            order.Total,
            order.OrderItems.Select(x => new OrderItemDto(
                    x.ProductId,
                    x.ProductName!,
                    x.UnitPrice,
                    x.SalesTaxRate,
                    x.Discount,
                    x.Units,
                    x.Total,
                    x.PictureUrl!))
                .ToArray(),
            order.SalesTaxGroups.Select(_ =>
                new Contracts.GetOrder.SalesTaxGroup(_.Rate, _.Total)).ToArray());
    }
}
