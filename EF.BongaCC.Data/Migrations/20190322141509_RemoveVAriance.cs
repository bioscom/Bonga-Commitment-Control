using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class RemoveVAriance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDetails_Commitments_CommitmentID",
                table: "ActivityDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_AppUsers_focalpointID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "variance",
                table: "Commitments");

            migrationBuilder.AlterColumn<long>(
                name: "focalpointID",
                table: "Commitments",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "CommitmentID",
                table: "ActivityDetails",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDetails_Commitments_CommitmentID",
                table: "ActivityDetails",
                column: "CommitmentID",
                principalTable: "Commitments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_AppUsers_focalpointID",
                table: "Commitments",
                column: "focalpointID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDetails_Commitments_CommitmentID",
                table: "ActivityDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_AppUsers_focalpointID",
                table: "Commitments");

            migrationBuilder.AlterColumn<long>(
                name: "focalpointID",
                table: "Commitments",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "variance",
                table: "Commitments",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CommitmentID",
                table: "ActivityDetails",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDetails_Commitments_CommitmentID",
                table: "ActivityDetails",
                column: "CommitmentID",
                principalTable: "Commitments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_AppUsers_focalpointID",
                table: "Commitments",
                column: "focalpointID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
