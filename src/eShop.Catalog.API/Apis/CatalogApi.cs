using Ardalis.Result.AspNetCore;
using eShop.Catalog.API.Application.Commands.CreateCatalogItem;
using eShop.Catalog.API.Application.Commands.DeleteCatalogItem;
using eShop.Catalog.API.Application.Commands.UpdateCatalogItem;
using eShop.Catalog.API.Application.Queries.GetAllCatalogBrands;
using eShop.Catalog.API.Application.Queries.GetAllCatalogItems;
using eShop.Catalog.API.Application.Queries.GetAllCatalogTypes;
using eShop.Catalog.API.Application.Queries.GetCatalogItemByObjectId;
using eShop.Catalog.API.Application.Queries.GetCatalogItemPictureByObjectId;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByBrand;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByTypeAndBrand;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByName;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByObjectIds;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsBySemanticRelevance;
using eShop.Catalog.API.Application.Queries.GetPaginatedCatalogItems;
using eShop.Catalog.Contracts.CreateCatalogItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result;

namespace eShop.Catalog.API.APIs;

public static class CatalogApi
{
    public static IEndpointRouteBuilder MapCatalogApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog").HasApiVersion(1.0);

        // Routes for querying catalog items.
        api.MapGet("/items", async (bool? includeDeleted, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetAllCatalogItemsQuery(includeDeleted ?? false)))
                .ToMinimalApiResult());
        api.MapGet("/items/page", async ([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetPaginatedCatalogItemsQuery(pageSize, pageIndex)))
                .ToMinimalApiResult());
        api.MapGet("/items/by", async (Guid[] ids, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCatalogItemsByObjectIdsQuery(ids)))
                .ToMinimalApiResult());
        api.MapGet("/items/{objectId:guid}", async (Guid objectId, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCatalogItemByObjectIdQuery(objectId)))
                .ToMinimalApiResult());
        api.MapGet("/items/by/{name:minLength(1)}", async (string name, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCatalogItemsByNameQuery(name, pageSize, pageIndex)))
                .ToMinimalApiResult());
        api.MapGet("/items/{objectId}/pic", async (Guid objectId, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCatalogItemPictureByObjectIdQuery(objectId)))
                .ToPhysicalFileResult());

        // Routes for resolving catalog items using AI.
        api.MapGet("/items/withSemanticRelevance/{text:minLength(1)}", async (string text, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCatalogItemsBySemanticRelevanceQuery(text, pageSize, pageIndex)))
                .ToMinimalApiResult());

        // Routes for resolving catalog items by type and brand.
        api.MapGet("/items/type/{catalogType}/brand/{catalogBrand?}", async (Guid catalogType, Guid? catalogBrand, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCatalogItemsByTypeAndBrandQuery(catalogType, catalogBrand, pageSize, pageIndex)))
                .ToMinimalApiResult());
        api.MapGet("/items/type/all/brand/{catalogBrand}", async (Guid catalogBrand, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCatalogItemsByBrandQuery(catalogBrand, pageSize, pageIndex)))
                .ToMinimalApiResult());
        api.MapGet("/catalogTypes", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetAllCatalogTypesQuery()))
                .ToMinimalApiResult());
        api.MapGet("/catalogBrands", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetAllCatalogBrandsQuery()))
                .ToMinimalApiResult());

        // Routes for modifying catalog items.
        api.MapPut("/items/{objectId:guid}", async ([FromRoute] Guid objectId, [FromBody] Contracts.UpdateCatalogItem.CatalogItemDto dto, [FromServices] IMediator mediator) =>
            (await mediator.Send(new UpdateCatalogItemCommand(objectId, dto)))
                .ToMinimalApiResult());
        api.MapPost("/items", async ([FromBody] CreateCatalogItemDto dto, [FromServices] IMediator mediator) =>
            (await mediator.Send(new CreateCatalogItemCommand(dto)))
                .ToMinimalApiResult());
        api.MapDelete("/items/{objectId}", async (Guid objectId, [FromServices] IMediator mediator) =>
            (await mediator.Send(new DeleteCatalogItemCommand(objectId)))
                .ToMinimalApiResult());

        return api;
    }

    private static Microsoft.AspNetCore.Http.IResult ToPhysicalFileResult(this Result<PictureDto> result)
    {
        if (result.Status == ResultStatus.Ok)
        {
            PictureDto pictureDto = result.Value;
            return TypedResults.PhysicalFile(pictureDto.Path, pictureDto.MimeType, lastModified: pictureDto.LastModified);
        }
        else
        {
            return result.ToMinimalApiResult();
        }
    }
}
