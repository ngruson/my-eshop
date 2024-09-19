using eShop.Customer.API.Extensions;
using eShop.Customer.API;
using Asp.Versioning;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

IApiVersioningBuilder withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

var orders = app.NewVersionedApi("Customers");

orders.MapCustomerApiV1()
      .RequireAuthorization();

app.UseDefaultOpenApi();
app.Run();
