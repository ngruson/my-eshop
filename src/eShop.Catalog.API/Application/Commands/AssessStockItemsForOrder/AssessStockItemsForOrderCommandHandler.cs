using Ardalis.Result;
using eShop.Catalog.API.Application.Commands.CreateCatalogItem;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.AssessStockItemsForOrder;
using eShop.Shared.Data;
using eShop.Shared.Features;
using eShop.Shared.IntegrationEvents;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.AssessStockItemsForOrder;

internal class AssessStockItemsForOrderCommandHandler(
    ILogger<CreateCatalogItemCommandHandler> logger,
    IRepository<CatalogItem> repository,
    IIntegrationEventService integrationEventService,
    IOptions<FeaturesConfiguration> features)
        : IRequestHandler<AssessStockItemsForOrderCommand, Result<AssessStockItemsForOrderResponseDto>>
{
    private readonly ILogger<CreateCatalogItemCommandHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;
    private readonly IIntegrationEventService integrationEventService = integrationEventService;

    public async Task<Result<AssessStockItemsForOrderResponseDto>> Handle(AssessStockItemsForOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Assessing stock items for order...");

            List<ConfirmedOrderStockItem> confirmedOrderStockItems = [];

            foreach (Contracts.AssessStockItemsForOrder.OrderStockItem orderStockItem in request.Dto.OrderStockItems)
            {
                CatalogItem? catalogItem = await this.repository.SingleOrDefaultAsync(
                    new GetCatalogItemByObjectIdSpecification(orderStockItem.ProductId),
                    cancellationToken);

                bool hasStock = catalogItem!.AvailableStock >= orderStockItem.Units;
                ConfirmedOrderStockItem confirmedOrderStockItem = new(catalogItem.ObjectId, hasStock);
                confirmedOrderStockItems.Add(confirmedOrderStockItem);
            }

            if (features.Value.Workflow.Enabled is false)
            {
                IntegrationEvent integrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
                    ? new OrderStockRejectedIntegrationEvent(request.Dto.OrderId,
                        [.. confirmedOrderStockItems])
                    : new OrderStockConfirmedIntegrationEvent(request.Dto.OrderId);

                await this.integrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
            }

            this.logger.LogInformation("Stock items assessed for order");

            AssessStockItemsForOrderResponseDto responseDto = new([.. confirmedOrderStockItems]);

            return responseDto;
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to assess stock items.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
