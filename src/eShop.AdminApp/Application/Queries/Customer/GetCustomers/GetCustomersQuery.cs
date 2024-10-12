using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomers;

public record GetCustomersQuery(bool ShowDeleted) : IRequest<Result<List<CustomerViewModel>>>;
