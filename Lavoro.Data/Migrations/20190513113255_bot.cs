using Microsoft.EntityFrameworkCore.Migrations;

namespace Lavoro.Data.Migrations
{
    public partial class bot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Bot",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bot",
                table: "Users");
        }
    }
}
