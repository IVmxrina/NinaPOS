using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinaPOS.Migrations
{
    /// <inheritdoc />
    public partial class FuncionesCobro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MetodoPago",
                table: "Transacciones",
                newName: "CantidadTarjeta");

            migrationBuilder.AddColumn<decimal>(
                name: "CantidadEfectivo",
                table: "Transacciones",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadEfectivo",
                table: "Transacciones");

            migrationBuilder.RenameColumn(
                name: "CantidadTarjeta",
                table: "Transacciones",
                newName: "MetodoPago");
        }
    }
}
