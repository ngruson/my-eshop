namespace eShop.Identity.API.Services;

public partial class RedirectService : IRedirectService
{
    public string ExtractRedirectUriFromReturnUrl(string url)
    {
        var decodedUrl = System.Net.WebUtility.HtmlDecode(url);
        var results = MyRegex().Split(decodedUrl);
        if (results.Length < 2)
            return "";

        string result = results[1];

        string splitKey;
        if (result.Contains("signin-oidc"))
            splitKey = "signin-oidc";
        else
            splitKey = "scope";

        results = Regex.Split(result, splitKey);
        if (results.Length < 2)
            return "";

        result = results[0];

        return result.Replace("%3A", ":").Replace("%2F", "/").Replace("&", "");
    }

    [GeneratedRegex("redirect_uri=")]
    private static partial Regex MyRegex();
}
