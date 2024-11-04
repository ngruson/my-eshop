using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace eShop.AdminApp.Application.Queries.Order.GetOrders;

internal class OrderViewModel(Guid objectId, string orderNumber, DateTime orderDate, string buyerName, string orderStatus, decimal total)
{
    public Guid ObjectId { get; set; } = objectId;

    [Display(Name = "Order Number")]
    public string OrderNumber { get; private init; } = orderNumber;

    [Display(Name = "Order Date")]
    public DateTime OrderDate { get; private init; } = orderDate;

    [Display(Name = "Buyer Name")]
    public string BuyerName { get; private init; } = buyerName;

    [Display(Name = "Order Status")]
    public string OrderStatus { get; private init; } = orderStatus;

    public string Total { get; private init; } = total.ToString("C", new CultureInfo("en-US"));
}
