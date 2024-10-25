using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogTypes;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetAllCatalogTypes;

internal record GetAllCatalogTypesQuery : IRequest<Result<CatalogTypeDto[]>>;
