using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MessingSystem.Migrations
{
    public partial class DbUpdate04102503 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfEntry",
                table: "Rooms",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfEntry",
                table: "Rooms");
        }
    }
}
