using Microsoft.EntityFrameworkCore.Migrations;

namespace MessingSystem.Migrations
{
    public partial class DbUpdate041004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Item",
                table: "CafeterialBills");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Item",
                table: "CafeterialBills",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
