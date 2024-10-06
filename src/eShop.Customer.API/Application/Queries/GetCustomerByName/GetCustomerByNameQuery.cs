using Ardalis.Result;
using eShop.Customer.Contracts.GetCustomer;

namespace eShop.Customer.API.Application.Queries.GetCustomerByName;

public record GetCustomerByNameQuery(string Name) : IRequest<Result<CustomerDto>>;
