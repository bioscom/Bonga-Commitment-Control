using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EF.BongaCC.Core.Model
{
    public class FileUpload : BaseEntity
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Upload Files")]
        public byte[] UploadFiles { get; set; }

        public string FileNames { get; set; }

        public long? CommitmentID { get; set; }
        public virtual Commitments Commitments { get; set; }

        public long? BudgetBookCommitmentsID { get; set; }
        public virtual BudgetBookCommitments BudgetBookCommitments { get; set; }
    }
}
