using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Denizbank.Migrations
{
    /// <inheritdoc />
    public partial class CardColumnNameChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cards",
                newName: "CardName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CardName",
                table: "Cards",
                newName: "Name");
        }
    }
}
