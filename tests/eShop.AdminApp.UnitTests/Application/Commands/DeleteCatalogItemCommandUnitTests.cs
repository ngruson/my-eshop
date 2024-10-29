using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Commands.Catalog.DeleteCatalogItem;
using eShop.Catalog.Contracts;
using eShop.ServiceInvocation.CatalogService;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class DeleteCatalogItemCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogItemDeleted(
        DeleteCatalogItemCommand command,
        [Substitute, Frozen] ICatalogService catalogService,
        DeleteCatalogItemCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogService.Received().DeleteCatalogItem(command.ObjectId);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        DeleteCatalogItemCommand command,
        [Substitute, Frozen] ICatalogService catalogService,
        DeleteCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogService.DeleteCatalogItem(command.ObjectId)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogService.Received().DeleteCatalogItem(command.ObjectId);
    }
}
