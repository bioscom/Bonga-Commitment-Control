using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class CommitmentsViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Capex/Opex")]
        [EnumDataType(typeof(enuCapexOpex))]
        [Required]
        public int CapexOpex { get; set; }

        [Display(Name = "Capex/Opex")]
        public string sCapexOpex { get; set; }

        [Display(Name = "PR Number")]
        [Required]
        public string PRNumber { get; set; }

        [Display(Name = "PR Value (F$)")]
        [Required]
        public decimal PRValue { get; set; }

        [Display(Name = "PO Number")]
        [Required]
        public string PONumber { get; set; }

        [Display(Name = "PO Value (F$)")]
        [Required]
        public decimal POValue { get; set; }

        [Display(Name = "BCC No.")]
        public string comitmntno { get; set; }

        [Display(Name = "Commitment")]
        [Required]
        public decimal commitment { get; set; }

        [Display(Name = "Activity Description")]
        [Required]
        public string title { get; set; }

        [Display(Name = "Justification")]
        [Required]
        public string justification { get; set; }

        [Display(Name = "Threat")]
        [Required]
        public string threat { get; set; }

        //[Display(Name = "Contract No Vendor:")]
        //[Required]
        //public string contractnovendor { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [UIHint("Date")]
        [Required]
        public DateTime periodfrom { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [UIHint("Date")]
        public DateTime periodto { get; set; }

        [Display(Name = "Purchasing Group")]
        [Required]
        public long groupID { get; set; }
        public string Group { get; set; }

        [Display(Name = "Planned or Emmergency")]
        [Required]
        public long typeID { get; set; }
        public string PlannedEmmergency { get; set; }

        [Display(Name = "Request Status")]
        [Required]
        public long statusID { get; set; }
        public string Status { get; set; }

        [Display(Name = "Contract/Proc. Vehicle")]
        [Required]
        public long vehicleID { get; set; }

        public string VehicleProcurement { get; set; }

        [Display(Name = "Asset")]
        [Required]
        public long assetID { get; set; }

        public string Asset { get; set; }

        [Display(Name = "Facility")]
        public long facilityID { get; set; }

        public string Facility { get; set; }

        [Display(Name = "Department")]
        public long departmentID { get; set; }
        public string Department { get; set; }

        [Display(Name = "Cost Object")]
        [Required]
        public long wbsID { get; set; }
        public string wbs { get; set; }

        [Display(Name = "Team")]
        public long teamID { get; set; }
        public string sTeam { get; set; }

        //Support/Approver groups

        [Display(Name = "Sponsor")]
        [Required]
        public long sponsorID { get; set; }
        public string Sponsor { get; set; }

        [Display(Name = "Checked By")]
        [Required]
        public long checkedbyID { get; set; }
        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        [Display(Name = "Focal Point")]
        public long? focalpointID { get; set; }

        [Display(Name = "Focal Point")]
        public string FocalPoint { get; set; }

        ////approval process

        [Display(Name = "Stand:")]
        public long? approvalID { get; set; }
        public string ApprovalStatus { get; set; }

        [Display(Name = "Detailed Activity")]
        public string approvalComment { get; set; }

        [Display(Name = "Savings")]
        public decimal savings { get; set; }

        //[Display(Name = "Variance")]
        //public String variance { get; set; }

        [Display(Name = "Approved By")]
        public long approverID { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [UIHint("Date")]
        public DateTime DateSubmitted { get; set; }
    }
}

public enum enuCapexOpex
{
    Capex = 1,
    Opex = 2,
}

public class OpsCaps
{
    public static string CapexOpexDesc(enuCapexOpex eCapexOpex)
    {
        string sRet = "UnKnown Account";
        try
        {
            switch (eCapexOpex)
            {
                case enuCapexOpex.Capex: sRet = "Capex"; break;
                case enuCapexOpex.Opex: sRet = "Opex"; break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return sRet;
    }
}

public enum enuDirectAllocated
{
    Direct = 1,
    Allocated = 2,
}


public class DirectAllocated
{
    public static string DirectAllocatedDesc(enuDirectAllocated eDirectAllocated)
    {
        string sRet = "UnKnown Account";
        try
        {
            switch (eDirectAllocated)
            {
                case enuDirectAllocated.Direct: sRet = "Direct"; break;
                case enuDirectAllocated.Allocated: sRet = "Allocated"; break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return sRet;
    }

    public static int IntDirectAllocated(string sVal)
    {
        int iRet = 0;
        try
        {
            switch (sVal.ToUpper())
            {
                case "DIRECT": iRet = 1; break;
                case "ALLOCATED": iRet = 2; break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return iRet;
    }
}