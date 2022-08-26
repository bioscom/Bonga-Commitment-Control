using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class BudgetBookCommitment2ViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        public long? BudgetBookID { get; set; }

        [Display(Name = "BCC No.")]
        public string Comitmntno { get; set; }

        //Date of Target CCP session
        [Display(Name = "CCP Session Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [UIHint("Date")]
        [Required]
        public DateTime CCPSessionDate { get; set; }

        [Display(Name = "Focal Point")]
        [Required]
        public long? FocalPointID { get; set; }

        [Display(Name = "Focal Point")]
        public string FocalPoint { get; set; }

        ////approval process

        [Display(Name = "Stand:")]
        public long? ApprovalID { get; set; }
        public string ApprovalStatus { get; set; }

        [Display(Name = "Detailed Activity")]
        public string ApprovalComment { get; set; }

        [Display(Name = "Savings")]
        public decimal Savings { get; set; }

        [Display(Name = "Approved By")]
        public long? ApproverID { get; set; }

        [Display(Name = "Activity Description")]
        [Required]
        public string title { get; set; }
        public int iYear { get; set; }
    }

}
