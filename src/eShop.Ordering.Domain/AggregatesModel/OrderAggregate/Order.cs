using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

public class Order
    : Entity, IAggregateRoot
{
    public DateTime OrderDate { get; private set; }

    // Address is a Value Object pattern example persisted as EF Core 2.0 owned entity
    [Required]
    public Address? Address { get; private set; }

    public int? BuyerId { get; private set; }

    public Buyer? Buyer { get; private set; }

    public OrderStatus OrderStatus { get; private set; }
    
    public string? Description { get; private set; }

    // Draft orders have this set to true. Currently we don't check anywhere the draft status of an Order, but we could do it if needed
#pragma warning disable CS0414 // The field 'Order._isDraft' is assigned but its value is never used
    private bool _isDraft;
#pragma warning restore CS0414

    // DDD Patterns comment
    // Using a private collection field, better for DDD Aggregate's encapsulation
    // so OrderItems cannot be added from "outside the AggregateRoot" directly to the collection,
    // but only through the method OrderAggregateRoot.AddOrderItem() which includes behavior.
    private readonly List<OrderItem> _orderItems;
   
    public IReadOnlyCollection<OrderItem> OrderItems => this._orderItems.AsReadOnly();

    public int? PaymentId { get; private set; }

    public static Order NewDraft()
    {
        var order = new Order
        {
            _isDraft = true
        };
        return order;
    }

    protected Order()
    {
        this._orderItems = [];
        this._isDraft = false;
    }

    public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber,
            string cardHolderName, DateTime cardExpiration, int? buyerId = null, int? paymentMethodId = null) : this()
    {
        this.BuyerId = buyerId;
        this.PaymentId = paymentMethodId;
        this.OrderStatus = OrderStatus.Submitted;
        this.OrderDate = DateTime.UtcNow;
        this.Address = address;

        // Add the OrderStarterDomainEvent to the domain events collection 
        // to be raised/dispatched when committing changes into the Database [ After DbContext.SaveChanges() ]
        this.AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber,
                                    cardSecurityNumber, cardHolderName, cardExpiration);
    }

    // DDD Patterns comment
    // This Order AggregateRoot's method "AddOrderItem()" should be the only way to add Items to the Order,
    // so any behavior (discounts, etc.) and validations are controlled by the AggregateRoot 
    // in order to maintain consistency between the whole Aggregate. 
    public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        var existingOrderForProduct = this._orderItems.SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct is not null)
        {
            //if previous line exist modify it with higher discount  and units..
            if (discount > existingOrderForProduct.Discount)
            {
                existingOrderForProduct.SetNewDiscount(discount);
            }

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            //add validated new order item
            var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
            this._orderItems.Add(orderItem);
        }
    }

    public void SetPaymentMethodVerified(int buyerId, int paymentId)
    {
        this.BuyerId = buyerId;
        this.PaymentId = paymentId;
    }
    
    public void SetAwaitingValidationStatus()
    {
        if (this.OrderStatus == OrderStatus.Submitted)
        {
            this.AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id, _orderItems));
            this.OrderStatus = OrderStatus.AwaitingValidation;
        }
    }

    public void SetStockConfirmedStatus()
    {
        if (this.OrderStatus == OrderStatus.AwaitingValidation)
        {
            this.AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(this.Id));

            this.OrderStatus = OrderStatus.StockConfirmed;
            this.Description = "All the items were confirmed with available stock.";
        }
    }

    public void SetPaidStatus()
    {
        if (this.OrderStatus == OrderStatus.StockConfirmed)
        {
            this.AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(this.Id, this.OrderItems));

            this.OrderStatus = OrderStatus.Paid;
            this.Description = "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"";
        }
    }

    public void SetShippedStatus()
    {
        if (this.OrderStatus != OrderStatus.Paid)
        {
            this.StatusChangeException(OrderStatus.Shipped);
        }

        this.OrderStatus = OrderStatus.Shipped;
        this.Description = "The order was shipped.";
        this.AddDomainEvent(new OrderShippedDomainEvent(this));
    }

    public void SetCancelledStatus()
    {
        if (this.OrderStatus == OrderStatus.Paid ||
            this.OrderStatus == OrderStatus.Shipped)
        {
            this.StatusChangeException(OrderStatus.Cancelled);
        }

        this.OrderStatus = OrderStatus.Cancelled;
        this.Description = $"The order was cancelled.";
        this.AddDomainEvent(new OrderCancelledDomainEvent(this));
    }

    public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
    {
        if (this.OrderStatus == OrderStatus.AwaitingValidation)
        {
            this.OrderStatus = OrderStatus.Cancelled;

            var itemsStockRejectedProductNames = this.OrderItems
                .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                .Select(c => c.ProductName);

            var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
            this.Description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
        }
    }

    private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber,
            string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
    {
        var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardTypeId,
                                                                    cardNumber, cardSecurityNumber,
                                                                    cardHolderName, cardExpiration);

        this.AddDomainEvent(orderStartedDomainEvent);
    }

    private void StatusChangeException(OrderStatus orderStatusToChange)
    {
        throw new OrderingDomainException($"Is not possible to change the order status from {this.OrderStatus} to {orderStatusToChange}.");
    }

    public decimal GetTotal() => this._orderItems.Sum(o => o.Units * o.UnitPrice);

    // Used in unit tests
    public void SetBuyer(Buyer buyer)
    {
        this.Buyer = buyer;
    }
}
