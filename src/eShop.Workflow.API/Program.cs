using Asp.Versioning;
using Asp.Versioning.Builder;
using eShop.ServiceDefaults;
using eShop.Shared.Features;
using eShop.Workflow.API;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FeaturesConfiguration>(builder.Configuration.GetSection("Features"));

builder.AddServiceDefaults();
builder.AddApplicationServices();

IApiVersioningBuilder withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

IVersionedEndpointRouteBuilder orders = app.NewVersionedApi("Orders");

orders.MapOrdersApiV1()
    .RequireAuthorization();

app.UseDefaultOpenApi();

await app.RunAsync();
