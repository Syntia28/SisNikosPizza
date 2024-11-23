using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class fdjsiofsdifo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaPedido",
                table: "DetallePedido");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "DetallePedido");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoPedido",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRecogo",
                table: "pedido",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mesa",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Referencia",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoPedido",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedido_ProductoId",
                table: "DetallePedido",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallePedido_producto_ProductoId",
                table: "DetallePedido",
                column: "ProductoId",
                principalTable: "producto",
                principalColumn: "ProductoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallePedido_producto_ProductoId",
                table: "DetallePedido");

            migrationBuilder.DropIndex(
                name: "IX_DetallePedido_ProductoId",
                table: "DetallePedido");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "EstadoPedido",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "FechaRecogo",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "Mesa",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "Referencia",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "TipoPedido",
                table: "pedido");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPedido",
                table: "DetallePedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "DetallePedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
