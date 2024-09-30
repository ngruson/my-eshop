using Ardalis.Result;
using eShop.Ordering.Contracts.CreateOrder;
using MediatR;

namespace eShop.AdminApp.Application.Commands.CreateOrder;

internal record CreateOrderCommand(Guid RequestId, CreateOrderDto Dto) : IRequest<Result>;
