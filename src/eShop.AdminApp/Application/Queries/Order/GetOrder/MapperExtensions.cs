using System.Globalization;
using eShop.AdminApp.Application.Commands.Order.UpdateOrder;
using eShop.Ordering.Contracts.GetOrder;

namespace eShop.AdminApp.Application.Queries.Order.GetOrder;

internal static class MapperExtensions
{
    internal static OrderViewModel Map(this OrderDto order)
    {
        return new OrderViewModel
        {
            ObjectId = order.ObjectId,
            OrderNumber = order.OrderNumber.ToString(),
            OrderDate = order.OrderDate.ToShortDateString(),
            BuyerName = order.BuyerName,
            OrderStatus = order.OrderStatus,
            Address = new AddressViewModel
            {
                Street = order.Address.Street,
                City = order.Address.City,
                State = order.Address.State,
                Country = order.Address.Country,
                ZipCode = order.Address.ZipCode
            },
            Total = order.Total.ToString("C", new CultureInfo("en-US")),
            OrderItems = order.OrderItems.Select(x => new OrderItemViewModel
            {
                ProductName = x.ProductName,
                PictureUrl = x.PictureUrl,
                UnitPrice = x.UnitPrice.ToString("C", new CultureInfo("en-US")),
                Discount = x.Discount.ToString("C", new CultureInfo("en-US")),
                Units = x.Units,
                Total = x.Total.ToString("C", new CultureInfo("en-US"))
            })                
            .ToArray()
        };
    }

    internal static UpdateOrderCommand Map(this OrderViewModel order)
    {
        return new UpdateOrderCommand(
            order.ObjectId,
            new Ordering.Contracts.UpdateOrder.OrderDto(
                new Ordering.Contracts.UpdateOrder.AddressDto(
                    order.Address.City,
                    order.Address.Country,
                    order.Address.State,
                    order.Address.Street,
                    order.Address.ZipCode
                )));
    }
}
