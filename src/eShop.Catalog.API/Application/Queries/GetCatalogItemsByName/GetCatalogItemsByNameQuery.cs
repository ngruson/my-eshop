using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogItems;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemsByName;

internal record GetCatalogItemsByNameQuery(string Name, int PageSize = 10, int PageIndex = 0) : IRequest<Result<PaginatedItems<CatalogItemDto>>>;
