using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAApplication.Migrations
{
    public partial class _17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_AspNetUsers_UserId",
                table: "Application");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Application",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Application_UserId",
                table: "Application",
                newName: "IX_Application_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_AspNetUsers_UserID",
                table: "Application",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_AspNetUsers_UserID",
                table: "Application");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Application",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Application_UserID",
                table: "Application",
                newName: "IX_Application_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_AspNetUsers_UserId",
                table: "Application",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
