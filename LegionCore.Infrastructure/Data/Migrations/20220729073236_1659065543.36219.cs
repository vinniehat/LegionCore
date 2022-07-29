using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegionCore.Web.Data.Migrations
{
    public partial class _165906554336219 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "AspNetRoles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "AspNetRoles");
        }
    }
}
