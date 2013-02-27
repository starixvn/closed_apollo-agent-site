<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Transfer.UpdateMultiYesBalance_Handler" %>

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
    using System.Collections;

    using Fanex.Age.UI;
    using Sunplus.Agent.Model.BL.Transfer;
    using Sunplus.Agent.Model.Entities;

    public class UpdateMultiYesBalance_Handler : TplHandler
    {
        private int status = 0;
        private int totalPage = 0;
        private int pageSize = 0;
        private string userName = string.Empty;
        private bool isYesterdayBalance = false;
        private string custid = string.Empty;
        private string isClose = string.Empty;
        private bool checkAll = false;
        private string errMsg = string.Empty;
        private int errCode = 0;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.status = Convert.ToInt16(Request.Form["status"]);
            this.totalPage = Convert.ToInt16(Request.Form["totalPage"]);
            this.pageSize = Convert.ToInt16(Request.Form["pagesize"]);
            this.userName = Request.Form["username"];
            this.isYesterdayBalance = Convert.ToBoolean(Request.Form["isYesterdayBalance"]);

            this.custid = Request.Form["custid"];
            this.isClose = GetStatus(status);
            this.checkAll = Convert.ToBoolean(Request.Form["ischeck"]);
        }

        private string GetStatus(int status)
        {
            string _status = "All";
            switch (this.status)
            {
                case 0:
                    _status = "All";
                    break;
                case 1:
                    _status = "Open";
                    break;
                case 2:
                    _status = "Suspend";
                    break;
                case 3:
                    _status = "Close";
                    break;
                default:
                    _status = "Disabled";
                    break;
            }

            return _status;
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            TransferBO bo = new TransferBO(CurrentUserSession.NetSessionId);
            DataTable dtb = new DataTable();

            if (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal)
            {
                errCode = 601;
                errMsg = Resources.Lang.lblNotAuthorized;
                return new Result(errCode, errMsg);
            }

            if (checkAll && userName != "")
            {
                dtb = bo.GetDataTransfer(status, 1, ref totalPage, totalPage * pageSize, userName, isYesterdayBalance);
                if (dtb.Rows.Count > 0)
                {
                    for (int i = 0; i < dtb.Rows.Count; i++)
                    {
                        custid += dtb.Rows[i]["custid"] + ",";
                    }

                    custid = custid.Substring(0, custid.Length - 1);
                }

                checkAll = false;
            }

            errCode = bo.UpdateTransferYesBalance(custid, 0, true, checkAll, isClose, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);

            return new Result(errCode, errMsg);
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }
    }
}