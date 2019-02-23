using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic.DataAccess.Migrations
{
    public partial class ChangeUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Insurance",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Login",
                table: "Users",
                newName: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Login");

            migrationBuilder.AddColumn<string>(
                name: "Insurance",
                table: "Users",
                nullable: true);
        }
    }
}
