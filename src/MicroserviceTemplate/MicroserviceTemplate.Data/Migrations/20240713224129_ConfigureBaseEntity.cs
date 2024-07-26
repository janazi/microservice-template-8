using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroserviceTemplate.Infra.Data.Migrations;

/// <inheritdoc />
public partial class ConfigureBaseEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "UId",
            table: "Vehicles",
            type: "uniqueidentifier",
            nullable: false,
            defaultValueSql: "NEWSEQUENTIALID()",
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.CreateIndex(
            name: "IX_Vehicles_DateCreated",
            table: "Vehicles",
            column: "DateCreated");

        migrationBuilder.CreateIndex(
            name: "IX_Vehicles_UId",
            table: "Vehicles",
            column: "UId",
            unique: true,
            filter: "[IsDeleted] = 0");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Vehicles_DateCreated",
            table: "Vehicles");

        migrationBuilder.DropIndex(
            name: "IX_Vehicles_UId",
            table: "Vehicles");

        migrationBuilder.AlterColumn<Guid>(
            name: "UId",
            table: "Vehicles",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldDefaultValueSql: "NEWSEQUENTIALID()");
    }
}