using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.UpdateCustomer;

namespace eShop.Customer.API.Application.Commands.CreateCustomer;

internal static class MapperExtensions
{
    public static Domain.AggregatesModel.CustomerAggregate.Customer MapFromDto(this CreateCustomerDto dto)
    {
        return new Domain.AggregatesModel.CustomerAggregate.Customer(
            dto.FirstName,
            dto.LastName,
            dto.CardNumber,
            dto.SecurityNumber,
            dto.Expiration,
            dto.CardHolderName,
            dto.CardType,
            dto.Street,
            dto.City,
            dto.State,
            dto.Country,
            dto.ZipCode);
    }
}
