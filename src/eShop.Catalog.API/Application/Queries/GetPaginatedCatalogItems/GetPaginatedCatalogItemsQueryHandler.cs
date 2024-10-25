using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetPaginatedCatalogItems;

internal class GetPaginatedCatalogItemsQueryHandler(
    ILogger<GetPaginatedCatalogItemsQueryHandler> logger,
    IRepository<CatalogItem> repository) : IRequestHandler<GetPaginatedCatalogItemsQuery, Result<PaginatedItems<CatalogItemDto>>>
{
    private readonly ILogger<GetPaginatedCatalogItemsQueryHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;

    public async Task<Result<PaginatedItems<CatalogItemDto>>> Handle(GetPaginatedCatalogItemsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting paginated catalog items");

            List<CatalogItem> catalogItems = await this.repository.ListAsync(
                new GetCatalogItemsForPageSpecification(request.PageSize, request.PageIndex),
                cancellationToken);

            int totalItems = await this.repository.CountAsync(
                new GetCatalogItemsSpecification(false),
                cancellationToken);

            var foundResult = Ardalis.GuardClauses.Guard.Against.CatalogItemsNullOrEmpty(catalogItems, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved {Count} catalog items with page size {PageSize} and page index {PageIndex}.",
                catalogItems.Count, request.PageSize, request.PageIndex);

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
