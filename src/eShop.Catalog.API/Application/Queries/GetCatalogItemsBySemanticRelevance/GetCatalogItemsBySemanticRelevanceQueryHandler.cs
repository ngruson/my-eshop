using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByName;
using eShop.Catalog.API.Services;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using MediatR;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemsBySemanticRelevance;

internal class GetCatalogItemsBySemanticRelevanceQueryHandler(
    ILogger<GetCatalogItemsByNameQueryHandler> logger,
    IRepository<CatalogItem> repository,
    IMediator mediator,
    ICatalogAI catalogAI) : IRequestHandler<GetCatalogItemsBySemanticRelevanceQuery, Result<PaginatedItems<CatalogItemDto>>>
{
    private readonly ILogger<GetCatalogItemsByNameQueryHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;
    private readonly IMediator mediator = mediator;
    private readonly ICatalogAI catalogAI = catalogAI;

    public async Task<Result<PaginatedItems<CatalogItemDto>>> Handle(GetCatalogItemsBySemanticRelevanceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting catalog items by semantic relevance");

            if (!this.catalogAI.IsEnabled)
            {
                return await this.mediator.Send(new GetCatalogItemsByNameQuery(request.Text, request.PageSize, request.PageIndex),
                    cancellationToken);
            }

            // Create an embedding for the input search
            Vector? vector = await this.catalogAI.GetEmbeddingAsync(request.Text);

            // Get the total number of items
            int totalItems = await this.repository.CountAsync(cancellationToken);

            // Get the next page of items, ordered by most similar (smallest distance) to the input search
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                List<CatalogItemSemanticRelevance> itemsByDistance = await this.repository.ListAsync(
                    new GetCatalogItemsSemanticRelevanceSpecification(vector!, request.PageSize, request.PageIndex),
                    cancellationToken);

                this.logger.LogDebug("Results from {text}: {results}", request.Text,
                    string.Join(", ", itemsByDistance.Select(c => $"{c.Name} => {c.CosineDistance}")));
            }

            List<CatalogItem> catalogItems = await this.repository.ListAsync(
                    new GetCatalogItemsBySemanticRelevanceSpecification(vector!, request.PageSize, request.PageIndex),
                    cancellationToken);

            Result foundResult = Ardalis.GuardClauses.Guard.Against.CatalogItemsNullOrEmpty(catalogItems, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved {Count} catalog items for text '{Text}' with page size {PageSize} and page index {PageIndex}.",
                catalogItems.Count, request.Text, request.PageSize, request.PageIndex);

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
