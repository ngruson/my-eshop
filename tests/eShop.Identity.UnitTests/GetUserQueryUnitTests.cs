using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Identity.API.Api.Queries.GetUser;
using eShop.Identity.API.Models;
using eShop.Identity.Contracts.GetUser;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Identity.UnitTests;

public class GetUserQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccess_WhenUserExists(
        [Frozen, Substitute] UserManager<ApplicationUser> userManager,
        GetUserQueryHandler sut,
        GetUserQuery query,
        ApplicationUser user)
    {
        // Arrange

        userManager.FindByNameAsync(query.UserName)
            .Returns(user);

        // Act

        Result<UserDto> result = await sut.Handle(query, default);

        // Assert

        Assert.True(result.IsSuccess);

        await userManager.Received().FindByNameAsync(query.UserName);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFound_WhenUserDoesNotExist(
        [Frozen, Substitute] UserManager<ApplicationUser> userManager,
        GetUserQueryHandler sut,
        GetUserQuery query)
    {
        // Arrange

        // Act

        Result<UserDto> result = await sut.Handle(query, default);

        // Assert

        Assert.True(result.IsNotFound());

        await userManager.Received().FindByNameAsync(query.UserName);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnError_WhenExceptionIsThrown(
        [Frozen, Substitute] UserManager<ApplicationUser> userManager,
        GetUserQueryHandler sut,
        GetUserQuery query)
    {
        // Arrange

        userManager.FindByNameAsync(query.UserName)
            .ThrowsAsync<Exception>();

        // Act

        Result<UserDto> result = await sut.Handle(query, default);

        // Assert

        Assert.True(result.IsError());

        await userManager.Received().FindByNameAsync(query.UserName);
    }
}
