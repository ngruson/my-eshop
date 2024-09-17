using Ardalis.Result;
using eShop.Customer.Contracts.GetCustomer;

namespace eShop.Customer.API.Application.Queries.GetCustomer;

internal record GetCustomerQuery(string FirstName, string LastName) : IRequest<Result<CustomerDto>>
{
}
