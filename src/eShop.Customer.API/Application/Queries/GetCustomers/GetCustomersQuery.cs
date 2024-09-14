using Ardalis.Result;
using eShop.Customer.Contracts.GetCustomers;
using MediatR;

namespace eShop.Customer.API.Application.Queries.GetCustomers;

internal class GetCustomersQuery : IRequest<Result<List<CustomerDto>>>
{
}
