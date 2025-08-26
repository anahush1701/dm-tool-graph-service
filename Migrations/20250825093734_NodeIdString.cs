using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphService.Migrations
{
    /// <inheritdoc />
    public partial class NodeIdString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropPrimaryKey(
                name: "PK_Nodes",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Nodes");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Nodes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");
 
            migrationBuilder.AddPrimaryKey(
                name: "PK_Nodes",
                table: "Nodes",
                column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Target",
                table: "Links",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "Links",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Nodes",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Nodes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Nodes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nodes",
                table: "Nodes",
                column: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Target",
                table: "Links",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "Links",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
