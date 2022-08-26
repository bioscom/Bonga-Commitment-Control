using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class ActivityType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ActivityTypeID",
                table: "Commitments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ActivityType",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ActivityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityType", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_ActivityTypeID",
                table: "Commitments",
                column: "ActivityTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_ActivityType_ActivityTypeID",
                table: "Commitments",
                column: "ActivityTypeID",
                principalTable: "ActivityType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_ActivityType_ActivityTypeID",
                table: "Commitments");

            migrationBuilder.DropTable(
                name: "ActivityType");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_ActivityTypeID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "ActivityTypeID",
                table: "Commitments");
        }
    }
}
