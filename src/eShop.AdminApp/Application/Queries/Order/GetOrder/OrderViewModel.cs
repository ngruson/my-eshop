using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.Order.GetOrder;

public class OrderViewModel
{
    public Guid ObjectId { get; set; }

    [Display(Name = "Order Number")]
    public required string OrderNumber { get; set; }

    [Display(Name = "Order Date")]
    public required string OrderDate { get; set; }

    [Display(Name = "Buyer Name")]
    public string? BuyerName { get; set; }

    [Display(Name = "Order Status")]
    public required string OrderStatus { get; set; }

    public required AddressViewModel Address { get; set; }

    public required string Total { get; set; }

    public required OrderItemViewModel[] OrderItems { get; set; }
}
