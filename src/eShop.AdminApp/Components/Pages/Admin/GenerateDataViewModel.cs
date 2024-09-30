namespace eShop.AdminApp.Components.Pages.Admin;

internal class GenerateDataViewModel(int ordersToCreate)
{
    public int OrdersToCreate { get; set; } = ordersToCreate;
}
