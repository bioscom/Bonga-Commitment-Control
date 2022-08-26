using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BudgetBookCommitmentsXXX7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BudgetBookCommitmentsID",
                table: "FileUpload",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalComment",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ApprovalID",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ApproverID",
                table: "BudgetBookCommitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Comitmntno",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Savings",
                table: "BudgetBookCommitments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "BudgetBookCommitmentsID",
                table: "ActivityDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_BudgetBookCommitmentsID",
                table: "FileUpload",
                column: "BudgetBookCommitmentsID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBookCommitments_ApprovalID",
                table: "BudgetBookCommitments",
                column: "ApprovalID");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDetails_BudgetBookCommitmentsID",
                table: "ActivityDetails",
                column: "BudgetBookCommitmentsID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDetails_BudgetBookCommitments_BudgetBookCommitmentsID",
                table: "ActivityDetails",
                column: "BudgetBookCommitmentsID",
                principalTable: "BudgetBookCommitments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBookCommitments_Discipline_ApprovalID",
                table: "BudgetBookCommitments",
                column: "ApprovalID",
                principalTable: "Discipline",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileUpload_BudgetBookCommitments_BudgetBookCommitmentsID",
                table: "FileUpload",
                column: "BudgetBookCommitmentsID",
                principalTable: "BudgetBookCommitments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDetails_BudgetBookCommitments_BudgetBookCommitmentsID",
                table: "ActivityDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBookCommitments_Discipline_ApprovalID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileUpload_BudgetBookCommitments_BudgetBookCommitmentsID",
                table: "FileUpload");

            migrationBuilder.DropIndex(
                name: "IX_FileUpload_BudgetBookCommitmentsID",
                table: "FileUpload");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBookCommitments_ApprovalID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropIndex(
                name: "IX_ActivityDetails_BudgetBookCommitmentsID",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "BudgetBookCommitmentsID",
                table: "FileUpload");

            migrationBuilder.DropColumn(
                name: "ApprovalComment",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "ApprovalID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "ApproverID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "Comitmntno",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "Savings",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "BudgetBookCommitmentsID",
                table: "ActivityDetails");
        }
    }
}
