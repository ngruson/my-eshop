using Ardalis.Result;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.DeleteCatalogItem;

internal record DeleteCatalogItemCommand(Guid ObjectId) : IRequest<Result>;
