using Ardalis.Result;
using eShop.Ordering.Contracts.GetCardTypes;

namespace eShop.Ordering.API.Application.Queries.GetCardTypes;

internal record GetCardTypesQuery : IRequest<Result<CardTypeDto[]>>;
