using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.PaymentProcessor.Contracts;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Refit;

public class PaymentProcessorApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task process_payment(
            [Substitute, Frozen] IPaymentProcessorApi paymentProcessorApi,
            PaymentProcessorApiClient.Refit.PaymentProcessorApiClient sut,
            PaymentStatus paymentStatus)
    {
        // Arrange

        paymentProcessorApi.ProcessPayment()
            .Returns(paymentStatus);

        // Act

        PaymentStatus actual = await sut.ProcessPayment();

        // Assert

        Assert.Equivalent(actual, paymentStatus);
    }
}
