using Ardalis.Result;
using MediatR;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemPictureByObjectId;

internal record GetCatalogItemPictureByObjectIdQuery(Guid ObjectId) : IRequest<Result<PictureDto>>;
