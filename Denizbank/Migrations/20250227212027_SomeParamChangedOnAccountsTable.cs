using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Denizbank.Migrations
{
    /// <inheritdoc />
    public partial class SomeParamChangedOnAccountsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Özel SQL komutu ile DateOnly'den integer'a dönüşüm
            migrationBuilder.Sql(
                "ALTER TABLE \"Cards\" ALTER COLUMN \"CutOfDate\" TYPE integer USING EXTRACT(DAY FROM \"CutOfDate\")::integer"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "CutOfDate",
                table: "Cards",
                type: "date",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}