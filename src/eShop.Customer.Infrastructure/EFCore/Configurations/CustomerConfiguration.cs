using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Customer.Infrastructure.EFCore.Configurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.CustomerAggregate.Customer>
{
    public void Configure(EntityTypeBuilder<Domain.AggregatesModel.CustomerAggregate.Customer> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("customers");

        entityTypeBuilder.Ignore(b => b.DomainEvents);

        entityTypeBuilder.Property(o => o.Id)
            .UseHiLo("customer-seq");

        entityTypeBuilder.Property(o => o.UserName)
            .IsRequired();

        entityTypeBuilder.Property(o => o.FirstName)
            .IsRequired();

        entityTypeBuilder.Property(o => o.LastName)
            .IsRequired();

        entityTypeBuilder.Property(o => o.Street)
            .IsRequired();

        entityTypeBuilder.Property(o => o.ZipCode)
            .IsRequired();

        entityTypeBuilder.Property(o => o.City)
            .IsRequired();        

        entityTypeBuilder.Property(o => o.Country)
            .IsRequired();

        entityTypeBuilder.Property(o => o.State)
            .IsRequired();

        //entityTypeBuilder.Property(o => o.CardNumber)
        //    .IsRequired();

        //entityTypeBuilder.Property(o => o.SecurityNumber)
        //    .IsRequired();

        //entityTypeBuilder.Property(o => o.Expiration)
        //    .IsRequired();

        //entityTypeBuilder.Property(o => o.CardHolderName)            

        //entityTypeBuilder.Property(o => o.CardType)
        //    .IsRequired();        
    }
}
