using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class BudgetBookCommitments : BaseEntity
    {
        public string title { get; set; }
        public string Comitmntno { get; set; }
        public decimal Commitment { get; set; }

        public string PRNumber { get; set; }

        public decimal PRValue { get; set; }

        public string PONumber { get; set; }

        public decimal POValue { get; set; }

        public DateTime? CCPSessionDate { get; set; }

        public long? FocalPointID { get; set; }
        public virtual AppUsers FocalPoint { get; set; }
        public long? BudgetBookID { get; set; }
        public virtual BudgetBook BudgetBook { get; set; }

        //approval process
        public string ApprovalComment { get; set; }
        public decimal? Savings { get; set; }
        //public long? ApproverID { get; set; }
        public int iYear { get; set; }

        //approval process
        public long? ApprovalID { get; set; }
        public virtual ApprovalDecision ApprovalDecision { get; set; }
        public virtual ICollection<ActivityDetails> ActivityDetails { get; set; }
        public virtual ICollection<FileUpload> FileUploads { get; set; }

        public long? ApproverID { get; set; }
        public long? SponsorID { get; set; }
        public long? LineManagerID { get; set; }
        public long? ActivityOwnerID { get; set; }


        public long? groupID { get; set; }    // Foreign key
        public virtual PurchasingGroup PurchasingGroup { get; set; }    // Navigation properties
        public long? typeID { get; set; }    // Foreign key
        public virtual PlannedEmmergency PlannedEmmergency { get; set; }    // Navigation properties
        public long? teamID { get; set; }
        public virtual Team Teams { get; set; }
        public long? vehicleID { get; set; }    // Foreign key
        public virtual ContractProcurementVehicle ContractProcurementVehicle { get; set; }    // Navigation properties
        public long? statusID { get; set; }    // Foreign key
        public virtual RequestStatus RequestStatus { get; set; }    // Navigation properties
        public string justification { get; set; }
        public string threat { get; set; }
        public string contractnovendor { get; set; }
        public string sPeriodfrom { get; set; }
    }
}
