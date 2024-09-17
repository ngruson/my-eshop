using Ardalis.Result;
using eShop.Customer.Contracts.UpdateCustomer;

namespace eShop.Customer.API.Application.Commands.UpdateCustomer;

internal class UpdateCustomerCommand(UpdateCustomerDto dto) : IRequest<Result>
{
    public UpdateCustomerDto Dto { get; } = dto;
}
