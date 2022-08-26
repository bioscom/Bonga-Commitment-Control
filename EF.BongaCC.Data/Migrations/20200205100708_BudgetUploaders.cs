using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BudgetUploaders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YYear",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YYear",
                table: "BudgetBook");
        }
    }
}
