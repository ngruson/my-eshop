using Ardalis.Result;
using eShop.Catalog.API.Application.Commands.CreateCatalogItem;
using eShop.Catalog.API.Specifications;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.AssessStockItemsForOrder;

internal class AssessStockItemsForOrderCommandHandler(
    ILogger<CreateCatalogItemCommandHandler> logger,
    IRepository<CatalogItem> repository,
    IIntegrationEventService integrationEventService)
        : IRequestHandler<AssessStockItemsForOrderCommand, Result>
{
    private readonly ILogger<CreateCatalogItemCommandHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;
    private readonly IIntegrationEventService integrationEventService = integrationEventService;

    public async Task<Result> Handle(AssessStockItemsForOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Assessing stock items for order...");

            List<ConfirmedOrderStockItem> confirmedOrderStockItems = [];

            foreach (OrderStockItem orderStockItem in request.OrderStockItems)
            {
                CatalogItem? catalogItem = await this.repository.SingleOrDefaultAsync(
                    new GetCatalogItemByObjectIdSpecification(orderStockItem.ProductId),
                    cancellationToken);

                bool hasStock = catalogItem!.AvailableStock >= orderStockItem.Units;
                ConfirmedOrderStockItem confirmedOrderStockItem = new(catalogItem.Id, hasStock);

                confirmedOrderStockItems.Add(confirmedOrderStockItem);
            }

            IntegrationEvent integrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
                ? new OrderStockRejectedIntegrationEvent(request.OrderId, confirmedOrderStockItems)
            : new OrderStockConfirmedIntegrationEvent(request.OrderId);

            await this.integrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);

            this.logger.LogInformation("Stock items assessed for order");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to assess stock items.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
