using eShop.Customer.Contracts.UpdateCustomer;

namespace eShop.Customer.API.Application.Commands.UpdateCustomer;
internal static class MapperExtensions
{
    public static void MapFromDto(this UpdateCustomerDto dto, Domain.AggregatesModel.CustomerAggregate.Customer customer)
    {
        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.CardNumber = dto.CardNumber;
        customer.SecurityNumber = dto.SecurityNumber;
        customer.Expiration = dto.Expiration;
        customer.CardHolderName = dto.CardHolderName;
        customer.CardType = dto.CardType;
        customer.Street = dto.Street;
        customer.City = dto.City;
        customer.State = dto.State;
        customer.Country = dto.Country;
        customer.ZipCode = dto.ZipCode;
    }
}
