using eShop.Customer.Infrastructure.EFCore;
using eShop.Customer.Infrastructure.Seed;
using eShop.Shared.Behaviors;
using eShop.Shared.Data;
using eShop.Shared.Data.EntityFramework;

namespace eShop.Customer.API.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        // Add the authentication services to DI
        builder.AddDefaultAuthentication();

        builder.Services.AddDbContext<CustomerDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("customerDb"));
        });
        builder.Services.AddScoped<eShopDbContext>(sp => sp.GetRequiredService<CustomerDbContext>());
        builder.EnrichNpgsqlDbContext<CustomerDbContext>();

        builder.Services.AddMigration<CustomerDbContext>(builder.Configuration, typeof(CustomersSeed));

        builder.Services.AddHttpContextAccessor();

        // Configure Mediator
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });

        builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        builder.Services.AddScoped<CustomersSeed>();
    }
}
