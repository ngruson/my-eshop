using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemsByBrand;

internal class GetCatalogItemsByBrandQueryHandler(
    ILogger<GetCatalogItemsByBrandQueryHandler> logger,
    IRepository<CatalogItem> repository) : IRequestHandler<GetCatalogItemsByBrandQuery, Result<PaginatedItems<CatalogItemDto>>>
{
    private readonly ILogger<GetCatalogItemsByBrandQueryHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;

    public async Task<Result<PaginatedItems<CatalogItemDto>>> Handle(GetCatalogItemsByBrandQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting catalog items by brand '{Brand}' with page size {PageSize} and page index {PageIndex}.",
                request.CatalogBrand, request.PageSize, request.PageIndex);

            int totalItems = await this.repository.CountAsync(
                new GetCatalogItemsByBrandSpecification(request.CatalogBrand),
                cancellationToken);

            List<CatalogItem> catalogItems = await this.repository
                .ListAsync(new GetCatalogItemsForPageByBrandSpecification(
                    request.CatalogBrand, request.PageSize, request.PageIndex),
                    cancellationToken);

            var foundResult = Ardalis.GuardClauses.Guard.Against.CatalogItemsNullOrEmpty(catalogItems, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved {Count} catalog items by brand '{Brand}' with page size {PageSize} and page index {PageIndex}.",
                catalogItems.Count, request.CatalogBrand, request.PageSize, request.PageIndex);

            return new PaginatedItems<CatalogItemDto>(
                request.PageIndex,
                request.PageSize,
                totalItems,
                catalogItems.MapToCatalogItemDtoList());
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog items.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
