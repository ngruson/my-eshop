using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Commands.DeleteCatalogItem;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Commands;

public class DeleteCatalogItemCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogItemIsSoftDeleted(
        DeleteCatalogItemCommand command,
        CatalogItem catalogItem,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        DeleteCatalogItemCommandHandler sut)
    {
        // Arrange

        repository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);        

        // Assert

        Assert.True(result.IsSuccess);
        Assert.True(catalogItem.IsDeleted);
        Assert.NotNull(catalogItem.DeletedAtUtc);
        await repository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await repository.Received().UpdateAsync(catalogItem, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundWhenCatalogItemDoesNotExist(
        DeleteCatalogItemCommand command,
        CatalogItem catalogItem,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        DeleteCatalogItemCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await repository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await repository.DidNotReceive().UpdateAsync(catalogItem, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        DeleteCatalogItemCommand command,
        CatalogItem catalogItem,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        DeleteCatalogItemCommandHandler sut)
    {
        // Arrange

        repository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
        await repository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await repository.DidNotReceive().UpdateAsync(catalogItem, default);
    }
}
