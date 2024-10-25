using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Services;
using eShop.Catalog.API.Specifications;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.UpdateCatalogItem;

internal class UpdateCatalogItemCommandHandler(
    ILogger<UpdateCatalogItemCommandHandler> logger,
    IRepository<CatalogItem> catalogItemRepository,
    IRepository<CatalogType> catalogTypeRepository,
    IRepository<CatalogBrand> catalogBrandRepository,
    ICatalogAI catalogAI,
    IIntegrationEventService eventService) : IRequestHandler<UpdateCatalogItemCommand, Result>
{
    //private readonly ILogger<UpdateCatalogItemCommandHandler> logger = logger;
    //private readonly IRepository<CatalogItem> catalogItemRepository = catalogItemRepository;
    //private readonly IRepository<CatalogType> catalogTypeRepository = catalogTypeRepository;
    //private readonly IRepository<CatalogBrand> catalogBrandRepository = catalogBrandRepository;
    //private readonly ICatalogAI catalogAI = catalogAI;
    //private readonly IIntegrationEventService eventService = eventService;

    public async Task<Result> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Updating catalog item");

            CatalogItem? catalogItem = await catalogItemRepository.FirstOrDefaultAsync(
                new GetCatalogItemByObjectIdSpecification(request.ObjectId),
                cancellationToken);

            var foundResult = Guard.Against.CatalogItemNull(catalogItem, logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            CatalogType? catalogType = await this.GetCatalogType(request.Dto.CatalogType, cancellationToken);
            var catalogTypeFoundResult = Guard.Against.CatalogTypeNull(catalogType, logger);
            if (!catalogTypeFoundResult.IsSuccess)
            {
                return catalogTypeFoundResult;
            }

            CatalogBrand? catalogBrand = catalogBrand = await this.GetCatalogBrand(request.Dto.CatalogBrand, cancellationToken);
            var catalogBrandFoundResult = Guard.Against.CatalogBrandNull(catalogBrand, logger);
            if (!catalogBrandFoundResult.IsSuccess)
            {
                return catalogBrandFoundResult;
            }            

            // Update current product
            bool priceModified = catalogItem!.Price != request.Dto.Price;
            decimal priceOriginalValue = catalogItem.Price;

            request.Dto.MapFromDto(catalogItem!, catalogType!, catalogBrand!);            
            catalogItem.Embedding = await catalogAI.GetEmbeddingAsync(catalogItem);

            if (priceModified) // Save product's data and publish integration event through the Event Bus if price has changed
            {
                //Create Integration Event to be published through the Event Bus
                var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.ObjectId, request.Dto.Price, priceOriginalValue);

                // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
                await eventService.AddAndSaveEventAsync(priceChangedEvent, cancellationToken);
            }

            await catalogItemRepository.UpdateAsync(catalogItem, cancellationToken);

            logger.LogInformation("Catalog item updated");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to update catalog item.";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }

    private async Task<CatalogType?> GetCatalogType(Guid objectId, CancellationToken cancellationToken)
    {
        return await catalogTypeRepository.FirstOrDefaultAsync(
            new GetCatalogTypeByObjectIdSpecification(objectId),
            cancellationToken);
    }

    private async Task<CatalogBrand?> GetCatalogBrand(Guid objectId, CancellationToken cancellationToken)
    {
        return await catalogBrandRepository.FirstOrDefaultAsync(
            new GetCatalogBrandByObjectIdSpecification(objectId),
            cancellationToken);
    }
}
