using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Migrations;

/// <inheritdoc />
public partial class CardTypeMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_paymentMethods_cardTypes_CardTypeId",
            schema: "ordering",
            table: "paymentmethods");

        migrationBuilder.AlterColumn<int>(
            name: "CardTypeId",
            schema: "ordering",
            table: "paymentmethods",
            type: "integer",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<string>(
            name: "Address_ZipCode",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Address_Street",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Address_State",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Address_Country",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Address_City",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            schema: "ordering",
            table: "cardTypes",
            type: "character varying(200)",
            maxLength: 200,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "character varying(200)",
            oldMaxLength: 200);

        migrationBuilder.AddForeignKey(
            name: "FK_paymentMethods_cardTypes_CardTypeId",
            schema: "ordering",
            table: "paymentmethods",
            column: "CardTypeId",
            principalSchema: "ordering",
            principalTable: "cardTypes",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_paymentMethods_cardTypes_CardTypeId",
            schema: "ordering",
            table: "paymentmethods");

        migrationBuilder.AlterColumn<int>(
            name: "CardTypeId",
            schema: "ordering",
            table: "paymentmethods",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Address_ZipCode",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "Address_Street",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "Address_State",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "Address_Country",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "Address_City",
            schema: "ordering",
            table: "orders",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            schema: "ordering",
            table: "cardTypes",
            type: "character varying(200)",
            maxLength: 200,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "character varying(200)",
            oldMaxLength: 200,
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_paymentMethods_cardTypes_CardTypeId",
            schema: "ordering",
            table: "paymentmethods",
            column: "CardTypeId",
            principalSchema: "ordering",
            principalTable: "cardTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
