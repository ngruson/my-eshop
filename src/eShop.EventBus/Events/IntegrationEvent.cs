namespace eShop.EventBus.Events;

public record IntegrationEvent
{
    public IntegrationEvent()
    {

        this.Id = Guid.NewGuid();
        this.CreationDate = DateTime.UtcNow;
    }

    public string MessageType => this.GetType().Name;

    [JsonInclude]
    public Guid Id { get; set; }

    [JsonInclude]
    public DateTime CreationDate { get; set; }
}
