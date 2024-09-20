using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.GetCustomers;

public class GetCustomersQuery : IRequest<Result<List<CustomerViewModel>>>
{
}
