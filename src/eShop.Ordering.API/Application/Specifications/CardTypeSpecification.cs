using Ardalis.Specification;

namespace eShop.Ordering.API.Application.Specifications;

public class CardTypeSpecification : Specification<CardType>, ISingleResultSpecification<CardType>
{
    public CardTypeSpecification(string name)
    {
        this.Query.Where(x => x.Name == name);
    }
}
