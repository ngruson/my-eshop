using Ardalis.Result;
using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.GetCatalogItem;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItem;

internal class GetCatalogItemQueryHandler(
    ILogger<GetCatalogItemQueryHandler> logger,
    ICatalogApi catalogApi)
        : IRequestHandler<GetCatalogItemQuery, Result<CatalogItemViewModel>>
{
    private readonly ILogger<GetCatalogItemQueryHandler> logger = logger;
    private readonly ICatalogApi catalogApi = catalogApi;

    public async Task<Result<CatalogItemViewModel>> Handle(GetCatalogItemQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving catalog item {ObjectId} from API...", request.ObjectId);

            CatalogItemDto catalogItem = await this.catalogApi.GetCatalogItem(request.ObjectId);

            this.logger.LogInformation("Retrieved catalog item {ObjectId} from API", request.ObjectId);

            return catalogItem
                 .MapToCatalogItemViewModel();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog item.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
