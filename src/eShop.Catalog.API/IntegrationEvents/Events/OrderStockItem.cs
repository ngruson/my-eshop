namespace eShop.Catalog.API.IntegrationEvents.Events;

public record OrderStockItem(Guid ProductId, int Units);
