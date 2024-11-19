using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class chanagecategoriaidProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_producto_categoria_CategoriaId",
                table: "producto");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "producto",
                newName: "Categoriaid");

            migrationBuilder.RenameIndex(
                name: "IX_producto_CategoriaId",
                table: "producto",
                newName: "IX_producto_Categoriaid");

            migrationBuilder.AlterColumn<string>(
                name: "ImagenUrl",
                table: "producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_producto_categoria_Categoriaid",
                table: "producto",
                column: "Categoriaid",
                principalTable: "categoria",
                principalColumn: "CategoriaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_producto_categoria_Categoriaid",
                table: "producto");

            migrationBuilder.RenameColumn(
                name: "Categoriaid",
                table: "producto",
                newName: "CategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_producto_Categoriaid",
                table: "producto",
                newName: "IX_producto_CategoriaId");

            migrationBuilder.AlterColumn<string>(
                name: "ImagenUrl",
                table: "producto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_producto_categoria_CategoriaId",
                table: "producto",
                column: "CategoriaId",
                principalTable: "categoria",
                principalColumn: "CategoriaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
