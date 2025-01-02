using Dapr.Client;
using eShop.PaymentProcessor.Contracts;
using eShop.ServiceInvocation.Auth;

namespace eShop.ServiceInvocation.PaymentProcessorApiClient.Dapr;

public class PaymentProcessorApiClient(DaprClient daprClient, AccessTokenAccessorFactory accessTokenAccessorFactory)
    : BaseDaprApiClient(daprClient, accessTokenAccessorFactory), IPaymentProcessorApiClient
{
    private readonly string basePath = "/api/workflow";
    protected override string AppId => "payment-processor";

    public async Task<PaymentStatus> ProcessPayment()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            this.basePath);

        return await this.DaprClient.InvokeMethodAsync<PaymentStatus>(request);
    }
}
