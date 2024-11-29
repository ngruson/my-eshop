namespace eShop.Invoicing.API;

internal static class InvoiceApi
{
    public static RouteGroupBuilder MapInvoiceApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("api/invoices").HasApiVersion(1.0);

        return api;
    }
}
