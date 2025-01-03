using eShop.Catalog.Contracts.AssessStockItemsForOrder;

namespace eShop.Workflow.API.Activities;

public record AssessStockItemsActivityInput(Guid OrderId, Guid UserId, OrderStockItem[] OrderStockItems);
