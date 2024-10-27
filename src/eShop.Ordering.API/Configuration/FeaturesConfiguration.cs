namespace eShop.Ordering.API.Configuration;
internal class FeaturesConfiguration
{
    public PublishSubscribeConfiguration PublishSubscribe { get; set; } = new PublishSubscribeConfiguration();
}
internal class PublishSubscribeConfiguration
{
    public EventBusType EventBus { get; set; } = EventBusType.RabbitMq;
}
internal enum EventBusType
{
    RabbitMq,
    Dapr
}
