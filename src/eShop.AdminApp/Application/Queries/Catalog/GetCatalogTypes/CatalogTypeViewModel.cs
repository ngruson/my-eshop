using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogTypes;

public record CatalogTypeViewModel(string objectId, string name)
{
    public string ObjectId { get; init; } = objectId;

    [Display(Name = "Type")]
    public string Name { get; init; } = name;
}
