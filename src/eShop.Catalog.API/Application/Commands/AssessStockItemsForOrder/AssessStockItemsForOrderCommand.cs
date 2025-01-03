using Ardalis.Result;
using eShop.Catalog.Contracts.AssessStockItemsForOrder;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.AssessStockItemsForOrder;

internal record AssessStockItemsForOrderCommand(AssessStockItemsForOrderRequestDto Dto) : IRequest<Result<AssessStockItemsForOrderResponseDto>>;
