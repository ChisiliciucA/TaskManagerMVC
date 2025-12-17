using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagerMVC.Migrations
{
    public partial class AddConversionCountColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversionCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionCount",
                table: "Users");
        }
    }
}
