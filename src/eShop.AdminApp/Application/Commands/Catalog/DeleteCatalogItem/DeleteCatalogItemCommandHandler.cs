using Ardalis.Result;
using eShop.ServiceInvocation.CatalogApiClient;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Catalog.DeleteCatalogItem;

public class DeleteCatalogItemCommandHandler(
    ILogger<DeleteCatalogItemCommandHandler> logger,
    ICatalogApiClient catalogApiClient) : IRequestHandler<DeleteCatalogItemCommand, Result>
{
    private readonly ILogger<DeleteCatalogItemCommandHandler> logger = logger;
    private readonly ICatalogApiClient catalogApiClient = catalogApiClient;

    public async Task<Result> Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Deleting catalog item {ObjectId}...", request.ObjectId);

            await this.catalogApiClient.DeleteCatalogItem(request.ObjectId);

            this.logger.LogInformation("Catalog item deleted");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to delete catalog item";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
