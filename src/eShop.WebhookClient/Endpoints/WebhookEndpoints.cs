using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace eShop.WebhookClient.Endpoints;

public static class WebhookEndpoints
{
    public static IEndpointRouteBuilder MapWebhookEndpoints(this IEndpointRouteBuilder app)
    {
        const string webhookCheckHeader = "X-eshop-whtoken";

        IConfiguration configuration = app.ServiceProvider.GetRequiredService<IConfiguration>();
        if (!bool.TryParse(configuration["ValidateToken"], out bool validateToken))
        {
            validateToken = false; // or handle the error as needed
        }
        string? tokenToValidate = configuration["WebhookClientOptions:Token"];

        app.MapMethods("/check", [HttpMethods.Options], Results<Ok, BadRequest<string>> ([FromHeader(Name = webhookCheckHeader)] string value, HttpResponse response) =>
        {
            if (!validateToken || value == tokenToValidate)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    response.Headers.Append(webhookCheckHeader, value);
                }

                return TypedResults.Ok();
            }

            return TypedResults.BadRequest("Invalid token");
        });

        app.MapPost("/webhook-received", async (WebhookData hook, HttpRequest request, ILogger<Program> logger, HooksRepository hooksRepository) =>
        {
            StringValues token = request.Headers[webhookCheckHeader];

            logger.LogInformation("Received hook with token {Token}. My token is {MyToken}. Token validation is set to {ValidateToken}", token, tokenToValidate, validateToken);

            if (!validateToken || tokenToValidate == token)
            {
                logger.LogInformation("Received hook is going to be processed");
                WebHookReceived newHook = new()
                {
                    Data = hook.Payload,
                    When = hook.When,
                    Token = token
                };
                await hooksRepository.AddNew(newHook);
                logger.LogInformation("Received hook was processed.");
                return Results.Ok(newHook);
            }

            logger.LogInformation("Received hook is NOT processed - Bad Request returned.");
            return Results.BadRequest();
        });

        return app;
    }
}
