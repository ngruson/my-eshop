using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.Order.GetOrder;

public record OrderItemViewModel
{
    [Display(Name = "Product Name")]
    public string ProductName { get; set; }

    [Display(Name = "Picture")]
    public string PictureUrl { get; set; }

    [Display(Name = "Unit Price")]
    public string UnitPrice { get; set; }

    public string Discount { get; set; }

    public int Units { get; set; }

    public string Total { get; set; }
}
