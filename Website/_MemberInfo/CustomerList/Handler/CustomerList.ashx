<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.CustomerList.Agent.MemberList_Handler" %>

using System;
using System.Web;
using System.Collections;
using System.Data;

using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.SubAcc;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._MemberInfo.CustomerList.Agent
{
    public class MemberList_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public int custid = 0;
        public bool isSuspended = false;
        public bool isClosed = false;
        public bool isAllowAutoMPT = false;
        public bool isOutright = false;
        public bool isClearCredit = false;
        public bool isSync = false;

        public override void ProcessRequest(ref Hashtable p)
        {
            p.AddHttpParameters(new string[]{
                "custId",
                "isClosed",
                "isSuspended",
                "isAllowAutoMPT",
                "isOutright",
                "isSyncCasino",
                "isClearCredit"
            }, Request);
        }

        public override object ValidateRequest(Hashtable p)
        {
            custid = Convert.ToInt32(p["custId"]);
            if (custid <= 0 || CurrentUserSession.IsInternal)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            bool.TryParse(p["isSuspended"].ToString(), out isSuspended);
            bool.TryParse(p["isClosed"].ToString(), out isClosed);
            bool.TryParse(p["isAllowAutoMPT"].ToString(), out isAllowAutoMPT);
            bool.TryParse(p["isOutright"].ToString(), out isOutright);
            bool.TryParse(p["isClearCredit"].ToString(), out isClearCredit);

            Sunplus.Agent.Model.BL.Casino.CasinoBO casino = new Model.BL.Casino.CasinoBO(CurrentUserSession.NetSessionId);
            isSync = casino.IsSynchronized(custid);

            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);

            int errCode = 0;
            string errMsg = string.Empty;

            if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
            {
                return new Result(-1, Resources.Lang.lblNotAuthorized);
            }

            if (p["isClosed"] + "" != "-1")
            {
                errCode = bo.UpdateClosedStatus(custid, isClosed, isSync, isClearCredit, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref errMsg);

                if (p["isClearCredit"] + "" != "-1")
                {
                    errCode = bo.UpdateCreditBalance(CurrentUserSession.RoleId - 1, custid, 100, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref errMsg);
                }
            }

            if (p["isSuspended"] + "" != "-1")
            {
                errCode = bo.UpdateSuspendedStatus(custid, isSuspended, isSync, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref errMsg);
            }

            if (p["isAllowAutoMPT"] + "" != "-1")
            {
                errCode = bo.UpdateAutoMasterPTStatus(custid, !isAllowAutoMPT, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref errMsg);

            }

            if (p["isOutright"] + "" != "-1")
            {
                errCode = bo.UpdateOutrightStatus(custid, !isOutright, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref errMsg);
            }

            return new Result(errCode, errMsg);

        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }
    }
}