using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.GetCatalogItems;

public record GetCatalogItemsQuery : IRequest<Result<CatalogItemViewModel[]>>;
