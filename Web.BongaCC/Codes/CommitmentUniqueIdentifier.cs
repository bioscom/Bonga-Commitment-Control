using EF.BongaCC.Core.Model;
using EF.BongaCC.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.Codes
{
    public static class CommitmentUniqueIdentifier
    {
        public struct CommitmentIdentity
        {
            public long? ID;
            public string sCommitmentNumber;
        }

        public static CommitmentIdentity CommitmentIdentifier(IRepository<BudgetBookCommitments> repo)
        {
            CommitmentIdentity e = new CommitmentIdentity();
            try
            {
                string sNo = "";

                e.ID = repo.GetAll().Result == null ? 1 : repo.GetAll().Result.LastOrDefault().ID + 1;
                var sVal = e.ID.ToString();

                if (sVal.Length == 1) sNo = "000" + sVal;
                else if (sVal.Length == 2) sNo = "00" + sVal;
                else if (sVal.Length == 3) sNo = "0" + sVal;
                else if (sVal.Length >= 4) sNo = sVal;

                e.sCommitmentNumber = "SNEP" + DateTime.Now.Year.ToString().Remove(0, 2) + sNo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
            }

            return e;
        }
    }
}
