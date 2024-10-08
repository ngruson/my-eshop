// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using eShop.Customer.Infrastructure.EFCore;

#nullable disable

namespace eShop.Customer.Infrastructure.Migrations
{
    [DbContext(typeof(CustomerDbContext))]
    partial class CustomerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("customer")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("customer-seq")
                .IncrementsBy(10);

            modelBuilder.Entity("eShop.Customer.Domain.AggregatesModel.CustomerAggregate.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "customer-seq");

                    b.Property<Guid>("ObjectId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");                    

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CardHolderName")
                        .HasColumnType("text");

                    b.Property<string>("CardNumber")
                        .HasColumnType("text");

                    b.Property<int>("CardType")
                        .HasColumnType("integer");

                    b.Property<string>("Expiration")
                        .HasColumnType("text");

                    b.Property<string>("SecurityNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("customers", "customer");
                });
#pragma warning restore 612, 618
        }
    }
}
