using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionPedidoPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Pagado",
                table: "pedido",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pagado",
                table: "pedido");
        }
    }
}
