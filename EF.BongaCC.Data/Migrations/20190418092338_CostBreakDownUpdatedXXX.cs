using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class CostBreakDownUpdatedXXX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDetails_Commitments_CommitmentID",
                table: "ActivityDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBookCommitments_BudgetBook_BudgetBookID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropIndex(
                name: "IX_ActivityDetails_CommitmentID",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "CommitmentID",
                table: "ActivityDetails");

            migrationBuilder.AlterColumn<long>(
                name: "BudgetBookID",
                table: "BudgetBookCommitments",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBookCommitments_BudgetBook_BudgetBookID",
                table: "BudgetBookCommitments",
                column: "BudgetBookID",
                principalTable: "BudgetBook",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBookCommitments_BudgetBook_BudgetBookID",
                table: "BudgetBookCommitments");

            migrationBuilder.AlterColumn<long>(
                name: "BudgetBookID",
                table: "BudgetBookCommitments",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CommitmentID",
                table: "ActivityDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDetails_CommitmentID",
                table: "ActivityDetails",
                column: "CommitmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDetails_Commitments_CommitmentID",
                table: "ActivityDetails",
                column: "CommitmentID",
                principalTable: "Commitments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBookCommitments_BudgetBook_BudgetBookID",
                table: "BudgetBookCommitments",
                column: "BudgetBookID",
                principalTable: "BudgetBook",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
