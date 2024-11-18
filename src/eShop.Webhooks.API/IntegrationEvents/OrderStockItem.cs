namespace eShop.Webhooks.API.IntegrationEvents;

public record OrderStockItem(Guid ProductId, int Units);
