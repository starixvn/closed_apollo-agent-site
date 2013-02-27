<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Transfer.UpdateMultiYesBalanceMember_Handler" %>

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

    public class UpdateMultiYesBalanceMember_Handler : TplHandler
    {
        private int status = 0;
        private int totalPage = 0;
        private int pageSize = 0;
        private int counter = 0;
        private string userName = string.Empty;
        private bool isYesterdayBalance = false;
        private string custid = string.Empty;
        private string custidCheckSync = string.Empty;
        private string custIds = string.Empty;
        private string custIdList = string.Empty;
        private string isClose = string.Empty;
        private bool checkAll = false;
        private string errMsg = string.Empty;
        private int errCode = 0;
        private string arr_errMsg = string.Empty;
        private int arr_errCode = 0;
        private string[] arrCustid;
        private Stack custIdStack = new Stack();

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.status = Convert.ToInt16(Request.Form["status"].ToString());
            this.totalPage = Convert.ToInt16(Request.Form["totalPage"].ToString());
            this.pageSize = Convert.ToInt16(Request.Form["pagesize"].ToString());
            this.userName = Request.Form["username"].ToString();
            this.isYesterdayBalance = Convert.ToBoolean(Request.Form["isYesterdayBalance"].ToString());
            this.isClose = GetStatus(this.status);
            this.custid = Request.Form["custid"].ToString();
            this.custidCheckSync = Request.Form["custidSync"].ToString();
            this.checkAll = Convert.ToBoolean(Request.Form["ischeck"].ToString());

            if (string.IsNullOrEmpty(custidCheckSync))
            {
                custIds = custid;
            }
            else if (string.IsNullOrEmpty(custid))
            {
                custIds = custidCheckSync;
            }
            else
            {
                custIds = custid + "," + custidCheckSync;
            }
        }

        private void CreateCustIdList()
        {
            arrCustid = custIds.Trim().Split(',');
            if (arrCustid.Length > 0)
            {
                for (int i = 0; i < arrCustid.Length; i++)
                {
                    this.counter++;
                    if (this.counter >= Convert.ToInt16(Sunplus.Agent.Model.BL.Definitions.AppSettings.GetValue("MaxCustidTransfer")))
                    {
                        custIdList += arrCustid[i] + "_";
                        this.counter = 0;
                    }
                    else
                    {
                        custIdList += arrCustid[i] + ",";
                    }
                }
            }
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

        private void ResetMaxTransfer(bool IsShowCasino, bool IsCasinoDisabled)
        {
            if (IsShowCasino && !IsCasinoDisabled)
            {
                CasinoLibrary.SBAgent.Customer csCustomer = new CasinoLibrary.SBAgent.Customer();
                try
                {
                    string strUserName = CurrentUserSession.SubAccId == 0 ? CurrentUserSession.UserName : CurrentUserSession.SubUserName;
                    if (custidCheckSync.Length > 0)
                    {
                        csCustomer.FullSettlementResetMaxTransfer(custidCheckSync, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), CurrentUserSession.CustId, strUserName, 2);
                    }
                }
                catch { }
            }
        }
        private void ProgessTransfer(TransferBO bo, string _CustID)
        {
            errCode = bo.TransferYesCleareBalanceForAgent(_CustID, checkAll, isClose, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);

            arr_errCode += errCode;
            if (!string.IsNullOrEmpty(errMsg))
            {

                arr_errMsg += errMsg + "_";
            }

            if (!string.IsNullOrEmpty(arr_errMsg) && custIdStack.Count == 0)
            {
                arr_errMsg = arr_errMsg.Substring(0, arr_errMsg.Length - 1);
            }

            if (errCode == 0)
            {
                ResetMaxTransfer(Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino, CurrentUserSession.IsCasinoDisabled);
                OnProgessTransfer(bo);
            }
            else if (errMsg.Contains("_"))
            {
                string[] _arr = errMsg.Trim().Split('_');
                if (_arr[1] == "true")
                {
                    OnProgessTransfer(bo);
                }
            }
        }

        private void OnProgessTransfer(TransferBO bo)
        {
            if (custIdStack.Count > 0)
            {
                string _CustID = (string)custIdStack.Pop();
                ProgessTransfer(bo, _CustID);
            }
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

            if (checkAll && this.userName != "")
            {
                dtb = bo.GetDataTransfer(this.status, 1, ref this.totalPage, this.totalPage * this.pageSize, this.userName, this.isYesterdayBalance);
                if (dtb.Rows.Count > 0)
                {
                    custIds = string.Empty;
                    for (int i = 0; i < dtb.Rows.Count; i++)
                    {
                        custIds += dtb.Rows[i]["custid"] + ",";
                    }
                    custIds = custIds.Substring(0, custIds.Length - 1);
                }

                checkAll = false;
            }

            CreateCustIdList();

            if (custIdList.Length > 0)
            {
                string[] _arr = custIdList.Trim().Split('_');

                for (int i = _arr.Length; i > 0; i--)
                {
                    custIdStack.Push(_arr[i - 1]);
                }

                OnProgessTransfer(bo);
            }

            return new Result(arr_errCode, arr_errMsg);
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

    }
}