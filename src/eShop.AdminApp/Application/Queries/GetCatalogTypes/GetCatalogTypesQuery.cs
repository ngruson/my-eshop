using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.GetCatalogTypes;

public record GetCatalogTypesQuery : IRequest<Result<CatalogTypeViewModel[]>>;
