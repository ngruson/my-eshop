using MediatR;

namespace eShop.Shared.Data;

public abstract class Entity
{
    int? _requestedHashCode;
    int _Id;
    public virtual int Id
    {
        get
        {
            return this._Id;
        }
        protected set
        {
            this._Id = value;
        }
    }

    private List<INotification>? _domainEvents;
    public IReadOnlyCollection<INotification>? DomainEvents => this._domainEvents?.AsReadOnly();

    public void AddDomainEvent(INotification eventItem)
    {
        this._domainEvents = this._domainEvents ?? [];
        this._domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        this._domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        this._domainEvents?.Clear();
    }

    public bool IsTransient()
    {
        return this.Id == default;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Entity)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (this.GetType() != obj.GetType())
            return false;

        var item = (Entity)obj;

        if (item.IsTransient() || this.IsTransient())
            return false;
        else
            return item.Id == this.Id;
    }

    public override int GetHashCode()
    {
        if (!this.IsTransient())
        {
            if (!this._requestedHashCode.HasValue)
                this._requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return this._requestedHashCode.Value;
        }
        else
            return base.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (Equals(left, null))
            return Equals(right, null);
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}
