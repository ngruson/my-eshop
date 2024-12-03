using eShop.Ordering.Domain.AggregatesModel.SalesTaxRateAggregate;

namespace eShop.Ordering.Infrastructure.EntityConfigurations;

internal class SalesTaxRateEntityTypeConfiguration : IEntityTypeConfiguration<SalesTaxRate>
{
    public void Configure(EntityTypeBuilder<SalesTaxRate> builder)
    {
        builder.ToTable("salesTaxRates");

        builder.Ignore(b => b.DomainEvents);

        builder.Property(o => o.Id)
            .UseHiLo("salesTaxRateSeq");

        builder.HasData(
            new SalesTaxRate(1, "AL", 0.05m),
            new SalesTaxRate(2, "AK", 0m),
            new SalesTaxRate(3, "AZ", 0.056m),
            new SalesTaxRate(4, "AR", 0.065m),
            new SalesTaxRate(5, "CA", 0.0825m),
            new SalesTaxRate(6, "CO", 0.029m),
            new SalesTaxRate(7, "CT", 0.0635m),
            new SalesTaxRate(8, "DC", 0.06m),
            new SalesTaxRate(9, "DE", 0m),
            new SalesTaxRate(10, "FL", 0.06m),
            new SalesTaxRate(11, "GA", 0.04m),
            new SalesTaxRate(12, "HI", 0.04m),
            new SalesTaxRate(13, "ID", 0.06m),
            new SalesTaxRate(14, "IL", 0.0625m),
            new SalesTaxRate(15, "IN", 0.07m),
            new SalesTaxRate(16, "IA", 0.06m),
            new SalesTaxRate(17, "KS", 0.065m),
            new SalesTaxRate(18, "KY", 0.06m),
            new SalesTaxRate(19, "LA", 0.0445m),
            new SalesTaxRate(20, "ME", 0.055m),
            new SalesTaxRate(21, "MD", 0.06m),
            new SalesTaxRate(22, "MA", 0.0625m),
            new SalesTaxRate(23, "MI", 0.06m),
            new SalesTaxRate(24, "MN", 0.06875m),
            new SalesTaxRate(25, "MS", 0.07m),
            new SalesTaxRate(26, "MO", 0.0423m),
            new SalesTaxRate(27, "MT", 0m),
            new SalesTaxRate(28, "NE", 0.055m),
            new SalesTaxRate(29, "NV", 0.0685m),
            new SalesTaxRate(30, "NH", 0m),
            new SalesTaxRate(31, "NJ", 0.06625m),
            new SalesTaxRate(32, "NM", 0.05125m),
            new SalesTaxRate(33, "NY", 0.04m),
            new SalesTaxRate(34, "NC", 0.0475m),
            new SalesTaxRate(35, "ND", 0.05m),
            new SalesTaxRate(36, "OH", 0.0575m),
            new SalesTaxRate(37, "OK", 0.045m),
            new SalesTaxRate(38, "OR", 0m),
            new SalesTaxRate(39, "PA", 0.06m),
            new SalesTaxRate(40, "RI", 0.07m),
            new SalesTaxRate(41, "SC", 0.06m),
            new SalesTaxRate(42, "SD", 0.045m),
            new SalesTaxRate(43, "TN", 0.07m),
            new SalesTaxRate(44, "TX", 0.0625m),
            new SalesTaxRate(45, "UT", 0.0485m),
            new SalesTaxRate(46, "VT", 0.06m),
            new SalesTaxRate(47, "VA", 0.043m),
            new SalesTaxRate(48, "WA", 0.065m),
            new SalesTaxRate(49, "WV", 0.06m),
            new SalesTaxRate(50, "WI", 0.05m),
            new SalesTaxRate(51, "WY", 0.04m)
        );
    }
}
