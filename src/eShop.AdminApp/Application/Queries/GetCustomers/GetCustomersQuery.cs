using Ardalis.Result;
using eShop.MasterData.Contracts;
using MediatR;

namespace eShop.AdminApp.Application.Queries.GetCustomers;

public class GetCustomersQuery() : IRequest<Result<List<CustomerViewModel>>>
{
}
