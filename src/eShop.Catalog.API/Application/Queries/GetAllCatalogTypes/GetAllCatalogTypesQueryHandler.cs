using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetAllCatalogTypes;

internal class GetAllCatalogTypesQueryHandler(
    ILogger<GetAllCatalogTypesQueryHandler> logger,
    IRepository<CatalogType> repository) : IRequestHandler<GetAllCatalogTypesQuery, Result<CatalogTypeDto[]>>
{
    private readonly ILogger<GetAllCatalogTypesQueryHandler> logger = logger;
    private readonly IRepository<CatalogType> repository = repository;

    public async Task<Result<CatalogTypeDto[]>> Handle(GetAllCatalogTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting catalog types");

            List<CatalogType> catalogTypes = await this.repository.ListAsync(
                new GetAllCatalogTypesSpecification(),
                cancellationToken);

            Result foundResult = Ardalis.GuardClauses.Guard.Against.CatalogTypesNullOrEmpty(catalogTypes, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved {Count} catalog types.", catalogTypes.Count);

            return catalogTypes
                .MapToCatalogTypeDtoList();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog types.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
