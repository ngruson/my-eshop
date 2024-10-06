using Ardalis.Result;
using eShop.Customer.Contracts.GetCustomer;

namespace eShop.Customer.API.Application.Queries.GetCustomerByObjectId;

internal record GetCustomerByObjectIdQuery(Guid ObjectId) : IRequest<Result<CustomerDto>>
{
}
