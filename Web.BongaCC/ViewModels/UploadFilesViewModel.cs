using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class UploadFilesViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "File Name")]
        public string FileNames { get; set; }

        [Required]
        public IFormFile UploadFiles { get; set; }

        [Display(Name = "File Upload Size (bytes)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public long UploadFilesSize { get; set; }

        [Display(Name = "Date Uploaded")]
        //[DisplayFormat(DataFormatString = "{0:F}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime UploadDT { get; set; }

        public long? CommitmentID { get; set; }

        public long? BudgetBookCommitmentsID { get; set; }
    }
}
