using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItem;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemByObjectId;

internal class GetCatalogItemByObjectIdQueryHandler(
    ILogger<GetCatalogItemByObjectIdQueryHandler> logger,
    IRepository<CatalogItem> repository) : IRequestHandler<GetCatalogItemByObjectIdQuery, Result<CatalogItemDto>>
{
    private readonly ILogger<GetCatalogItemByObjectIdQueryHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;

    public async Task<Result<CatalogItemDto>> Handle(GetCatalogItemByObjectIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting catalog item by object id {ObjectId}.",
                request.ObjectId);

            CatalogItem? catalogItem = await this.repository
                .FirstOrDefaultAsync(new GetCatalogItemByObjectIdSpecification(request.ObjectId),
                    cancellationToken);

            var foundResult = Ardalis.GuardClauses.Guard.Against.CatalogItemNull(catalogItem, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved catalog item by object id {ObjectId}.", request.ObjectId);

            return catalogItem!.MapToCatalogItemDto();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog item.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
