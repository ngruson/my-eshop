using System.Collections.Concurrent;

namespace eShop.WebhookClient.Services;

public class HooksRepository
{
    private readonly ConcurrentQueue<WebHookReceived> data = new();
    private readonly ConcurrentDictionary<OnChangeSubscription, object?> onChangeSubscriptions = new();

    public Task AddNew(WebHookReceived hook)
    {
        this.data.Enqueue(hook);

        foreach (KeyValuePair<OnChangeSubscription, object?> subscription in this.onChangeSubscriptions)
        {
            try
            {
                subscription.Key.NotifyAsync();
            }
            catch (Exception)
            {
                // It's the subscriber's responsibility to report/handle any exceptions
                // that occur during their callback
            }
        }

        return Task.CompletedTask;
    }

    public Task<IEnumerable<WebHookReceived>> GetAll()
    {
        return Task.FromResult(this.data.AsEnumerable());
    }

    public IDisposable Subscribe(Func<Task> callback)
    {
        OnChangeSubscription subscription = new(callback, this);
        this.onChangeSubscriptions.TryAdd(subscription, null);
        return subscription;
    }

    private class OnChangeSubscription(Func<Task> callback, HooksRepository owner) : IDisposable
    {
        public Task NotifyAsync() => callback();

        public void Dispose() => owner.onChangeSubscriptions.Remove(this, out _);
    }
}
