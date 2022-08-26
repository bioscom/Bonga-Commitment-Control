using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class commitmentTblUdt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_Area_AreaID",
                table: "Commitments");

            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_Department_departmenID",
                table: "Commitments");

            migrationBuilder.DropIndex(
                name: "IX_Commitments_AreaID",
                table: "Commitments");

            migrationBuilder.DropColumn(
                name: "AreaID",
                table: "Commitments");

            migrationBuilder.RenameColumn(
                name: "departmenID",
                table: "Commitments",
                newName: "departmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Commitments_departmenID",
                table: "Commitments",
                newName: "IX_Commitments_departmentID");

            migrationBuilder.AlterColumn<bool>(
                name: "CapexOpex",
                table: "Commitments",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_Department_departmentID",
                table: "Commitments",
                column: "departmentID",
                principalTable: "Department",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitments_Department_departmentID",
                table: "Commitments");

            migrationBuilder.RenameColumn(
                name: "departmentID",
                table: "Commitments",
                newName: "departmenID");

            migrationBuilder.RenameIndex(
                name: "IX_Commitments_departmentID",
                table: "Commitments",
                newName: "IX_Commitments_departmenID");

            migrationBuilder.AlterColumn<int>(
                name: "CapexOpex",
                table: "Commitments",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<long>(
                name: "AreaID",
                table: "Commitments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_AreaID",
                table: "Commitments",
                column: "AreaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_Area_AreaID",
                table: "Commitments",
                column: "AreaID",
                principalTable: "Area",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commitments_Department_departmenID",
                table: "Commitments",
                column: "departmenID",
                principalTable: "Department",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
