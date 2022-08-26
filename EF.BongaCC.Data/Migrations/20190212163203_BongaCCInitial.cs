using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.BongaCC.Data.Migrations
{
    public partial class BongaCCInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    UserMail = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    RefInd = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    LoginTime = table.Column<DateTime>(nullable: false),
                    IsGuestAcct = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Deligate = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    AreaName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    AssetName = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ContractProcurementVehicle",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    VehicleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractProcurementVehicle", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Discipline",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Decision = table.Column<string>(nullable: true),
                    ColorCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discipline", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRate",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ExchangeRateValue = table.Column<decimal>(nullable: false),
                    YYear = table.Column<int>(nullable: false),
                    MMonth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRate", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PlannedEmmergency",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    PlanEmmerType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedEmmergency", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PurchasingGroup",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    GroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingGroup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatus",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ReqstStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TeamName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WBS",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CostObjects = table.Column<string>(nullable: true),
                    CostObjectsDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WBS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    FacilityName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Agg = table.Column<int>(nullable: false),
                    Location = table.Column<int>(nullable: false),
                    AreaID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Facility_Area_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commitments",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    TeamIndicator = table.Column<string>(nullable: true),
                    CapexOpex = table.Column<int>(nullable: false),
                    PRNumber = table.Column<string>(nullable: true),
                    PRValue = table.Column<decimal>(nullable: false),
                    PONumber = table.Column<string>(nullable: true),
                    POValue = table.Column<decimal>(nullable: false),
                    comitmntno = table.Column<string>(nullable: true),
                    commitment = table.Column<decimal>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    justification = table.Column<string>(nullable: true),
                    threat = table.Column<string>(nullable: true),
                    contractnovendor = table.Column<string>(nullable: true),
                    periodfrom = table.Column<DateTime>(nullable: false),
                    periodto = table.Column<DateTime>(nullable: false),
                    previous = table.Column<DateTime>(nullable: false),
                    napimsNaira = table.Column<decimal>(nullable: false),
                    napimsDollar = table.Column<decimal>(nullable: false),
                    napimsFunctionalDollar = table.Column<decimal>(nullable: false),
                    requestNaira = table.Column<decimal>(nullable: false),
                    requestDollar = table.Column<decimal>(nullable: false),
                    requestFunctionalDollar = table.Column<decimal>(nullable: false),
                    groupID = table.Column<long>(nullable: false),
                    typeID = table.Column<long>(nullable: false),
                    statusID = table.Column<long>(nullable: false),
                    vehicleID = table.Column<long>(nullable: false),
                    assetID = table.Column<long>(nullable: false),
                    facilityID = table.Column<long>(nullable: false),
                    departmenID = table.Column<long>(nullable: false),
                    wbsID = table.Column<long>(nullable: false),
                    teamID = table.Column<long>(nullable: false),
                    sponsorID = table.Column<long>(nullable: false),
                    approvalID = table.Column<long>(nullable: false),
                    ApprovalDecisionID = table.Column<long>(nullable: true),
                    approvalComment = table.Column<string>(nullable: true),
                    savings = table.Column<decimal>(nullable: false),
                    variance = table.Column<string>(nullable: true),
                    AreaID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commitments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Commitments_Discipline_ApprovalDecisionID",
                        column: x => x.ApprovalDecisionID,
                        principalTable: "Discipline",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commitments_Area_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commitments_Asset_assetID",
                        column: x => x.assetID,
                        principalTable: "Asset",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commitments_Department_departmenID",
                        column: x => x.departmenID,
                        principalTable: "Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commitments_Facility_facilityID",
                        column: x => x.facilityID,
                        principalTable: "Facility",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commitments_PurchasingGroup_groupID",
                        column: x => x.groupID,
                        principalTable: "PurchasingGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commitments_AppUsers_sponsorID",
                        column: x => x.sponsorID,
                        principalTable: "AppUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commitments_RequestStatus_statusID",
                        column: x => x.statusID,
                        principalTable: "RequestStatus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commitments_Team_teamID",
                        column: x => x.teamID,
                        principalTable: "Team",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commitments_PlannedEmmergency_typeID",
                        column: x => x.typeID,
                        principalTable: "PlannedEmmergency",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commitments_ContractProcurementVehicle_vehicleID",
                        column: x => x.vehicleID,
                        principalTable: "ContractProcurementVehicle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commitments_WBS_wbsID",
                        column: x => x.wbsID,
                        principalTable: "WBS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDetails",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    CommitmentID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ActivityDetails_Commitments_CommitmentID",
                        column: x => x.CommitmentID,
                        principalTable: "Commitments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDetails_CommitmentID",
                table: "ActivityDetails",
                column: "CommitmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_ApprovalDecisionID",
                table: "Commitments",
                column: "ApprovalDecisionID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_AreaID",
                table: "Commitments",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_assetID",
                table: "Commitments",
                column: "assetID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_departmenID",
                table: "Commitments",
                column: "departmenID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_facilityID",
                table: "Commitments",
                column: "facilityID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_groupID",
                table: "Commitments",
                column: "groupID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_sponsorID",
                table: "Commitments",
                column: "sponsorID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_statusID",
                table: "Commitments",
                column: "statusID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_teamID",
                table: "Commitments",
                column: "teamID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_typeID",
                table: "Commitments",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_vehicleID",
                table: "Commitments",
                column: "vehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_wbsID",
                table: "Commitments",
                column: "wbsID");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_AreaID",
                table: "Facility",
                column: "AreaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDetails");

            migrationBuilder.DropTable(
                name: "ExchangeRate");

            migrationBuilder.DropTable(
                name: "Commitments");

            migrationBuilder.DropTable(
                name: "Discipline");

            migrationBuilder.DropTable(
                name: "Asset");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "PurchasingGroup");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "RequestStatus");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "PlannedEmmergency");

            migrationBuilder.DropTable(
                name: "ContractProcurementVehicle");

            migrationBuilder.DropTable(
                name: "WBS");

            migrationBuilder.DropTable(
                name: "Area");
        }
    }
}
