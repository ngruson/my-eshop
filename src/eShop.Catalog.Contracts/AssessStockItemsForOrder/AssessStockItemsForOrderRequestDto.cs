namespace eShop.Catalog.Contracts.AssessStockItemsForOrder;

public record AssessStockItemsForOrderRequestDto(Guid OrderId, OrderStockItem[] OrderStockItems);
