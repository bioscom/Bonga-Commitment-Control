using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BudgetingBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BudgetBasisID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "BudgetBasis",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    BudgetBase = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetBasis", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_BudgetBasisID",
                table: "Commitments",
                column: "BudgetBasisID");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_BudgetBasis_BudgetBasisID",
                table: "Commitments",
                column: "BudgetBasisID",
                principalTable: "BudgetBasis",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_BudgetBasis_BudgetBasisID",
                table: "Commitments");

            migrationBuilder.DropTable(
                name: "BudgetBasis");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_BudgetBasisID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "BudgetBasisID",
                table: "Commitments");
        }
    }
}
