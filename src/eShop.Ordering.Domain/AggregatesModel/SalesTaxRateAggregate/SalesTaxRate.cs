using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.SalesTaxRateAggregate;

public class SalesTaxRate : Entity, IAggregateRoot
{
    protected SalesTaxRate() { }
    public SalesTaxRate(int id, string state, decimal rate)
    {
        this.Id = id;
        this.State = state;
        this.Rate = rate;
    }

    public string? State { get; private set; }
    public decimal Rate { get; private set; }
}
