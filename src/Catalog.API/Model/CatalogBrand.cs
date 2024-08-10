using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Catalog.API.Model;

public class CatalogBrand : IAggregateRoot
{
    public int Id { get; set; }

    [Required]
    public required string Brand { get; set; }
}
