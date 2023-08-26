using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDTorneus.Migrations
{
    /// <inheritdoc />
    public partial class AutorizacionesPlanilleroProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutorizacionesPlanilleros_Usuarios_UsuarioIdOrganizadorId",
                table: "AutorizacionesPlanilleros");

            migrationBuilder.DropForeignKey(
                name: "FK_AutorizacionesPlanilleros_Usuarios_UsuarioIdPlanilleroId",
                table: "AutorizacionesPlanilleros");

            migrationBuilder.RenameColumn(
                name: "UsuarioIdPlanilleroId",
                table: "AutorizacionesPlanilleros",
                newName: "UsuarioPlanilleroId");

            migrationBuilder.RenameColumn(
                name: "UsuarioIdOrganizadorId",
                table: "AutorizacionesPlanilleros",
                newName: "UsuarioOrganizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_AutorizacionesPlanilleros_UsuarioIdPlanilleroId",
                table: "AutorizacionesPlanilleros",
                newName: "IX_AutorizacionesPlanilleros_UsuarioPlanilleroId");

            migrationBuilder.RenameIndex(
                name: "IX_AutorizacionesPlanilleros_UsuarioIdOrganizadorId",
                table: "AutorizacionesPlanilleros",
                newName: "IX_AutorizacionesPlanilleros_UsuarioOrganizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutorizacionesPlanilleros_Usuarios_UsuarioOrganizadorId",
                table: "AutorizacionesPlanilleros",
                column: "UsuarioOrganizadorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AutorizacionesPlanilleros_Usuarios_UsuarioPlanilleroId",
                table: "AutorizacionesPlanilleros",
                column: "UsuarioPlanilleroId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutorizacionesPlanilleros_Usuarios_UsuarioOrganizadorId",
                table: "AutorizacionesPlanilleros");

            migrationBuilder.DropForeignKey(
                name: "FK_AutorizacionesPlanilleros_Usuarios_UsuarioPlanilleroId",
                table: "AutorizacionesPlanilleros");

            migrationBuilder.RenameColumn(
                name: "UsuarioPlanilleroId",
                table: "AutorizacionesPlanilleros",
                newName: "UsuarioIdPlanilleroId");

            migrationBuilder.RenameColumn(
                name: "UsuarioOrganizadorId",
                table: "AutorizacionesPlanilleros",
                newName: "UsuarioIdOrganizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_AutorizacionesPlanilleros_UsuarioPlanilleroId",
                table: "AutorizacionesPlanilleros",
                newName: "IX_AutorizacionesPlanilleros_UsuarioIdPlanilleroId");

            migrationBuilder.RenameIndex(
                name: "IX_AutorizacionesPlanilleros_UsuarioOrganizadorId",
                table: "AutorizacionesPlanilleros",
                newName: "IX_AutorizacionesPlanilleros_UsuarioIdOrganizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutorizacionesPlanilleros_Usuarios_UsuarioIdOrganizadorId",
                table: "AutorizacionesPlanilleros",
                column: "UsuarioIdOrganizadorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AutorizacionesPlanilleros_Usuarios_UsuarioIdPlanilleroId",
                table: "AutorizacionesPlanilleros",
                column: "UsuarioIdPlanilleroId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
