using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace eShop.Basket.UnitTests;

internal class BasketApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost _app;

    public BasketApiFixture()
    {
        DistributedApplicationOptions options = new() { AssemblyName = typeof(BasketApiFixture).Assembly.FullName, DisableDashboard = true };
        IDistributedApplicationBuilder appBuilder = DistributedApplication.CreateBuilder(options);
        this._app = appBuilder.Build();
    }
    public async Task InitializeAsync()
    {
        await this._app.StartAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await this._app.StopAsync();
        if (this._app is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            this._app.Dispose();
        }
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await base.DisposeAsync();
        await this._app.StopAsync();
        if (this._app is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            this._app.Dispose();
        }
    }
}
