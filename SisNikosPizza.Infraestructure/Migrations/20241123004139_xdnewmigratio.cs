using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class xdnewmigratio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "carrito",
                columns: table => new
                {
                    CarritoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrito", x => x.CarritoId);
                    table.ForeignKey(
                        name: "FK_carrito_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "carritoProductos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    CarritoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carritoProductos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_carritoProductos_carrito_CarritoId",
                        column: x => x.CarritoId,
                        principalTable: "carrito",
                        principalColumn: "CarritoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_carritoProductos_producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "producto",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_carrito_OwnerId",
                table: "carrito",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_carritoProductos_CarritoId",
                table: "carritoProductos",
                column: "CarritoId");

            migrationBuilder.CreateIndex(
                name: "IX_carritoProductos_ProductoId",
                table: "carritoProductos",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carritoProductos");

            migrationBuilder.DropTable(
                name: "carrito");
        }
    }
}
