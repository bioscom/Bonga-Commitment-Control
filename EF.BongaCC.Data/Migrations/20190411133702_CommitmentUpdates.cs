using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class CommitmentUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_Activity_ActivityID",
                table: "Commitments");

            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_ActivityType_ActivityTypeID",
                table: "Commitments");

            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_BudgetBasis_BudgetBasisID",
                table: "Commitments");

            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_Contract_ContractID",
                table: "Commitments");

            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_Scope_ScopeID",
                table: "Commitments");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_ActivityID",
                table: "Commitments");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_ActivityTypeID",
                table: "Commitments");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_BudgetBasisID",
                table: "Commitments");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_ContractID",
                table: "Commitments");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_ScopeID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "ActivityID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "ActivityTypeID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "BudgetBasisID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "ContractID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "ScopeID",
                table: "Commitments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ActivityID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ActivityTypeID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "BudgetBasisID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ContractID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ScopeID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_ActivityID",
                table: "Commitments",
                column: "ActivityID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_ActivityTypeID",
                table: "Commitments",
                column: "ActivityTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_BudgetBasisID",
                table: "Commitments",
                column: "BudgetBasisID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_ContractID",
                table: "Commitments",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_ScopeID",
                table: "Commitments",
                column: "ScopeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_Activity_ActivityID",
                table: "Commitments",
                column: "ActivityID",
                principalTable: "Activity",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_ActivityType_ActivityTypeID",
                table: "Commitments",
                column: "ActivityTypeID",
                principalTable: "ActivityType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_BudgetBasis_BudgetBasisID",
                table: "Commitments",
                column: "BudgetBasisID",
                principalTable: "BudgetBasis",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_Contract_ContractID",
                table: "Commitments",
                column: "ContractID",
                principalTable: "Contract",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_Scope_ScopeID",
                table: "Commitments",
                column: "ScopeID",
                principalTable: "Scope",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
