using eShop.Customer.Infrastructure.EFCore.Configurations;
using eShop.Shared.Data.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;

namespace eShop.Customer.Infrastructure.EFCore;

public class CustomerDbContext(DbContextOptions<CustomerDbContext> options, IMediator mediator) : eShopDbContext(options, mediator)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("customer");
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ConfigureSmartEnum();
    }
}
