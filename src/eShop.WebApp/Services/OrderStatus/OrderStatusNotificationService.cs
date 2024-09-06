namespace eShop.WebApp.Services;

public class OrderStatusNotificationService
{
    // Locking manually because we need multiple values per key, and only need to lock very briefly
    private readonly object _subscriptionsLock = new();
    private readonly Dictionary<string, HashSet<Subscription>> _subscriptionsByBuyerId = [];

    public IDisposable SubscribeToOrderStatusNotifications(string buyerId, Func<Task> callback)
    {
        var subscription = new Subscription(this, buyerId, callback);

        lock (this._subscriptionsLock)
        {
            if (!this._subscriptionsByBuyerId.TryGetValue(buyerId, out var subscriptions))
            {
                subscriptions = [];
                this._subscriptionsByBuyerId.Add(buyerId, subscriptions);
            }

            subscriptions.Add(subscription);
        }

        return subscription;
    }

    public Task NotifyOrderStatusChangedAsync(string buyerId)
    {
        lock (this._subscriptionsLock)
        {
            return this._subscriptionsByBuyerId.TryGetValue(buyerId, out var subscriptions)
                ? Task.WhenAll(subscriptions.Select(s => s.NotifyAsync()))
                : Task.CompletedTask;
        }
    }

    private void Unsubscribe(string buyerId, Subscription subscription)
    {
        lock (this._subscriptionsLock)
        {
            if (this._subscriptionsByBuyerId.TryGetValue(buyerId, out var subscriptions))
            {
                subscriptions.Remove(subscription);
                if (subscriptions.Count == 0)
                {
                    this._subscriptionsByBuyerId.Remove(buyerId);
                }
            }
        }
    }

    private class Subscription(OrderStatusNotificationService owner, string buyerId, Func<Task> callback) : IDisposable
    {
        public Task NotifyAsync()
        {
            return callback();
        }

        public void Dispose()
            => owner.Unsubscribe(buyerId, this);
    }
}
