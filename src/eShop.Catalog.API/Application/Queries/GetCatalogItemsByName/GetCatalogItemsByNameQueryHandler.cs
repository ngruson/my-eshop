using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemsByName;

internal class GetCatalogItemsByNameQueryHandler(
    ILogger<GetCatalogItemsByNameQueryHandler> logger,
    IRepository<CatalogItem> repository) : IRequestHandler<GetCatalogItemsByNameQuery, Result<PaginatedItems<CatalogItemDto>>>
{
    private readonly ILogger<GetCatalogItemsByNameQueryHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;

    public async Task<Result<PaginatedItems<CatalogItemDto>>> Handle(GetCatalogItemsByNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting catalog items by name '{Name}' with page size {PageSize} and page index {PageIndex}.",
                request.Name, request.PageSize, request.PageIndex);

            int totalItems = await this.repository.CountAsync(
                new GetCatalogItemsStartingWithNameSpecification(request.Name),
                cancellationToken);

            List<CatalogItem> catalogItems = await this.repository.ListAsync(
                new GetCatalogItemsForPageStartingWithNameSpecification(request.Name, request.PageSize, request.PageIndex),
                cancellationToken);

            Result foundResult = Ardalis.GuardClauses.Guard.Against.CatalogItemsNullOrEmpty(catalogItems, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved {Count} catalog items for name '{Name}' with page size {PageSize} and page index {PageIndex}.",
                catalogItems.Count, request.Name, request.PageSize, request.PageIndex);

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
