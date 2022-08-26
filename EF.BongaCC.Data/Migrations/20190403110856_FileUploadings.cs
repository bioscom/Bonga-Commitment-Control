using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class FileUploadings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "approvalID",
                table: "Commitments",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateTable(
                name: "FileUpload",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 60, nullable: false),
                    UploadFiles = table.Column<byte[]>(nullable: false),
                    CommitmentID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUpload", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FileUpload_Commitments_CommitmentID",
                        column: x => x.CommitmentID,
                        principalTable: "Commitments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_CommitmentID",
                table: "FileUpload",
                column: "CommitmentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileUpload");

            migrationBuilder.AlterColumn<long>(
                name: "approvalID",
                table: "Commitments",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
