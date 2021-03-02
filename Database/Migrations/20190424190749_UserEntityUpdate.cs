using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class UserEntityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "AspNetUsers",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Documents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "Surname");
        }
    }
}
