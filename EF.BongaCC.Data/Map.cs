using System;
using EF.BongaCC.Core.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
//TODO: add-migration -context BongaCCDbContext [Parameter name]

namespace EF.BongaCC.Data
{
    public class CommitmentsMap
    {
        public CommitmentsMap(EntityTypeBuilder<Commitments> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.TeamIndicator);
            entityBuilder.Property(t => t.CapexOpex);
            entityBuilder.Property(t => t.PRNumber);
            entityBuilder.Property(t => t.PRValue);
            entityBuilder.Property(t => t.PONumber);
            entityBuilder.Property(t => t.POValue);
            entityBuilder.Property(t => t.comitmntno);
            entityBuilder.Property(t => t.commitment);
            entityBuilder.Property(t => t.title);
            entityBuilder.Property(t => t.justification);
            entityBuilder.Property(t => t.threat);
            entityBuilder.Property(t => t.contractnovendor);
            entityBuilder.Property(t => t.periodfrom);
            entityBuilder.Property(t => t.periodto);
            entityBuilder.Property(t => t.previous);
            entityBuilder.Property(t => t.napimsNaira);
            entityBuilder.Property(t => t.napimsDollar);
            entityBuilder.Property(t => t.napimsFunctionalDollar);
            entityBuilder.Property(t => t.requestNaira);
            entityBuilder.Property(t => t.requestDollar);
            entityBuilder.Property(t => t.requestFunctionalDollar);

            entityBuilder.Property(t => t.groupID);     // Foreign key
            entityBuilder.Property(t => t.typeID);      // Foreign key
            entityBuilder.Property(t => t.statusID);    // Foreign key
            entityBuilder.Property(t => t.vehicleID);   // Foreign key
            entityBuilder.Property(t => t.assetID);     // Foreign key
            entityBuilder.Property(t => t.facilityID);  // Foreign key
            entityBuilder.Property(t => t.departmentID); // Foreign key
            entityBuilder.Property(t => t.wbsID);       // Foreign key

            entityBuilder.Property(t => t.sponsorID);   // Foreign key
            entityBuilder.Property(t => t.checkedbyID); // Foreign key
            entityBuilder.Property(t => t.approverID);  // Foreign key
            //entityBuilder.Property(t => t.initiatorID); // Foreign key
            entityBuilder.Property(t => t.focalpointID);// Foreign key
            entityBuilder.Property(t => t.approvalID);  // Foreign key

            entityBuilder.Property(t => t.approvalComment);
            entityBuilder.Property(t => t.savings);
            //entityBuilder.Property(t => t.variance);

            //Relationships
            entityBuilder.HasOne(e => e.Teams).WithMany(e => e.Commitments).HasForeignKey(e => e.teamID);
            entityBuilder.HasOne(e => e.Asset).WithMany(e => e.Commitments).HasForeignKey(e => e.assetID);
            entityBuilder.HasOne(e => e.Department).WithMany(e => e.Commitments).HasForeignKey(e => e.departmentID);
            entityBuilder.HasOne(e => e.PlannedEmmergency).WithMany(e => e.Commitments).HasForeignKey(e => e.typeID);
            entityBuilder.HasOne(e => e.RequestStatus).WithMany(e => e.Commitments).HasForeignKey(e => e.statusID);
            entityBuilder.HasOne(e => e.ContractProcurementVehicle).WithMany(e => e.Commitments).HasForeignKey(e => e.vehicleID);
            entityBuilder.HasOne(e => e.Facility).WithMany(e => e.Commitments).HasForeignKey(e => e.facilityID);
            //entityBuilder.HasOne(e => e.WBS).WithMany(e => e.Commitments).HasForeignKey(e => e.wbsID);
            entityBuilder.HasOne(e => e.PurchasingGroup).WithMany(e => e.Commitments).HasForeignKey(e => e.groupID);
            //entityBuilder.HasOne(e => e.BudgetBasis).WithMany(e => e.Commitments).HasForeignKey(e => e.BudgetBasisID);
            //entityBuilder.HasOne(e => e.Activity).WithMany(e => e.Commitments).HasForeignKey(e => e.ActivityID);
            //entityBuilder.HasOne(e => e.Scope).WithMany(e => e.Commitments).HasForeignKey(e => e.ScopeID);
            //entityBuilder.HasOne(e => e.ActivityType).WithMany(e => e.Commitments).HasForeignKey(e => e.ActivityTypeID);
            //entityBuilder.HasOne(e => e.Contract).WithMany(e => e.Commitments).HasForeignKey(e => e.ContractID);
            //Support/Approver groups
            entityBuilder.HasOne(e => e.FocalPoint).WithMany(e => e.Commitments).HasForeignKey(e => e.focalpointID);
        }
    }

    public class BudgetBookMap
    {
        public BudgetBookMap(EntityTypeBuilder<BudgetBook> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.CapexOpexID);         // Foreign key
            entityBuilder.Property(t => t.DirectAllocated);
            entityBuilder.Property(t => t.ActivityCodeID);      // Foreign key
            entityBuilder.Property(t => t.ActivityID);          // Foreign key
            entityBuilder.Property(t => t.ActivityTypeID);      // Foreign key
            entityBuilder.Property(t => t.BudgetBasisID);       // Foreign key
            entityBuilder.Property(t => t.ContractID);          // Foreign key
            entityBuilder.Property(t => t.ScopeID);             // Foreign key
            //entityBuilder.Property(t => t.ActivityOwnerID);     // Foreign key
            //entityBuilder.Property(t => t.LineManagerID);     // Foreign key
            //entityBuilder.Property(t => t.SponsorID);           // Foreign key
            entityBuilder.Property(t => t.UapCodeID);           // Foreign key
            entityBuilder.Property(t => t.UapRollUpCodeID);     // Foreign key
            entityBuilder.Property(t => t.wbsID);               // Foreign key
            entityBuilder.Property(t => t.NAPIMSBUDGETDollar);
            entityBuilder.Property(t => t.NAPIMSBUDGETFDollar);
            entityBuilder.Property(t => t.NAPIMSBUDGETNaira);
            entityBuilder.Property(t => t.YYear);

            //entityBuilder.Property(t => t.OPYearBudgetDollar);
            //entityBuilder.Property(t => t.OPYearBudgetFDollar);
            //entityBuilder.Property(t => t.OPYearBudgetNaira);
            //entityBuilder.Property(t => t.Q1FYLEDollar);
            //entityBuilder.Property(t => t.Q1FYLEFDollar);
            //entityBuilder.Property(t => t.Q1FYLENaira);
            //entityBuilder.Property(t => t.Q2FYLEDollar);
            //entityBuilder.Property(t => t.Q2FYLEFDollar);
            //entityBuilder.Property(t => t.Q2FYLENaira);
            //entityBuilder.Property(t => t.Q3FYLEDollar);
            //entityBuilder.Property(t => t.Q3FYLEFDollar);
            //entityBuilder.Property(t => t.Q3FYLENaira);
            //entityBuilder.Property(t => t.Q4FYLEDollar);
            //entityBuilder.Property(t => t.Q4FYLEFDollar);
            //entityBuilder.Property(t => t.Q4FYLENaira);

            //Relationships
            entityBuilder.HasOne(e => e.ActivityCode).WithMany(e => e.BudgetBook).HasForeignKey(e => e.ActivityCodeID);
            entityBuilder.HasOne(e => e.Activity).WithMany(e => e.BudgetBook).HasForeignKey(e => e.ActivityID);
            entityBuilder.HasOne(e => e.ActivityType).WithMany(e => e.BudgetBook).HasForeignKey(e => e.ActivityTypeID);
            entityBuilder.HasOne(e => e.BudgetBasis).WithMany(e => e.BudgetBook).HasForeignKey(e => e.BudgetBasisID);
            entityBuilder.HasOne(e => e.Contract).WithMany(e => e.BudgetBook).HasForeignKey(e => e.ContractID);
            entityBuilder.HasOne(e => e.Scope).WithMany(e => e.BudgetBook).HasForeignKey(e => e.ScopeID);
            entityBuilder.HasOne(e => e.UapCode).WithMany(e => e.BudgetBook).HasForeignKey(e => e.UapCodeID);
            entityBuilder.HasOne(e => e.WBS).WithMany(e => e.BudgetBook).HasForeignKey(e => e.wbsID);
            entityBuilder.HasOne(e => e.UapRollUpCode).WithMany(e => e.BudgetBook).HasForeignKey(e => e.UapRollUpCodeID);
            //entityBuilder.HasOne(e => e.ActivityDetails).WithMany(e => e.BudgetBook).HasForeignKey(e => e.wbsID);
            entityBuilder.HasOne(e => e.CapexOpex).WithMany(e => e.BudgetBook).HasForeignKey(e => e.CapexOpexID);

            //Support/Approver groups
            //entityBuilder.HasOne(e => e.Sponsor).WithMany(e => e.BudgetBook).HasForeignKey(e => e.SponsorID);
        }
    }

    public class BudgetBookFinanceYearMap
    {
        public BudgetBookFinanceYearMap(EntityTypeBuilder<BudgetBookFinanceYear> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);            
            entityBuilder.Property(t => t.NAPIMSBUDGETDollar);
            entityBuilder.Property(t => t.NAPIMSBUDGETFDollar);
            entityBuilder.Property(t => t.NAPIMSBUDGETNaira);
            entityBuilder.Property(t => t.OPYearBudgetDollar);
            entityBuilder.Property(t => t.OPYearBudgetFDollar);
            entityBuilder.Property(t => t.OPYearBudgetNaira);

            entityBuilder.Property(t => t.Q1FYLEDollar);
            entityBuilder.Property(t => t.Q1FYLEFDollar);
            entityBuilder.Property(t => t.Q1FYLENaira);
            entityBuilder.Property(t => t.Q2FYLEDollar);
            entityBuilder.Property(t => t.Q2FYLEFDollar);
            entityBuilder.Property(t => t.Q2FYLENaira);
            entityBuilder.Property(t => t.Q3FYLEDollar);
            entityBuilder.Property(t => t.Q3FYLEFDollar);
            entityBuilder.Property(t => t.Q3FYLENaira);
            entityBuilder.Property(t => t.Q4FYLEDollar);
            entityBuilder.Property(t => t.Q4FYLEFDollar);
            entityBuilder.Property(t => t.Q4FYLENaira);
            entityBuilder.Property(t => t.YYear);

            //Relationships
            //entityBuilder.HasOne(e => e.BudgetBook).WithMany(e => e.BudgetBookFinanceYears).HasForeignKey(e => e.BudgetBookID);
        }
    }

    public class BudgetBookCommitmentsMap
    {
        public BudgetBookCommitmentsMap(EntityTypeBuilder<BudgetBookCommitments> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.Commitment);
            entityBuilder.Property(t => t.CCPSessionDate);
            entityBuilder.Property(t => t.title);

            entityBuilder.Property(t => t.PRNumber);
            entityBuilder.Property(t => t.PRValue);
            entityBuilder.Property(t => t.PONumber);
            entityBuilder.Property(t => t.POValue);

            entityBuilder.Property(t => t.justification);
            entityBuilder.Property(t => t.threat);
            entityBuilder.Property(t => t.contractnovendor);
            entityBuilder.Property(t => t.sPeriodfrom);

            entityBuilder.Property(t => t.ApprovalComment);
            entityBuilder.Property(t => t.Savings);
            entityBuilder.Property(t => t.iYear);
            entityBuilder.Property(t => t.ApproverID);
            entityBuilder.Property(t => t.ActivityOwnerID);
            entityBuilder.Property(t => t.LineManagerID);
            entityBuilder.Property(t => t.SponsorID);

            
            //Relationships
            entityBuilder.HasOne(e => e.BudgetBook).WithMany(e => e.BudgetBookCommitments).HasForeignKey(e => e.BudgetBookID);
            entityBuilder.HasOne(e => e.FocalPoint).WithMany(e => e.BudgetBookCommitments).HasForeignKey(e => e.FocalPointID);
            entityBuilder.HasOne(e => e.ApprovalDecision).WithMany(e => e.BudgetBookCommitments).HasForeignKey(e => e.ApprovalID);

            entityBuilder.HasOne(e => e.Teams).WithMany(e => e.BudgetBookCommitments).HasForeignKey(e => e.teamID);
            entityBuilder.HasOne(e => e.PlannedEmmergency).WithMany(e => e.BudgetBookCommitments).HasForeignKey(e => e.typeID);
            entityBuilder.HasOne(e => e.RequestStatus).WithMany(e => e.BudgetBookCommitments).HasForeignKey(e => e.statusID);
            entityBuilder.HasOne(e => e.ContractProcurementVehicle).WithMany(e => e.BudgetBookCommitments).HasForeignKey(e => e.vehicleID);
            entityBuilder.HasOne(e => e.PurchasingGroup).WithMany(e => e.BudgetBookCommitments).HasForeignKey(e => e.groupID);
        }
    }

    public class ActivityDetailsMap
    {
        public ActivityDetailsMap(EntityTypeBuilder<ActivityDetails> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.Description);
            entityBuilder.Property(t => t.Quantity);
            entityBuilder.Property(t => t.Rate);
            entityBuilder.Property(t => t.FixedExchangeRate);
            entityBuilder.Property(t => t.iYear);
            entityBuilder.HasOne(e => e.Currencies).WithMany(e => e.ActivityDetails).HasForeignKey(e => e.CurrenciesID);
            entityBuilder.HasOne(e => e.BudgetBookCommitments).WithMany(e => e.ActivityDetails).HasForeignKey(e => e.BudgetBookCommitmentsID);
            entityBuilder.HasOne(e => e.BudgetBook).WithMany(e => e.ActivityDetails).HasForeignKey(e => e.BudgetBookID);
        }
    }

    public class ApprovalDecisionMap
    {
        public ApprovalDecisionMap(EntityTypeBuilder<ApprovalDecision> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.Decision);
            entityBuilder.Property(t => t.ColorCode);
        }
    }

    public class CurrenciesMap
    {
        public CurrenciesMap(EntityTypeBuilder<Currencies> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.CurrencyName);
            entityBuilder.Property(t => t.Country);
            entityBuilder.Property(t => t.Codes);
            entityBuilder.Property(t => t.Number);
        }
    }

    public class FileUploadMap
    {
        public FileUploadMap(EntityTypeBuilder<FileUpload> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.Title);
            entityBuilder.Property(t => t.UploadFiles);
            entityBuilder.HasOne(e => e.Commitments).WithMany(e => e.FileUploads).HasForeignKey(e => e.CommitmentID);
            entityBuilder.HasOne(e => e.BudgetBookCommitments).WithMany(e => e.FileUploads).HasForeignKey(e => e.BudgetBookCommitmentsID);
        }
    }

    public class AppUsersMap
    {
        public AppUsersMap(EntityTypeBuilder<AppUsers> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.Deligate);
            entityBuilder.Property(t => t.FullName);
            entityBuilder.Property(t => t.IsGuestAcct);
            entityBuilder.Property(t => t.LoginTime);
            entityBuilder.Property(t => t.Password);
            entityBuilder.Property(t => t.RefInd);
            entityBuilder.Property(t => t.Status);
            entityBuilder.Property(t => t.UserMail);
            entityBuilder.Property(t => t.UserName);
            entityBuilder.Property(t => t.RoleId);
            entityBuilder.HasOne(e => e.LineManager).WithMany(e => e.ActivityOwners).HasForeignKey(e => e.LineManagerID);
        }
    }    

    public class AssetMap
    {
        public AssetMap(EntityTypeBuilder<Asset> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.AssetName);
            entityBuilder.Property(t => t.Location);
        }
    }

    public class ContractProcurementVehicleMap
    {
        public ContractProcurementVehicleMap(EntityTypeBuilder<ContractProcurementVehicle> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.VehicleName);
        }
    }

    public class DepartmentMap
    {
        public DepartmentMap(EntityTypeBuilder<Department> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.DepartmentName);
        }
    }

    public class ExchangeRateMap
    {
        public ExchangeRateMap(EntityTypeBuilder<ExchangeRate> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.FloatingExchangeRate);
            entityBuilder.Property(t => t.MMonth);
            entityBuilder.Property(t => t.YYear);
            entityBuilder.Property(t => t.iDay);
        }
    }

    public class AreaMap
    {
        public AreaMap(EntityTypeBuilder<Area> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.AreaName);
        }
    }

    public class FacilityMap
    {
        public FacilityMap(EntityTypeBuilder<Facility> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.FacilityName);
            entityBuilder.Property(t => t.Agg);
            entityBuilder.Property(t => t.Location);
            entityBuilder.HasOne(e => e.Area).WithMany(e => e.Facilities).HasForeignKey(e => e.AreaID);
        }
    }

    public class PlannedEmmergencyMap
    {
        public PlannedEmmergencyMap(EntityTypeBuilder<PlannedEmmergency> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.PlanEmmerType);
        }
    }

    public class PurchasingGroupMap
    {
        public PurchasingGroupMap(EntityTypeBuilder<PurchasingGroup> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.GroupName);
        }
    }

    public class RequestStatusMap
    {
        public RequestStatusMap(EntityTypeBuilder<RequestStatus> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.ReqstStatus);
        }
    }

    public class TeamMap
    {
        public TeamMap(EntityTypeBuilder<Team> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.TeamName);
        }
    }

    public class WBSMap
    {
        public WBSMap(EntityTypeBuilder<WBS> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.CostObjects);
            entityBuilder.Property(t => t.CostObjectsDescription);
        }
    }

    public class BudgetBasisMap
    {
        public BudgetBasisMap(EntityTypeBuilder<BudgetBasis> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.BudgetBase);
        }
    }

    public class ActivityMap
    {
        public ActivityMap(EntityTypeBuilder<Activity> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.Description);
        }
    }

    public class ScopeMap
    {
        public ScopeMap(EntityTypeBuilder<Scope> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.Purpose);
        }
    }

    public class ActivityTypeMap
    {
        public ActivityTypeMap(EntityTypeBuilder<ActivityType> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.ActivityName);
        }
    }

    public class ContractMap
    {
        public ContractMap(EntityTypeBuilder<Contract> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.ContractName);
        }
    }

    public class UapCodeMap
    {
        public UapCodeMap(EntityTypeBuilder<UapCode> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.UapCodeDesc);
        }
    }

    public class UapRollUpCodeMap
    {
        public UapRollUpCodeMap(EntityTypeBuilder<UapRollUpCode> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.UapRollUpCodeDesc);
        }
    }

    public class ActivityCodeMap
    {
        public ActivityCodeMap(EntityTypeBuilder<ActivityCode> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.ActivityCodeDesc);
            entityBuilder.Property(t => t.ActivityOwnerID);
            entityBuilder.Property(t => t.AccountableManagerID);
            entityBuilder.HasOne(e => e.LineManager).WithMany(e => e.ActivityCodes).HasForeignKey(e => e.LineManagerID);
            entityBuilder.HasOne(e => e.ActivityCodeWorkStream).WithMany(e => e.ActivityCodes).HasForeignKey(e => e.ActivityCodeWorkStreamID);
        }
    }

    public class ActivityCodeWorkStreamMap
    {
        public ActivityCodeWorkStreamMap(EntityTypeBuilder<ActivityCodeWorkStream> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.WorkStream);
            entityBuilder.Property(t => t.WorkStreamDesc);
            entityBuilder.Property(t => t.WorkFlowType);
        }
    }

    public class CapexOpexMap
    {
        public CapexOpexMap(EntityTypeBuilder<CXOX> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.CapexOpex);
        }
    }


    public class BudgetUploaderMap
    {
        public BudgetUploaderMap(EntityTypeBuilder<BudgetUploader> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.ActivityType);
            entityBuilder.Property(t => t.DirectAllocated);
            entityBuilder.Property(t => t.UapCode);
            entityBuilder.Property(t => t.UapRollUpCode);
            entityBuilder.Property(t => t.ActivityName);
            entityBuilder.Property(t => t.ActivityCode);
            entityBuilder.Property(t => t.CostCenter);
            entityBuilder.Property(t => t.Activity);
            entityBuilder.Property(t => t.ActivityOwner);
            entityBuilder.Property(t => t.LineManager);
            entityBuilder.Property(t => t.AccountableManager);
            entityBuilder.Property(t => t.ScopePurpose);
            entityBuilder.Property(t => t.Contract);
            entityBuilder.Property(t => t.Budgetbasis);
            entityBuilder.Property(t => t.OPYearBudget);
        }
    }

    public class BudgetUploaderTransformedMap
    {
        public BudgetUploaderTransformedMap(EntityTypeBuilder<BudgetUploaderTransformed> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ID);
            entityBuilder.Property(t => t.ActivityType);
            entityBuilder.Property(t => t.DirectAllocated);
            entityBuilder.Property(t => t.UapCode);
            entityBuilder.Property(t => t.UapRollUpCode);
            entityBuilder.Property(t => t.ActivityName);
            entityBuilder.Property(t => t.ActivityCode);
            entityBuilder.Property(t => t.CostCenter);
            entityBuilder.Property(t => t.Activity);
            entityBuilder.Property(t => t.ActivityOwner);
            entityBuilder.Property(t => t.LineManager);
            entityBuilder.Property(t => t.AccountableManager);
            entityBuilder.Property(t => t.ScopePurpose);
            entityBuilder.Property(t => t.Contract);
            entityBuilder.Property(t => t.Budgetbasis);
            entityBuilder.Property(t => t.OPYearBudget);
        }
    }
}

//TODO: add-migration -context BongaCCDbContext [Parameter name]
//Update-Database 