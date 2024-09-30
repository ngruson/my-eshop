using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Identity.API.Api.Queries.GetUsers;
using eShop.Identity.API.Models;
using eShop.Identity.Contracts.GetUsers;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using NSubstitute;

namespace eShop.Identity.UnitTests;

public class GetUsersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccess_WhenUserExists(
        [Frozen, Substitute] UserManager<ApplicationUser> userManager,
        GetUsersQueryHandler sut,
        GetUsersQuery query,
        List<ApplicationUser> users)
    {
        // Arrange

        var mockUsers = users.BuildMock();

        userManager.Users
            .Returns(mockUsers);

        // Act

        Result<List<UserDto>> result = await sut.Handle(query, default);

        // Assert

        Assert.True(result.IsSuccess);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnError_WhenExceptionIsThrown(
        GetUsersQueryHandler sut,
        GetUsersQuery query)
    {
        // Arrange

        // Act

        Result<List<UserDto>> result = await sut.Handle(query, default);

        // Assert

        Assert.True(result.IsError());
    }
}
