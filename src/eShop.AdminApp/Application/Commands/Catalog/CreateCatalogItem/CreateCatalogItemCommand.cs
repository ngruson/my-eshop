using Ardalis.Result;
using eShop.Catalog.Contracts.CreateCatalogItem;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Catalog.CreateCatalogItem;

internal record CreateCatalogItemCommand(CreateCatalogItemDto Dto) : IRequest<Result>;
