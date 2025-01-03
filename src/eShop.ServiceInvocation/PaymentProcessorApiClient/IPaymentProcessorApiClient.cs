using eShop.PaymentProcessor.Contracts;

namespace eShop.ServiceInvocation.PaymentProcessorApiClient;

public interface IPaymentProcessorApiClient
{
    Task<PaymentStatus> ProcessPayment();
}
