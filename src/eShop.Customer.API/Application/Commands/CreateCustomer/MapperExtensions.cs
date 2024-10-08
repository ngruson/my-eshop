using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Domain.AggregatesModel.CustomerAggregate;

namespace eShop.Customer.API.Application.Commands.CreateCustomer;

internal static class MapperExtensions
{
    public static Domain.AggregatesModel.CustomerAggregate.Customer MapFromDto(this CreateCustomerDto dto)
    {
        return new Domain.AggregatesModel.CustomerAggregate.Customer(
            Guid.NewGuid(),
            dto.UserName,
            dto.FirstName,
            dto.LastName,
            dto.Street,
            dto.City,
            dto.State,
            dto.Country,
            dto.ZipCode,
            dto.CardNumber,
            dto.SecurityNumber,
            dto.Expiration,
            dto.CardHolderName,
            dto.CardType is not null ? CardType.FromName(dto.CardType) : null);
    }
}
