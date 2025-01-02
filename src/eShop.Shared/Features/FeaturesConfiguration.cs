namespace eShop.Shared.Features;
public class FeaturesConfiguration
{
    public PublishSubscribeConfiguration PublishSubscribe { get; set; } = new PublishSubscribeConfiguration();
    public ServiceInvocationConfiguration ServiceInvocation { get; set; } = new ServiceInvocationConfiguration();
    public WorkflowConfiguration Workflow { get; set; } = new WorkflowConfiguration();
}
public class PublishSubscribeConfiguration
{
    public string PubsubName { get; set; } = "pubsub";
    public EventBusType EventBus { get; set; } = EventBusType.RabbitMq;
    public string TopicName { get; set; } = "eShop_event_bus";
}
public class ServiceInvocationConfiguration
{
    public ServiceInvocationType ServiceInvocationType { get; set; }
}
public class StateConfiguration
{
    public required string StateStoreName { get; set; }
}
public class WorkflowConfiguration
{
    public bool Enabled { get; set; }
}
public enum EventBusType
{
    RabbitMq,
    Dapr
}
public enum ServiceInvocationType
{
    Refit,
    Dapr
}
