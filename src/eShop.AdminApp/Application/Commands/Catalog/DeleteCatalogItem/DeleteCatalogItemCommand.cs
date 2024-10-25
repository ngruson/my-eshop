using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Catalog.DeleteCatalogItem;

public record DeleteCatalogItemCommand(Guid ObjectId) : IRequest<Result>;
