using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetAllCatalogItems;

internal class GetAllCatalogItemsQueryHandler(
    ILogger<GetAllCatalogItemsQueryHandler> logger,
    IRepository<CatalogItem> repository) : IRequestHandler<GetAllCatalogItemsQuery, Result<CatalogItemDto[]>>
{
    private readonly ILogger<GetAllCatalogItemsQueryHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;

    public async Task<Result<CatalogItemDto[]>> Handle(GetAllCatalogItemsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting catalog items");

            List<CatalogItem> catalogItems =
                await this.repository.ListAsync(new GetCatalogItemsSpecification(request.IncludeDeleted), cancellationToken);

            var foundResult = Ardalis.GuardClauses.Guard.Against.CatalogItemsNullOrEmpty(catalogItems, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved {Count} catalog items.", catalogItems.Count);

            return catalogItems
                .MapToCatalogItemDtoList();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog items.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
