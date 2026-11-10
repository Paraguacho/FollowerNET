using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Follower.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProductividad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contribuciones-Sociales",
                columns: table => new
                {
                    IdContribucion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EgresadoId = table.Column<int>(type: "int", nullable: false),
                    NombreActividad = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoContribucion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SectorAtendido = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resultados = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoEvidencia = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contribuciones-Sociales", x => x.IdContribucion);
                    table.ForeignKey(
                        name: "FK_Contribuciones-Sociales_Egresados_EgresadoId",
                        column: x => x.EgresadoId,
                        principalTable: "Egresados",
                        principalColumn: "IdEgresado",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Productividades-Cientificas",
                columns: table => new
                {
                    IdProductividad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EgresadoId = table.Column<int>(type: "int", nullable: false),
                    TipoProducto = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Titulo = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RevistaOLibro = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Editorial = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnoPublicacion = table.Column<int>(type: "int", nullable: true),
                    DoiUrl = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Isbn = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Participacion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoPdf = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreProfesorMied = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productividades-Cientificas", x => x.IdProductividad);
                    table.ForeignKey(
                        name: "FK_Productividades-Cientificas_Egresados_EgresadoId",
                        column: x => x.EgresadoId,
                        principalTable: "Egresados",
                        principalColumn: "IdEgresado",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Proyectos-Investigacion",
                columns: table => new
                {
                    IdProyecto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EgresadoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Institucion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Periodo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Funcion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos-Investigacion", x => x.IdProyecto);
                    table.ForeignKey(
                        name: "FK_Proyectos-Investigacion_Egresados_EgresadoId",
                        column: x => x.EgresadoId,
                        principalTable: "Egresados",
                        principalColumn: "IdEgresado",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reconocomientos-Academicos",
                columns: table => new
                {
                    IdReconomiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EgresadoId = table.Column<int>(type: "int", nullable: false),
                    TipoRegistro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nivel = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnoInicio = table.Column<int>(type: "int", nullable: true),
                    AnoFin = table.Column<int>(type: "int", nullable: false),
                    NombreReconocimiento = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstitucionOtorgante = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotivoOCategoria = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoParticipacion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstadoActual = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoEvidencia = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reconocomientos-Academicos", x => x.IdReconomiento);
                    table.ForeignKey(
                        name: "FK_Reconocomientos-Academicos_Egresados_EgresadoId",
                        column: x => x.EgresadoId,
                        principalTable: "Egresados",
                        principalColumn: "IdEgresado",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Resumenes-Productividad",
                columns: table => new
                {
                    IdResumen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EgresadoId = table.Column<int>(type: "int", nullable: false),
                    TienePremios = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TieneCertificaciones = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TieneMembresias = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TieneContribucionSocial = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumenes-Productividad", x => x.IdResumen);
                    table.ForeignKey(
                        name: "FK_Resumenes-Productividad_Egresados_EgresadoId",
                        column: x => x.EgresadoId,
                        principalTable: "Egresados",
                        principalColumn: "IdEgresado",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Coautores-Productos",
                columns: table => new
                {
                    IdCoautor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductividadId = table.Column<int>(type: "int", nullable: false),
                    Nombres = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoPaterno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoMaterno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rol = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coautores-Productos", x => x.IdCoautor);
                    table.ForeignKey(
                        name: "FK_Coautores-Productos_Productividades-Cientificas_Productivida~",
                        column: x => x.ProductividadId,
                        principalTable: "Productividades-Cientificas",
                        principalColumn: "IdProductividad",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Coautores-Productos_ProductividadId",
                table: "Coautores-Productos",
                column: "ProductividadId");

            migrationBuilder.CreateIndex(
                name: "IX_Contribuciones-Sociales_EgresadoId",
                table: "Contribuciones-Sociales",
                column: "EgresadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Productividades-Cientificas_EgresadoId",
                table: "Productividades-Cientificas",
                column: "EgresadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos-Investigacion_EgresadoId",
                table: "Proyectos-Investigacion",
                column: "EgresadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reconocomientos-Academicos_EgresadoId",
                table: "Reconocomientos-Academicos",
                column: "EgresadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Resumenes-Productividad_EgresadoId",
                table: "Resumenes-Productividad",
                column: "EgresadoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coautores-Productos");

            migrationBuilder.DropTable(
                name: "Contribuciones-Sociales");

            migrationBuilder.DropTable(
                name: "Proyectos-Investigacion");

            migrationBuilder.DropTable(
                name: "Reconocomientos-Academicos");

            migrationBuilder.DropTable(
                name: "Resumenes-Productividad");

            migrationBuilder.DropTable(
                name: "Productividades-Cientificas");
        }
    }
}
