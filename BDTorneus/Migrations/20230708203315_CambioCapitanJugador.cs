using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDTorneus.Migrations
{
    /// <inheritdoc />
    public partial class CambioCapitanJugador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscripciones_Usuarios_UsuarioId",
                table: "Inscripciones");

            migrationBuilder.DropIndex(
                name: "IX_Inscripciones_UsuarioId",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "Capitan",
                table: "Jugadores");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Inscripciones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Capitan",
                table: "Equipos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InscripcionUsuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    InscripcionesTorneoId = table.Column<int>(type: "int", nullable: false),
                    InscripcionesEquipoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscripcionUsuario", x => new { x.UsuarioId, x.InscripcionesTorneoId, x.InscripcionesEquipoId });
                    table.ForeignKey(
                        name: "FK_InscripcionUsuario_Inscripciones_InscripcionesTorneoId_InscripcionesEquipoId",
                        columns: x => new { x.InscripcionesTorneoId, x.InscripcionesEquipoId },
                        principalTable: "Inscripciones",
                        principalColumns: new[] { "TorneoId", "EquipoId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InscripcionUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InscripcionUsuario_InscripcionesTorneoId_InscripcionesEquipoId",
                table: "InscripcionUsuario",
                columns: new[] { "InscripcionesTorneoId", "InscripcionesEquipoId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InscripcionUsuario");

            migrationBuilder.DropColumn(
                name: "Capitan",
                table: "Equipos");

            migrationBuilder.AddColumn<bool>(
                name: "Capitan",
                table: "Jugadores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Inscripciones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_UsuarioId",
                table: "Inscripciones",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripciones_Usuarios_UsuarioId",
                table: "Inscripciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
