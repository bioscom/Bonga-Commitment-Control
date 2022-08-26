using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BudgetBookCommitmentsXXX5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetBookCommitments",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Commitment = table.Column<decimal>(nullable: false),
                    CCPSessionDate = table.Column<DateTime>(nullable: false),
                    FocalPointID = table.Column<long>(nullable: true),
                    BudgetBookID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetBookCommitments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BudgetBookCommitments_BudgetBook_BudgetBookID",
                        column: x => x.BudgetBookID,
                        principalTable: "BudgetBook",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBookCommitments_AppUsers_FocalPointID",
                        column: x => x.FocalPointID,
                        principalTable: "AppUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBook_CapexOpexID",
                table: "BudgetBook",
                column: "CapexOpexID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBookCommitments_BudgetBookID",
                table: "BudgetBookCommitments",
                column: "BudgetBookID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBookCommitments_FocalPointID",
                table: "BudgetBookCommitments",
                column: "FocalPointID");

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

            migrationBuilder.DropTable(
                name: "BudgetBookCommitments");

            migrationBuilder.DropIndex(
                name: "IX_BudgetBook_CapexOpexID",
                table: "BudgetBook");

            migrationBuilder.AddColumn<int>(
                name: "CapexOpex",
                table: "BudgetBook",
                nullable: false,
                defaultValue: 0);
        }
    }
}
