using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.ServiceInvocation.CatalogService;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;

public class GetCatalogItemsQueryHandler(
    ILogger<GetCatalogItemsQueryHandler> logger,
    ICatalogService catalogService) : IRequestHandler<GetCatalogItemsQuery, Result<CatalogItemViewModel[]>>
{
    private readonly ILogger<GetCatalogItemsQueryHandler> logger = logger;
    private readonly ICatalogService catalogService = catalogService;

    public async Task<Result<CatalogItemViewModel[]>> Handle(GetCatalogItemsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving catalog items.");

            CatalogItemDto[] catalogItems = await this.catalogService.GetCatalogItems(request.IncludeDeleted);

            this.logger.LogInformation("Catalog items retrieved: {Count}", catalogItems.Length);

            return catalogItems.MapToCatalogItemViewModelArray();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog items.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
