using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BudgetBookUpgrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityCode",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ActivityCodeDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCode", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UapCode",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    UapCodeDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UapCode", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UapRollUpCode",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    UapRollUpCodeDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UapRollUpCode", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BudgetBook",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CapexOpexID = table.Column<long>(nullable: false),
                    DirectAllocated = table.Column<int>(nullable: false),
                    UapCodeID = table.Column<long>(nullable: false),
                    UapRollUpCodeID = table.Column<long>(nullable: false),
                    ActivityTypeID = table.Column<long>(nullable: false),
                    ActivityCodeID = table.Column<long>(nullable: false),
                    wbsID = table.Column<long>(nullable: false),
                    ActivityID = table.Column<long>(nullable: false),
                    ActivityOwnerID = table.Column<long>(nullable: true),
                    SponsorID = table.Column<long>(nullable: true),
                    ScopeID = table.Column<long>(nullable: false),
                    ContractID = table.Column<long>(nullable: false),
                    BudgetBasisID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_BudgetBook", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BudgetBook_ActivityCode_ActivityCodeID",
                        column: x => x.ActivityCodeID,
                        principalTable: "ActivityCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBook_Activity_ActivityID",
                        column: x => x.ActivityID,
                        principalTable: "Activity",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBook_ActivityType_ActivityTypeID",
                        column: x => x.ActivityTypeID,
                        principalTable: "ActivityType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBook_BudgetBasis_BudgetBasisID",
                        column: x => x.BudgetBasisID,
                        principalTable: "BudgetBasis",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBook_Contract_ContractID",
                        column: x => x.ContractID,
                        principalTable: "Contract",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBook_Scope_ScopeID",
                        column: x => x.ScopeID,
                        principalTable: "Scope",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBook_AppUsers_SponsorID",
                        column: x => x.SponsorID,
                        principalTable: "AppUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetBook_UapCode_UapCodeID",
                        column: x => x.UapCodeID,
                        principalTable: "UapCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBook_UapRollUpCode_UapRollUpCodeID",
                        column: x => x.UapRollUpCodeID,
                        principalTable: "UapRollUpCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBook_WBS_wbsID",
                        column: x => x.wbsID,
                        principalTable: "WBS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    //table.ForeignKey(
                    //    name: "FK_BudgetBook_CXOX_CapexOpexID",
                    //    column: x => x.CapexOpexID,
                    //    principalTable: "CapexOpex",
                    //    principalColumn: "ID",
                    //    onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_ActivityCodeID",
                table: "BudgetBook",
                column: "ActivityCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_ActivityID",
                table: "BudgetBook",
                column: "ActivityID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_ActivityTypeID",
                table: "BudgetBook",
                column: "ActivityTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_BudgetBasisID",
                table: "BudgetBook",
                column: "BudgetBasisID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_ContractID",
                table: "BudgetBook",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_ScopeID",
                table: "BudgetBook",
                column: "ScopeID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_SponsorID",
                table: "BudgetBook",
                column: "SponsorID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_UapCodeID",
                table: "BudgetBook",
                column: "UapCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_UapRollUpCodeID",
                table: "BudgetBook",
                column: "UapRollUpCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_wbsID",
                table: "BudgetBook",
                column: "wbsID");
            //migrationBuilder.CreateIndex(
            //    name: "IX_BudgetBook_CapexOpexID",
            //    table: "BudgetBook",
            //    column: "CapexOpexID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetBook");

            migrationBuilder.DropTable(
                name: "ActivityCode");

            migrationBuilder.DropTable(
                name: "UapCode");

            migrationBuilder.DropTable(
                name: "UapRollUpCode");
        }
    }
}
