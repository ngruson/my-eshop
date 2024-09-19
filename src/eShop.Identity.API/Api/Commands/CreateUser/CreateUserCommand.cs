using Ardalis.Result;
using eShop.Identity.Contracts.CreateUser;
using MediatR;

namespace eShop.Identity.API.Api.Commands.CreateUser;

public class CreateUserCommand(CreateUserDto Dto) : IRequest<Result>
{
    public CreateUserDto Dto { get; } = Dto;
}
