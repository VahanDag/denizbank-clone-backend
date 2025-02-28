using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Denizbank.Migrations
{
    /// <inheritdoc />
    public partial class CardTypeChanged_SystemCardAdeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardName",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "CardType",
                table: "Cards",
                newName: "CardTypeId");

            migrationBuilder.CreateTable(
                name: "SystemCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardName = table.Column<string>(type: "text", nullable: false),
                    CardType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemCards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardTypeId",
                table: "Cards",
                column: "CardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_SystemCards_CardTypeId",
                table: "Cards",
                column: "CardTypeId",
                principalTable: "SystemCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_SystemCards_CardTypeId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "SystemCards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardTypeId",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "CardTypeId",
                table: "Cards",
                newName: "CardType");

            migrationBuilder.AddColumn<string>(
                name: "CardName",
                table: "Cards",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
