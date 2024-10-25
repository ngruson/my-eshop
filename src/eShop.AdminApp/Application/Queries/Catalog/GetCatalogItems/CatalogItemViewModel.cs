using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;

public record CatalogItemViewModel(
    Guid ObjectId,
    string Name,
    string Description,
    decimal price,
    CatalogTypeViewModel CatalogType,
    CatalogBrandViewModel CatalogBrand,
    int availableStock,
    int restockThreshold,
    int maxStockThreshold,
    bool onReorder)
{
    [Display(Name = "Available stock")]
    public int AvailableStock { get; init; } = availableStock;

    [Display(Name = "Restock threshold")]
    public int RestockThreshold { get; init; } = restockThreshold;

    [Display(Name = "Max stock threshold")]
    public int MaxStockThreshold { get; init; } = maxStockThreshold;

    [Display(Name = "On reorder")]
    public string OnReorder { get; init; } = FormatReorder(onReorder);

    [DisplayFormat(DataFormatString = "{0:C}")]
    public string Price { get; init; } = FormatPrice(price);

    private static string FormatReorder(bool onReorder)
    {
        return onReorder ? "Yes" : "No";
    }

    private static string FormatPrice(decimal price)
    {
        var usCulture = new CultureInfo("en-US");
        return string.Format(usCulture, "{0:C}", price);
    }
}
