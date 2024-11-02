using System.Security.Claims;
using eShop.Webhooks.API.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace eShop.Webhooks.API.Apis;

public static class WebHooksApi
{
    public static RouteGroupBuilder MapWebHooksApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("/api/webhooks").HasApiVersion(1.0);

        api.MapGet("/", async (WebhooksContext context, ClaimsPrincipal user) =>
        {
            string? userId = user.GetUserId();
            List<WebhookSubscription> data = await context.Subscriptions.Where(s => s.UserId == userId).ToListAsync();
            return TypedResults.Ok(data);
        });

        api.MapGet("/{id:int}", async Task<Results<Ok<WebhookSubscription>, NotFound<string>>> (
            WebhooksContext context,
            ClaimsPrincipal user,
            int id) =>
        {
            string? userId = user.GetUserId();
            WebhookSubscription? subscription = await context.Subscriptions
                .SingleOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (subscription != null)
            {
                return TypedResults.Ok(subscription);
            }
            return TypedResults.NotFound($"Subscriptions {id} not found");
        });

        api.MapPost("/", async Task<Results<Created, BadRequest<string>>> (
            WebhookSubscriptionRequest request,
            IGrantUrlTesterService grantUrlTester,
            WebhooksContext context,
            ClaimsPrincipal user) =>
        {
            bool grantOk = await grantUrlTester.TestGrantUrl(request.Url!, request.GrantUrl!, request.Token ?? string.Empty);

            if (grantOk)
            {
                WebhookSubscription subscription = new()
                {
                    Date = DateTime.UtcNow,
                    DestUrl = request.Url,
                    Token = request.Token,
                    Type = Enum.Parse<WebhookType>(request.Event!, ignoreCase: true),
                    UserId = user.GetUserId()
                };

                context.Add(subscription);
                await context.SaveChangesAsync();

                return TypedResults.Created($"/api/webhooks/{subscription.Id}");
            }
            else
            {
                return TypedResults.BadRequest($"Invalid grant URL: {request.GrantUrl}");
            }
        })
        .ValidateWebhookSubscriptionRequest();

        api.MapDelete("/{id:int}", async Task<Results<Accepted, NotFound<string>>> (
            WebhooksContext context,
            ClaimsPrincipal user,
            int id) =>
        {
            string? userId = user.GetUserId();
            WebhookSubscription? subscription = await context.Subscriptions.SingleOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (subscription != null)
            {
                context.Remove(subscription);
                await context.SaveChangesAsync();
                return TypedResults.Accepted($"/api/webhooks/{subscription.Id}");
            }

            return TypedResults.NotFound($"Subscriptions {id} not found");
        });

        return api;
    }
}
