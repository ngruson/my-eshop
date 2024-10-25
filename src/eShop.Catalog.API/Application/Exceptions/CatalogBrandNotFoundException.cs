namespace eShop.Catalog.API.Application.Exceptions;

internal class CatalogBrandNotFoundException : Exception
{
    public CatalogBrandNotFoundException() : base("Catalog brand not found")
    {
    }
}
