using Ardalis.Result;
using eShop.Identity.Contracts.GetUser;
using MediatR;

namespace eShop.Identity.API.Api.Queries.GetUser;

internal record GetUserQuery(string UserName) : IRequest<Result<UserDto>>;
