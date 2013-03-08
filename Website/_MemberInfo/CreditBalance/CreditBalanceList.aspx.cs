// --------------------------------------------------------------------------
// <copyright file="CreditBalanceList.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Simon" date="2012.05.05">Render credit balance list</change>
//   <change who="Kaden.Trinh" date="2012.05.30">Refactor code and check StyleCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.CreditBalance
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Reports.Components;
    using Resources;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.MemberInfo.CreditBalance;
    using Sunplus.Agent.Model.BL.Transfer;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.UI._MemberInfo.CustomerList.Components;
    using Sunplus.Agent.Utilities;

    public partial class CreditBalanceList_Page : ContextMenuColumnsBase
    {
        private CreditBalanceList_Template template = new CreditBalanceList_Template();
        private int pageSize = Sunplus.Agent.Model.BL.Definitions.AppSettings.PageSize;
        private int pageIndex = 1;
        private int totalPages = 0;
        private int custId = 0, status = 0;
        private string userName = string.Empty;
        private string customerList = string.Empty;
        private DataTable dataTable;
        private DataTable lcsWinLimit;
        private bool showLCS = false;

        public override void RenderContent(HtmlTextWriter writer)
        {
            // For Show Hide Column

            var arrayText = new List<string> { Lang.status, Lang.firstname, Lang.lastname, Lang.credit, Lang.balance, Lang.yesterday + " " + Lang.balance, Lang.betcredit, Lang.outstanding, Lang.memberturnover, Lang.lastlogin, Lang.lastloginIP, Lang.username };
            var arrayCss = new List<string> { "ST", "FN", "LN", "CD", "BL", "YB", "BC", "OT", "TO", "LL", "LI", "UN" };
            this.showLCS = (this.customerList == "MemberList" || CurrentUserSession.IsAgent) && CurrentUserSession.IsLiveCasinoSync && AppSettings.IsEnableMemberWinLimit;

            if (this.showLCS)
            {
                arrayText.Add(Lang.LCSWinLimit);
                arrayCss.Add("LCS");
            }

            this.RegisterCommonScriptObjects("tblMain", new string[] { "headerTop" }, "ctxContainer", this);
            this.template.ContextMenu = this.RenderContextMenu(arrayText.ToArray(), arrayCss.ToArray(), null, true);
            this.template.ShowHideColumnClasses = this.RenderTableClasses(arrayCss.ToArray());
            this.RegisterJavascriptObject("sessionId", CurrentUserSession.NetSessionId);

            this.RegisterJavascriptObject("wrnTransferSuccessful", Lang.transfersuccessful);
            this.RegisterJavascriptObject("UserNameDefault", Lang.enterusername);
            this.RegisterJavascriptObject("CustId", this.custId);
            this.RegisterJavascriptObject("PageSize", this.pageSize);
            writer.Write(this.CreditBalanceList(this.pageIndex, this.userName, this.status));
        }

        public string CreditBalanceList(int pageIndex, string username, int status)
        {
            if (!this.showLCS)
            {
                this.template.LCSWinLimit.Clear();
            }

            StringBuilder content = new StringBuilder();

            // Printer
            string printClick = "getPrint('tbl-container')";
            this.template.printer = new PrintControl(this.template, printClick);

            // Searching
            string submitGetCustomer = "searchByUsername('CreditBalanceList.aspx','" + this.customerList + "')";
            this.template.searchControl = new SearchUserName(this.template, this.userName, status, submitGetCustomer);

            // Header
            this.template.no = Lang.no;
            this.template.status = Lang.status;
            this.template.username = Lang.username;
            this.template.firstname = Lang.firstname;
            this.template.lastname = Lang.lastname;
            this.template.credit = Lang.credit;
            this.template.balance = Lang.balance;
            this.template.yesbalance = Lang.yesterdaybalance;
            this.template.betcredit = Lang.betcredit;
            this.template.outtxn = Lang.outstanding;
            this.template.memberturnover = Lang.memberturnover;
            this.template.lastlogin = Lang.lastlogin;
            this.template.lastloginip = Lang.lastloginIP;

            // Tab Menu Page
            if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
            {
                this.template.AllowTransferSection.allowtransfer = this.TransferCondition(this.custId);
            }
            else
            {
                this.template.AllowTransferSection.Hide();
            }

            // Data Rows
            CreditBalanceList_Rows_Block tplRow = new CreditBalanceList_Rows_Block();

            string closedBg = Fanex.Age.UI.Common.RowCssClosed;
            string suspendedBg = Fanex.Age.UI.Common.RowCssSuspended;
            string disabledBg = Fanex.Age.UI.Common.RowCssDisabled;
            string evenBg = Fanex.Age.UI.Common.RowCssEven;
            string oddBg = Fanex.Age.UI.Common.RowCssOdd;

            string isBalance = string.Empty;
            string isYesbalance = string.Empty;
            int i = 0;
            if (this.dataTable.Rows.Count > 0)
            {
                string dtrCustId = string.Empty, dtrUserName = string.Empty;
                foreach (DataRow dr in this.dataTable.Rows)
                {
                    i++;
                    tplRow = new CreditBalanceList_Rows_Block();
                    dtrCustId = dr["custid"].ToString();
                    dtrUserName = dr["username"].ToString();

                    if (!this.showLCS)
                    {
                        tplRow.LCSValue.Clear();
                    }
                    else if (this.lcsWinLimit != null)
                    {
                        tplRow.LCSValue.LCS = this.FormatLCS(this.lcsWinLimit.Rows[i - 1].Field<decimal>("lcswinpercent"));
                    }
                    else
                    {
                        tplRow.LCSValue.LCS = string.Empty;
                    }

                    // Background Rows Color
                    if (Convert.ToInt32(dr["enabled"]) != 1)
                    {
                        tplRow.BgRow = disabledBg;
                        tplRow.status_value = Lang.disabled;
                    }
                    else if (Convert.ToInt32(dr["closed"]) != 0)
                    {
                        tplRow.BgRow = closedBg;
                        tplRow.status_value = Lang.closed;
                    }
                    else if (Convert.ToInt32(dr["suspended"]) != 0)
                    {
                        tplRow.BgRow = suspendedBg;
                        tplRow.status_value = Lang.suspended;
                    }
                    else if (i % 2 != 0)
                    {
                        tplRow.BgRow = oddBg;
                        tplRow.status_value = Lang.open;
                    }
                    else
                    {
                        tplRow.BgRow = evenBg;
                        tplRow.status_value = Lang.open;
                    }

                    tplRow.no_value = dr["RowNumber"].ToString();
                    isBalance = Formater.FormatNumber2(dr["balance"], CurrentUserSession.LangKey);
                    isYesbalance = Formater.FormatNumber2(dr["yesterday_balance"], CurrentUserSession.LangKey);
                    tplRow.YesAgent.custId = dtrCustId;
                    if (CurrentUserSession.RoleId == (int)UserTypes.Super || CurrentUserSession.RoleId == (int)UserTypes.Master)
                    {
                        tplRow.UrlMasterSuper.Hide();
                        tplRow.UrlMasterSuper1.Hide();

                        // User Name
                        if (this.customerList == "MemberList")
                        {
                            tplRow.PermissionUrlCredit.RoleId = "1";
                            tplRow.UrlAgent.SetValue(dtrUserName);
                            tplRow.UrlAgent1.SetValue(dtrUserName);
                        }
                        else if (this.customerList == "AgentList")
                        {
                            tplRow.PermissionUrlCredit.RoleId = "2";
                            tplRow.UrlAgent.Action = tplRow.UrlAgent1.Action = "MemberList";
                            tplRow.UrlAgent.CustId = tplRow.UrlAgent1.CustId = dtrCustId;
                            tplRow.UrlAgent.username_value = tplRow.UrlAgent1.username_value = dtrUserName;
                        }
                        else
                        {
                            tplRow.PermissionUrlCredit.RoleId = "3";
                            tplRow.UrlAgent.Action = tplRow.UrlAgent1.Action = "AgentList";
                            tplRow.UrlAgent.CustId = tplRow.UrlAgent1.CustId = dtrCustId;
                            tplRow.UrlAgent.username_value = tplRow.UrlAgent1.username_value = dtrUserName;
                        }

                        // Yesterday Balance
                        if (Convert.ToDecimal(isYesbalance) == 0)
                        {
                            if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
                            {
                                tplRow.YesAgent.SetValue("<font color='#1e1eeb'>" + isYesbalance + "</font>");
                            }
                            else
                            {
                                tplRow.YesAgent.SetValue(isYesbalance);
                            }
                        }
                        else if (Convert.ToDecimal(isYesbalance) < 0)
                        {
                            tplRow.YesAgent.SetValue("<font color='red'>" + isYesbalance + "</font>");
                        }
                        else
                        {
                            tplRow.YesAgent.SetValue(isYesbalance);
                        }

                        // Outstanding
                        if (this.customerList != "MemberList" || !UserSession.Current.SubPageAccess.ViewReports)
                        {
                            tplRow.LinkOut.Hide();
                            tplRow.UrlOut.outtxn_value = Formater.FormatNumber2(dr["outstanding"], CurrentUserSession.LangKey);
                        }
                        else
                        {
                            tplRow.UrlOut.Hide();
                            tplRow.LinkOut.CustId = dtrCustId;
                            tplRow.LinkOut.UserName = dtrUserName;
                            tplRow.LinkOut.outtxn_value = Formater.FormatNumber2(dr["outstanding"], CurrentUserSession.LangKey);
                        }
                    }
                    else
                    {
                        tplRow.UrlAgent.Hide();
                        tplRow.UrlAgent1.Hide();

                        tplRow.UrlMasterSuper.username_value = tplRow.UrlMasterSuper1.username_value = dtrUserName;

                        // Yesterday Balance
                        if (Convert.ToDecimal(isYesbalance) == 0)
                        {
                            string strTemp = CurrentUserSession.IsAllowedTotalBalanceTransfer || (CurrentUserSession.IsSubAcc && !CurrentUserSession.SubPageAccess.Transfer) ? isYesbalance : "<font color='#1e1eeb'>" + isYesbalance + "<font>";
                            tplRow.YesAgent.SetValue(strTemp);
                        }
                        else
                        {
                            string strTemp = Convert.ToDecimal(isYesbalance) < 0 ? "<font color='red'>" + isYesbalance + "</font>" : isYesbalance;
                            if (CurrentUserSession.IsAllowedTotalBalanceTransfer || (CurrentUserSession.IsSubAcc && !CurrentUserSession.SubPageAccess.Transfer))
                            {
                                tplRow.YesAgent.SetValue(strTemp);
                            }
                            else
                            {
                                tplRow.YesAgent.yesbalance_value = strTemp;
                            }
                        }

                        // Outstanding
                        if (!UserSession.Current.SubPageAccess.ViewReports)
                        {
                            tplRow.LinkOut.Hide();
                            tplRow.UrlOut.outtxn_value = Formater.FormatNumber2(dr["outstanding"], CurrentUserSession.LangKey);
                        }
                        else
                        {
                            tplRow.UrlOut.Hide();
                            tplRow.LinkOut.CustId = dtrCustId;
                            tplRow.LinkOut.UserName = dtrUserName;
                            tplRow.LinkOut.outtxn_value = Formater.FormatNumber2(dr["outstanding"], CurrentUserSession.LangKey);
                        }
                    }

                    tplRow.firstname_value = dr["firstname"].ToString();
                    tplRow.lastname_value = dr["lastname"].ToString();

                    // For Column Credit
                    tplRow.PermissionUrlCredit.credit_value = tplRow.PermissionCredit.credit_value = Formater.FormatNumber2(dr["credit"], CurrentUserSession.LangKey);
                    if (UserSession.Current.SubPageAccess.UpdateAccount)
                    {
                        tplRow.PermissionCredit.Hide();
                    }
                    else
                    {
                        tplRow.PermissionUrlCredit.Hide();
                    }

                    tplRow.PermissionUrlCredit.custId = dtrCustId;
                    tplRow.PermissionUrlCredit.usn = dtrUserName;

                    // Balance
                    tplRow.LinkAgent.custId = dtrCustId;
                    if (Convert.ToDecimal(isBalance) == 0)
                    {
                        string strTemp = string.Empty;
                        strTemp = CurrentUserSession.IsAllowedTotalBalanceTransfer && CurrentUserSession.RoleId == (int)UserTypes.Agent ? "<font color='#1e1eeb'>" + isBalance + "<font>" : isBalance;
                        tplRow.LinkAgent.SetValue(strTemp);
                    }
                    else
                    {
                        string strTemp = Convert.ToDecimal(isBalance) < 0 ? "<font color='red'>" + isBalance + "</font>" : isBalance;
                        if (CurrentUserSession.IsAllowedTotalBalanceTransfer && CurrentUserSession.RoleId == (int)UserTypes.Agent)
                        {
                            tplRow.LinkAgent.balance_value = strTemp;
                        }
                        else
                        {
                            tplRow.LinkAgent.SetValue(strTemp);
                        }
                    }

                    tplRow.betcredit_value = Formater.FormatNumber2(dr["betcredit"], CurrentUserSession.LangKey);
                    tplRow.memberturnover_value = Formater.FormatNumber2(dr["turnover"], CurrentUserSession.LangKey);
                    tplRow.lastlogin_value = string.IsNullOrEmpty(dr["lastlogged"] + string.Empty) ? "-" : Formater.FormatDateTimeEn12(dr["lastlogged"]);
                    tplRow.lastloginip_value = Sunplus.Agent.Model.Common.AgentIPFormat.FormatIP(dr["iplogin"].ToString());

                    content.Append(tplRow);
                }
            }
            else
            {
                NoInfo_Template noInfoRow = new NoInfo_Template();
                noInfoRow.NumCols = "14";
                content.Append(noInfoRow);
            }

            this.template.Rows.SetValue(content.ToString());

            this.RenderPaging();
            return this.template;
        }

        public string TransferCondition(int custId)
        {
            string transferTime = string.Empty;
            string optionDay = string.Empty;
            string[] arrDay = { Lang.sunday, Lang.monday, Lang.tuesday, Lang.wednesday, Lang.thursday, Lang.friday, Lang.saturday };
            TransferBO transferBo = new TransferBO(CurrentUserSession.NetSessionId);
            string transferOption = transferBo.GetOptionTransferCondition(custId);
            transferTime += Lang.allowtransfer;
            if (!string.IsNullOrEmpty(transferOption))
            {
                char[] arrchar = transferOption.ToCharArray();
                if (arrchar.Length > 0)
                {
                    for (int i = 1; i < arrchar.Length - 1; i++)
                    {
                        if (arrchar[i].ToString().Equals("1") == true)
                        {
                            optionDay += " - " + arrDay[i].ToString();
                        }
                    }

                    if (arrchar[0].ToString().Equals("1") == true)
                    {
                        optionDay += " - " + arrDay[0].ToString();
                    }

                    if (optionDay.Length > 0)
                    {
                        optionDay = optionDay.Substring(3);
                    }

                    if (arrchar[7].ToString().Equals("1") == true)
                    {
                        transferTime += " " + optionDay;
                    }
                    else
                    {
                        transferTime += " " + Lang.daily;
                    }
                }
            }

            return transferTime;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            if (CurrentUserSession.RoleId == (int)UserTypes.Super)
            {
                this.customerList = (this.Request["action"] == null) ? Common.GetLevel(CurrentUserSession.RoleId) : this.Request["action"];
            }
            else if (CurrentUserSession.RoleId == (int)UserTypes.Master)
            {
                this.customerList = (this.Request["action"] == null) ? "AgentList" : this.Request["action"];
            }
            else
            {
                this.customerList = (this.Request["action"] == null) ? string.Empty : this.Request["action"];
            }

            this.userName = (this.Request["username"] == null || this.Request["username"] == "All" || this.Request["username"] == Lang.enterusername) ? string.Empty : HttpUtility.HtmlDecode(this.Request["username"]);
            this.userName = this.userName.Length == 0 ? string.Empty : this.userName;

            this.custId = (this.Request["custid"] == null) ? CurrentUserSession.CustId : Convert.ToInt32(this.Request["custid"]);
            this.status = (this.Request["status"] == null) ? 1 : Convert.ToInt32(this.Request["status"]);

            // GetStringRequest
            this.pageSize = this.GetIntRequest("pagesize", this.pageSize);
            this.pageIndex = this.GetIntRequest("pageindex", this.pageIndex);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            CreditBalanceBO bo = new CreditBalanceBO(CurrentUserSession.NetSessionId);

            if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
            {
                this.dataTable = bo.GetCreditBalanceListAg(this.custId, this.userName, this.status, this.pageIndex, this.pageSize, ref this.totalPages);
            }
            else if (CurrentUserSession.RoleId == (int)UserTypes.Master)
            {
                if (this.customerList == "MemberList")
                {
                    this.dataTable = bo.GetCreditBalanceListAg(this.custId, this.userName, this.status, this.pageIndex, this.pageSize, ref this.totalPages);
                }
                else
                {
                    this.dataTable = bo.GetCreditBalanceListMag(this.custId, this.userName, this.status, this.pageIndex, this.pageSize, ref this.totalPages);
                }
            }
            else
            {
                if (this.customerList == "AgentList")
                {
                    this.dataTable = bo.GetCreditBalanceListMag(this.custId, this.userName, this.status, this.pageIndex, this.pageSize, ref this.totalPages);
                }
                else if (this.customerList == "MemberList")
                {
                    this.dataTable = bo.GetCreditBalanceListAg(this.custId, this.userName, this.status, this.pageIndex, this.pageSize, ref this.totalPages);
                }
                else
                {
                    this.dataTable = bo.GetCreditBalanceListSag(this.custId, this.userName, this.status, this.pageIndex, this.pageSize, ref this.totalPages);
                }
            }

            if ((Request.Cookies["sessionId"] == null || Request.Cookies["sessionId"].Value == CurrentUserSession.NetSessionId) &&
                (CurrentUserSession.RoleId == (int)UserTypes.Agent ||
                this.customerList == "MemberList"))
            {
                this.lcsWinLimit = bo.GetCreditBalanceListLCSWinLimit(this.custId, this.userName, this.status, this.pageIndex, this.pageSize);
            }

            return this.dataTable;
        }

        private void RenderPaging()
        {
            string pagingControlName_top = "_PagingTop";
            string pagingControlName_bottom = "MemberList" + CurrentUserSession.SiteName + "_PagingBottom";

            // Paging on Top
            List<object> arrPageSize = new List<object>(new object[] { 10, 50, 100, 500 });
            this.template.PagingHeader.pagingControl = new Paging_Control(this.template, pagingControlName_top, this.pageSize, Request.Url.AbsoluteUri, this.totalPages * this.pageSize, false);
            this.template.PagingHeader.selPageSize = Lang.pagesize + Utilities.HtmlGenerator.ComboBox("sel" + pagingControlName_top, (object)this.pageSize, arrPageSize, pagingControlName_top + ".SetPageSize(this.value)");
            //   this.template.PagingHeader.selected = null;
            //   this.template.PagingHeader.button = null;

            // Paging on bottom
            if (this.totalPages > 1)
            {
                this.template.PagingFooter.pagingControl = new Paging_Control(this.template, pagingControlName_bottom, this.pageSize, Request.Url.AbsoluteUri, this.totalPages * this.pageSize, false);
            }
            else
            {
                this.template.PagingFooter = null;
            }
        }

        private string FormatLCS(decimal value)
        {
            var ret = value == -1m ? Resources.Lang.Unlimited : value >= 100m ? "<font color='Red'>" + value.ToString("#,##0.##") + "%</font>" : value.ToString("#,##0.##") + "%";

            return ret;
        }
    }
}