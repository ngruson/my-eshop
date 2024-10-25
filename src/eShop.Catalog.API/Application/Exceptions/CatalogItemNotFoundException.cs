namespace eShop.Catalog.API.Application.Exceptions;

internal class CatalogItemNotFoundException : Exception
{
    public CatalogItemNotFoundException() : base("Catalog item not found")
    {
    }
}
