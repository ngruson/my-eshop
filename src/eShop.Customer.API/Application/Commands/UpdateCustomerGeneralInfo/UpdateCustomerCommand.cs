using Ardalis.Result;
using eShop.Customer.Contracts.UpdateCustomerGeneralInfo;

namespace eShop.Customer.API.Application.Commands.UpdateCustomerGeneralInfo;

internal record UpdateCustomerCommand(Guid ObjectId, UpdateCustomerDto Dto) : IRequest<Result>;
