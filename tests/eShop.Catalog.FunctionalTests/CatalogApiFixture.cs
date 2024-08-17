using Microsoft.AspNetCore.Mvc.Testing;

namespace eShop.Catalog.FunctionalTests;

public sealed class CatalogApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost _app;

    public IResourceBuilder<PostgresServerResource> Postgres { get; private set; }
    private string _connectionString;

    private readonly Dictionary<string, string> _configuration = [];
    public Dictionary<string, string> Configuration => this._configuration;

    public CatalogApiFixture()
    {
        var options = new DistributedApplicationOptions { AssemblyName = typeof(CatalogApiFixture).Assembly.FullName, DisableDashboard = true };
        var appBuilder = DistributedApplication.CreateBuilder(options);
        this.Postgres = appBuilder.AddPostgres("CatalogDB")
            .WithImage("ankane/pgvector")
            .WithImageTag("latest");
        this._app = appBuilder.Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                { $"ConnectionStrings:{this.Postgres.Resource.Name.ToLower()}", this._connectionString },
                });

            if (this._configuration is not null)
            {
                config.AddInMemoryCollection(this._configuration);
            }
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
            if (this._configuration is not null)
            {
                config.AddInMemoryCollection(this._configuration);
            }
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
}
