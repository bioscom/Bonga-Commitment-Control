using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class Workstream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ActivityCodeWorkStreamID",
                table: "ActivityCode",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityCodeWorkStream",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    WorkStream = table.Column<string>(nullable: true),
                    WorkStreamDesc = table.Column<string>(nullable: true),
                    WorkFlowType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCodeWorkStream", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCode_ActivityCodeWorkStreamID",
                table: "ActivityCode",
                column: "ActivityCodeWorkStreamID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityCode_ActivityCodeWorkStream_ActivityCodeWorkStreamID",
                table: "ActivityCode",
                column: "ActivityCodeWorkStreamID",
                principalTable: "ActivityCodeWorkStream",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityCode_ActivityCodeWorkStream_ActivityCodeWorkStreamID",
                table: "ActivityCode");

            migrationBuilder.DropTable(
                name: "ActivityCodeWorkStream");

            migrationBuilder.DropIndex(
                name: "IX_ActivityCode_ActivityCodeWorkStreamID",
                table: "ActivityCode");

            migrationBuilder.DropColumn(
                name: "ActivityCodeWorkStreamID",
                table: "ActivityCode");
        }
    }
}
