using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemsByTypeAndBrand;

internal record GetCatalogItemsByTypeAndBrandQuery(Guid CatalogType, Guid? CatalogBrand, int PageSize = 10, int PageIndex = 0)
    : IRequest<Result<PaginatedItems<CatalogItemDto>>>;
