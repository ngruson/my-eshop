using eShop.Customer.Contracts.UpdateCustomer;

namespace eShop.Customer.API.Application.Commands.UpdateCustomer;
internal static class MapperExtensions
{
    public static void MapFromDto(this UpdateCustomerDto dto, Domain.AggregatesModel.CustomerAggregate.Customer customer)
    {
        customer.UserName = dto.UserName;
        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Street = dto.Street;
        customer.ZipCode = dto.ZipCode;
        customer.City = dto.City;
        customer.State = dto.State;
        customer.Country = dto.Country;
    }
}
