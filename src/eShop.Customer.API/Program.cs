using eShop.Customer.API.Extensions;
using eShop.Customer.API;
using eShop.Customer.Infrastructure.EFCore;
using eShop.Customer.Infrastructure.Seed;
using Asp.Versioning;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

IApiVersioningBuilder withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    CustomerDbContext dbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    dbContext.Database.EnsureCreated();
    CustomersSeed customersSeed = scope.ServiceProvider.GetRequiredService<CustomersSeed>();

    Task seedTask = Task.Run(async() => await customersSeed.SeedAsync());
    seedTask.Wait();
}

app.MapDefaultEndpoints();

var orders = app.NewVersionedApi("Orders");

orders.MapCustomerApiV1()
      .RequireAuthorization();

app.UseDefaultOpenApi();
app.Run();
