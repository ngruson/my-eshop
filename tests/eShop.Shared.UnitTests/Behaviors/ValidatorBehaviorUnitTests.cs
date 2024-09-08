using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Shared.Behaviors;
using eShop.Shared.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace eShop.Shared.UnitTests.Behaviors;

public class ValidatorBehaviorUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task with_valid_request_return_response(
        TestRequest request,
        [Substitute, Frozen] RequestHandlerDelegate<TestResponse> requestHandlerDelegate,
        TestResponse response)
    {
        // Arrange

        requestHandlerDelegate.Invoke().Returns(response);

        List<IValidator<TestRequest>> validators = [new TestRequestValidator()];

        ValidatorBehavior<TestRequest, TestResponse> sut = new(
            validators,
            Substitute.For<ILogger<ValidatorBehavior<TestRequest, TestResponse>>>());

        // Act

        TestResponse result = await sut.Handle(request, requestHandlerDelegate, default);

        // Assert

        Assert.Equal(response, result);
    }

    [Theory, AutoNSubstituteData]
    internal async Task with_invalid_request_throw_exception(
        TestRequest request,
        [Substitute, Frozen] RequestHandlerDelegate<TestResponse> requestHandlerDelegate,
        TestResponse response)
    {
        // Arrange

        requestHandlerDelegate.Invoke().Returns(response);

        List<IValidator<TestRequest>> validators = [new TestRequestValidator()];

        ValidatorBehavior<TestRequest, TestResponse> sut = new(
            validators,
            Substitute.For<ILogger<ValidatorBehavior<TestRequest, TestResponse>>>());

        request.Name = null;

        // Act

        async Task func() => await sut.Handle(request, requestHandlerDelegate, default);

        // Assert

        await Assert.ThrowsAsync<DomainException>(func);
    }
}
