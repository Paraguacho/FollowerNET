using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Follower.Migrations
{
    /// <inheritdoc />
    public partial class formacionActual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsFormacionActual",
                table: "Formaciones_Academicas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsFormacionActual",
                table: "Formaciones_Academicas");
        }
    }
}
