using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BudgetBookDeductions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BudgetBookID",
                table: "ActivityDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDetails_BudgetBookID",
                table: "ActivityDetails",
                column: "BudgetBookID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDetails_BudgetBook_BudgetBookID",
                table: "ActivityDetails",
                column: "BudgetBookID",
                principalTable: "BudgetBook",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDetails_BudgetBook_BudgetBookID",
                table: "ActivityDetails");

            migrationBuilder.DropIndex(
                name: "IX_ActivityDetails_BudgetBookID",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "BudgetBookID",
                table: "ActivityDetails");
        }
    }
}
