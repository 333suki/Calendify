using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class EventToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Event_EventID",
                table: "Attendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Events_EventID",
                table: "Attendance",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Events_EventID",
                table: "Attendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Event_EventID",
                table: "Attendance",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
