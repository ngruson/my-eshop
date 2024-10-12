using Ardalis.Result;
using eShop.Customer.Contracts.GetCustomers;

namespace eShop.Customer.API.Application.Queries.GetCustomers;

internal record GetCustomersQuery(bool IncludeDeleted) : IRequest<Result<List<CustomerDto>>>;
