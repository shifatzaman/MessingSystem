using Microsoft.EntityFrameworkCore.Migrations;

namespace MessingSystem.Migrations
{
    public partial class DbUpdate041005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MessMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MessMembers");
        }
    }
}
