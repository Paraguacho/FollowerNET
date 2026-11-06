using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Follower.Migrations
{
    /// <inheritdoc />
    public partial class cambioNombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PerfilesAcademicos_Egresados_EgresadoId",
                table: "PerfilesAcademicos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerfilesAcademicos",
                table: "PerfilesAcademicos");

            migrationBuilder.RenameTable(
                name: "PerfilesAcademicos",
                newName: "Perfiles_Academicos");

            migrationBuilder.RenameIndex(
                name: "IX_PerfilesAcademicos_EgresadoId",
                table: "Perfiles_Academicos",
                newName: "IX_Perfiles_Academicos_EgresadoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Perfiles_Academicos",
                table: "Perfiles_Academicos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfiles_Academicos_Egresados_EgresadoId",
                table: "Perfiles_Academicos",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfiles_Academicos_Egresados_EgresadoId",
                table: "Perfiles_Academicos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Perfiles_Academicos",
                table: "Perfiles_Academicos");

            migrationBuilder.RenameTable(
                name: "Perfiles_Academicos",
                newName: "PerfilesAcademicos");

            migrationBuilder.RenameIndex(
                name: "IX_Perfiles_Academicos_EgresadoId",
                table: "PerfilesAcademicos",
                newName: "IX_PerfilesAcademicos_EgresadoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerfilesAcademicos",
                table: "PerfilesAcademicos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PerfilesAcademicos_Egresados_EgresadoId",
                table: "PerfilesAcademicos",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
