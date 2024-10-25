using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;

public record GetCatalogItemsQuery(bool IncludeDeleted) : IRequest<Result<CatalogItemViewModel[]>>;
