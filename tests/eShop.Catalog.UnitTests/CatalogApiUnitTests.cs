using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Catalog.API.Specifications;
using eShop.Catalog.API.APIs;
using eShop.Catalog.API.IntegrationEvents.Events;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Services;
using eShop.Shared.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Pgvector;

namespace eShop.Catalog.API.UnitTests;
public class CatalogApiUnitTests
{
    public class MapCatalogApi
    {
        [Theory, AutoNSubstituteData]
        internal void success(
            IEndpointRouteBuilder routeBuilder)
        {
            // Arrange

            // Act

            routeBuilder = routeBuilder.MapCatalogApiV1();

            // Assert

            Assert.Single(routeBuilder.DataSources);
        }
    }

    public class  GetAllItems
    {
        [Theory, AutoNSubstituteData]
        internal async Task success(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            PaginationRequest paginationRequest,
            List<CatalogItem> catalogItems)
        {
            // Arrange

            repository.CountAsync().Returns(catalogItems.Count);

            repository.ListAsync(Arg.Any<GetCatalogItemsForPageSpecification>(), default)
                .Returns(catalogItems);

            // Act

            Results<Ok<PaginatedItems<CatalogItem>>, BadRequest<string>> results =
                await CatalogApi.GetAllItems(paginationRequest, repository);

            // Assert

            Assert.IsType<Ok<PaginatedItems<CatalogItem>>>(results.Result);
        }
    }

    public class GetItemsByIds
    {
        [Theory, AutoNSubstituteData]
        internal async Task success(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            List<CatalogItem> catalogItems
        )
        {
            // Arrange

            repository.ListAsync(Arg.Any<GetCatalogItemsByIdsSpecification>(), default)
                .Returns(catalogItems);

            // Act

            Ok<List<CatalogItem>> results = await CatalogApi.GetItemsByIds(repository, catalogItems.Select(_ => _.Id).ToArray());

            // Assert

            Assert.IsType<Ok<List<CatalogItem>>>(results);
        }
    }

    public class GetItemById
    {
        [Theory, AutoNSubstituteData]
        internal async Task with_valid_id_return_catalog_item(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogItem catalogItem
        )
        {
            // Arrange

            repository.SingleOrDefaultAsync(Arg.Any<GetCatalogItemByIdSpecification>())
                .Returns(catalogItem);

            // Act

            Results<Ok<CatalogItem>, NotFound, BadRequest<string>> result = await CatalogApi.GetItemById(repository, catalogItem.Id);

            // Assert

            Assert.IsType<Ok<CatalogItem>>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        internal async Task with_invalid_id_return_bad_request(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogItem catalogItem
        )
        {
            // Arrange

            catalogItem.Id = -1;

            // Act

            Results<Ok<CatalogItem>, NotFound, BadRequest<string>> result = await CatalogApi.GetItemById(repository, catalogItem.Id);

            // Assert

            Assert.IsType<BadRequest<string>>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_catalog_item_does_not_exist_return_not_found(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogItem catalogItem
        )
        {
            // Arrange

            // Act

            Results<Ok<CatalogItem>, NotFound, BadRequest<string>> result = await CatalogApi.GetItemById(repository, catalogItem.Id);

            // Assert

            Assert.IsType<NotFound>(result.Result);
        }
    }

    public class GetItemsByName
    {
        [Theory, AutoNSubstituteData]
        internal async Task with_valid_id_return_catalog_item(
            PaginationRequest paginationRequest,
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            List<CatalogItem> catalogItems,
            string name
        )
        {
            // Arrange

            repository.CountAsync(Arg.Any<GetCatalogItemsStartingWithNameSpecification>())
                .Returns(catalogItems.Count);
               

            repository.ListAsync(Arg.Any<GetCatalogItemsForPageStartingWithNameSpecification>())
                .Returns(catalogItems);

            // Act

            Ok<PaginatedItems<CatalogItem>> result = await CatalogApi.GetItemsByName(paginationRequest, repository, name);

            // Assert

            Assert.IsType<Ok<PaginatedItems<CatalogItem>>>(result);
        }
    }

    public class GetItemPictureById
    {
        [Theory, AutoNSubstituteData]
        internal async Task when_catalog_item_exists_return_catalog_item(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            IWebHostEnvironment env,
            CatalogItem catalogItem
        )
        {
            // Arrange

            repository.GetByIdAsync(catalogItem.Id)
                .Returns(catalogItem);

            // Act

            Results<NotFound, PhysicalFileHttpResult> result = await CatalogApi.GetItemPictureById(repository, env, catalogItem.Id);

            // Assert

            Assert.IsType<PhysicalFileHttpResult>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_catalog_item_does_not_exist_return_not_found(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            IWebHostEnvironment env,
            CatalogItem catalogItem
        )
        {
            // Arrange

            // Act

            Results<NotFound, PhysicalFileHttpResult> result = await CatalogApi.GetItemPictureById(repository, env, catalogItem.Id);

            // Assert

            Assert.IsType<NotFound>(result.Result);
        }
    }

    public class GetItemsBySemanticRelevance
    {
        [Theory, AutoNSubstituteData]
        internal async Task with_ai_enabled_return_catalog_items_by_relevance(
            PaginationRequest paginationRequest,
            [Substitute, Frozen] ICatalogAI catalogAI,
            CatalogServices services,
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            List<CatalogItem> catalogItems,
            string text,
            Vector vector
        )
        {
            // Arrange

            catalogAI.IsEnabled.Returns(true);
            catalogAI.GetEmbeddingAsync(text)
                .Returns(vector);

            repository.CountAsync()
                .Returns(catalogItems.Count);

            repository.ListAsync()
                .Returns(catalogItems);

            // Act

            Results<BadRequest<string>, RedirectToRouteHttpResult, Ok<PaginatedItems<CatalogItem>>> result =
                await CatalogApi.GetItemsBySemanticRelevance(paginationRequest, services, repository, text);

            // Assert

            Assert.IsType<Ok<PaginatedItems<CatalogItem>>>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        internal async Task with_ai_and_logging_enabled_return_catalog_items_by_relevance(
            PaginationRequest paginationRequest,
            [Substitute, Frozen] ICatalogAI catalogAI,
            CatalogServices services,
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            List<CatalogItem> catalogItems,
            string text,
            Vector vector
        )
        {
            // Arrange

            catalogAI.IsEnabled.Returns(true);
            catalogAI.GetEmbeddingAsync(text)
                .Returns(vector);
            services.Logger.IsEnabled(LogLevel.Debug)
                .Returns(true);

            repository.CountAsync()
                .Returns(catalogItems.Count);


            repository.ListAsync()
                .Returns(catalogItems);

            // Act

            Results<BadRequest<string>, RedirectToRouteHttpResult, Ok<PaginatedItems<CatalogItem>>> result =
                await CatalogApi.GetItemsBySemanticRelevance(paginationRequest, services, repository, text);

            // Assert

            Assert.IsType<Ok<PaginatedItems<CatalogItem>>>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        internal async Task with_ai_disabled_return_catalog_items_by_name(
            PaginationRequest paginationRequest,
            [Substitute, Frozen] ICatalogAI catalogAI,
            CatalogServices services,
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            List<CatalogItem> catalogItems,
            string text
        )
        {
            // Arrange

            repository.CountAsync(Arg.Any<GetCatalogItemsStartingWithNameSpecification>())
                .Returns(catalogItems.Count);

            repository.ListAsync(Arg.Any<GetCatalogItemsForPageStartingWithNameSpecification>())
                .Returns(catalogItems);

            // Act

            Results<BadRequest<string>, RedirectToRouteHttpResult, Ok<PaginatedItems<CatalogItem>>> result =
                await CatalogApi.GetItemsBySemanticRelevance(paginationRequest, services, repository, text);

            // Assert

            Assert.IsType<Ok<PaginatedItems<CatalogItem>>>(result.Result);

            await catalogAI.DidNotReceive().GetEmbeddingAsync(Arg.Any<string>());
        }
    }

    public class GetItemsByBrandAndTypeId
    {
        [Theory, AutoNSubstituteData]
        internal async Task return_catalog_items(
            PaginationRequest paginationRequest,
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            List<CatalogItem> catalogItems,
            int typeId,
            int? brandId
        )
        {
            // Arrange

            repository.CountAsync(Arg.Any<GetCatalogItemsByBrandAndTypeSpecification>())
                .Returns(catalogItems.Count);


            repository.ListAsync(Arg.Any<GetCatalogItemsForPageByBrandAndTypeSpecification>())
                .Returns(catalogItems);

            // Act

            Ok<PaginatedItems<CatalogItem>> result = await CatalogApi.GetItemsByBrandAndTypeId(paginationRequest, repository, typeId, brandId);

            // Assert

            Assert.IsType<Ok<PaginatedItems<CatalogItem>>>(result);
        }
    }

    public class GetItemsByBrandId
    {
        [Theory, AutoNSubstituteData]
        internal async Task return_catalog_items(
            PaginationRequest paginationRequest,
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            List<CatalogItem> catalogItems,
            int? brandId
        )
        {
            // Arrange

            repository.CountAsync(Arg.Any<GetCatalogItemsByBrandIdSpecification>())
                .Returns(catalogItems.Count);


            repository.ListAsync(Arg.Any<GetCatalogItemsForPageByBrandIdSpecification>())
                .Returns(catalogItems);

            // Act

            Ok<PaginatedItems<CatalogItem>> result = await CatalogApi.GetItemsByBrandId(paginationRequest, repository, brandId);

            // Assert

            Assert.IsType<Ok<PaginatedItems<CatalogItem>>>(result);
        }
    }

    public class UpdateItem
    {
        [Theory, AutoNSubstituteData]
        internal async Task when_price_has_not_changed_update_catalog_item(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogServices services,
            CatalogItem catalogItem
        )
        {
            // Arrange

            repository.GetByIdAsync(catalogItem.Id)
                .Returns(catalogItem);

            // Act

            Results<Created, NotFound<string>> result = await CatalogApi.UpdateItem(repository, services, catalogItem);

            // Assert

            Assert.IsType<Created>(result.Result);

            await repository.Received().UpdateAsync(catalogItem);
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_price_has_changed_update_catalog_item_with_integration_event(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogServices services,
            CatalogItem catalogItem,
            CatalogItem updatedCatalogItem
        )
        {
            // Arrange

            repository.GetByIdAsync(updatedCatalogItem.Id)
                .Returns(catalogItem);

            // Act

            Results<Created, NotFound<string>> result = await CatalogApi.UpdateItem(repository, services, updatedCatalogItem);

            // Assert

            Assert.IsType<Created>(result.Result);

            await services.EventService.Received().SaveEventAndDbChangesAsync(
                repository,
                Arg.Any<ProductPriceChangedIntegrationEvent>(),
                Arg.Any<Func<Task>>(),
                default);

            await services.EventService.Received().PublishThroughEventBusAsync(Arg.Any<ProductPriceChangedIntegrationEvent>(), default);
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_catalog_item_does_not_exist_return_not_found(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogServices services,
            CatalogItem updatedCatalogItem
        )
        {
            // Arrange

            // Act

            Results<Created, NotFound<string>> result = await CatalogApi.UpdateItem(repository, services, updatedCatalogItem);

            // Assert

            Assert.IsType<NotFound<string>>(result.Result);
        }
    }

    public class CreateItem
    {
        [Theory, AutoNSubstituteData]
        internal async Task when_price_has_not_changed_update_catalog_item(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogServices services,
            CatalogItem catalogItem
        )
        {
            // Arrange

            // Act

            Results<Created, NotFound<string>> result = await CatalogApi.CreateItem(repository, services, catalogItem);

            // Assert

            Assert.IsType<Created>(result.Result);

            await services.CatalogAI.Received().GetEmbeddingAsync(Arg.Any<CatalogItem>());

            await repository.Received().AddAsync(Arg.Any<CatalogItem>());
        }
    }

    public class DeleteItemById
    {
        [Theory, AutoNSubstituteData]
        internal async Task when_catalog_item_exists_and_is_deleted_return_no_content(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogItem catalogItem
        )
        {
            // Arrange

            repository.GetByIdAsync(catalogItem.Id)
                .Returns(catalogItem);

            // Act

            Results<NoContent, NotFound> result = await CatalogApi.DeleteItemById(repository, catalogItem.Id);

            // Assert

            Assert.IsType<NoContent>(result.Result);

            await repository.Received().DeleteAsync(Arg.Any<CatalogItem>());
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_catalog_item_does_not_exist_return_not_found(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogItem catalogItem
        )
        {
            // Arrange

            // Act

            Results<NoContent, NotFound> result = await CatalogApi.DeleteItemById(repository, catalogItem.Id);

            // Assert

            Assert.IsType<NotFound>(result.Result);

            await repository.DidNotReceive().DeleteAsync(Arg.Any<CatalogItem>());
        }
    }
}
