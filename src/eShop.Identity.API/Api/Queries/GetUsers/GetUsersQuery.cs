using Ardalis.Result;
using eShop.Identity.Contracts.GetUsers;
using MediatR;

namespace eShop.Identity.API.Api.Queries.GetUsers;

public record GetUsersQuery : IRequest<Result<List<UserDto>>>;
