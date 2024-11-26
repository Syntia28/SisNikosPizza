using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class xjiofdjso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "pedido",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_OwnerId",
                table: "pedido",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_pedido_AspNetUsers_OwnerId",
                table: "pedido",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pedido_AspNetUsers_OwnerId",
                table: "pedido");

            migrationBuilder.DropIndex(
                name: "IX_pedido_OwnerId",
                table: "pedido");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
