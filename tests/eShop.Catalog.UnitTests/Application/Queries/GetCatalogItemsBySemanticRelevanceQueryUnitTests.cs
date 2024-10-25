using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByName;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsBySemanticRelevance;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Services;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetCatalogItemsBySemanticRelevanceQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogItemsExistAndCatalogAiIsDisabled(
        GetCatalogItemsBySemanticRelevanceQuery query,
        PaginatedItems<CatalogItemDto> paginatedItems,
        [Substitute, Frozen] IMediator mediator,
        GetCatalogItemsBySemanticRelevanceQueryHandler sut)
    {
        // Arrange

        mediator.Send(Arg.Any<GetCatalogItemsByNameQuery>(), default)
            .Returns(paginatedItems);

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        await mediator.Received().Send(Arg.Any<GetCatalogItemsByNameQuery>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogItemsExistAndCatalogAiIsEnabled(
        GetCatalogItemsBySemanticRelevanceQuery query,
        List<CatalogItem> catalogItems,
        [Substitute, Frozen] ICatalogAI catalogAI,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsBySemanticRelevanceQueryHandler sut)
    {
        // Arrange

        catalogAI.IsEnabled.Returns(true);
        repository.ListAsync(Arg.Any<GetCatalogItemsBySemanticRelevanceSpecification>(), default)
            .Returns(catalogItems);

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsBySemanticRelevanceSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogItemsExistAndCatalogAiIsEnabledAndLogDebugIsEnabled(
        GetCatalogItemsBySemanticRelevanceQuery query,
        List<CatalogItem> catalogItems,
        List<CatalogItemSemanticRelevance> catalogItemsSemanticRelevance,
        [Substitute, Frozen] ILogger<GetCatalogItemsByNameQueryHandler> logger,
        [Substitute, Frozen] ICatalogAI catalogAI,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsBySemanticRelevanceQueryHandler sut)
    {
        // Arrange

        logger.IsEnabled(LogLevel.Debug).Returns(true);
        catalogAI.IsEnabled.Returns(true);
        repository.ListAsync(Arg.Any<GetCatalogItemsBySemanticRelevanceSpecification>(), default)
            .Returns(catalogItems);
        repository.ListAsync(Arg.Any<GetCatalogItemsSemanticRelevanceSpecification>(), default)
            .Returns(catalogItemsSemanticRelevance);

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsSemanticRelevanceSpecification>(), default);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsBySemanticRelevanceSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenNoCatalogItemsExistAndCatalogAiIsEnabled(
        GetCatalogItemsBySemanticRelevanceQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        [Substitute, Frozen] ICatalogAI catalogAI,
        GetCatalogItemsBySemanticRelevanceQueryHandler sut)
    {
        // Arrange

        catalogAI.IsEnabled.Returns(true);

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsBySemanticRelevanceSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrownAndCatalogAiIsEnabled(
        GetCatalogItemsBySemanticRelevanceQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        [Substitute, Frozen] ICatalogAI catalogAI,
        GetCatalogItemsBySemanticRelevanceQueryHandler sut)
    {
        // Arrange

        catalogAI.IsEnabled.Returns(true);

        repository.ListAsync(Arg.Any<GetCatalogItemsBySemanticRelevanceSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsBySemanticRelevanceSpecification>(), default);
    }
}
