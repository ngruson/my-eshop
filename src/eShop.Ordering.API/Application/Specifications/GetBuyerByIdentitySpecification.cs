using Ardalis.Specification;

namespace eShop.Ordering.API.Application.Specifications;

public class GetBuyerByIdentitySpecification : Specification<Buyer>, ISingleResultSpecification<Buyer>
{
    public GetBuyerByIdentitySpecification(Guid identity)
    {
        this.Query.Where(_ => _.IdentityGuid == identity)
            .Include(_ => _.PaymentMethods);
    }
}
