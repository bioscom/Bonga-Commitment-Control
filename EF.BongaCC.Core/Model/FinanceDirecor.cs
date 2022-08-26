using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FinanceDirecor
/// </summary>

namespace EF.BongaCC.Core.Model
{
    public class CommitmentControlUsers : BaseEntity
    {
        public string UserName { get; set; }
        public string UserMail { get; set; }
        public string FullName { get; set; }
        public string RefInd { get; set; }

        //public CommitmentControlUsers()
        //{

        //}

        //public CommitmentControlUsers(DataRow dr)
        //{
        //    m_iUserId = int.Parse(dr["USERID"].ToString());
        //    m_sUserName = dr["USERNAME"].ToString();
        //    m_sUserMail = dr["EMAIL"].ToString();
        //    m_sFullName = dr["FULLNAME"].ToString();
        //    m_sRefInd = dr["REFIND"].ToString();
        //}

        //public structUserMailIdx structUserIdx
        //{
        //    get
        //    {
        //        return new structUserMailIdx(m_sFullName, m_sUserMail, m_sUserName);
        //    }
        //}
    }
}