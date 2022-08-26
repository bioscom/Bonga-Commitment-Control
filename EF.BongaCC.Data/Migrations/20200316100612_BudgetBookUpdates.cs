using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BudgetBookUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ActivityOwnerID",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LineManagerID",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SponsorID",
                table: "BudgetBookCommitments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityOwnerID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "LineManagerID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "SponsorID",
                table: "BudgetBookCommitments");
        }
    }
}
