using eShop.Catalog.Contracts.AssessStockItemsForOrder;

namespace eShop.Workflow.API.Activities;

public record ConfirmStockActivityInput(Guid OrderId, Guid UserId, ConfirmedOrderStockItem[] ConfirmedOrderStockItems);
