using Ardalis.Result;
using eShop.Catalog.Contracts.GetCatalogBrands;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetAllCatalogBrands;

internal record GetAllCatalogBrandsQuery : IRequest<Result<CatalogBrandDto[]>>;
