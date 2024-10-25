using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItem;

public class CatalogItemViewModel(
    Guid objectId,
    string name,
    string description,
    decimal price,
    string pictureFileName,
    string catalogType,
    string catalogBrand,    
    int availableStock,
    int restockThreshold,
    int maxStockThreshold,
    bool onReorder)
{
    public CatalogItemViewModel() : this(Guid.Empty, string.Empty, string.Empty, 0, string.Empty, string.Empty, string.Empty, 0, 0, 0, false)
    {
    }

    public Guid ObjectId { get; set; } = objectId;

    [Required]
    public string Name { get; set; } = name;

    [Required]
    public string Description { get; set; } = description;

    public decimal Price { get; set; } = price;

    public string PictureFileName { get; set; } = pictureFileName;

    [Required]
    public string CatalogType { get; set; } = catalogType;

    [Required]
    public string CatalogBrand { get; set; } = catalogBrand;

    public int AvailableStock { get; set; } = availableStock;
    public int RestockThreshold { get; set; } = restockThreshold;
    public int MaxStockThreshold { get; set; } = maxStockThreshold;
    public bool OnReorder { get; set; } = onReorder;
}
