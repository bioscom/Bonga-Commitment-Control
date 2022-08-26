using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class FixedExchangeRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FixedExchangeRate",
                table: "ActivityDetails",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedExchangeRate",
                table: "ActivityDetails");
        }
    }
}
