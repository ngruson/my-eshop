using Ardalis.Specification;

namespace eShop.IntegrationEventLogEF.Specifications;
internal class GetEventSpecification : Specification<IntegrationEventLogEntry>, ISingleResultSpecification<IntegrationEventLogEntry>
{
    public GetEventSpecification(Guid eventId)
    {
        this.Query.Where(ie => ie.EventId == eventId);
    }
}
