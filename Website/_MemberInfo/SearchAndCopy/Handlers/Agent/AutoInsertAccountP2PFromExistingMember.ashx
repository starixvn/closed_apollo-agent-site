<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent.AutoInsertAccountP2PFromExistingMember_Handler" %>

/*
 * Created 20120724@Jeff - auto insert account P2P from exist
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent
{
    using System;
    using System.Web;
    using System.Data;
    using System.Collections;
    using System.Collections.Generic;

    using DAL;
    using BLL;

    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Model.BL.MemberInfo.SearchAndCopy;
    using Sunplus.Agent.Model.BL.MemberInfo.P2P;
    using Sunplus.Agent.Utilities;

    public class AutoInsertAccountP2PFromExistingMember_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {

        int custId = 0;
        int custIdOld = 0;
        string errMsg = string.Empty;
        int errCode = 0;
        IList<int> errCodes = null;
        IList<string> errMsgs = null;
        Hashtable infoCustomer = new Hashtable();
        bool isMinPTChange = false;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request.Form["custid"], out custId);
            int.TryParse(Request["custidOld"], out custIdOld);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (custId < 0)
            {
                errCode = 101;
                errMsg = Resources.Lang.notExist;
                return new Result(errCode, errMsg);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            try
            {
                P2PBO bo = new P2PBO(CurrentUserSession.NetSessionId);
                int count = 0;
                DataSet ds = bo.GetP2PInfo(custIdOld, 1, out errCode, out errMsg);
                decimal maxTranCredit;
                bool chkResetTranLimit;
                IList<int> bettypeIds = new List<int>();
                IList<decimal> discount = new List<decimal>();
                string pars = "[{";

                decimal.TryParse(ds.Tables[0].Rows[0]["transferp2p"].ToString(), out maxTranCredit);
                bool.TryParse(ds.Tables[0].Rows[0]["isautoresetp2p"].ToString(), out chkResetTranLimit);

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    int betType;
                    bool value;
                    count++;

                    if (count >= 3)
                    {
                        int.TryParse(dr["bettype"].ToString(), out betType);
                        bool.TryParse(dr["StakeLevel1"].ToString(), out value);
                        pars += value ? "true," : "false,";
                        bool.TryParse(dr["StakeLevel2"].ToString(), out value);
                        pars += value ? "true," : "false,";
                        bool.TryParse(dr["StakeLevel3"].ToString(), out value);
                        pars += value ? "true," : "false,";
                        bool.TryParse(dr["StakeLevel4"].ToString(), out value);
                        pars += value ? "true," : "false,";
                        bool.TryParse(dr["StakeLevel5"].ToString(), out value);
                        pars += value ? "true};{" : "false,};{";

                        bettypeIds.Add(betType);
                    }
                }

                pars = pars.Substring(0, pars.Length - 2);
                pars += "]";

                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    decimal tmp;
                    decimal.TryParse(dr["discount"].ToString(), out tmp);
                    discount.Add(tmp);
                }

                UpdateBetSetting(bo, custId, maxTranCredit, chkResetTranLimit);
                UpdateGameSetting(bo, custId, bettypeIds, pars);
                UpdateCommission(bo, custId, discount[0], discount[1]);
            }
            catch (Exception e)
            {
                Logs.Write(e.ToString());
                errMsg = e.ToString();
            }

            return new Result(errCode, errMsg);
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        private void UpdateBetSetting(P2PBO bo, int requestCustId, decimal maxTranCredit, bool chkResetTranLimit)
        {
            bo.UpdateP2PMaxTransfer
                    (
                        requestCustId,
                        1,
                        maxTranCredit,
                        chkResetTranLimit,
                        CurrentUserSession.SubAccId,
                        CurrentUserSession.UserId,
                        CurrentUserSession.LangKey,
                        Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        out errCode,
                        out errMsg
                    );
        }

        private void UpdateGameSetting(P2PBO bo, int requestCustId, IList<int> bettypeIds, string pars)
        {
            bo.UpdateP2PGameSetting
                    (
                        requestCustId,
                        CurrentUserSession.SubAccId,
                        Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        CurrentUserSession.UserId,
                        CurrentUserSession.LangKey,
                        bettypeIds,
                        pars,
                        1,
                        out errCodes,
                        out errMsgs
                    );
        }

        private void UpdateCommission(P2PBO bo, int requestCustId, decimal discount_1, decimal discount_2)
        {
            bo.UpdateP2PComm
                    (
                       requestCustId,
                       1,
                       discount_1,
                       discount_2,
                       CurrentUserSession.SubAccId,
                       CurrentUserSession.UserId,
                       CurrentUserSession.LangKey,
                       Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                       out errCode,
                       out errMsg
                    );
        }
    }
}