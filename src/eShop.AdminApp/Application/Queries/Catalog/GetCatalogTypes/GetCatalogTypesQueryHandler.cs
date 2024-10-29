using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.ServiceInvocation.CatalogService;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogTypes;

public class GetCatalogTypesQueryHandler(
    ILogger<GetCatalogTypesQueryHandler> logger,
    ICatalogService catalogService) : IRequestHandler<GetCatalogTypesQuery, Result<CatalogTypeViewModel[]>>
{
    private readonly ILogger<GetCatalogTypesQueryHandler> logger = logger;
    private readonly ICatalogService catalogService = catalogService;

    public async Task<Result<CatalogTypeViewModel[]>> Handle(GetCatalogTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving catalog types.");

            CatalogTypeDto[] catalogTypes = await this.catalogService.GetTypes();

            this.logger.LogInformation("Catalog types retrieved: {Count}", catalogTypes.Length);

            return catalogTypes.MapToCatalogTypeViewModelArray();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog types.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
