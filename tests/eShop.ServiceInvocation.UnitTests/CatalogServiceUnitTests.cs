using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.CreateCatalogItem;
using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.ServiceInvocation.CatalogService;
using eShop.Shared.Data;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests;

public class CatalogServiceUnitTests
{
    public class GetCatalogItem
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogItem(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            Guid objectId,
            Catalog.Contracts.GetCatalogItem.CatalogItemDto catalogItem)
        {
            // Arrange

            catalogApi.GetCatalogItem(objectId)
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
        public async Task return_catalogItems(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            Catalog.Contracts.GetCatalogItems.CatalogItemDto[] catalogItems)
        {
            // Arrange

            catalogApi.GetCatalogItems()
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
        public async Task return_catalogItems_given_catalogType(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            Guid catalogType,
            Guid catalogBrand,
            PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> catalogItems)
        {
            // Arrange

            catalogApi.GetPaginatedCatalogItemsByTypeAndBrand(catalogType, catalogBrand, 10, 0)
                .Returns(catalogItems);

            // Act

            PaginatedItems<CatalogItemViewModel> actual = await sut.GetPaginatedCatalogItems(
                catalogType,
                catalogBrand,
                10,
                0);

            // Assert
            Assert.Equivalent(actual, catalogItems);
            await catalogApi.Received().GetPaginatedCatalogItemsByTypeAndBrand(catalogType, catalogBrand, 10, 0);
            await catalogApi.DidNotReceive().GetPaginatedCatalogItemsByBrand(catalogBrand, 10, 0);
            await catalogApi.DidNotReceive().GetPaginatedCatalogItems(10, 0);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_catalogItems_given_catalogBrand(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            Guid catalogBrand,
            PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> catalogItems)
        {
            // Arrange

            catalogApi.GetPaginatedCatalogItemsByBrand(catalogBrand, 10, 0)
                .Returns(catalogItems);

            // Act

            PaginatedItems<CatalogItemViewModel> actual = await sut.GetPaginatedCatalogItems(
                null,
                catalogBrand,
                10,
                0);

            // Assert
            Assert.Equivalent(actual, catalogItems);
            await catalogApi.DidNotReceive().GetPaginatedCatalogItemsByTypeAndBrand(Arg.Any<Guid>(), catalogBrand, 10, 0);
            await catalogApi.Received().GetPaginatedCatalogItemsByBrand(catalogBrand, 10, 0);
            await catalogApi.DidNotReceive().GetPaginatedCatalogItems(10, 0);
        }

        [Theory, AutoNSubstituteData]
        public async Task return_catalogItems_given_no_catalogType_and_no_catalogBrand(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            Guid catalogBrand,
            PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> catalogItems)
        {
            // Arrange

            catalogApi.GetPaginatedCatalogItems(10, 0)
                .Returns(catalogItems);

            // Act

            PaginatedItems<CatalogItemViewModel> actual = await sut.GetPaginatedCatalogItems(
                null,
                null,
                10,
                0);

            // Assert
            Assert.Equivalent(actual, catalogItems);
            await catalogApi.DidNotReceive().GetPaginatedCatalogItemsByTypeAndBrand(Arg.Any<Guid>(), catalogBrand, 10, 0);
            await catalogApi.DidNotReceive().GetPaginatedCatalogItemsByBrand(catalogBrand, 10, 0);
            await catalogApi.Received().GetPaginatedCatalogItems(10, 0);
        }
    }

    public class GetCatalogItemsByIds
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogItems(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            Guid[] ids,
            Catalog.Contracts.GetCatalogItems.CatalogItemDto[] catalogItems)
        {
            // Arrange

            catalogApi.GetCatalogItemsByIds(ids)
                .Returns(catalogItems);

            // Act

            CatalogItemViewModel[] actual = await sut.GetCatalogItems(ids);

            // Assert

            Assert.Equivalent(actual, catalogItems);
            await catalogApi.Received().GetCatalogItemsByIds(ids);
        }
    }

    public class GetPaginatedCatalogItemsWithSemanticRelevance
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogItems(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            string text,
            PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> catalogItems)
        {
            // Arrange

            catalogApi.GetPaginatedCatalogItemsWithSemanticRelevance(text, 10, 0)
                .Returns(catalogItems);

            // Act

            PaginatedItems<CatalogItemViewModel> actual = await sut.GetPaginatedCatalogItemsWithSemanticRelevance(
                text,
                10,
                0);

            // Assert

            Assert.Equivalent(actual, catalogItems);
            await catalogApi.Received().GetPaginatedCatalogItemsWithSemanticRelevance(text, 10, 0);
        }
    }

    public class GetBrands
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogBrands(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            CatalogBrandDto[] catalogBrands)
        {
            // Arrange

            catalogApi.GetCatalogBrands()
                .Returns(catalogBrands);

            // Act

            CatalogBrandDto[] actual = await sut.GetBrands();

            // Assert

            Assert.Equivalent(actual, catalogBrands);
            await catalogApi.Received().GetCatalogBrands();
        }
    }

    public class GetTypes
    {
        [Theory, AutoNSubstituteData]
        public async Task return_catalogTypes(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            CatalogTypeDto[] catalogTypes)
        {
            // Arrange

            catalogApi.GetCatalogTypes()
                .Returns(catalogTypes);

            // Act

            CatalogTypeDto[] actual = await sut.GetTypes();

            // Assert

            Assert.Equivalent(actual, catalogTypes);
            await catalogApi.Received().GetCatalogTypes();
        }
    }

    public class CreateCatalogItem
    {
        [Theory, AutoNSubstituteData]
        public async Task create_catalogItem(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            CreateCatalogItemDto dto)
        {
            // Arrange

            // Act

            await sut.CreateCatalogItem(dto);

            // Assert

            await catalogApi.Received().CreateCatalogItem(dto);
        }
    }

    public class UpdateCatalogItem
    {
        [Theory, AutoNSubstituteData]
        public async Task update_catalogItem(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            Guid objectId,
            Catalog.Contracts.UpdateCatalogItem.CatalogItemDto dto)
        {
            // Arrange

            // Act

            await sut.UpdateCatalogItem(objectId, dto);

            // Assert

            await catalogApi.Received().UpdateCatalogItem(objectId, dto);
        }
    }

    public class DeleteCatalogItem
    {
        [Theory, AutoNSubstituteData]
        public async Task delete_catalogItem(
            [Substitute, Frozen] ICatalogApi catalogApi,
            CatalogService.Refit.CatalogService sut,
            Guid objectId)
        {
            // Arrange

            // Act

            await sut.DeleteCatalogItem(objectId);

            // Assert

            await catalogApi.Received().DeleteCatalogItem(objectId);
        }
    }
}