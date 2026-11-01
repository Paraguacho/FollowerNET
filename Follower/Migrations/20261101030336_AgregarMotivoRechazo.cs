using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Follower.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMotivoRechazo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdEgresados",
                table: "Egresados",
                newName: "IdEgresado");

            migrationBuilder.AddColumn<string>(
                name: "Siglas",
                table: "Programas_Educativos",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "FechaEgreso",
                table: "Egresados",
                type: "int",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoRechazo",
                table: "Egresados",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Siglas",
                table: "Programas_Educativos");

            migrationBuilder.DropColumn(
                name: "MotivoRechazo",
                table: "Egresados");

            migrationBuilder.RenameColumn(
                name: "IdEgresado",
                table: "Egresados",
                newName: "IdEgresados");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaEgreso",
                table: "Egresados",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
