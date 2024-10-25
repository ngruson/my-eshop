using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetAllCatalogBrands;

internal class GetAllCatalogBrandsQueryHandler(
    ILogger<GetAllCatalogBrandsQueryHandler> logger,
    IRepository<CatalogBrand> repository) : IRequestHandler<GetAllCatalogBrandsQuery, Result<CatalogBrandDto[]>>
{
    private readonly ILogger<GetAllCatalogBrandsQueryHandler> logger = logger;
    private readonly IRepository<CatalogBrand> repository = repository;

    public async Task<Result<CatalogBrandDto[]>> Handle(GetAllCatalogBrandsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting catalog brands");

            List<CatalogBrand> catalogBrands = await this.repository.ListAsync(
                new GetAllCatalogBrandsSpecification(),
                cancellationToken);

            var foundResult = Ardalis.GuardClauses.Guard.Against.CatalogBrandsNullOrEmpty(catalogBrands, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved {Count} catalog brands.", catalogBrands.Count);

            return catalogBrands
                .MapToCatalogBrandDtoList();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog brands.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
