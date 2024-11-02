using eShop.AdminApp.Components;
using eShop.AdminApp.Extensions;
using eShop.ServiceDefaults;
using Microsoft.FluentUI.AspNetCore.Components;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

// Add services to the container.
builder.Services
    .AddHttpClient()
    .AddFluentUIComponents(options => options.ValidateClassNames = false)
    .AddRazorComponents()    
    .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor();

builder.AddApplicationServices();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
