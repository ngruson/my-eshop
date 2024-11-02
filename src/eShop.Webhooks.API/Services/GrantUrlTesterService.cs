namespace eShop.Webhooks.API.Services;

class GrantUrlTesterService(IHttpClientFactory factory, ILogger<IGrantUrlTesterService> logger) : IGrantUrlTesterService
{
    public async Task<bool> TestGrantUrl(string urlHook, string url, string token)
    {
        if (!CheckSameOrigin(urlHook, url))
        {
            logger.LogWarning("Url of the hook ({UrlHook} and the grant url ({Url} do not belong to same origin)", urlHook, url);
            return false;
        }

        HttpClient client = factory.CreateClient();
        HttpRequestMessage msg = new(HttpMethod.Options, url);
        msg.Headers.Add("X-eshop-whtoken", token);

        logger.LogInformation("Sending the OPTIONS message to {Url} with token \"{Token}\"", url, token ?? string.Empty);

        try
        {
            HttpResponseMessage response = await client.SendAsync(msg);
            string? tokenReceived = response.Headers.TryGetValues("X-eshop-whtoken", out IEnumerable<string>? tokenValues) ? tokenValues.FirstOrDefault() : null;
            string? tokenExpected = string.IsNullOrWhiteSpace(token) ? null : token;

            logger.LogInformation("Response code is {StatusCode} for url {Url} and token in header was {TokenReceived} (expected token was {TokenExpected})", response.StatusCode, url, tokenReceived, tokenExpected);

            return response.IsSuccessStatusCode && tokenReceived == tokenExpected;
        }
        catch (Exception ex)
        {
            logger.LogWarning("Exception {TypeName} when sending OPTIONS request. Url can't be granted.", ex.GetType().Name);

            return false;
        }
    }

    private static bool CheckSameOrigin(string urlHook, string url)
    {
        Uri firstUrl = new(urlHook, UriKind.Absolute);
        Uri secondUrl = new(url, UriKind.Absolute);

        return firstUrl.Scheme == secondUrl.Scheme &&
            firstUrl.Port == secondUrl.Port &&
            firstUrl.Host == secondUrl.Host;
    }
}
