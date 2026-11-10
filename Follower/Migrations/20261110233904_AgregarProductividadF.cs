using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Follower.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProductividadF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coautores-Productos_Productividades-Cientificas_Productivida~",
                table: "Coautores-Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Contribuciones-Sociales_Egresados_EgresadoId",
                table: "Contribuciones-Sociales");

            migrationBuilder.DropForeignKey(
                name: "FK_Productividades-Cientificas_Egresados_EgresadoId",
                table: "Productividades-Cientificas");

            migrationBuilder.DropForeignKey(
                name: "FK_Proyectos-Investigacion_Egresados_EgresadoId",
                table: "Proyectos-Investigacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Reconocomientos-Academicos_Egresados_EgresadoId",
                table: "Reconocomientos-Academicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumenes-Productividad_Egresados_EgresadoId",
                table: "Resumenes-Productividad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resumenes-Productividad",
                table: "Resumenes-Productividad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reconocomientos-Academicos",
                table: "Reconocomientos-Academicos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proyectos-Investigacion",
                table: "Proyectos-Investigacion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Productividades-Cientificas",
                table: "Productividades-Cientificas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contribuciones-Sociales",
                table: "Contribuciones-Sociales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coautores-Productos",
                table: "Coautores-Productos");

            migrationBuilder.RenameTable(
                name: "Resumenes-Productividad",
                newName: "Resumenes_Productividad");

            migrationBuilder.RenameTable(
                name: "Reconocomientos-Academicos",
                newName: "Reconocomientos_Academicos");

            migrationBuilder.RenameTable(
                name: "Proyectos-Investigacion",
                newName: "Proyectos_Investigacion");

            migrationBuilder.RenameTable(
                name: "Productividades-Cientificas",
                newName: "Productividades_Cientificas");

            migrationBuilder.RenameTable(
                name: "Contribuciones-Sociales",
                newName: "Contribuciones_Sociales");

            migrationBuilder.RenameTable(
                name: "Coautores-Productos",
                newName: "Coautores_Productos");

            migrationBuilder.RenameIndex(
                name: "IX_Resumenes-Productividad_EgresadoId",
                table: "Resumenes_Productividad",
                newName: "IX_Resumenes_Productividad_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Reconocomientos-Academicos_EgresadoId",
                table: "Reconocomientos_Academicos",
                newName: "IX_Reconocomientos_Academicos_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Proyectos-Investigacion_EgresadoId",
                table: "Proyectos_Investigacion",
                newName: "IX_Proyectos_Investigacion_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Productividades-Cientificas_EgresadoId",
                table: "Productividades_Cientificas",
                newName: "IX_Productividades_Cientificas_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Contribuciones-Sociales_EgresadoId",
                table: "Contribuciones_Sociales",
                newName: "IX_Contribuciones_Sociales_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Coautores-Productos_ProductividadId",
                table: "Coautores_Productos",
                newName: "IX_Coautores_Productos_ProductividadId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resumenes_Productividad",
                table: "Resumenes_Productividad",
                column: "IdResumen");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reconocomientos_Academicos",
                table: "Reconocomientos_Academicos",
                column: "IdReconomiento");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proyectos_Investigacion",
                table: "Proyectos_Investigacion",
                column: "IdProyecto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Productividades_Cientificas",
                table: "Productividades_Cientificas",
                column: "IdProductividad");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contribuciones_Sociales",
                table: "Contribuciones_Sociales",
                column: "IdContribucion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coautores_Productos",
                table: "Coautores_Productos",
                column: "IdCoautor");

            migrationBuilder.AddForeignKey(
                name: "FK_Coautores_Productos_Productividades_Cientificas_Productivida~",
                table: "Coautores_Productos",
                column: "ProductividadId",
                principalTable: "Productividades_Cientificas",
                principalColumn: "IdProductividad",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contribuciones_Sociales_Egresados_EgresadoId",
                table: "Contribuciones_Sociales",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productividades_Cientificas_Egresados_EgresadoId",
                table: "Productividades_Cientificas",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proyectos_Investigacion_Egresados_EgresadoId",
                table: "Proyectos_Investigacion",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reconocomientos_Academicos_Egresados_EgresadoId",
                table: "Reconocomientos_Academicos",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resumenes_Productividad_Egresados_EgresadoId",
                table: "Resumenes_Productividad",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coautores_Productos_Productividades_Cientificas_Productivida~",
                table: "Coautores_Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Contribuciones_Sociales_Egresados_EgresadoId",
                table: "Contribuciones_Sociales");

            migrationBuilder.DropForeignKey(
                name: "FK_Productividades_Cientificas_Egresados_EgresadoId",
                table: "Productividades_Cientificas");

            migrationBuilder.DropForeignKey(
                name: "FK_Proyectos_Investigacion_Egresados_EgresadoId",
                table: "Proyectos_Investigacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Reconocomientos_Academicos_Egresados_EgresadoId",
                table: "Reconocomientos_Academicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumenes_Productividad_Egresados_EgresadoId",
                table: "Resumenes_Productividad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resumenes_Productividad",
                table: "Resumenes_Productividad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reconocomientos_Academicos",
                table: "Reconocomientos_Academicos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proyectos_Investigacion",
                table: "Proyectos_Investigacion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Productividades_Cientificas",
                table: "Productividades_Cientificas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contribuciones_Sociales",
                table: "Contribuciones_Sociales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coautores_Productos",
                table: "Coautores_Productos");

            migrationBuilder.RenameTable(
                name: "Resumenes_Productividad",
                newName: "Resumenes-Productividad");

            migrationBuilder.RenameTable(
                name: "Reconocomientos_Academicos",
                newName: "Reconocomientos-Academicos");

            migrationBuilder.RenameTable(
                name: "Proyectos_Investigacion",
                newName: "Proyectos-Investigacion");

            migrationBuilder.RenameTable(
                name: "Productividades_Cientificas",
                newName: "Productividades-Cientificas");

            migrationBuilder.RenameTable(
                name: "Contribuciones_Sociales",
                newName: "Contribuciones-Sociales");

            migrationBuilder.RenameTable(
                name: "Coautores_Productos",
                newName: "Coautores-Productos");

            migrationBuilder.RenameIndex(
                name: "IX_Resumenes_Productividad_EgresadoId",
                table: "Resumenes-Productividad",
                newName: "IX_Resumenes-Productividad_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Reconocomientos_Academicos_EgresadoId",
                table: "Reconocomientos-Academicos",
                newName: "IX_Reconocomientos-Academicos_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Proyectos_Investigacion_EgresadoId",
                table: "Proyectos-Investigacion",
                newName: "IX_Proyectos-Investigacion_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Productividades_Cientificas_EgresadoId",
                table: "Productividades-Cientificas",
                newName: "IX_Productividades-Cientificas_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Contribuciones_Sociales_EgresadoId",
                table: "Contribuciones-Sociales",
                newName: "IX_Contribuciones-Sociales_EgresadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Coautores_Productos_ProductividadId",
                table: "Coautores-Productos",
                newName: "IX_Coautores-Productos_ProductividadId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resumenes-Productividad",
                table: "Resumenes-Productividad",
                column: "IdResumen");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reconocomientos-Academicos",
                table: "Reconocomientos-Academicos",
                column: "IdReconomiento");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proyectos-Investigacion",
                table: "Proyectos-Investigacion",
                column: "IdProyecto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Productividades-Cientificas",
                table: "Productividades-Cientificas",
                column: "IdProductividad");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contribuciones-Sociales",
                table: "Contribuciones-Sociales",
                column: "IdContribucion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coautores-Productos",
                table: "Coautores-Productos",
                column: "IdCoautor");

            migrationBuilder.AddForeignKey(
                name: "FK_Coautores-Productos_Productividades-Cientificas_Productivida~",
                table: "Coautores-Productos",
                column: "ProductividadId",
                principalTable: "Productividades-Cientificas",
                principalColumn: "IdProductividad",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contribuciones-Sociales_Egresados_EgresadoId",
                table: "Contribuciones-Sociales",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productividades-Cientificas_Egresados_EgresadoId",
                table: "Productividades-Cientificas",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proyectos-Investigacion_Egresados_EgresadoId",
                table: "Proyectos-Investigacion",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reconocomientos-Academicos_Egresados_EgresadoId",
                table: "Reconocomientos-Academicos",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resumenes-Productividad_Egresados_EgresadoId",
                table: "Resumenes-Productividad",
                column: "EgresadoId",
                principalTable: "Egresados",
                principalColumn: "IdEgresado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
