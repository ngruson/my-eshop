using Ardalis.Result;
using eShop.Catalog.API.Services;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.CreateCatalogItem;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.CreateCatalogItem;

internal class CreateCatalogItemCommandHandler(
    ILogger<CreateCatalogItemCommandHandler> logger,
    IRepository<CatalogItem> repository,
    IRepository<CatalogType> catalogTypeRepository,
    IRepository<CatalogBrand> catalogBrandRepository,
    ICatalogAI catalogAI) : IRequestHandler<CreateCatalogItemCommand, Result<CatalogItemDto>>
{
    private readonly ILogger<CreateCatalogItemCommandHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;
    private readonly IRepository<CatalogType> catalogTypeRepository = catalogTypeRepository;
    private readonly IRepository<CatalogBrand> catalogBrandRepository = catalogBrandRepository;

    public async Task<Result<CatalogItemDto>> Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Creating catalog item...");

            CatalogType? catalogType = await this.catalogTypeRepository.FirstOrDefaultAsync(
                new GetCatalogTypeByObjectIdSpecification(request.Dto.CatalogType),
                cancellationToken);

            if (catalogType == null)
            {
                return Result.NotFound("Catalog type not found");
            }

            CatalogBrand? catalogBrand = await this.catalogBrandRepository.FirstOrDefaultAsync(
                new GetCatalogBrandByObjectIdSpecification(request.Dto.CatalogBrand),
                cancellationToken);

            if (catalogBrand == null)
            {
                return Result.NotFound("Catalog brand not found");
            }

            CatalogItem catalogItem = request.Dto.MapFromDto(catalogType, catalogBrand);
            catalogItem.Embedding = await catalogAI.GetEmbeddingAsync(catalogItem);

            await this.repository.AddAsync(catalogItem, cancellationToken);

            this.logger.LogInformation("Catalog item created");

            return catalogItem.MapToDto();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to create catalog item.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
