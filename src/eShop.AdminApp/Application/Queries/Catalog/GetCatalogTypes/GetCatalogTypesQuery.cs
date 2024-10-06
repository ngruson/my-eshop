using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogTypes;

public record GetCatalogTypesQuery : IRequest<Result<CatalogTypeViewModel[]>>;
