using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemsByObjectIds;

internal class GetCatalogItemsByObjectIdsQueryHandler(
    ILogger<GetCatalogItemsByObjectIdsQueryHandler> logger,
    IRepository<CatalogItem> repository) : IRequestHandler<GetCatalogItemsByObjectIdsQuery, Result<CatalogItemDto[]>>
{
    private readonly ILogger<GetCatalogItemsByObjectIdsQueryHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;

    public async Task<Result<CatalogItemDto[]>> Handle(GetCatalogItemsByObjectIdsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting catalog items by object ids {ObjectIds}.",
                request.ObjectIds);

            List<CatalogItem> catalogItems = await this.repository
                .ListAsync(new GetCatalogItemsByIdsSpecification(request.ObjectIds),
                    cancellationToken);

            Result foundResult = Ardalis.GuardClauses.Guard.Against.CatalogItemsNullOrEmpty(catalogItems, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved {Count} catalog items by object ids {ObjectIds}.",
                catalogItems.Count, request.ObjectIds);

            return catalogItems.MapToCatalogItemDtoList();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog items.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
