using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class fjoidsjifods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "venta",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_venta_OwnerId",
                table: "venta",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_venta_AspNetUsers_OwnerId",
                table: "venta",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_venta_AspNetUsers_OwnerId",
                table: "venta");

            migrationBuilder.DropIndex(
                name: "IX_venta_OwnerId",
                table: "venta");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "venta");
        }
    }
}
