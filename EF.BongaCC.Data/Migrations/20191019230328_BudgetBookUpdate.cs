using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BudgetBookUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NAPIMSBUDGETDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "NAPIMSBUDGETFDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "NAPIMSBUDGETNaira",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "OPYearBudgetDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "OPYearBudgetFDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "OPYearBudgetNaira",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q1FYLEDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q1FYLEFDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q1FYLENaira",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q2FYLEDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q2FYLEFDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q2FYLENaira",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q3FYLEDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q3FYLEFDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q3FYLENaira",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q4FYLEDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q4FYLEFDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "Q4FYLENaira",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "YYear",
                table: "BudgetBook");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "NAPIMSBUDGETDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NAPIMSBUDGETFDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NAPIMSBUDGETNaira",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OPYearBudgetDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OPYearBudgetFDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OPYearBudgetNaira",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q1FYLEDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q1FYLEFDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q1FYLENaira",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q2FYLEDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q2FYLEFDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q2FYLENaira",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q3FYLEDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q3FYLEFDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q3FYLENaira",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q4FYLEDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q4FYLEFDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Q4FYLENaira",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "YYear",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0);
        }
    }
}
