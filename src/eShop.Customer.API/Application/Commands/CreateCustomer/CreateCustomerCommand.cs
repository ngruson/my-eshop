using Ardalis.Result;
using eShop.Customer.Contracts.CreateCustomer;

namespace eShop.Customer.API.Application.Commands.CreateCustomer;

internal record CreateCustomerCommand(CreateCustomerDto Dto) : IRequest<Result>;
