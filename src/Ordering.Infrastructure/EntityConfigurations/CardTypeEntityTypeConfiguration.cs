namespace eShop.Ordering.Infrastructure.EntityConfigurations;

class CardTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CardType>
{
    public void Configure(EntityTypeBuilder<CardType> cardTypesConfiguration)
    {
        cardTypesConfiguration.ToTable("cardtypes");

        cardTypesConfiguration.HasKey(ct => ct.Value);

        cardTypesConfiguration.Property(ct => ct.Value)
            .ValueGeneratedNever();

        cardTypesConfiguration.Property(ct => ct.Name)
            .HasMaxLength(200);
    }
}
