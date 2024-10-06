namespace eShop.Ordering.Infrastructure.EntityConfigurations;

class CardTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CardType>
{
    public void Configure(EntityTypeBuilder<CardType> cardTypesConfiguration)
    {
        cardTypesConfiguration.ToTable("cardTypes");

        cardTypesConfiguration.HasKey(ct => ct.Id);
        cardTypesConfiguration.Property(b => b.Id)
            .UseHiLo("cardTypeSeq");

        cardTypesConfiguration.Property(ct => ct.Name)
            .HasMaxLength(200);
    }
}
