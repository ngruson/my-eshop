using Ardalis.Result;
using eShop.Catalog.API.Application.GuardClauses;
using eShop.Catalog.API.Specifications;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemPictureByObjectId;

internal class GetCatalogItemPictureByObjectIdQueryHandler(
    ILogger<GetCatalogItemPictureByObjectIdQueryHandler> logger,
    IRepository<CatalogItem> repository,
    IWebHostEnvironment environment) : IRequestHandler<GetCatalogItemPictureByObjectIdQuery, Result<PictureDto>>
{
    private readonly ILogger<GetCatalogItemPictureByObjectIdQueryHandler> logger = logger;
    private readonly IRepository<CatalogItem> repository = repository;

    public async Task<Result<PictureDto>> Handle(GetCatalogItemPictureByObjectIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting catalog item picture by object id {ObjectId}.",
                request.ObjectId);

            CatalogItem? catalogItem = await this.repository
                .FirstOrDefaultAsync(new GetCatalogItemByObjectIdSpecification(request.ObjectId),
                    cancellationToken);

            var foundResult = Ardalis.GuardClauses.Guard.Against.CatalogItemNull(catalogItem, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved catalog item picture by object id {ObjectId}.", request.ObjectId);

            string path = GetFullPath(environment.ContentRootPath, catalogItem!.PictureFileName!);
            string? imageFileExtension = Path.GetExtension(catalogItem.PictureFileName);
            string mimeType = GetImageMimeTypeFromImageFileExtension(imageFileExtension!);
            DateTime lastModified = File.GetLastWriteTimeUtc(path);

            return new PictureDto(path, mimeType, lastModified);
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve catalog item.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }

    private static string GetFullPath(string contentRootPath, string pictureFileName) =>
        Path.Combine(contentRootPath, "Pics", pictureFileName);

    private static string GetImageMimeTypeFromImageFileExtension(string extension) => extension switch
    {
        ".png" => "image/png",
        ".gif" => "image/gif",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".bmp" => "image/bmp",
        ".tiff" => "image/tiff",
        ".wmf" => "image/wmf",
        ".jp2" => "image/jp2",
        ".svg" => "image/svg+xml",
        ".webp" => "image/webp",
        _ => "application/octet-stream",
    };
}
