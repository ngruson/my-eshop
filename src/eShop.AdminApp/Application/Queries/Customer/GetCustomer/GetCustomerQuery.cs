using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomer;

public record GetCustomerQuery(Guid ObjectId) : IRequest<Result<CustomerViewModel>>;
