using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

public class CardType
        : Entity, IAggregateRoot
{
    public string? Name { get; set; }

    public CardType(string name)
    {
        this.Name = name;
        this.ObjectId = Guid.NewGuid();
    }
}
