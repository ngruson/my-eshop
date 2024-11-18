using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrderItem : Entity
{
    [Required]
    public string? ProductName { get; private set; }
    
    public string? PictureUrl { get; private set;}
    
    public decimal UnitPrice { get; private set;}
    
    public decimal Discount { get; private set; }
    
    public int Units { get; private set; }

    public Guid ProductId { get; private set; }

    public decimal Total => (this.UnitPrice * this.Units) - this.Discount;

    protected OrderItem() { }

    public OrderItem(Guid productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        if (units <= 0)
        {
            throw new OrderingDomainException("Invalid number of units");
        }

        if ((unitPrice * units) < discount)
        {
            throw new OrderingDomainException("The total of order item is lower than applied discount");
        }

        this.ProductId = productId;
        this.ProductName = productName;
        this.UnitPrice = unitPrice;
        this.Discount = discount;
        this.Units = units;
        this.PictureUrl = pictureUrl;
    }
    
    public void SetNewDiscount(decimal discount)
    {
        if (discount < 0)
        {
            throw new OrderingDomainException("Discount is not valid");
        }

        this.Discount = discount;
    }

    public void AddUnits(int units)
    {
        if (units < 0)
        {
            throw new OrderingDomainException("Invalid units");
        }

        this.Units += units;
    }
}
