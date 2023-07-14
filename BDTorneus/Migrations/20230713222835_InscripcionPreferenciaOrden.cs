using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDTorneus.Migrations
{
    /// <inheritdoc />
    public partial class InscripcionPreferenciaOrden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrdenPagoMP",
                table: "Inscripciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferenciaMP",
                table: "Inscripciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrdenPagoMP",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "PreferenciaMP",
                table: "Inscripciones");
        }
    }
}
