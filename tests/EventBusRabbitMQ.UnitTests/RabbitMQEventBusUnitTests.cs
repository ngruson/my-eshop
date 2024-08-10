namespace EventBusRabbitMQ.UnitTests;

public class RabbitMQEventBusUnitTests
{
    //[Theory]
    //[AutoMoqData]
    //public async Task Test1(
    //    [Frozen] Mock<IConnection> mockConnection,
    //    [Frozen] Mock<IServiceProvider> mockServiceProvider,
    //    [Frozen] Mock<IOptions<EventBusOptions>> mockEventBusOptions,
    //    IntegrationEvent @event
    //    )
    //{
    //    // Arrange

    //    mockConnection.Setup(x => x.IsOpen).Returns(true);

    //    mockConnection.Setup(x => x.CreateModel())
    //        .Returns(Mock.Of<IModel>());

    //    mockServiceProvider
    //        .Setup(x => x.GetService(typeof(IConnection)))
    //        .Returns(mockConnection.Object);

    //    using RabbitMQEventBus sut = new(
    //        new Mock<ILogger<RabbitMQEventBus>>().Object,
    //        mockServiceProvider.Object,
    //        mockEventBusOptions.Object,
    //        new Mock<IOptions<EventBusSubscriptionInfo>>().Object,
    //        new Mock<RabbitMQTelemetry>().Object);

    //    // Act

    //    await sut.StartAsync(default);

    //    Result result = await sut.PublishAsync(@event);

    //    // Assert

    //    Assert.True(result.IsSuccess);
    //}
}
