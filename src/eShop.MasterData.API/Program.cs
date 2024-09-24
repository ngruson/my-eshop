using eShop.MasterData.API.Extensions;
using eShop.MasterData.API;
using Asp.Versioning;
using eShop.ServiceDefaults;
using Asp.Versioning.Builder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

IApiVersioningBuilder withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

IVersionedEndpointRouteBuilder api = app.NewVersionedApi("MasterData");
api.MapCountryApiV1()
    .RequireAuthorization();

api.MapStateApiV1()
    .RequireAuthorization();

app.UseDefaultOpenApi();
app.Run();
