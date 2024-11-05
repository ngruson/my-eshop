namespace eShop.WebApp.Services.OrderStatus;

public class OrderStatusNotificationService
{
    // Locking manually because we need multiple values per key, and only need to lock very briefly
    private readonly Lock _subscriptionsLock = new();
    private readonly Dictionary<Guid, HashSet<Subscription>> _subscriptionsByBuyerId = [];

    public IDisposable SubscribeToOrderStatusNotifications(Guid buyerId, Func<Task> callback)
    {
        Subscription subscription = new(this, buyerId, callback);

        lock (this._subscriptionsLock)
        {
            if (!this._subscriptionsByBuyerId.TryGetValue(buyerId, out HashSet<Subscription>? subscriptions))
            {
                subscriptions = [];
                this._subscriptionsByBuyerId.Add(buyerId, subscriptions);
            }

            subscriptions.Add(subscription);
        }

        return subscription;
    }

    public Task NotifyOrderStatusChangedAsync(Guid buyerId)
    {
        lock (this._subscriptionsLock)
        {
            return this._subscriptionsByBuyerId.TryGetValue(buyerId, out HashSet<Subscription>? subscriptions)
                ? Task.WhenAll(subscriptions.Select(s => s.NotifyAsync()))
                : Task.CompletedTask;
        }
    }

    private void Unsubscribe(Guid buyerId, Subscription subscription)
    {
        lock (this._subscriptionsLock)
        {
            if (this._subscriptionsByBuyerId.TryGetValue(buyerId, out HashSet<Subscription>? subscriptions))
            {
                subscriptions.Remove(subscription);
                if (subscriptions.Count == 0)
                {
                    this._subscriptionsByBuyerId.Remove(buyerId);
                }
            }
        }
    }

    private class Subscription(OrderStatusNotificationService owner, Guid buyerId, Func<Task> callback) : IDisposable
    {
        public Task NotifyAsync()
        {
            return callback();
        }

        public void Dispose()
            => owner.Unsubscribe(buyerId, this);
    }
}
