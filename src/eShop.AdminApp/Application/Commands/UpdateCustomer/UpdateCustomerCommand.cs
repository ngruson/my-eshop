using Ardalis.Result;
using eShop.Customer.Contracts.UpdateCustomer;
using MediatR;

namespace eShop.AdminApp.Application.Commands.UpdateCustomer;

internal class UpdateCustomerCommand(UpdateCustomerDto dto) : IRequest<Result>
{
    public UpdateCustomerDto Dto { get; } = dto;
}
