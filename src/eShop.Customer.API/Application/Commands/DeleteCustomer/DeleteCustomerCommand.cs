using Ardalis.Result;

namespace eShop.Customer.API.Application.Commands.DeleteCustomer;

internal record DeleteCustomerCommand(Guid ObjectId) : IRequest<Result>
{
}
