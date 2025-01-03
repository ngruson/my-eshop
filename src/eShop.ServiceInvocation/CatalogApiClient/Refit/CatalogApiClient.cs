using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.AssessStockItemsForOrder;
using eShop.Catalog.Contracts.CreateCatalogItem;
using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.Shared.Data;

namespace eShop.ServiceInvocation.CatalogApiClient.Refit;

public class CatalogApiClient(ICatalogApi catalogApi) : ICatalogApiClient
{
    public async Task<CatalogItemViewModel> GetCatalogItem(Guid objectId)
    {
        Catalog.Contracts.GetCatalogItem.CatalogItemDto dto = await catalogApi.GetCatalogItem(objectId);
        return dto.Map();
    }

    public async Task<Catalog.Contracts.GetCatalogItems.CatalogItemDto[]> GetCatalogItems(bool includeDeleted = false)
    {
        return await catalogApi.GetCatalogItems(includeDeleted);
    }

    public async Task<PaginatedItems<CatalogItemViewModel>> GetPaginatedCatalogItems(Guid? catalogType, Guid? catalogBrand, int pageSize, int pageIndex)
    {
        PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> paginatedItems;

        if (catalogType.HasValue)
        {
            paginatedItems = await catalogApi.GetPaginatedCatalogItemsByTypeAndBrand(catalogType.Value, catalogBrand, pageSize, pageIndex);
        }
        else if (catalogBrand.HasValue)
        {
            paginatedItems = await catalogApi.GetPaginatedCatalogItemsByBrand(catalogBrand.Value, pageSize, pageIndex);
        }
        else
        {
            paginatedItems = await catalogApi.GetPaginatedCatalogItems(pageSize, pageIndex);
        }

        return paginatedItems.Map();
    }

    public async Task<CatalogItemViewModel[]> GetCatalogItems(Guid[] ids)
    {
        Catalog.Contracts.GetCatalogItems.CatalogItemDto[] items = await catalogApi.GetCatalogItemsByIds(ids);
        return items.Map();
    }

    public async Task<PaginatedItems<CatalogItemViewModel>> GetPaginatedCatalogItemsWithSemanticRelevance(string text, int pageSize, int pageIndex)
    {
        PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> paginatedItems =
            await catalogApi.GetPaginatedCatalogItemsWithSemanticRelevance(text, pageSize, pageIndex);

        return paginatedItems.Map();
    }

    public async Task<CatalogBrandDto[]> GetBrands()
    {
        return await catalogApi.GetCatalogBrands();
    }

    public async Task<CatalogTypeDto[]> GetTypes()
    {
        return await catalogApi.GetCatalogTypes();
    }

    public async Task CreateCatalogItem(CreateCatalogItemDto dto)
    {
        await catalogApi.CreateCatalogItem(dto);
    }

    public async Task DeleteCatalogItem(Guid objectId)
    {
        await catalogApi.DeleteCatalogItem(objectId);
    }

    public async Task UpdateCatalogItem(Guid objectId, Catalog.Contracts.UpdateCatalogItem.CatalogItemDto dto)
    {
        await catalogApi.UpdateCatalogItem(objectId, dto);
    }

    public async Task<AssessStockItemsForOrderResponseDto> AssessStockItemsForOrder(AssessStockItemsForOrderRequestDto dto)
    {
        return await catalogApi.AssessStockItemsForOrder(dto);
    }
}
