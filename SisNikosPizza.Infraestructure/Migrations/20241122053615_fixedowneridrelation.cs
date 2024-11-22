using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisNikosPizza.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class fixedowneridrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pedido_AspNetUsers_OwnerId1",
                table: "pedido");

            migrationBuilder.DropIndex(
                name: "IX_pedido_OwnerId1",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "OwnerId1",
                table: "pedido");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "pedido",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "pedido",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId1",
                table: "pedido",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_pedido_OwnerId1",
                table: "pedido",
                column: "OwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_pedido_AspNetUsers_OwnerId1",
                table: "pedido",
                column: "OwnerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
