using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Follower.Migrations
{
    /// <inheritdoc />
    public partial class InicialSeguridadYRutasSaaS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Programas_Educativos",
                columns: table => new
                {
                    IdPrograma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombrePrograma = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programas_Educativos", x => x.IdPrograma);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreRol = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Secciones_Personalizadas",
                columns: table => new
                {
                    IdSeccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPrograma = table.Column<int>(type: "int", nullable: false),
                    TituloSeccion = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrdenAparicion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secciones_Personalizadas", x => x.IdSeccion);
                    table.ForeignKey(
                        name: "FK_Secciones_Personalizadas_Programas_Educativos_IdPrograma",
                        column: x => x.IdPrograma,
                        principalTable: "Programas_Educativos",
                        principalColumn: "IdPrograma",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Correo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContrasenaHash = table.Column<string>(type: "varchar(265)", maxLength: 265, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Campos_Personalizados",
                columns: table => new
                {
                    IdCampo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdSeccion = table.Column<int>(type: "int", nullable: false),
                    NombreEtiqueta = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDato = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsObligatorio = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campos_Personalizados", x => x.IdCampo);
                    table.ForeignKey(
                        name: "FK_Campos_Personalizados_Secciones_Personalizadas_IdSeccion",
                        column: x => x.IdSeccion,
                        principalTable: "Secciones_Personalizadas",
                        principalColumn: "IdSeccion",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Egresados",
                columns: table => new
                {
                    IdEgresados = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdPrograma = table.Column<int>(type: "int", nullable: false),
                    EstadoSolicitud = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrivacidadPerfil = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombres = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoPaterno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoMaterno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaEgreso = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ciudad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pais = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FotoPerfilUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AutorizaDirectorio = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaAceptacionPrivacidad = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaResolucion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IdUsuarioResolvio = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egresados", x => x.IdEgresados);
                    table.ForeignKey(
                        name: "FK_Egresados_Programas_Educativos_IdPrograma",
                        column: x => x.IdPrograma,
                        principalTable: "Programas_Educativos",
                        principalColumn: "IdPrograma",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Egresados_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Egresados_Usuarios_IdUsuarioResolvio",
                        column: x => x.IdUsuarioResolvio,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Programas_Responsables",
                columns: table => new
                {
                    IdProgramaResponsable = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPrograma = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programas_Responsables", x => x.IdProgramaResponsable);
                    table.ForeignKey(
                        name: "FK_Programas_Responsables_Programas_Educativos_IdPrograma",
                        column: x => x.IdPrograma,
                        principalTable: "Programas_Educativos",
                        principalColumn: "IdPrograma",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Programas_Responsables_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Opciones_Catalogo_Campo",
                columns: table => new
                {
                    IdOpcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCampo = table.Column<int>(type: "int", nullable: false),
                    ValorOpcion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opciones_Catalogo_Campo", x => x.IdOpcion);
                    table.ForeignKey(
                        name: "FK_Opciones_Catalogo_Campo_Campos_Personalizados_IdCampo",
                        column: x => x.IdCampo,
                        principalTable: "Campos_Personalizados",
                        principalColumn: "IdCampo",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Valores_Campo_Egresados",
                columns: table => new
                {
                    IdValor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdEgresado = table.Column<int>(type: "int", nullable: false),
                    IdCampo = table.Column<int>(type: "int", nullable: false),
                    ValorCapturado = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valores_Campo_Egresados", x => x.IdValor);
                    table.ForeignKey(
                        name: "FK_Valores_Campo_Egresados_Campos_Personalizados_IdCampo",
                        column: x => x.IdCampo,
                        principalTable: "Campos_Personalizados",
                        principalColumn: "IdCampo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Valores_Campo_Egresados_Egresados_IdEgresado",
                        column: x => x.IdEgresado,
                        principalTable: "Egresados",
                        principalColumn: "IdEgresados",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Campos_Personalizados_IdSeccion",
                table: "Campos_Personalizados",
                column: "IdSeccion");

            migrationBuilder.CreateIndex(
                name: "IX_Egresados_IdPrograma",
                table: "Egresados",
                column: "IdPrograma");

            migrationBuilder.CreateIndex(
                name: "IX_Egresados_IdUsuario",
                table: "Egresados",
                column: "IdUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Egresados_IdUsuarioResolvio",
                table: "Egresados",
                column: "IdUsuarioResolvio");

            migrationBuilder.CreateIndex(
                name: "IX_Opciones_Catalogo_Campo_IdCampo",
                table: "Opciones_Catalogo_Campo",
                column: "IdCampo");

            migrationBuilder.CreateIndex(
                name: "IX_Programas_Responsables_IdPrograma_IdUsuario",
                table: "Programas_Responsables",
                columns: new[] { "IdPrograma", "IdUsuario" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programas_Responsables_IdUsuario",
                table: "Programas_Responsables",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_Personalizadas_IdPrograma",
                table: "Secciones_Personalizadas",
                column: "IdPrograma");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Valores_Campo_Egresados_IdCampo",
                table: "Valores_Campo_Egresados",
                column: "IdCampo");

            migrationBuilder.CreateIndex(
                name: "IX_Valores_Campo_Egresados_IdEgresado",
                table: "Valores_Campo_Egresados",
                column: "IdEgresado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opciones_Catalogo_Campo");

            migrationBuilder.DropTable(
                name: "Programas_Responsables");

            migrationBuilder.DropTable(
                name: "Valores_Campo_Egresados");

            migrationBuilder.DropTable(
                name: "Campos_Personalizados");

            migrationBuilder.DropTable(
                name: "Egresados");

            migrationBuilder.DropTable(
                name: "Secciones_Personalizadas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Programas_Educativos");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
