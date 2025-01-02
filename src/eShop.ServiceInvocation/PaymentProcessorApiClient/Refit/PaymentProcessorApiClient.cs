using eShop.PaymentProcessor.Contracts;

namespace eShop.ServiceInvocation.PaymentProcessorApiClient.Refit;

public class PaymentProcessorApiClient(IPaymentProcessorApi paymentProcessorApi) : IPaymentProcessorApiClient
{
    public async Task<PaymentStatus> ProcessPayment()
    {
        return await paymentProcessorApi.ProcessPayment();
    }
}
