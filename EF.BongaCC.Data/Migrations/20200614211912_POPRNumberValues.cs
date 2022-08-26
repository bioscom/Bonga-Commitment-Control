using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class POPRNumberValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PONumber",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "POValue",
                table: "BudgetBookCommitments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PRNumber",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PRValue",
                table: "BudgetBookCommitments",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PONumber",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "POValue",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "PRNumber",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "PRValue",
                table: "BudgetBookCommitments");
        }
    }
}
