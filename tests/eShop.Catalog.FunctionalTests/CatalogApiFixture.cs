using Microsoft.AspNetCore.Mvc.Testing;

namespace eShop.Catalog.FunctionalTests;

public sealed class CatalogApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost _app;

    public IResourceBuilder<PostgresServerResource> Postgres { get; private set; }
    public IResourceBuilder<RabbitMQServerResource> RabbitMq { get; private set; }
    private string _dbConnectionString;
    private string _rabbitMqConnectionString;

    public CatalogApiFixture()
    {
        var options = new DistributedApplicationOptions { AssemblyName = typeof(CatalogApiFixture).Assembly.FullName, DisableDashboard = true };
        var appBuilder = DistributedApplication.CreateBuilder(options);
        this.Postgres = appBuilder.AddPostgres("CatalogDB")
            .WithImage("ankane/pgvector")
            .WithImageTag("latest");

        this.RabbitMq = appBuilder.AddRabbitMQ("eventbus");

        this._app = appBuilder.Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                { $"ConnectionStrings:{this.Postgres.Resource.Name.ToLower()}", this._dbConnectionString },
                { $"ConnectionStrings:{this.RabbitMq.Resource.Name.ToLower()}", this._rabbitMqConnectionString },
                { "MediatR:UseTransactionBehavior", bool.FalseString }
            });
        });
        return base.CreateHost(builder);
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
        this._dbConnectionString = await this.Postgres.Resource.GetConnectionStringAsync();
        this._rabbitMqConnectionString = await this.RabbitMq.Resource.ConnectionStringExpression.GetValueAsync(default);
    }
}
