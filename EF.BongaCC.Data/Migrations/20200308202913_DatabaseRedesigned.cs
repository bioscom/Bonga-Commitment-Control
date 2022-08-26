using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class DatabaseRedesigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBook_AppUsers_SponsorID",
                table: "BudgetBook");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBook_SponsorID",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "ActivityOwnerID",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "SponsorID",
                table: "BudgetBook");

            migrationBuilder.AddColumn<long>(
                name: "AppUsersID",
                table: "BudgetBook",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NAPIMSBUDGETDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NAPIMSBUDGETFDollar",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NAPIMSBUDGETNaira",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "AccountableManagerID",
                table: "ActivityCode",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActivityOwnerID",
                table: "ActivityCode",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_AppUsersID",
                table: "BudgetBook",
                column: "AppUsersID");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBook_AppUsers_AppUsersID",
                table: "BudgetBook",
                column: "AppUsersID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBook_AppUsers_AppUsersID",
                table: "BudgetBook");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBook_AppUsersID",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "AppUsersID",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "NAPIMSBUDGETDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "NAPIMSBUDGETFDollar",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "NAPIMSBUDGETNaira",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "AccountableManagerID",
                table: "ActivityCode");

            migrationBuilder.DropColumn(
                name: "ActivityOwnerID",
                table: "ActivityCode");

            migrationBuilder.AddColumn<long>(
                name: "ActivityOwnerID",
                table: "BudgetBook",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SponsorID",
                table: "BudgetBook",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_SponsorID",
                table: "BudgetBook",
                column: "SponsorID");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBook_AppUsers_SponsorID",
                table: "BudgetBook",
                column: "SponsorID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
