using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Client;
using eShop.Catalog.Contracts.AssessStockItemsForOrder;
using eShop.Catalog.Contracts.CreateCatalogItem;
using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.ServiceInvocation.Auth;
using eShop.ServiceInvocation.CatalogApiClient;
using eShop.Shared.Data;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class CatalogApiClientUnitTests
{
    public class AssessStockItemsForOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task assess_stock_items(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            AssessStockItemsForOrderRequestDto requestDto,
            AssessStockItemsForOrderResponseDto responseDto,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Post,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>(),
                requestDto)
                    .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<AssessStockItemsForOrderResponseDto>(httpRequestMessage)
                .Returns(responseDto);

            // Act

            AssessStockItemsForOrderResponseDto actual = await sut.AssessStockItemsForOrder(requestDto);

            // Assert

            Assert.Equivalent(actual, responseDto);
        }
    }

    public class GetCatalogItem
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogItem(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            Guid objectId,
            Catalog.Contracts.GetCatalogItem.CatalogItemDto catalogItem,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
                    .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<Catalog.Contracts.GetCatalogItem.CatalogItemDto>(httpRequestMessage)
                .Returns(catalogItem);

            // Act

            CatalogItemViewModel actual = await sut.GetCatalogItem(objectId);

            // Assert

            Assert.Equivalent(actual, catalogItem);
        }
    }

    public class GetCatalogItems
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalog_items(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            Catalog.Contracts.GetCatalogItems.CatalogItemDto[] catalogItems,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<Catalog.Contracts.GetCatalogItems.CatalogItemDto[]>(httpRequestMessage)
                .Returns(catalogItems);

            // Act

            Catalog.Contracts.GetCatalogItems.CatalogItemDto[] actual = await sut.GetCatalogItems();

            // Assert
            Assert.Equivalent(actual, catalogItems);
        }
    }

    public class GetPaginatedCatalogItems
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalog_items_given_catalogType(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            Guid catalogType,
            Guid catalogBrand,
            PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> catalogItems,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(httpRequestMessage)
                .Returns(catalogItems);

            // Act

            PaginatedItems<CatalogItemViewModel> actual = await sut.GetPaginatedCatalogItems(
                catalogType,
                catalogBrand,
                10,
                0);

            // Assert
            Assert.Equivalent(actual, catalogItems);
            await daprClient.Received().InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(httpRequestMessage);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_catalogItems_given_catalogBrand(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            Guid catalogBrand,
            PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> catalogItems,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(httpRequestMessage)
                .Returns(catalogItems);

            // Act

            PaginatedItems<CatalogItemViewModel> actual = await sut.GetPaginatedCatalogItems(
                null,
                catalogBrand,
                10,
                0);

            // Assert
            Assert.Equivalent(actual, catalogItems);
            await daprClient.Received().InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(httpRequestMessage);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_catalog_items_given_no_catalogType_and_no_catalogBrand(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> catalogItems,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(httpRequestMessage)
                .Returns(catalogItems);

            // Act

            PaginatedItems<CatalogItemViewModel> actual = await sut.GetPaginatedCatalogItems(
                null,
                null,
                10,
                0);

            // Assert
            Assert.Equivalent(actual, catalogItems);
            await daprClient.Received().InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(httpRequestMessage);
        }
    }

    public class GetCatalogItemsByIds
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogItems(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            Guid[] ids,
            Catalog.Contracts.GetCatalogItems.CatalogItemDto[] catalogItems,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<Catalog.Contracts.GetCatalogItems.CatalogItemDto[]>(httpRequestMessage)
                .Returns(catalogItems);

            // Act

            CatalogItemViewModel[] actual = await sut.GetCatalogItems(ids);

            // Assert

            Assert.Equivalent(actual, catalogItems);
            await daprClient.Received().InvokeMethodAsync<Catalog.Contracts.GetCatalogItems.CatalogItemDto[]>(httpRequestMessage);
        }
    }

    public class GetPaginatedCatalogItemsWithSemanticRelevance
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogItems(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            string text,
            PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> catalogItems,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(httpRequestMessage)
                .Returns(catalogItems);

            // Act

            PaginatedItems<CatalogItemViewModel> actual = await sut.GetPaginatedCatalogItemsWithSemanticRelevance(
                text,
                10,
                0);

            // Assert

            Assert.Equivalent(actual, catalogItems);
            await daprClient.Received().InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(httpRequestMessage);
        }
    }

    public class GetBrands
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogBrands(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            CatalogBrandDto[] catalogBrands,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<CatalogBrandDto[]>(httpRequestMessage)
                .Returns(catalogBrands);

            // Act

            CatalogBrandDto[] actual = await sut.GetBrands();

            // Assert

            Assert.Equivalent(actual, catalogBrands);
        }
    }

    public class GetCatalogTypes
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogTypes(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            CatalogTypeDto[] catalogTypes,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<CatalogTypeDto[]>(httpRequestMessage)
                .Returns(catalogTypes);

            // Act

            CatalogTypeDto[] actual = await sut.GetTypes();

            // Assert

            Assert.Equivalent(actual, catalogTypes);
        }
    }

    public class CreateCatalogItem
    {
        [Theory, AutoNSubstituteData]
        public async Task create_catalog_item(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            CreateCatalogItemDto dto,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Post,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>(),
                dto)
            .Returns(httpRequestMessage);

            // Act

            await sut.CreateCatalogItem(dto);

            // Assert

            await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
        }
    }

    public class UpdateCatalogItem
    {
        [Theory, AutoNSubstituteData]
        public async Task update_catalogItem(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            Guid objectId,
            Catalog.Contracts.UpdateCatalogItem.CatalogItemDto dto,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Put,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>(),
                dto)
            .Returns(httpRequestMessage);

            // Act

            await sut.UpdateCatalogItem(objectId, dto);

            // Assert

            await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
        }
    }

    public class DeleteCatalogItem
    {
        [Theory, AutoNSubstituteData]
        public async Task delete_catalogItem(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CatalogApiClient.Dapr.CatalogApiClient sut,
            Guid objectId,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Delete,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            // Act

            await sut.DeleteCatalogItem(objectId);

            // Assert

            await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
        }
    }
}
