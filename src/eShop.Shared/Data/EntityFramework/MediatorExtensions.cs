using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eShop.Shared.Data.EntityFramework;
internal static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
    {
        IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity>> domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count > 0);

        List<INotification> domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents!)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (INotification domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}
