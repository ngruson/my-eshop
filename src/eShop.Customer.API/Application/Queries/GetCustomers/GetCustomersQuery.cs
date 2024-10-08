using Ardalis.Result;
using eShop.Customer.Contracts.GetCustomers;

namespace eShop.Customer.API.Application.Queries.GetCustomers;

internal class GetCustomersQuery : IRequest<Result<List<CustomerDto>>>
{
}
