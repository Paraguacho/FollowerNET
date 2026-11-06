using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Follower.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTrayectoriaEgresado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experiencias_Laborales",
                columns: table => new
                {
                    IdExperienciaLaboral = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EgresadoId = table.Column<int>(type: "int", nullable: false),
                    Institucion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Puesto = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sector = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EsEmpleoActual = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ActividadesPrincipales = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NivelEducativo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RelacionConFormacion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiencias_Laborales", x => x.IdExperienciaLaboral);
                    table.ForeignKey(
                        name: "FK_Experiencias_Laborales_Egresados_EgresadoId",
                        column: x => x.EgresadoId,
                        principalTable: "Egresados",
                        principalColumn: "IdEgresado",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Formaciones_Academicas",
                columns: table => new
                {
                    IdFormacionAcademica = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EgresadoId = table.Column<int>(type: "int", nullable: false),
                    NombrePrograma = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Grado = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Institucion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pais = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstadoPrograma = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RecibioBeca = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TipoApoyo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstitucionOtorgante = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnoOtorgamiento = table.Column<int>(type: "int", nullable: true),
                    MontoAsignado = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formaciones_Academicas", x => x.IdFormacionAcademica);
                    table.ForeignKey(
                        name: "FK_Formaciones_Academicas_Egresados_EgresadoId",
                        column: x => x.EgresadoId,
                        principalTable: "Egresados",
                        principalColumn: "IdEgresado",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Situaciones_Laborales",
                columns: table => new
                {
                    IdSituacionLaboral = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EgreasdoId = table.Column<int>(type: "int", nullable: false),
                    EgresadoId = table.Column<int>(type: "int", nullable: false),
                    EstadoActual = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NuevoEmpleo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Promocion = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CambioInstitucion = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NuevasResponsabilidades = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IncorporacionPosgradoñ = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EmprendimientoPropio = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Situaciones_Laborales", x => x.IdSituacionLaboral);
                    table.ForeignKey(
                        name: "FK_Situaciones_Laborales_Egresados_EgresadoId",
                        column: x => x.EgresadoId,
                        principalTable: "Egresados",
                        principalColumn: "IdEgresado",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Experiencias_Laborales_EgresadoId",
                table: "Experiencias_Laborales",
                column: "EgresadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Formaciones_Academicas_EgresadoId",
                table: "Formaciones_Academicas",
                column: "EgresadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Situaciones_Laborales_EgresadoId",
                table: "Situaciones_Laborales",
                column: "EgresadoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experiencias_Laborales");

            migrationBuilder.DropTable(
                name: "Formaciones_Academicas");

            migrationBuilder.DropTable(
                name: "Situaciones_Laborales");
        }
    }
}
