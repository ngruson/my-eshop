using Ardalis.Result;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.AssessStockItemsForOrder;

internal record AssessStockItemsForOrderCommand(Guid OrderId, IEnumerable<OrderStockItem> OrderStockItems) : IRequest<Result>;
