using Ardalis.Result;
using eShop.Customer.Contracts.CreateCustomer;

namespace eShop.Customer.API.Application.Commands.CreateCustomer;

internal class CreateCustomerCommand(CreateCustomerDto dto) : IRequest<Result>
{
    public CreateCustomerDto Dto { get; } = dto;
}
