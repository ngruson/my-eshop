using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Commands.Catalog.CreateCatalogItem;
using eShop.Catalog.Contracts;
using eShop.ServiceInvocation.CatalogService;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class CreateCatalogItemCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        CreateCatalogItemCommand command,
        [Substitute, Frozen] ICatalogService catalogService,
        CreateCatalogItemCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogService.Received().CreateCatalogItem(command.Dto);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        CreateCatalogItemCommand command,
        [Substitute, Frozen] ICatalogService catalogService,
        CreateCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogService.CreateCatalogItem(command.Dto)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogService.Received().CreateCatalogItem(command.Dto);
    }
}
