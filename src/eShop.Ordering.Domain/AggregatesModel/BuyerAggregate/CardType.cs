using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

public class CardType
        : Entity, IAggregateRoot
{
    public string? Name { get; private set; }

    protected CardType() { }

    public CardType(string name)
    {
        this.Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
    }
}
