using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Follower.Migrations
{
    /// <inheritdoc />
    public partial class correccionnombre2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IncorporacionPosgradoñ",
                table: "Situaciones_Laborales",
                newName: "IncorporacionPosgrado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IncorporacionPosgrado",
                table: "Situaciones_Laborales",
                newName: "IncorporacionPosgradoñ");
        }
    }
}
