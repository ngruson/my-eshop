using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Customer.Infrastructure.Migrations;

/// <inheritdoc />
public partial class CardTypeMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "DeletedAtUtc",
            schema: "customer",
            table: "customers",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "CardType",
            schema: "customer",
            table: "customers",
            type: "integer",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "DeletedAtUtc",
            schema: "customer",
            table: "customers",
            type: "timestamp",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "CardType",
            schema: "customer",
            table: "customers",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);
    }
}
