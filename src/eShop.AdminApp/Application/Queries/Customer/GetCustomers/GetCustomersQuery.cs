using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomers;

public record GetCustomersQuery(bool IncludeDeleted) : IRequest<Result<List<CustomerViewModel>>>;
