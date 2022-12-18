using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bakers.Data.Migrations
{
    public partial class UpdateVariety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Variety",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Variety");
        }
    }
}
