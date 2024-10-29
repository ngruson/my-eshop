using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.ServiceInvocation.CatalogService;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogBrands;

public class GetCatalogBrandsQueryHandler(
    ILogger<GetCatalogBrandsQueryHandler> logger,
    ICatalogService catalogService) : IRequestHandler<GetCatalogBrandsQuery, Result<CatalogBrandViewModel[]>>
{
    private readonly ILogger<GetCatalogBrandsQueryHandler> logger = logger;
    private readonly ICatalogService catalogService = catalogService;

    public async Task<Result<CatalogBrandViewModel[]>> Handle(GetCatalogBrandsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving catalog brands.");

            CatalogBrandDto[] catalogTypes = await this.catalogService.GetBrands();

            this.logger.LogInformation("Catalog brands retrieved: {Count}", catalogTypes.Length);

            return catalogTypes.MapToCatalogBrandViewModelArray();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog brands.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
