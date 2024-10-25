using Ardalis.Specification;

namespace eShop.Ordering.API.Application.Specifications;

public class CardTypeSpecification : Specification<CardType>, ISingleResultSpecification<CardType>
{
    public CardTypeSpecification(Guid objectId)
    {
        this.Query.Where(x => x.ObjectId == objectId);
    }
}
