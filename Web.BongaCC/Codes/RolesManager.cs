using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.Codes
{
    public static class RolesManager
    {
        private class someValues
        {
            public int value { get; set; }
            public string Text { get; set; }
        }

        public static SelectList GetAllStatus()
        {
            List<someValues> st = new List<someValues>();

            someValues t = new someValues();
            t.Text = enuStatus.Active.ToString();
            t.value = (int)enuStatus.Active;
            st.Add(t);

            t = new someValues();
            t.Text = enuStatus.Deactivated.ToString();
            t.value = (int)enuStatus.Deactivated;
            st.Add(t);

            var result = st.ToList().OrderBy(o => o.Text)
                .Select(x => new SelectListItem
                {
                    Value = x.value.ToString(),
                    Text = x.Text,
                }).ToList();

            return new SelectList(result, "Value", "Text");
        }

        public static SelectList GetAllRoles()
        {
            List<someValues> st = new List<someValues>();

            someValues t = new someValues();
            t.Text = "Administrator"; //enuRole.Admin.ToString();
            t.value = (int)enuRole.Admin;
            st.Add(t);

            t = new someValues();
            t.Text = "Corporate"; //enuRole.Approver.ToString();
            t.value = (int)enuRole.Corporate;
            st.Add(t);

            t = new someValues();
            t.Text = "Activity Owner"; //enuRole.ActivityOwner.ToString();
            t.value = (int)enuRole.ActivityOwner;
            st.Add(t);

            t = new someValues();
            t.Text = "Focal Point"; //enuRole.FocalPoint.ToString();
            t.value = (int)enuRole.FocalPoint;
            st.Add(t);

            t = new someValues();
            t.Text = "Accountable Manager"; // enuRole.AccountableManager.ToString();
            t.value = (int)enuRole.AccountableManager;
            st.Add(t);

            t = new someValues();
            t.Text = "Line Manager"; //enuRole.LineManager.ToString();
            t.value = (int)enuRole.LineManager;
            st.Add(t);

            var result = st.ToList().OrderBy(o => o.Text)
                .Select(x => new SelectListItem
                {
                    Value = x.value.ToString(),
                    Text = x.Text,
                }).ToList();

            return new SelectList(result, "Value", "Text");
        }

        public static string GetRoleByRole(int RoleId)
        {
            string sRole = "";

            if ((int)enuRole.Admin == RoleId) { sRole = "Administrator"; }
            else if ((int)enuRole.Corporate == RoleId) { sRole = "Corporate"; }
            else if ((int)enuRole.ActivityOwner == RoleId) { sRole = "Activity Owner"; }
            else if ((int)enuRole.FocalPoint == RoleId) { sRole = "Focal Point"; }
            else if ((int)enuRole.AccountableManager == RoleId) { sRole = "Sponsor"; }
            else if ((int)enuRole.LineManager == RoleId) { sRole = "Line Manager"; }

            return sRole;
        }

        public static SelectList GetDirectAllocated()
        {
            List<someValues> st = new List<someValues>();

            someValues t = new someValues();
            t.Text = enuDirectAllocated.Allocated.ToString();
            t.value = (int)enuDirectAllocated.Allocated;
            st.Add(t);

            t = new someValues();
            t.Text = enuDirectAllocated.Direct.ToString();
            t.value = (int)enuDirectAllocated.Direct;
            st.Add(t);

            var result = st.ToList().OrderBy(o => o.Text)
                .Select(x => new SelectListItem
                {
                    Value = x.value.ToString(),
                    Text = x.Text,
                }).ToList();

            return new SelectList(result, "Value", "Text");
        }

        public static SelectList GetAllCapexOpex()
        {
            List<someValues> st = new List<someValues>();

            someValues t = new someValues();
            t.Text = enuCapexOpex.Capex.ToString();
            t.value = (int)enuCapexOpex.Capex;
            st.Add(t);

            t = new someValues();
            t.Text = enuCapexOpex.Opex.ToString();
            t.value = (int)enuCapexOpex.Opex;
            st.Add(t);

            var result = st.ToList().OrderBy(o => o.Text)
                .Select(x => new SelectListItem
                {
                    Value = x.value.ToString(),
                    Text = x.Text,
                }).ToList();

            return new SelectList(result, "Value", "Text");
        }

        public static SelectList GetAllWorkFlowTypes()
        {
            List<someValues> st = new List<someValues>();

            someValues t = new someValues();
            t.Text = WorkFlowTypes.WorkFlowTypeDesc(WorkFlowTypes.enuWorkFlowType.FP_LM_CCP);
            t.value = (int)WorkFlowTypes.enuWorkFlowType.FP_LM_CCP;
            st.Add(t);

            t = new someValues();
            t.Text = WorkFlowTypes.WorkFlowTypeDesc(WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP);
            t.value = (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP;
            st.Add(t);

            var result = st.ToList().OrderBy(o => o.Text)
                .Select(x => new SelectListItem
                {
                    Value = x.value.ToString(),
                    Text = x.Text,
                }).ToList();

            return new SelectList(result, "Value", "Text");
        }

        public static SelectList GetApprovalDecisions()
        {
            List<someValues> st = new List<someValues>();

            someValues t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.SteppedDown);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.SteppedDown;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.Declined);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.Declined;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.Deffered);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.Deffered;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.Postponed);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.Postponed;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.TBD);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.TBD;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.Draft);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.Draft;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.LineManagerReview);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.LineManagerReview;
            st.Add(t);

            t = new someValues();
            t.Text = ApprovalDecisionType.ApprovalDecisionDesc(ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession);
            t.value = (int)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession;
            st.Add(t);          

            var result = st.ToList().OrderBy(o => o.Text)
                .Select(x => new SelectListItem
                {
                    Value = x.value.ToString(),
                    Text = x.Text,
                }).ToList();

            return new SelectList(result, "Value", "Text");
        }
    }
}

public class LstCapexOpex
{
    public LstCapexOpex()
    {
      
    }

    public enum CapexOpexx
    {
        Capex = 1,
        Opex = 2,
    };

    public static string CapexOpexDe(CapexOpexx eCapexOpexx)
    {
        string sRet = "UnKnown Account";
        try
        {
            switch (eCapexOpexx)
            {
                case CapexOpexx.Capex: sRet = "Capex"; break;
                case CapexOpexx.Opex: sRet = "Opex"; break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return sRet;
    }
}

public class structUserMail
{
    public string m_sUserName { get; set; }
    public string m_sUserMail { get; set; }

    public structUserMail()
    {

    }

    public structUserMail(string sUserName, string sUserMail)
    {
        try
        {
            m_sUserName = sUserName;
            m_sUserMail = sUserMail;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
    }
}

public class structUserMailIdx : structUserMail
{
    private string m_sUserId = "";

    public structUserMailIdx()
    {

    }

    public structUserMailIdx(structUserMailIdx eUser)
        : base(eUser.m_sUserName, eUser.m_sUserMail)
    {
        try
        {
            m_sUserId = eUser.userId;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
    }

    public structUserMailIdx(structUserMail eUser, string sUserId)
        : base(eUser.m_sUserName, eUser.m_sUserMail)
    {
        try
        {
            m_sUserId = sUserId;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
    }

    public structUserMailIdx(string sUserName, string sUserMail, string sUserId)
        : base(sUserName, sUserMail)
    {
        try
        {
            m_sUserId = sUserId;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
    }

    public string userId
    {
        get { return m_sUserId; }
    }
}

public class StatusReporter
{
    public StatusReporter()
    {

    }

    public enum Status
    {
        ApprovedAsPresented = 1,
        ApprovedWithModifications = 2,
        ProvisionalApproval = 3,
        SteppedDown = 4,
        Declined = 5,
        Deferred = 6,
        Postponed = 7,
        TBD = 8,
        Draft = 9,
        LineManagerReview = 10,
        CCPanelSession = 11,
    };

    public static string StatusDescription(Status eStatus)
    {
        string sRet = "UnKnown Account";
        try
        {
            switch (eStatus)
            {
                case Status.ApprovedAsPresented: sRet = "Approved as presented"; break;
                case Status.ApprovedWithModifications: sRet = "Approved with modification"; break;
                case Status.ProvisionalApproval: sRet = "Provisional approval"; break;
                case Status.SteppedDown: sRet = "Stepped down"; break;
                case Status.Declined: sRet = "Declined"; break;
                case Status.Deferred: sRet = "Deffered, further documentation required"; break;
                case Status.Postponed: sRet = "Postponed"; break;
                case Status.TBD: sRet = "Pending Activity Owner's Review"; break;
                case Status.Draft: sRet = "Undergoing focal point's update"; break;
                case Status.LineManagerReview: sRet = "Pending Line Manager's Review"; break;
                case Status.CCPanelSession: sRet = "Pending CC Panel Review"; break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return sRet;
    }

    //public static string StatusDescription2(Status eStatus, long? iApproverId)
    //{
    //    string sRet = "UnKnown Account";
    //    try
    //    {
    //        switch (eStatus)
    //        {
    //            case Status.ApprovedAsPresented: sRet = "Approved as presented"; break;
    //            case Status.ApprovedWithModifications: sRet = "Approved with modification"; break;
    //            case Status.ProvisionalApproval: sRet = "Provisional approval"; break;
    //            case Status.SteppedDown: sRet = "Stepped down"; break;
    //            case Status.Declined: sRet = "Declined"; break;
    //            case Status.Deferred: sRet = "Deffered, further documentation required"; break;
    //            case Status.Postponed: sRet = "Postponed"; break;
    //            case Status.TBD: sRet = "Pending Activity Owner's Review"; break;
    //            case Status.Draft: sRet = "Undergoing focal point's update"; break;
    //            case Status.LineManagerReview: sRet = (iApproverId != null) ? "Pending CC Panel Review" : "Pending Line Manager's Review"; break;
    //            case Status.CCPanelSession: sRet = "Pending CC Panel Review"; break;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
    //    }
    //    return sRet;
    //}
}

public static class WorkFlowTypes
{
    public static string WorkFlowTypeDesc(enuWorkFlowType eWorkFlowType)
    {
        string sRet = "TBD";
        try
        {
            switch (eWorkFlowType)
            {
                case enuWorkFlowType.FP_LM_CCP: sRet = "Focal Point -> Line Manager -> CC Panel"; break;
                case enuWorkFlowType.FP_AO_LM_CCP: sRet = "Focal Point -> Activity Owner -> Line Manager -> CC Panel"; break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return sRet;
    }

    public enum enuWorkFlowType
    {
        FP_LM_CCP = 1,
        FP_AO_LM_CCP = 2,
    }
}

public static class ApprovalDecisionType
{
    public static string ApprovalDecisionDesc(enuApprovalDecision eApprovalDecision)
    {
        string sRet = "UnKnown Account";
        try
        {
            switch (eApprovalDecision)
            {
                case enuApprovalDecision.ApprovedAsPresented: sRet = "APPROVED AS PRESENTED"; break;
                case enuApprovalDecision.ApprovedWithModifications: sRet = "APPROVED WITH MODIFICATION(S)"; break;
                case enuApprovalDecision.ProvisionalApproval: sRet = "PROVISIONAL APPROVAL"; break;
                case enuApprovalDecision.SteppedDown: sRet = "STEPPED DOWN"; break;
                case enuApprovalDecision.Declined: sRet = "DECLINED"; break;
                case enuApprovalDecision.Deffered: sRet = "DEFFERED (FURTHER DOCUMENTATION REQUIRED)"; break;
                case enuApprovalDecision.Postponed: sRet = "POSTPONED"; break;
                case enuApprovalDecision.TBD: sRet = "TBD"; break;
                case enuApprovalDecision.Draft: sRet = "DRAFT"; break;
                case enuApprovalDecision.LineManagerReview: sRet = "PENDING LINE MANAGER'S REVIEW"; break;
                case enuApprovalDecision.PendingCCPanelSession: sRet = "PENDING CCPANEL SESSION"; break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return sRet;
    }

    public enum enuApprovalDecision
    {
        ApprovedAsPresented = 1,
        ApprovedWithModifications = 2,
        ProvisionalApproval = 3,
        SteppedDown = 4,
        Declined = 5,
        Deffered = 6,
        Postponed = 7,
        TBD = 8,
        Draft = 9,
        LineManagerReview = 10,
        PendingCCPanelSession = 11,
    }
}
