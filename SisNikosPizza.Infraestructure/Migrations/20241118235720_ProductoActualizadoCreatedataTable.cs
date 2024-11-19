using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductoActualizadoCreatedataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Precio",
                table: "producto",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "producto",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
