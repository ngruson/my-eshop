using Ardalis.Result;
using eShop.Catalog.Contracts;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Catalog.UpdateCatalogItem;

internal class UpdateCatalogItemCommandHandler(
    ILogger<UpdateCatalogItemCommandHandler> logger,
    ICatalogApi catalogApi)
        : IRequestHandler<UpdateCatalogItemCommand, Result>
{
    private readonly ILogger<UpdateCatalogItemCommandHandler> logger = logger;
    private readonly ICatalogApi catalogApi = catalogApi;

    public async Task<Result> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Updating catalog item...");

            await this.catalogApi.UpdateCatalogItem(request.ObjectId, request.Dto);

            this.logger.LogInformation("Catalog item updated");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to update catalog item.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
