using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Identity.API.Api.Commands.CreateUser;
using eShop.Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Identity.UnitTests;

public class CreateUserUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task WhenCreateUser_ReturnSuccess(
        [Frozen, Substitute] UserManager<ApplicationUser> userManager,
        CreateUserCommandHandler sut,
        CreateUserCommand command)
    {
        // Arrange

        userManager.FindByNameAsync(command.Dto.UserName)
            .Returns((ApplicationUser?)null);

        userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Success);

        // Act

        Result result = await sut.Handle(command, default);

        // Assert

        Assert.True(result.IsSuccess);

        await userManager.Received().CreateAsync(
            Arg.Is<ApplicationUser>(u => u.UserName == command.Dto.UserName), Arg.Any<string>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenUserExists_ReturnSuccess(
        [Frozen, Substitute] UserManager<ApplicationUser> userManager,
        CreateUserCommandHandler sut,
        CreateUserCommand command,
        ApplicationUser applicationUser)
    {
        // Arrange

        userManager.FindByNameAsync(command.Dto.UserName)
            .Returns(applicationUser);

        // Act

        Result result = await sut.Handle(command, default);

        // Assert

        Assert.True(result.IsSuccess);

        await userManager.DidNotReceive().CreateAsync(
            Arg.Is<ApplicationUser>(u => u.UserName == command.Dto.UserName), Arg.Any<string>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenCreateUserFails_ReturnError(
        [Frozen, Substitute] UserManager<ApplicationUser> userManager,
        CreateUserCommandHandler sut,
        CreateUserCommand command,
        IdentityError identityError)
    {
        // Arrange

        userManager.FindByNameAsync(command.Dto.UserName)
            .Returns((ApplicationUser?)null);

        userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(identityError));

        // Act

        Result result = await sut.Handle(command, default);

        // Assert

        Assert.True(result.IsError());

        await userManager.Received().CreateAsync(
            Arg.Is<ApplicationUser>(u => u.UserName == command.Dto.UserName), Arg.Any<string>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenExceptionIsThrown_ReturnError(
        [Frozen, Substitute] UserManager<ApplicationUser> userManager,
        CreateUserCommandHandler sut,
        CreateUserCommand command)
    {
        // Arrange

        userManager.FindByNameAsync(command.Dto.UserName)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, default);

        // Assert

        Assert.True(result.IsError());

        await userManager.DidNotReceive().CreateAsync(
            Arg.Is<ApplicationUser>(u => u.UserName == command.Dto.UserName), Arg.Any<string>());
    }
}
