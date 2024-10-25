using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogItems;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemsByObjectIds;

internal record GetCatalogItemsByObjectIdsQuery(Guid[] ObjectIds) : IRequest<Result<CatalogItemDto[]>>;
