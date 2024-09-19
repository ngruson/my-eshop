namespace eShop.Ordering.Infrastructure.Idempotency;

public class RequestManager(OrderingContext context) : IRequestManager
{
    private readonly OrderingContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<bool> ExistAsync(Guid id)
    {
        var request = await this._context.
            FindAsync<ClientRequest>(id);

        return request != null;
    }

    public async Task CreateRequestForCommandAsync<T>(Guid id)
    {
        var exists = await this.ExistAsync(id);

        var request = exists ?
            throw new OrderingDomainException($"Request with {id} already exists") :
            new ClientRequest()
            {
                Id = id,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            };

        this._context.Add(request);

        await this._context.SaveChangesAsync();
    }
}
