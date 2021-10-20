using Microsoft.EntityFrameworkCore.Migrations;

namespace MessingSystem.Migrations
{
    public partial class DbUpdate041021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Item",
                table: "ExtraMessings");

            migrationBuilder.AddColumn<int>(
                name: "ItemType",
                table: "ExtraMessings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ExtraMessings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "ExtraMessings");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ExtraMessings");

            migrationBuilder.AddColumn<string>(
                name: "Item",
                table: "ExtraMessings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
