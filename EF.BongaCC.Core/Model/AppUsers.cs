using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class AppUsers : BaseEntity
    {
        public long? LineManagerID { get; set; }
        public string UserName { get; set; }
        public string UserMail { get; set; }
        public string FullName { get; set; }
        public string RefInd { get; set; }
        public int Status { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsGuestAcct { get; set; }
        public string Password { get; set; }
        public int Deligate { get; set; }
        public int RoleId { get; set; }

        public virtual AppUsers LineManager { get; set; }
        public virtual HashSet<AppUsers> ActivityOwners { get; set; }

        public virtual ICollection<Commitments> Commitments { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }
        public virtual ICollection<BudgetBookCommitments> BudgetBookCommitments { get; set; }

        public virtual ICollection<ActivityCode> ActivityCodes { get; set; }
    }
}
