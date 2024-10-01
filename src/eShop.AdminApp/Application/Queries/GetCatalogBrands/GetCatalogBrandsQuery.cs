using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.GetCatalogBrands;

public record GetCatalogBrandsQuery : IRequest<Result<CatalogBrandViewModel[]>>;
