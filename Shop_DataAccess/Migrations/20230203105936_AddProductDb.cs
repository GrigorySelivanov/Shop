using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestModelId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_TestModelId",
                table: "Product",
                column: "TestModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_TestModel_TestModelId",
                table: "Product",
                column: "TestModelId",
                principalTable: "TestModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_TestModel_TestModelId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_TestModelId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TestModelId",
                table: "Product");
        }
    }
}
