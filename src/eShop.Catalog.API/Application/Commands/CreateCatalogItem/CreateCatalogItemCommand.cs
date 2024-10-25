using Ardalis.Result;
using eShop.Catalog.Contracts.CreateCatalogItem;
using MediatR;

namespace eShop.Catalog.API.Application.Commands.CreateCatalogItem;

internal record CreateCatalogItemCommand(CreateCatalogItemDto Dto) : IRequest<Result<CatalogItemDto>>;
