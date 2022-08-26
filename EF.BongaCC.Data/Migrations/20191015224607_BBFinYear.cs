using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BBFinYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetBookFinanceYear",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    BudgetBookID = table.Column<long>(nullable: true),
                    OPYearBudgetNaira = table.Column<decimal>(nullable: false),
                    OPYearBudgetDollar = table.Column<decimal>(nullable: false),
                    OPYearBudgetFDollar = table.Column<decimal>(nullable: false),
                    NAPIMSBUDGETNaira = table.Column<decimal>(nullable: false),
                    NAPIMSBUDGETDollar = table.Column<decimal>(nullable: false),
                    NAPIMSBUDGETFDollar = table.Column<decimal>(nullable: false),
                    Q1FYLENaira = table.Column<decimal>(nullable: false),
                    Q1FYLEDollar = table.Column<decimal>(nullable: false),
                    Q1FYLEFDollar = table.Column<decimal>(nullable: false),
                    Q2FYLENaira = table.Column<decimal>(nullable: false),
                    Q2FYLEDollar = table.Column<decimal>(nullable: false),
                    Q2FYLEFDollar = table.Column<decimal>(nullable: false),
                    Q3FYLENaira = table.Column<decimal>(nullable: false),
                    Q3FYLEDollar = table.Column<decimal>(nullable: false),
                    Q3FYLEFDollar = table.Column<decimal>(nullable: false),
                    Q4FYLENaira = table.Column<decimal>(nullable: false),
                    Q4FYLEDollar = table.Column<decimal>(nullable: false),
                    Q4FYLEFDollar = table.Column<decimal>(nullable: false),
                    YYear = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetBookFinanceYear", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BudgetBookFinanceYear_BudgetBook_BudgetBookID",
                        column: x => x.BudgetBookID,
                        principalTable: "BudgetBook",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBookFinanceYear_BudgetBookID",
                table: "BudgetBookFinanceYear",
                column: "BudgetBookID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetBookFinanceYear");
        }
    }
}
