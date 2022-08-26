using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BudgetUploaderTransformed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetUploaderTransformeds",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ActivityType = table.Column<string>(nullable: true),
                    DirectAllocated = table.Column<string>(nullable: true),
                    UapCode = table.Column<string>(nullable: true),
                    UapRollUpCode = table.Column<string>(nullable: true),
                    ActivityName = table.Column<string>(nullable: true),
                    ActivityCode = table.Column<string>(nullable: true),
                    LineManager = table.Column<string>(nullable: true),
                    CostCenter = table.Column<string>(nullable: true),
                    Activity = table.Column<string>(nullable: true),
                    ActivityOwner = table.Column<string>(nullable: true),
                    AccountableManager = table.Column<string>(nullable: true),
                    ScopePurpose = table.Column<string>(nullable: true),
                    Contract = table.Column<string>(nullable: true),
                    Budgetbasis = table.Column<string>(nullable: true),
                    OPYearBudget = table.Column<decimal>(nullable: false),
                    YYear = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetUploaderTransformeds", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetUploaderTransformeds");
        }
    }
}
