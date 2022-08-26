using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class CommitmentBBYear2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "iYear",
                table: "BudgetBookCommitments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "iYear",
                table: "ActivityDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "iYear",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "iYear",
                table: "ActivityDetails");
        }
    }
}
