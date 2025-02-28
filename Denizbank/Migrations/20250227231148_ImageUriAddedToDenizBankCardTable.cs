using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Denizbank.Migrations
{
    /// <inheritdoc />
    public partial class ImageUriAddedToDenizBankCardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURI",
                table: "DenizBankCard",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURI",
                table: "DenizBankCard");
        }
    }
}
