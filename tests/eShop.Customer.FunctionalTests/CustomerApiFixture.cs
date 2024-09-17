using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Customer.FunctionalTests;

public sealed class CustomerApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost _app;

    public IResourceBuilder<PostgresServerResource> Postgres { get; private set; }
    public IResourceBuilder<PostgresServerResource> IdentityDB { get; private set; }
    public IResourceBuilder<ProjectResource> IdentityApi { get; private set; }

    private string _connectionString;

    public CustomerApiFixture()
    {
        var options = new DistributedApplicationOptions { AssemblyName = typeof(CustomerApiFixture).Assembly.FullName, DisableDashboard = true };
        var appBuilder = DistributedApplication.CreateBuilder(options);

        this.Postgres = appBuilder.AddPostgres("customerDb");
        this.IdentityDB = appBuilder.AddPostgres("IdentityDB");
        this.IdentityApi = appBuilder.AddProject<Projects.eShop_Identity_API>("identity-api").WithReference(this.IdentityDB);

        this._app = appBuilder.Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                { $"ConnectionStrings:{this.Postgres.Resource.Name.ToLower()}", this._connectionString },
                { "Identity:Url", this.IdentityApi.GetEndpoint("http").Url }
            });
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter>(new AutoAuthorizeStartupFilter());
            });
        });
        return base.CreateHost(builder);
    }

    protected override TestServer CreateServer(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                { $"ConnectionStrings:{this.Postgres.Resource.Name.ToLower()}", this._connectionString },
                });
        });
        return base.CreateServer(builder);
    }

    public new async Task DisposeAsync()
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

    public async Task InitializeAsync()
    {
        await this._app.StartAsync();
        this._connectionString = await this.Postgres.Resource.GetConnectionStringAsync();
    }

    private class AutoAuthorizeStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<AutoAuthorizeMiddleware>();
                next(builder);
            };
        }
    }
}
