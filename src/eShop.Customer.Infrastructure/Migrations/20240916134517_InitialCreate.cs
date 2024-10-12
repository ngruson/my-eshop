using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "customer");

            migrationBuilder.CreateSequence(
                name: "customer-seq",
                schema: "customer",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ObjectId = table.Column<string>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    SecurityNumber = table.Column<string>(type: "text", nullable: true),
                    Expiration = table.Column<string>(type: "text", nullable: true),
                    CardHolderName = table.Column<string>(type: "text", nullable: true),
                    CardType = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAtUtc = table.Column<DateTime?>(type: "timestamp", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers",
                schema: "customer");

            migrationBuilder.DropSequence(
                name: "customer-seq",
                schema: "customer");
        }
    }
}
