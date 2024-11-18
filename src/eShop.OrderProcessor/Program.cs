WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();
