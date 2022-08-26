using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class ActivityCodeLineManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LineManagerID",
                table: "ActivityCode",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCode_LineManagerID",
                table: "ActivityCode",
                column: "LineManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityCode_AppUsers_LineManagerID",
                table: "ActivityCode",
                column: "LineManagerID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityCode_AppUsers_LineManagerID",
                table: "ActivityCode");

            migrationBuilder.DropIndex(
                name: "IX_ActivityCode_LineManagerID",
                table: "ActivityCode");

            migrationBuilder.DropColumn(
                name: "LineManagerID",
                table: "ActivityCode");
        }
    }
}
