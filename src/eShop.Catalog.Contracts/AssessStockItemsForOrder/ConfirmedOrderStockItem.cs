namespace eShop.Catalog.Contracts.AssessStockItemsForOrder;

public record ConfirmedOrderStockItem(Guid ProductId, bool HasStock);
