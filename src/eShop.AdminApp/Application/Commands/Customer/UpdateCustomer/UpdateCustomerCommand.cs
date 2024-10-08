using Ardalis.Result;
using eShop.Customer.Contracts.UpdateCustomerGeneralInfo;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Customer.UpdateCustomer;

internal record UpdateCustomerCommand(Guid ObjectId, UpdateCustomerDto Dto) : IRequest<Result>;
