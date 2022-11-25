using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAApplication.Migrations
{
    public partial class _123456 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    x = table.Column<int>(type: "int", nullable: false),
                    y = table.Column<int>(type: "int", nullable: false),
                    SlotID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_position", x => x.ID);
                    table.ForeignKey(
                        name: "FK_position_Slot_SlotID",
                        column: x => x.SlotID,
                        principalTable: "Slot",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_position_SlotID",
                table: "position",
                column: "SlotID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "position");
        }
    }
}
