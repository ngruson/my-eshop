using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogBrands;

public record GetCatalogBrandsQuery : IRequest<Result<CatalogBrandViewModel[]>>;
