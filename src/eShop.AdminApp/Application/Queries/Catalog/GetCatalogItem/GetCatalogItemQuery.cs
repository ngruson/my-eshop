using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItem;

internal record GetCatalogItemQuery(Guid ObjectId) : IRequest<Result<CatalogItemViewModel>>;
