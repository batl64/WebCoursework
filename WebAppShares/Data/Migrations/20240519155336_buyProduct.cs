using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppShares.Data.Migrations
{
    /// <inheritdoc />
    public partial class buyProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyProducts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BuyProductPurchasedGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsModelId = table.Column<int>(type: "int", nullable: false),
                    BuyProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyProductPurchasedGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyProductPurchasedGoods_BuyProducts_BuyProductId",
                        column: x => x.BuyProductId,
                        principalTable: "BuyProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyProductPurchasedGoods_Products_ProductsModelId",
                        column: x => x.ProductsModelId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyProductPurchasedGoods_BuyProductId",
                table: "BuyProductPurchasedGoods",
                column: "BuyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyProductPurchasedGoods_ProductsModelId",
                table: "BuyProductPurchasedGoods",
                column: "ProductsModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyProducts_UserId",
                table: "BuyProducts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyProductPurchasedGoods");

            migrationBuilder.DropTable(
                name: "BuyProducts");
        }
    }
}
