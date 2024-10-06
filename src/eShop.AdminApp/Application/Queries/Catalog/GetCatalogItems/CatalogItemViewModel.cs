using System.Globalization;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;

public record CatalogItemViewModel(int Id, string Name, string Description, decimal price)
{
    public string Price { get; private init; } = price.ToString("C", new CultureInfo("en-US"));
}
