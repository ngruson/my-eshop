namespace eShop.Catalog.API.Application.Exceptions;

internal class CatalogBrandsNotFoundException : Exception
{
    public CatalogBrandsNotFoundException() : base("Catalog brands not found")
    {
    }
}
