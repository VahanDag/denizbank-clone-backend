using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Denizbank.Migrations
{
    /// <inheritdoc />
    public partial class DenizBankCardTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_SystemCards_CardTypeId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "SystemCards");

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DenizBankCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardName = table.Column<string>(type: "text", nullable: false),
                    CardType = table.Column<int>(type: "integer", nullable: false),
                    CardDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenizBankCard", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_DenizBankCard_CardTypeId",
                table: "Cards",
                column: "CardTypeId",
                principalTable: "DenizBankCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_DenizBankCard_CardTypeId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "DenizBankCard");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Accounts");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_SystemCards_CardTypeId",
                table: "Cards",
                column: "CardTypeId",
                principalTable: "SystemCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
