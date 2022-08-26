using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class ApprovalProcessViewModel
    {
        //[ScaffoldColumn(false)]
        //public long? ID { get; set; }
        //Approval Section
        [Display(Name = "Stand:")]
        public long approvalID { get; set; }

        [Display(Name = "Detailed Activity")]
        public string approvalComment { get; set; }

        [Display(Name = "Savings")]
        public decimal savings { get; set; }

        //[Display(Name = "Variance")]
        //public String variance { get; set; }

        [Display(Name = "Approved By")]
        public long approverID { get; set; }
    }
}
