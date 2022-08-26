using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class NairaFunctionalDollarUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NairaRate",
                table: "ActivityDetails");

            migrationBuilder.AddColumn<long>(
                name: "CurrenciesID",
                table: "ActivityDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDetails_CurrenciesID",
                table: "ActivityDetails",
                column: "CurrenciesID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDetails_Currencies_CurrenciesID",
                table: "ActivityDetails",
                column: "CurrenciesID",
                principalTable: "Currencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDetails_Currencies_CurrenciesID",
                table: "ActivityDetails");

            migrationBuilder.DropIndex(
                name: "IX_ActivityDetails_CurrenciesID",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "CurrenciesID",
                table: "ActivityDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "NairaRate",
                table: "ActivityDetails",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
