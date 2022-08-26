using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BongaCCModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_AppUsers_sponsorID",
                table: "Commitments");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_sponsorID",
                table: "Commitments");

            migrationBuilder.AddColumn<long>(
                name: "approverID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "checkedbyID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "focalpointID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_focalpointID",
                table: "Commitments",
                column: "focalpointID");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_AppUsers_focalpointID",
                table: "Commitments",
                column: "focalpointID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_AppUsers_focalpointID",
                table: "Commitments");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_focalpointID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "approverID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "checkedbyID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "focalpointID",
                table: "Commitments");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_sponsorID",
                table: "Commitments",
                column: "sponsorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_AppUsers_sponsorID",
                table: "Commitments",
                column: "sponsorID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
