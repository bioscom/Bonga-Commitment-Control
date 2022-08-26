using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class FloatingExRateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExchangeRateValue",
                table: "ExchangeRate",
                newName: "FloatingExchangeRate");

            migrationBuilder.AddColumn<int>(
                name: "iDay",
                table: "ExchangeRate",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "iDay",
                table: "ExchangeRate");

            migrationBuilder.RenameColumn(
                name: "FloatingExchangeRate",
                table: "ExchangeRate",
                newName: "ExchangeRateValue");
        }
    }
}
