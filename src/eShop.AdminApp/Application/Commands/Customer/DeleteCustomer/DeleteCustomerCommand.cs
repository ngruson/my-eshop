using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Customer.DeleteCustomer;

internal record DeleteCustomerCommand(Guid ObjectId) : IRequest<Result>;
