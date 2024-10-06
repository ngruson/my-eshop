using Ardalis.Result;
using eShop.Customer.Contracts.CreateCustomer;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Customer.CreateCustomer;

internal class CreateCustomerCommand(CreateCustomerDto dto) : IRequest<Result>
{
    public CreateCustomerDto Dto { get; } = dto;
}
