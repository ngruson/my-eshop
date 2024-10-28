using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemsBySemanticRelevance;

internal record GetCatalogItemsBySemanticRelevanceQuery(string Text, int PageSize = 10, int PageIndex = 0) : IRequest<Result<PaginatedItems<CatalogItemDto>>>;
