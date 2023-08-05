using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDTorneus.Migrations
{
    /// <inheritdoc />
    public partial class TorneoPartido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PartidoSiguienteGuidId",
                table: "Partidos",
                newName: "PartidoSigPerdedor");

            migrationBuilder.RenameColumn(
                name: "GuidId",
                table: "Partidos",
                newName: "PartidoSigGanador");

            migrationBuilder.AddColumn<bool>(
                name: "DisparadorSiguienteFase",
                table: "Partidos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EstadoPartido",
                table: "Partidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grupo",
                table: "Partidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GuidPartido",
                table: "Partidos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "RondaDescanso",
                table: "Partidos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SeleccionEquipoDelGrupo",
                table: "Partidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_EquipoLocalId",
                table: "Partidos",
                column: "EquipoLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_EquipoVisitanteId",
                table: "Partidos",
                column: "EquipoVisitanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partidos_Equipos_EquipoLocalId",
                table: "Partidos",
                column: "EquipoLocalId",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Partidos_Equipos_EquipoVisitanteId",
                table: "Partidos",
                column: "EquipoVisitanteId",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partidos_Equipos_EquipoLocalId",
                table: "Partidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Partidos_Equipos_EquipoVisitanteId",
                table: "Partidos");

            migrationBuilder.DropIndex(
                name: "IX_Partidos_EquipoLocalId",
                table: "Partidos");

            migrationBuilder.DropIndex(
                name: "IX_Partidos_EquipoVisitanteId",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "DisparadorSiguienteFase",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "EstadoPartido",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "Grupo",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "GuidPartido",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "RondaDescanso",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "SeleccionEquipoDelGrupo",
                table: "Partidos");

            migrationBuilder.RenameColumn(
                name: "PartidoSigPerdedor",
                table: "Partidos",
                newName: "PartidoSiguienteGuidId");

            migrationBuilder.RenameColumn(
                name: "PartidoSigGanador",
                table: "Partidos",
                newName: "GuidId");
        }
    }
}
