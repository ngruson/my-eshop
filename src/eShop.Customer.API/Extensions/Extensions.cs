using eShop.Customer.Infrastructure.EFCore;
using eShop.Customer.Infrastructure.Seed;
using eShop.Identity.Contracts;
using eShop.Shared.Behaviors;
using eShop.Shared.Data;
using eShop.Shared.Data.EntityFramework;
using Refit;

namespace eShop.Customer.API.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        // Add the authentication services to DI
        builder.AddDefaultAuthentication();

        builder.Services
            .AddRefitClient<IIdentityApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{builder.Configuration["Identity:Url"]}"));

        services.AddDbContext<CustomerDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("customerDb"));
        });
        services.AddScoped<eShopDbContext>(sp => sp.GetRequiredService<CustomerDbContext>());
        builder.EnrichNpgsqlDbContext<CustomerDbContext>();

        services.AddMigration<CustomerDbContext>(typeof(CustomersSeed));

        services.AddHttpContextAccessor();

        // Configure Mediator
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });
       
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<CustomersSeed>();
    }
}
