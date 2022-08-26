using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for Commitments
/// </summary>

namespace EF.BongaCC.Core.Model
{
    public class Commitments : BaseEntity
    {
        public string TeamIndicator { get; set; }
        public int CapexOpex { get; set; }
        public string PRNumber { get; set; }
        public decimal PRValue { get; set; }
        public string PONumber { get; set; }
        public decimal POValue { get; set; }
        public string comitmntno { get; set; }
        public decimal commitment { get; set; }
        public string title { get; set; }
        public DateTime periodfrom { get; set; }
        public DateTime periodto { get; set; }
        public DateTime previous { get; set; }
        public decimal napimsNaira { get; set; }
        public decimal napimsDollar { get; set; }
        public decimal napimsFunctionalDollar { get; set; }
        public decimal requestNaira { get; set; }
        public decimal requestDollar { get; set; }
        public decimal requestFunctionalDollar { get; set; }



        public long groupID { get; set; }    // Foreign key
        public virtual PurchasingGroup PurchasingGroup { get; set; }    // Navigation properties
        public long typeID { get; set; }    // Foreign key
        public virtual PlannedEmmergency PlannedEmmergency { get; set; }    // Navigation properties
        public long teamID { get; set; }
        public virtual Team Teams { get; set; }
        public long vehicleID { get; set; }    // Foreign key
        public virtual ContractProcurementVehicle ContractProcurementVehicle { get; set; }    // Navigation properties
        public long statusID { get; set; }    // Foreign key
        public virtual RequestStatus RequestStatus { get; set; }    // Navigation properties
        public string justification { get; set; }
        public string threat { get; set; }
        public string contractnovendor { get; set; }
        public string sPeriodfrom { get; set; }


        public long assetID { get; set; }
        public virtual Asset Asset { get; set; }    // Navigation properties
        public long facilityID { get; set; }
        public virtual Facility Facility { get; set; }
        public long departmentID { get; set; }
        public virtual Department Department { get; set; }
        public long wbsID { get; set; }
        public virtual WBS WBS { get; set; }

       

        //Support/Approver groups
        public long sponsorID { get; set; }
        //public virtual AppUsers Sponsor { get; set; }

        public long checkedbyID { get; set; }
        //public virtual AppUsers CheckedBy { get; set; }
        public long approverID { get; set; }
        //public virtual AppUsers Approver { get; set; }

        public long? focalpointID { get; set; }
        public virtual AppUsers FocalPoint { get; set; }

        //approval process
        public long? approvalID { get; set; }
        public virtual ApprovalDecision ApprovalDecision { get; set; }

        public string approvalComment { get; set; }
        public decimal savings { get; set; }

        //public string variance { get; set; } //This should be by calculation

        //public virtual ICollection<ActivityDetails> ActivityDetails { get; set; }

        public virtual ICollection<FileUpload> FileUploads { get; set; }

    }
}