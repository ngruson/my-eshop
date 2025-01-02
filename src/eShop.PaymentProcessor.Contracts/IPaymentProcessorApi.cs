using Refit;

namespace eShop.PaymentProcessor.Contracts;

public interface IPaymentProcessorApi
{
    [Post("/api/workflow?api-version=1.0")]
    Task<PaymentStatus> ProcessPayment();
}
