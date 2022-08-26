using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class Activity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ActivityID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_ActivityID",
                table: "Commitments",
                column: "ActivityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_Activity_ActivityID",
                table: "Commitments",
                column: "ActivityID",
                principalTable: "Activity",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_Activity_ActivityID",
                table: "Commitments");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_ActivityID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "ActivityID",
                table: "Commitments");
        }
    }
}
