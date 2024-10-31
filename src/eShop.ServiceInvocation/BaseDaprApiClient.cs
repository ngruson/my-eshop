using System.Net.Http.Headers;
using Dapr.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace eShop.ServiceInvocation;

public abstract class BaseDaprApiClient(DaprClient daprClient, IHttpContextAccessor httpContextAccessor)
{
    private readonly KeyValuePair<string, string>[] defaultQueryStringParameters =
    [
        new KeyValuePair<string, string>("api-version", "1.0")
    ];

    protected DaprClient DaprClient => daprClient;
    protected abstract string AppId { get; }

    protected async Task<HttpRequestMessage> CreateRequest(HttpMethod httpMethod, string methodName)
    {
        return await this.CreateRequest(
            httpMethod,
            methodName,
            null,
            null);
    }

    protected async Task<HttpRequestMessage> CreateRequest(HttpMethod httpMethod, string methodName, KeyValuePair<string, string>[]? queryStringParameters)
    {
        return await this.CreateRequest(
            httpMethod,
            methodName,
            queryStringParameters,
            null);
    }

    protected async Task<HttpRequestMessage> CreateRequest(HttpMethod httpMethod, string methodName,
        KeyValuePair<string, string>[]? queryStringParameters, object? data)
    {
        var queryStringParametersConcat = this.defaultQueryStringParameters;
        if (queryStringParameters is not null)
        {
            queryStringParametersConcat = [.. queryStringParametersConcat, .. queryStringParameters];
        }

        HttpRequestMessage request;

        if (data is not null)
        {
            request = daprClient.CreateInvokeMethodRequest(
                httpMethod,
                this.AppId,
                methodName,
                queryStringParametersConcat,
                data);
        }
        else
        {
            request = daprClient.CreateInvokeMethodRequest(
                httpMethod,
                this.AppId,
                methodName,
                queryStringParametersConcat);
        }

        request.Headers.Authorization = await this.GetAuthToken();

        return request;
    }

    private async Task<AuthenticationHeaderValue?> GetAuthToken()
    {
        if (httpContextAccessor.HttpContext is HttpContext context)
        {
            string? accessToken = await context.GetTokenAsync("access_token");

            if (accessToken is not null)
            {
                return new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        return null;
    }
}
