using System.Text.Json;
using eShop.Catalog.API.Services;
using eShop.Shared.Data;
using eShop.Shared.DI;
using Pgvector;

namespace eShop.Catalog.API.Infrastructure;

public partial class CatalogContextSeed(
    IWebHostEnvironment env,
    IOptions<CatalogOptions> settings,
    ICatalogAI catalogAI,
    ILogger<CatalogContextSeed> logger) : IDbSeeder
{
    public async Task SeedAsync(ServiceProviderWrapper services)
    {
        var useCustomizationData = settings.Value.UseCustomizationData;
        var contentRootPath = env.ContentRootPath;
        var picturePath = env.WebRootPath;

        // Workaround from https://github.com/npgsql/efcore.pg/issues/292#issuecomment-388608426
        //context.Database.OpenConnection();
        //((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();

        IRepository<CatalogItem> catalogItemRepository = services.GetRequiredService<IRepository<CatalogItem>>();
        IRepository<CatalogBrand> catalogBrandRepository = services.GetRequiredService<IRepository<CatalogBrand>>();
        IRepository<CatalogType> catalogTypeRepository = services.GetRequiredService<IRepository<CatalogType>>();

        if (!await catalogItemRepository.AnyAsync())
        {
            var sourcePath = Path.Combine(contentRootPath, "Setup", "catalog.json");
            var sourceJson = File.ReadAllText(sourcePath);
            var sourceItems = JsonSerializer.Deserialize<CatalogSourceEntry[]>(sourceJson);

            await catalogBrandRepository.DeleteRangeAsync(await catalogBrandRepository.ListAsync());
            await catalogBrandRepository.AddRangeAsync(sourceItems!.Select(x => x.Brand).Distinct()
                .Select(brandName => new CatalogBrand { Brand = brandName }));
            IEnumerable<CatalogBrand> addedBrands = await catalogBrandRepository.ListAsync();
            logger.LogInformation("Seeded catalog with {NumBrands} brands", addedBrands.Count());

            await catalogTypeRepository.DeleteRangeAsync(await catalogTypeRepository.ListAsync());
            await catalogTypeRepository.AddRangeAsync(sourceItems!.Select(x => x.Type).Distinct()
                .Select(typeName => new CatalogType { Type = typeName }));
            IEnumerable<CatalogType> addedTypes = await catalogTypeRepository.ListAsync();
            logger.LogInformation("Seeded catalog with {NumTypes} types", addedTypes.Count());

            var brandIdsByName = addedBrands.ToDictionary(x => x.Brand, x => x.Id);
            var typeIdsByName = addedTypes.ToDictionary(x => x.Type, x => x.Id);

            CatalogItem[] catalogItems = sourceItems!.Select(source => new CatalogItem
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Price = source.Price,
                CatalogBrandId = brandIdsByName[source.Brand],
                CatalogTypeId = typeIdsByName[source.Type],
                AvailableStock = 100,
                MaxStockThreshold = 200,
                RestockThreshold = 10,
                PictureFileName = $"{source.Id}.webp",
            }).ToArray();

            if (catalogAI.IsEnabled)
            {
                logger.LogInformation("Generating {NumItems} embeddings", catalogItems.Length);
                IReadOnlyList<Vector> embeddings = await catalogAI.GetEmbeddingsAsync(catalogItems);
                for (int i = 0; i < catalogItems.Length; i++)
                {
                    catalogItems[i].Embedding = embeddings[i];
                }
            }

            await catalogItemRepository.AddRangeAsync(catalogItems);
            logger.LogInformation("Seeded catalog with {NumItems} items", catalogItems.Length);
        }
    }

    private class CatalogSourceEntry
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public required string Brand { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
    }
}
