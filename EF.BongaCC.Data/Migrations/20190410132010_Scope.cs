using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class Scope : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ScopeID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Scope",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Purpose = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scope", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_ScopeID",
                table: "Commitments",
                column: "ScopeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_Scope_ScopeID",
                table: "Commitments",
                column: "ScopeID",
                principalTable: "Scope",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_Scope_ScopeID",
                table: "Commitments");

            migrationBuilder.DropTable(
                name: "Scope");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_ScopeID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "ScopeID",
                table: "Commitments");
        }
    }
}
