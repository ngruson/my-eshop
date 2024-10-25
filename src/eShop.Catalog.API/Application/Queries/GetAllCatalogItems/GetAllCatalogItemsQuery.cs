using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogItems;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetAllCatalogItems;

internal record GetAllCatalogItemsQuery(bool IncludeDeleted) : IRequest<Result<CatalogItemDto[]>>;
