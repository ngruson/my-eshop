using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogItem;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemByObjectId;

internal record GetCatalogItemByObjectIdQuery(Guid ObjectId) : IRequest<Result<CatalogItemDto>>;
