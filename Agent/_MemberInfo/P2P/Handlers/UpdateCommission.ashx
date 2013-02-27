<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.P2P.UpdateCommission_Handler" %>

using System;
using System.Web;
using Sunplus.Agent.Model.BL.MemberInfo.P2P;
using Fanex.Age.UI;
using System.Collections.Generic;
using Resources;

namespace Sunplus.Agent.UI._MemberInfo.P2P
{

    public class UpdateCommission_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private int errCode = 0;
        private int requestCustRoleId = 0;
        private int requestCustId = 0;

        private P2PBO bo = null;
        private IList<int> custids = null;

        private string errMsg = null;
        private string errCodeStrs = string.Empty;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            data.AsResult(View as Result, true);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            IList<int> errCodes = null;
            IList<string> errMsgs = null;

            custids = new List<int>();

            var idStr = Request.QueryString["custids"];

            if (idStr.Contains("^"))
            {
                var tmpIds = idStr.Split('^');

                foreach (var s in tmpIds)
                    custids.Add(Convert.ToInt32(s.Trim()));
            }

            IList<int> subids = new List<int>();
            IList<int> userids = new List<int>();

            foreach (int id in custids)
            {
                subids.Add(CurrentUserSession.SubAccId);
                userids.Add(CurrentUserSession.UserId);
            }

            try
            {
                if (custids.Count == 0)//Single update
                {
                    bo.UpdateP2PComm
                        (
                           requestCustId,
                           requestCustRoleId,
                           decimal.Parse(Request.QueryString["discount_1"]),
                           decimal.Parse(Request.QueryString["discount_2"]),
                           CurrentUserSession.SubAccId,
                           CurrentUserSession.UserId,
                           CurrentUserSession.LangKey,
                           Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                           out errCode,
                           out errMsg
                        );
                }
                else//Multiple update
                {
                    bo.UpdateP2PComm
                        (
                           custids,
                           requestCustRoleId,
                           decimal.Parse(Request.QueryString["discount_1"]),
                           decimal.Parse(Request.QueryString["discount_2"]),
                           subids,
                           userids,
                           CurrentUserSession.LangKey,
                           Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                           out errCodes,
                           out errMsgs
                        );

                    for (var j = 0; j < errCodes.Count; j++)
                    {
                        this.errCodeStrs += errCodes[j].ToString() + ",";

                        if (errCodes[j] != 0)
                            errMsg += errCodes[j] != -999 ? errMsgs[j] + "," : Lang.accdisabled + ",";
                        else
                            errMsg += Lang.updatesuccessfull + ",";
                    }
                }
            }
            catch (Exception ex)
            {
                errCode = -1;
                errMsg = ex.Message;
            }

            if (errCodeStrs.Length > 0)
            {
                return new Result(0, this.FormatMsg(errCodeStrs, errMsg));
            }
            else
            {
                errMsg = errCode == 0 ? Lang.updatesuccessfull : errMsg;
                return new Result(0, this.FormatMsg(errCode.ToString(), errMsg));
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            var users = Request.QueryString["custids"];

            if (users != null && users.Contains("^"))
            {
                var userIds = users.Split('^');
                this.custids = new List<int>();

                foreach (string id in userIds)
                {
                    if (!string.IsNullOrEmpty(id.Trim()))
                        this.custids.Add(Convert.ToInt32(id));
                }
            }
            else
                this.requestCustId = Convert.ToInt32(Request.QueryString["custids"]);

            this.requestCustRoleId = Convert.ToInt32(Request.QueryString["roleid"]);

            bo = bo == null ? new P2PBO(CurrentUserSession.NetSessionId) : bo;
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, "1|" + Lang.commission + ": " + Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        private string FormatMsg(string code, string msg)
        {
            if (this.errCodeStrs.Length > 0)
                return this.errCodeStrs.Substring(0, this.errCodeStrs.Length - 1) + ":" + msg.Substring(0, msg.Length - 1);

            return code + ":" + msg;
        }
    }
}