using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.DeleteCatalogItem;

internal class DeleteCatalogItemCommandHandler(
    ILogger<DeleteCatalogItemCommandHandler> logger,
    IRepository<CatalogItem> repository) : IRequestHandler<DeleteCatalogItemCommand, Result>
{
    private readonly ILogger<DeleteCatalogItemCommandHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;

    public async Task<Result> Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Soft deleting catalog item");

            CatalogItem? catalogItem =
                await this.repository.FirstOrDefaultAsync(
                    new GetCatalogItemByObjectIdSpecification(request.ObjectId),
                    cancellationToken);

            Result foundResult = Guard.Against.CatalogItemNull(catalogItem, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            catalogItem!.IsDeleted = true;
            catalogItem.DeletedAtUtc = DateTime.UtcNow;

            await this.repository.UpdateAsync(catalogItem, cancellationToken);

            this.logger.LogInformation("Soft deleted catalog item");

            return Result.NoContent();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to soft delete catalog item.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
