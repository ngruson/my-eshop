using eShop.Shared.Behaviors;
using MediatR;

namespace eShop.Shared.UnitTests.Behaviors;
public class LoggingBehaviorUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_test_response(
        LoggingBehavior<TestRequest, TestResponse> sut,
        TestRequest request,
        RequestHandlerDelegate<TestResponse> requestHandlerDelegate)
    {
        // Arrange

        // Act

        TestResponse result = await sut.Handle(request, requestHandlerDelegate, default);

        // Assert

        Assert.NotNull(result);
    }
}
