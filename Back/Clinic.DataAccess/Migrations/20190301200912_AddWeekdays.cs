using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic.DataAccess.Migrations
{
    public partial class AddWeekdays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VisitTime",
                table: "Schedules",
                newName: "VisitDuration");

            migrationBuilder.AddColumn<string>(
                name: "Weekdays",
                table: "Schedules",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weekdays",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "VisitDuration",
                table: "Schedules",
                newName: "VisitTime");
        }
    }
}
