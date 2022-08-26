using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class CXOXUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CapexOpexID",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_CapexOpexID",
                table: "BudgetBook",
                column: "CapexOpexID");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetBook_CapexOpex_CapexOpexID",
                table: "BudgetBook",
                column: "CapexOpexID",
                principalTable: "CapexOpex",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetBook_CapexOpex_CapexOpexID",
                table: "BudgetBook");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBook_CapexOpexID",
                table: "BudgetBook");

            migrationBuilder.DropColumn(
                name: "CapexOpexID",
                table: "BudgetBook");

            migrationBuilder.AddColumn<int>(
                name: "CapexOpex",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0);
        }
    }
}
