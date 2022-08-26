using EF.BongaCC.Core.Model;
using Microsoft.EntityFrameworkCore;
using EF.BongaCC.Data;
using Microsoft.EntityFrameworkCore.Design;
/// <summary>
/// Summary description for EFDbContext
/// </summary>

namespace EF.BongaCC.Data
{
    public partial class BongaCCDbContext : DbContext
    {
        public BongaCCDbContext(DbContextOptions<BongaCCDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new ActivityMap(modelBuilder.Entity<Activity>());
            new ActivityCodeMap(modelBuilder.Entity<ActivityCode>());
            new ActivityCodeWorkStreamMap(modelBuilder.Entity<ActivityCodeWorkStream>());
            new ActivityDetailsMap(modelBuilder.Entity<ActivityDetails>());
            new ActivityTypeMap(modelBuilder.Entity<ActivityType>());
            new ApprovalDecisionMap(modelBuilder.Entity<ApprovalDecision>());
            new AppUsersMap(modelBuilder.Entity<AppUsers>());
            new AreaMap(modelBuilder.Entity<Area>());
            new AssetMap(modelBuilder.Entity<Asset>());
            new BudgetBasisMap(modelBuilder.Entity<BudgetBasis>());
            new BudgetBookCommitmentsMap(modelBuilder.Entity<BudgetBookCommitments>());
            new BudgetBookFinanceYearMap(modelBuilder.Entity<BudgetBookFinanceYear>());
            new BudgetBookMap(modelBuilder.Entity<BudgetBook>());
            new CapexOpexMap(modelBuilder.Entity<CXOX>());
            new CommitmentsMap(modelBuilder.Entity<Commitments>()); //Just leave this item. It may still be useful
            new ContractMap(modelBuilder.Entity<Contract>());
            new ContractProcurementVehicleMap(modelBuilder.Entity<ContractProcurementVehicle>());
            new CurrenciesMap(modelBuilder.Entity<Currencies>());
            new DepartmentMap(modelBuilder.Entity<Department>());
            new ExchangeRateMap(modelBuilder.Entity<ExchangeRate>());
            new FacilityMap(modelBuilder.Entity<Facility>());
            new FileUploadMap(modelBuilder.Entity<FileUpload>());
            new PlannedEmmergencyMap(modelBuilder.Entity<PlannedEmmergency>());
            new PurchasingGroupMap(modelBuilder.Entity<PurchasingGroup>());
            new RequestStatusMap(modelBuilder.Entity<RequestStatus>());
            new ScopeMap(modelBuilder.Entity<Scope>());
            new TeamMap(modelBuilder.Entity<Team>());
            new UapCodeMap(modelBuilder.Entity<UapCode>());
            new UapRollUpCodeMap(modelBuilder.Entity<UapRollUpCode>());
            new WBSMap(modelBuilder.Entity<WBS>());
            new BudgetUploaderMap(modelBuilder.Entity<BudgetUploader>());
            new BudgetUploaderTransformedMap(modelBuilder.Entity<BudgetUploaderTransformed>());
        }

        public class ToDoContextFactory : IDesignTimeDbContextFactory<BongaCCDbContext>
        {
            public BongaCCDbContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<BongaCCDbContext>();
                var conn = "Server=BIOSCOMONE\\bioscomsoft; Database=BONGA_CommitmentControl; Trusted_Connection=True; MultipleActiveResultSets=true";
                //var conn = "Server=PHC-L-90600\\SqlExpress;Database=BONGA_CommitmentControl;Trusted_Connection=True;MultipleActiveResultSets=true";
                //var conn = "Server=PHC-S-04164\\INST1; Database=BONGA_CommitmentControl; User Id=BONGA; Password=B_1nga; Trusted_Connection=False; MultipleActiveResultSets=true";

                builder.UseSqlServer(conn);
                return new BongaCCDbContext(builder.Options);
            }
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public virtual DbSet<Commitments> Commitments { get; set; }
        public virtual DbSet<ActivityDetails> ActivityDetails { get; set; }
        public virtual DbSet<ActivityCodeWorkStream> ActivityCodeWorkStream { get; set; }
        public virtual DbSet<ApprovalDecision> Discipline { get; set; }
        public virtual DbSet<AppUsers> AppUsers { get; set; }
        public virtual DbSet<Asset> Asset { get; set; }
        public virtual DbSet<ContractProcurementVehicle> ContractProcurementVehicle { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRate { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<PlannedEmmergency> PlannedEmmergency { get; set; }
        public virtual DbSet<PurchasingGroup> PurchasingGroup { get; set; }
        public virtual DbSet<RequestStatus> RequestStatus { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<FileUpload> FileUpload { get; set; }
        public virtual DbSet<UapCode> UapCode { get; set; }
        public virtual DbSet<UapRollUpCode> UapRollUpCode { get; set; }
        public virtual DbSet<ActivityType> ActivityType { get; set; }
        public virtual DbSet<ActivityCode> ActivityCode { get; set; }
        public virtual DbSet<WBS> WBS { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Scope> Scope { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<BudgetBasis> BudgetBasis { get; set; }
        public virtual DbSet<BudgetBook> BudgetBook { get; set; }
        public virtual DbSet<BudgetBookFinanceYear> BudgetBookFinanceYear { get; set; }
        public virtual DbSet<CXOX> CapexOpex { get; set; }
        public virtual DbSet<BudgetBookCommitments> BudgetBookCommitments { get; set; }
        public virtual DbSet<Currencies> Currencies { get; set; }
        public virtual DbSet<BudgetUploader> BudgetUploaders { get; set; }
        public virtual DbSet<BudgetUploaderTransformed> BudgetUploaderTransformeds { get; set; }
    }
}