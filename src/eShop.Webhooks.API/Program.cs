using Asp.Versioning;
using Asp.Versioning.Builder;
using eShop.Webhooks.API.Apis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();

IApiVersioningBuilder withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

IVersionedEndpointRouteBuilder webHooks = app.NewVersionedApi("Web Hooks");

webHooks.MapWebHooksApiV1()
        .RequireAuthorization();

app.UseDefaultOpenApi();
app.Run();
