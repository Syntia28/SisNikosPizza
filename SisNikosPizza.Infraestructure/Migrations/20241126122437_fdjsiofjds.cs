using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class fdjsiofjds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVenta_producto_ProductoId",
                table: "DetalleVenta");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVenta_producto_ProductoId",
                table: "DetalleVenta",
                column: "ProductoId",
                principalTable: "producto",
                principalColumn: "ProductoId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVenta_producto_ProductoId",
                table: "DetalleVenta");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVenta_producto_ProductoId",
                table: "DetalleVenta",
                column: "ProductoId",
                principalTable: "producto",
                principalColumn: "ProductoId");
        }
    }
}
