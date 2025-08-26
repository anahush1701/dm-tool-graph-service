using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphService.Migrations
{
    /// <inheritdoc />
    public partial class AddNewLinkProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Links",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "Links",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Links",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Links");
        }
    }
}
