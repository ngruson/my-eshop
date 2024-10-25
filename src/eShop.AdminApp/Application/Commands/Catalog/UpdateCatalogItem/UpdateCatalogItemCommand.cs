using Ardalis.Result;
using eShop.Catalog.Contracts.UpdateCatalogItem;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Catalog.UpdateCatalogItem;

internal record UpdateCatalogItemCommand(Guid ObjectId, CatalogItemDto Dto) : IRequest<Result>;
