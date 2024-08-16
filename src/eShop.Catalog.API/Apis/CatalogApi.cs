using Ardalis.Specification;
using Catalog.API.Specifications;
using eShop.Catalog.API.Extensions;
using eShop.Shared.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace eShop.Catalog.API.APIs;

public static class CatalogApi
{
    public static IEndpointRouteBuilder MapCatalogApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog").HasApiVersion(1.0);

        // Routes for querying catalog items.
        api.MapGet("/items", GetAllItems);
        api.MapGet("/items/by", GetItemsByIds);
        api.MapGet("/items/{id:int}", GetItemById);
        api.MapGet("/items/by/{name:minlength(1)}", GetItemsByName);
        api.MapGet("/items/{catalogItemId:int}/pic", GetItemPictureById);

        // Routes for resolving catalog items using AI.
        api.MapGet("/items/withsemanticrelevance/{text:minlength(1)}", GetItemsBySemanticRelevance);

        // Routes for resolving catalog items by type and brand.
        api.MapGet("/items/type/{typeId}/brand/{brandId?}", GetItemsByBrandAndTypeId);
        api.MapGet("/items/type/all/brand/{brandId:int?}", GetItemsByBrandId);
        api.MapGet("/catalogtypes", GetAllCatalogTypes);
        api.MapGet("/catalogbrands", GetAllCatalogBrands);

        // Routes for modifying catalog items.
        api.MapPut("/items", UpdateItem);
        api.MapPost("/items", CreateItem);
        api.MapDelete("/items/{id:int}", DeleteItemById);

        return api;
    }

    public static async Task<Results<Ok<PaginatedItems<CatalogItem>>, BadRequest<string>>> GetAllItems(
        [AsParameters] PaginationRequest paginationRequest,
        [FromServices] IRepository<CatalogItem> repository)
    {
        int pageSize = paginationRequest.PageSize;
        int pageIndex = paginationRequest.PageIndex;

        int totalItems = await repository.CountAsync();

        List<CatalogItem> itemsOnPage = await repository.ListAsync(new GetCatalogItemsForPageSpecification(pageSize, pageIndex));

        return TypedResults.Ok(new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<List<CatalogItem>>> GetItemsByIds(
        [FromServices] IRepository<CatalogItem> repository,
        int[] ids)
    {
        List<CatalogItem> items = await repository.ListAsync(new GetCatalogItemsByIdsSpecification(ids));
        return TypedResults.Ok(items);
    }

    public static async Task<Results<Ok<CatalogItem>, NotFound, BadRequest<string>>> GetItemById(
        [FromServices] IRepository<CatalogItem> repository,
        int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        CatalogItem? item = await repository.SingleOrDefaultAsync(new GetCatalogItemByIdSpecification(id));

        if (item == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(item);
    }

    public static async Task<Ok<PaginatedItems<CatalogItem>>> GetItemsByName(
        [AsParameters] PaginationRequest paginationRequest,
        [FromServices] IRepository<CatalogItem> repository,
        string name)
    {
        int pageSize = paginationRequest.PageSize;
        int pageIndex = paginationRequest.PageIndex;

        int totalItems = await repository.CountAsync(new GetCatalogItemsStartingWithNameSpecification(name));

        List<CatalogItem> itemsOnPage = await repository.ListAsync(new GetCatalogItemsForPageStartingWithNameSpecification(pageSize, pageIndex, name));

        return TypedResults.Ok(new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Results<NotFound, PhysicalFileHttpResult>> GetItemPictureById(
        IRepository<CatalogItem> repository, IWebHostEnvironment environment, int catalogItemId)
    {
        CatalogItem? item = await repository.GetByIdAsync(catalogItemId);

        if (item is null)
        {
            return TypedResults.NotFound();
        }

        string path = GetFullPath(environment.ContentRootPath, item.PictureFileName!);

        string? imageFileExtension = Path.GetExtension(item.PictureFileName);
        string mimeType = GetImageMimeTypeFromImageFileExtension(imageFileExtension!);
        DateTime lastModified = File.GetLastWriteTimeUtc(path);

        return TypedResults.PhysicalFile(path, mimeType, lastModified: lastModified);
    }

    public static async Task<Results<BadRequest<string>, RedirectToRouteHttpResult, Ok<PaginatedItems<CatalogItem>>>> GetItemsBySemanticRelevance(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        [FromServices] IRepository<CatalogItem> repository,
        string text)
    {
        int pageSize = paginationRequest.PageSize;
        int pageIndex = paginationRequest.PageIndex;

        if (!services.CatalogAI.IsEnabled)
        {
            return await GetItemsByName(paginationRequest, repository, text);
        }

        // Create an embedding for the input search
        Vector? vector = await services.CatalogAI.GetEmbeddingAsync(text);

        // Get the total number of items
        int totalItems = await repository.CountAsync();

        // Get the next page of items, ordered by most similar (smallest distance) to the input search
        List<CatalogItem> itemsOnPage;
        if (services.Logger.IsEnabled(LogLevel.Debug))
        {
            var itemsWithDistance = (await repository.ListAsync())
                .OrderBy(c => c.Embedding!.CosineDistance(vector!))
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            services.Logger.LogDebug("Results from {text}: {results}", text, string.Join(", ", itemsWithDistance.Select(c => $"{c.Name} => {c.Embedding!.CosineDistance(vector!)}")));

            itemsOnPage = [.. itemsWithDistance];
        }
        else
        {
            itemsOnPage = (await repository.ListAsync())
                .OrderBy(c => c.Embedding!.CosineDistance(vector!))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();
        }

        return TypedResults.Ok(new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<PaginatedItems<CatalogItem>>> GetItemsByBrandAndTypeId(
        [AsParameters] PaginationRequest paginationRequest,
        [FromServices] IRepository<CatalogItem> repository,
        int typeId,
        int? brandId)
    {
        int pageSize = paginationRequest.PageSize;
        int pageIndex = paginationRequest.PageIndex;

        int totalItems = await repository.CountAsync(
            new GetCatalogItemsByBrandAndTypeSpecification(typeId, brandId));

        List<CatalogItem> itemsOnPage = await repository
            .ListAsync(new GetCatalogItemsForPageByBrandAndTypeSpecification(typeId, brandId, pageSize, pageIndex));

        return TypedResults.Ok(new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<PaginatedItems<CatalogItem>>> GetItemsByBrandId(
        [AsParameters] PaginationRequest paginationRequest,
        [FromServices] IRepository<CatalogItem> repository,
        int? brandId)
    {
        int pageSize = paginationRequest.PageSize;
        int pageIndex = paginationRequest.PageIndex;

        int totalItems = await repository.CountAsync(new GetCatalogItemsByBrandIdSpecification(brandId));

        List<CatalogItem> itemsOnPage = await repository
            .ListAsync(new GetCatalogItemsForPageByBrandIdSpecification(brandId, pageSize, pageIndex));

        return TypedResults.Ok(new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<List<CatalogType>> GetAllCatalogTypes(
        [FromServices] IRepository<CatalogType> repository
    )
    {
        return await repository.ListAsync(new GetAllCatalogTypesSpecification());
    }

    public static async Task<List<CatalogBrand>> GetAllCatalogBrands(
        [FromServices] IRepository<CatalogBrand> repository
    )
    {
        return await repository.ListAsync(new GetAllCatalogBrandsSpecification());
    }

    public static async Task<Results<Created, NotFound<string>>> UpdateItem(
        [FromServices] IRepository<CatalogItem> repository,
        [AsParameters] CatalogServices services,
        CatalogItem productToUpdate)
    {
        CatalogItem? catalogItem = await repository.GetByIdAsync(productToUpdate.Id);

        if (catalogItem == null)
        {
            return TypedResults.NotFound($"Item with id {productToUpdate.Id} not found.");
        }

        // Update current product
        bool priceModified = catalogItem.Price != productToUpdate.Price;
        decimal priceOriginalValue = catalogItem.Price;

        catalogItem.Map(productToUpdate);
        catalogItem.Embedding = await services.CatalogAI.GetEmbeddingAsync(catalogItem);

        if (priceModified) // Save product's data and publish integration event through the Event Bus if price has changed
        {
            //Create Integration Event to be published through the Event Bus
            var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, productToUpdate.Price, priceOriginalValue);

            // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
            await services.EventService.SaveEventAndDbChangesAsync(
                repository,
                priceChangedEvent,
                async () => await repository.UpdateAsync(catalogItem),
                default);

            // Publish through the Event Bus and mark the saved event as published
            await services.EventService.PublishThroughEventBusAsync(priceChangedEvent, default);
        }
        else // Just save the updated product because the Product's Price hasn't changed.
        {
            await repository.UpdateAsync(catalogItem);
        }
        return TypedResults.Created($"/api/catalog/items/{productToUpdate.Id}");
    }

    public static async Task<Created> CreateItem(
        [FromServices] IRepository<CatalogItem> repository,
        [AsParameters] CatalogServices services,
        CatalogItem product)
    {
        var item = new CatalogItem
        {
            Id = product.Id,
            CatalogBrandId = product.CatalogBrandId,
            CatalogTypeId = product.CatalogTypeId,
            Description = product.Description,
            Name = product.Name,
            PictureFileName = product.PictureFileName,
            Price = product.Price,
            AvailableStock = product.AvailableStock,
            RestockThreshold = product.RestockThreshold,
            MaxStockThreshold = product.MaxStockThreshold
        };
        item.Embedding = await services.CatalogAI.GetEmbeddingAsync(item);

        await repository.AddAsync(item);

        return TypedResults.Created($"/api/catalog/items/{item.Id}");
    }

    public static async Task<Results<NoContent, NotFound>> DeleteItemById(
        [FromServices] IRepository<CatalogItem> repository,
        int id)
    {
        CatalogItem? item = await repository.GetByIdAsync(id);

        if (item is null)
        {
            return TypedResults.NotFound();
        }

        await repository.DeleteAsync(item);

        return TypedResults.NoContent();
    }

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

    public static string GetFullPath(string contentRootPath, string pictureFileName) =>
        Path.Combine(contentRootPath, "Pics", pictureFileName);
}
