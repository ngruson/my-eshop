using Ardalis.Result;
using eShop.Catalog.Contracts.UpdateCatalogItem;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.UpdateCatalogItem;

internal record UpdateCatalogItemCommand(Guid ObjectId, CatalogItemDto Dto) : IRequest<Result>;
