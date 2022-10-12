using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAApplication.Migrations
{
    public partial class ApplicationModel4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "resumePath",
                table: "Application",
                newName: "resume");

            migrationBuilder.RenameColumn(
                name: "photoPath",
                table: "Application",
                newName: "photo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "resume",
                table: "Application",
                newName: "resumePath");

            migrationBuilder.RenameColumn(
                name: "photo",
                table: "Application",
                newName: "photoPath");
        }
    }
}
