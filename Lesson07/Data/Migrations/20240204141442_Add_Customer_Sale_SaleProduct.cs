using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lesson07.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Customer_Sale_SaleProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Sale_SaleId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_SaleId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "SupplyProductId",
                table: "SaleProduct",
                newName: "ProductId");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "SaleProduct",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "SaleProduct",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "SaleProduct",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDiscount",
                table: "Sale",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDue",
                table: "Sale",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPaid",
                table: "Sale",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleProduct_ProductId",
                table: "SaleProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProduct_SaleId",
                table: "SaleProduct",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CustomerId",
                table: "Sale",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Customer_CustomerId",
                table: "Sale",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProduct_Product_ProductId",
                table: "SaleProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProduct_Sale_SaleId",
                table: "SaleProduct",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Customer_CustomerId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleProduct_Product_ProductId",
                table: "SaleProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleProduct_Sale_SaleId",
                table: "SaleProduct");

            migrationBuilder.DropIndex(
                name: "IX_SaleProduct_ProductId",
                table: "SaleProduct");

            migrationBuilder.DropIndex(
                name: "IX_SaleProduct_SaleId",
                table: "SaleProduct");

            migrationBuilder.DropIndex(
                name: "IX_Sale_CustomerId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "TotalDiscount",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "TotalDue",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "TotalPaid",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "SaleProduct",
                newName: "SupplyProductId");

            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_SaleId",
                table: "Customer",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Sale_SaleId",
                table: "Customer",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
