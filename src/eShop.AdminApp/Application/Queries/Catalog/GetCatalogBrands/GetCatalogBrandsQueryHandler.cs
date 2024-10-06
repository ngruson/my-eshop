using Ardalis.Result;
using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.GetCatalogBrands;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogBrands;

public class GetCatalogBrandsQueryHandler(
    ILogger<GetCatalogBrandsQueryHandler> logger,
    ICatalogApi catalogApi) : IRequestHandler<GetCatalogBrandsQuery, Result<CatalogBrandViewModel[]>>
{
    private readonly ILogger<GetCatalogBrandsQueryHandler> logger = logger;
    private readonly ICatalogApi catalogApi = catalogApi;

    public async Task<Result<CatalogBrandViewModel[]>> Handle(GetCatalogBrandsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving catalog brands.");

            CatalogBrandDto[] catalogTypes = await this.catalogApi.GetCatalogBrands();

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
