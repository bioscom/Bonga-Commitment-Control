using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class MoreFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sPeriodfrom",
                table: "Commitments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contractnovendor",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "groupID",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "justification",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sPeriodfrom",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "statusID",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "teamID",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "threat",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "typeID",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "vehicleID",
                table: "BudgetBookCommitments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBookCommitments_groupID",
                table: "BudgetBookCommitments",
                column: "groupID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBookCommitments_statusID",
                table: "BudgetBookCommitments",
                column: "statusID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBookCommitments_teamID",
                table: "BudgetBookCommitments",
                column: "teamID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBookCommitments_typeID",
                table: "BudgetBookCommitments",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBookCommitments_vehicleID",
                table: "BudgetBookCommitments",
                column: "vehicleID");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBookCommitments_PurchasingGroup_groupID",
                table: "BudgetBookCommitments",
                column: "groupID",
                principalTable: "PurchasingGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBookCommitments_RequestStatus_statusID",
                table: "BudgetBookCommitments",
                column: "statusID",
                principalTable: "RequestStatus",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBookCommitments_Team_teamID",
                table: "BudgetBookCommitments",
                column: "teamID",
                principalTable: "Team",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBookCommitments_PlannedEmmergency_typeID",
                table: "BudgetBookCommitments",
                column: "typeID",
                principalTable: "PlannedEmmergency",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBookCommitments_ContractProcurementVehicle_vehicleID",
                table: "BudgetBookCommitments",
                column: "vehicleID",
                principalTable: "ContractProcurementVehicle",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBookCommitments_PurchasingGroup_groupID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBookCommitments_RequestStatus_statusID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBookCommitments_Team_teamID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBookCommitments_PlannedEmmergency_typeID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBookCommitments_ContractProcurementVehicle_vehicleID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBookCommitments_groupID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBookCommitments_statusID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBookCommitments_teamID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBookCommitments_typeID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBookCommitments_vehicleID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "sPeriodfrom",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "contractnovendor",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "groupID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "justification",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "sPeriodfrom",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "statusID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "teamID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "threat",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "typeID",
                table: "BudgetBookCommitments");

            migrationBuilder.DropColumn(
                name: "vehicleID",
                table: "BudgetBookCommitments");
        }
    }
}
