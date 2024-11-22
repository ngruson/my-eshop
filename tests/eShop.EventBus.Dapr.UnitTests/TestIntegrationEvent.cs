using eShop.EventBus.Events;

namespace eShop.EventBus.Dapr.UnitTests;

internal record TestIntegrationEvent(string TestString, int TestInt) : IntegrationEvent;
