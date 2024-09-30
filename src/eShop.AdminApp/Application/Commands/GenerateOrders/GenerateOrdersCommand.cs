using Ardalis.Result;
using eShop.AdminApp.Application.Services;
using MediatR;

namespace eShop.AdminApp.Application.Commands.GenerateOrders;

internal record GenerateOrdersCommand(int OrdersToCreate, ProgressService<(int, string)> ProgressService) : IRequest<Result>;
