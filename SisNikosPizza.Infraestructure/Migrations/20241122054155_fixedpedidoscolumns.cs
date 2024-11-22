using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class fixedpedidoscolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pedido_AspNetUsers_OwnerId",
                table: "pedido");

            migrationBuilder.DropIndex(
                name: "IX_pedido_OwnerId",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "NombreProducto",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "PrecioTotal",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "PrecioUnitario",
                table: "pedido");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "pedido",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreProducto",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "PrecioTotal",
                table: "pedido",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PrecioUnitario",
                table: "pedido",
                type: "real",
                nullable: false,
                defaultValue: 0f);

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
    }
}
