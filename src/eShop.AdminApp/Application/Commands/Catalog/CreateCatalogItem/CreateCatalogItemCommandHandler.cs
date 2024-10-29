using Ardalis.Result;
using eShop.Catalog.Contracts;
using eShop.ServiceInvocation.CatalogService;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Catalog.CreateCatalogItem;

internal class CreateCatalogItemCommandHandler(
    ILogger<CreateCatalogItemCommandHandler> logger,
    ICatalogService catalogService)
        : IRequestHandler<CreateCatalogItemCommand, Result>
{
    private readonly ILogger<CreateCatalogItemCommandHandler> logger = logger;
    private readonly ICatalogService catalogService = catalogService;

    public async Task<Result> Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Creating catalog item...");

            await this.catalogService.CreateCatalogItem(request.Dto);

            this.logger.LogInformation("Catalog item created");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to create catalog item";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
