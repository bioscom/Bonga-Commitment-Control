using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class AppUserLineManagerMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_LineManagerID",
                table: "AppUsers",
                column: "LineManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AppUsers_LineManagerID",
                table: "AppUsers",
                column: "LineManagerID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AppUsers_LineManagerID",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_LineManagerID",
                table: "AppUsers");
        }
    }
}
