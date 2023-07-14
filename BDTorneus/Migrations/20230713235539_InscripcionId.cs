using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDTorneus.Migrations
{
    /// <inheritdoc />
    public partial class InscripcionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Inscripciones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Inscripciones");
        }
    }
}
