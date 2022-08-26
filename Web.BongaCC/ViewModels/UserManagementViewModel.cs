using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class UserManagementViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }
        [Display(Name = "Line Manager")]
        public long? LineManagerID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        public string UserMail { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Line Manager")]
        public string LineManagerFullName { get; set; }

        [Display(Name = "Ref. Ind.")]
        public string RefInd { get; set; }

        [Display(Name = "Status")]
        [EnumDataType(typeof(enuStatus))]
        public int Status { get; set; }

        [Display(Name = "Last Login Time")]
        public DateTime LoginTime { get; set; }

        [Display(Name = "Is Guest Account")]
        public bool IsGuestAcct { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Deligate")]
        public int Deligate { get; set; }

        [Display(Name = "Role")]
        [EnumDataType(typeof(enuRole))]
        public int RoleId { get; set; }

        public string Roles { get; set; }

        [Display(Name = "Status")]
        public string sStatus { get; set; }

        public UserManagementViewModel oUser { get; set; }
        public IEnumerable<UserManagementViewModel> lstUsers { get; set; }
    }
}

public enum enuRole
{
    NA = -1,
    Admin = 1,
    AccountableManager = 2,
    ActivityOwner = 3,
    Corporate = 4,
    FocalPoint = 5,
    LineManager = 6
}

public enum enuStatus
{
    Active = 1,
    Deactivated = 2
}
