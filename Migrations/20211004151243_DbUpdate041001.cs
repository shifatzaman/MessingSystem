using Microsoft.EntityFrameworkCore.Migrations;

namespace MessingSystem.Migrations
{
    public partial class DbUpdate041001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "role",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "unitprice",
                table: "inventoryitems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "users");

            migrationBuilder.DropColumn(
                name: "unitprice",
                table: "inventoryitems");
        }
    }
}
