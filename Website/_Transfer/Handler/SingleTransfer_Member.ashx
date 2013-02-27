<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Transfer.UpdateYesterdayBalanceMember_Handler" %>

// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.05.30">Improve performance, check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._Transfer
{
    using System;
    using System.Web;
    using System.Data;

    using Fanex.Age.UI;
    using Sunplus.Agent.Model.BL.Transfer;
    using Sunplus.Agent.Model.Entities;

    public class UpdateYesterdayBalanceMember_Handler : TplHandler
    {
        private string custId = string.Empty;
        private string isClose = string.Empty;
        private double amount = 0;
        private double oldAmount = 0;
        private string errMsg = string.Empty;
        private int errCode = 0;
        private bool isSyncCasino = false;
        private bool disableCasino = false;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            custId = Request.Form["custid"].ToString();
            amount = !string.IsNullOrEmpty(Request.Form["txtamount"]) ? Convert.ToDouble(Request.Form["txtamount"]) : 0;
            oldAmount = !string.IsNullOrEmpty(Request.Form["oldamount"]) ? Convert.ToDouble(Request.Form["oldamount"]) : 0;
            Boolean.TryParse(Request.Form["issynccasino"], out isSyncCasino);
            Boolean.TryParse(Request.Form["disablecasino"], out disableCasino);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            TransferBO bo = new TransferBO(CurrentUserSession.NetSessionId);
            DataTable resultchk = bo.TransferCheckExistingForAgent(custId);

            if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.Transfer)
            {
                errCode = 106;
                errMsg = Resources.Lang.lblNotAuthorized;

                return new Result(errCode, errMsg);
            }

            if (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal)
            {
                errCode = 601;
                errMsg = Resources.Lang.lblNotAuthorized;
                return new Result(errCode, errMsg);
            }

            if (resultchk.Rows.Count > 0)
            {
                errCode = 603;
                errMsg = Resources.Lang.transferonceeveryday;

                return new Result(errCode, errMsg);
            }

            errCode = bo.UpdateTransferYesBalance(custId, amount, false, false, isClose, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);
            if (errCode == 0)
            {
                if (amount.Equals(oldAmount) && isSyncCasino)
                {
                    CasinoLibrary.SBAgent.Customer csCustomer = new CasinoLibrary.SBAgent.Customer();
                    try
                    {
                        string strUserName = Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.UserName : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName;
                        csCustomer.FullSettlementResetMaxTransfer(custId, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, strUserName, 2);
                    }
                    catch { }
                }
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