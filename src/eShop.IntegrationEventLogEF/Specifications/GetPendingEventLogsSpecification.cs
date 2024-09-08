using Ardalis.Specification;

namespace eShop.IntegrationEventLogEF.Specifications;
internal class GetPendingEventLogsSpecification : Specification<IntegrationEventLogEntry>
{
    public GetPendingEventLogsSpecification(Guid transactionId, Type[] eventTypes)
    {
        this.Query.Where(e => e.TransactionId == transactionId && e.State == EventStateEnum.NotPublished);
        this.Query.OrderBy(o => o.CreationTime);
        //this.Query
        //    .Select(e => e.DeserializeJsonContent(eventTypes.FirstOrDefault(t => t.Name == e.EventTypeShortName)!));
    }
}
