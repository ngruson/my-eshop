using Ardalis.Result;

namespace eShop.Customer.API.Application.Commands.DeleteCustomer;

internal record DeleteCustomerCommand(string FirstName, string LastName) : IRequest<Result>
{
}
