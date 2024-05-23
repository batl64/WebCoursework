using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppShares.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdressBuyProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "BuyProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "BuyProducts");
        }
    }
}
