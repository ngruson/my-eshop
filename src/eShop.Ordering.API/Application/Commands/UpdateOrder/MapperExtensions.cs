using eShop.Ordering.Contracts.UpdateOrder;

namespace eShop.Ordering.API.Application.Commands.UpdateOrder;

internal static class MapperExtensions
{
    internal static void Map(this OrderDto dto, Domain.AggregatesModel.OrderAggregate.Order order)
    {
        Address address = new(
            dto.Address.Street,
            dto.Address.City,
            dto.Address.State,
            dto.Address.Country,
            dto.Address.ZipCode);

        order.SetAddress(address);
    }
}
