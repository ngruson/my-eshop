using eShop.EventBus.Options;
using eShop.Shared.Features;
using eShop.WebApp.Extensions;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.AddApplicationServices();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

FeaturesConfiguration features = app.Services.GetRequiredService<IOptions<FeaturesConfiguration>>().Value;
if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
{
    app.UseCloudEvents();
    app.MapSubscribeHandler();

    EventBusOptions eventBusOptions = app.Services.GetRequiredService<IOptions<EventBusOptions>>().Value;
    app.MapSubscriptionEndpoints(features, eventBusOptions);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAntiforgery();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapForwarder("/product-images/{id}", "http://catalog-api", "/api/catalog/items/{id}/pic");

app.Run();
