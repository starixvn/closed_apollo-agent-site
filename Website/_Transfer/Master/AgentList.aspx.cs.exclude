// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.05.30">Improve performance, check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._Transfer.Master
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.Transfer;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._Transfer.Components;
    using Sunplus.Agent.Utilities;

    public partial class AgentList_Page : Transfer_Page
    {
        #region Variable

        private AgentList_Template tpl = new AgentList_Template();
        private object[] disabledPopupMenu = null;
        private object[] textPopupMenu = null;
        private object[] idPopupMenu = null;

        #endregion Variable

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable transferData = (View as Dictionary<string, DataTable>)["TransferData"];
            RegisterJavascriptObject("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
            RegisterJavascriptObject("roleid", Sunplus.Agent.Model.Entities.UserSession.Current.RoleId);
            RegisterJavascriptObject("wrnTransfer", Resources.Lang.confirmtransfer + "?");
            RegisterJavascriptObject("wrnTransferSuccessful", Resources.Lang.transfersuccessful);
            RegisterJavascriptObject("userNameDefault", Resources.Lang.enterusername);
            RegisterJavascriptObject("CallBack", "OnTransferComplete()");
            RegisterJavascriptObject("status", status);
            RegisterJavascriptObject("pageIndex", pageIndex);
            RegisterJavascriptObject("totalPage", totalPage);
            RegisterJavascriptObject("pageSize", pageSize);
            RegisterJavascriptObject("userName", userName);
            RegisterJavascriptObject("isYesterdayBalance", isYesterdayBalance);

            this.InitPopupTransferMulti();
            this.RenderSearchControl();
            this.RenderContentBlock(transferData);
            this.RenderTotalBlock(transferData);

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            TransferBO bo = new TransferBO(CurrentUserSession.NetSessionId);
            if (userName == Resources.Lang.enterusername)
            {
                userName = string.Empty;
            }

            DataTable dtb = bo.GetDataTransfer(status, pageIndex, ref totalPage, pageSize, userName, isYesterdayBalance);

            viewData.Add("TransferData", dtb);
            return viewData;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            custId = GetIntRequest("custid", CurrentUserSession.CustId);
            isCheckAllYesBalance = GetStringRequest("allYesBal", isCheckAllYesBalance);

            if (string.Compare(isCheckAllYesBalance, "yes") == 0)
            {
                isYesterdayBalance = true;
            }

            userName = Resources.Lang.enterusername;
            userName = string.IsNullOrEmpty(Request["username"]) == false ? Request["username"] : userName;
            pageIndex = GetIntRequest("pageindex", pageIndex);
            status = GetIntRequest("status", status);

            htbQueryString.Add("custid", custId.ToString());
            htbQueryString.Add("username", userName);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            pageSize = GetIntRequest("pagesize", pageSize);
            if (pageSize > 500)
            {
                pageSize = 500;
            }
            else if (pageSize < 10)
            {
                pageSize = 10;
            }

            if (!CurrentUserSession.SubPageAccess.Transfer || CurrentUserSession.RoleId != 3)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }

        #endregion Override

        #region Utility

        public virtual void RenderPopupTransferMulti(object[] textsOther, object[] isDisableOther, object[] idsOther)
        {
            MenuPopup menuTransferMulti = new MenuPopup();
            string transferMulti = "divTransferMulti";
            string other = menuTransferMulti.RenderLink(textsOther, idsOther, isDisableOther, transferMulti);

            this.tpl.Extend_tpl.Append(other);
        }

        public void RenderSearchControl()
        {
            string getCustomer = "GetTransferList('AgentList.aspx')";
            if (string.Compare(isCheckAllYesBalance, "yes") == 0)
            {
                this.tpl.searchControl = new SearchUserName(this.tpl, userName, status, getCustomer, true, true);
                this.tpl.allowTransfer = new AllowTransfer_Control(true, false, true);
                isYesterdayBalance = true;
            }
            else
            {
                this.tpl.searchControl = new SearchUserName(this.tpl, userName, status, getCustomer, true, false);
                this.tpl.allowTransfer = new AllowTransfer_Control(true, false, true);
            }
            ////Printer
            string printClick = "getPrint('print')";
            this.tpl.print = new PrintControl(this.tpl, printClick);
        }

        public void RenderContentBlock(DataTable dtb)
        {
            AgentList_Content_Block tplBlock = new AgentList_Content_Block();
            bool enable = false;
            bool isClosed = false;
            bool isSuspended = false;
            Dictionary<object, bool> dictStatus = new Dictionary<object, bool>();

            this.tpl.Content.Clear();
            this.tpl.Total.Clear();
            foreach (DataRow dr in dtb.Rows)
            {
                tplBlock.no_value = dr["RowNumber"].ToString();
                tplBlock.username_value = dr["username"].ToString();
                tplBlock.agbalance_yes_value = Formater.FormatNumber2(dr["yesterday_agentBalance"], langKey);
                tplBlock.agbalance_value = Formater.FormatNumber2(dr["agentBalance"], langKey);
                tplBlock.agcash_yes_value = Formater.FormatNumber2(dr["yesterday_agentCashBalance"], langKey);
                tplBlock.agcash_value = Formater.FormatNumber2(dr["agentCashBalance"], langKey);
                tplBlock.outstading_value = Formater.FormatNumber2(dr["agentOutstanding"], langKey);
                tplBlock.givencredit_value = Formater.FormatNumber2(dr["agentCredit"], langKey);
                tplBlock.usedcredit_value = Formater.FormatNumber2(dr["agentUsedCredit"], langKey);
                tplBlock.firstname_value = dr["firstname"].ToString();
                tplBlock.lastname_value = dr["lastname"].ToString();
                tplBlock.custid = dr["custid"].ToString();

                decimal amt = Convert.ToDecimal(Formater.FormatNumber4(dr["yesterday_agentBalance"], langKey)) * (-1);
                decimal agentBalance = Convert.ToDecimal(dr["agentBalance"]);
                decimal yesAgentBalance = Convert.ToDecimal(dr["yesterday_agentBalance"]);
                tplBlock.balance_yesterday_value = amt.ToString();
                tplBlock.disable = string.Empty;

                if (UserSession.Current.IsAllowedTotalBalanceTransfer)
                {
                    tplBlock.Link_Yes_Balance.Hide();
                    if (agentBalance == decimal.Zero)
                    {
                        tplBlock.disable = "disabled";
                        tplBlock.Link_Balance.Hide();
                    }
                    else
                    {
                        tplBlock.Link_Balance.Show();
                        tplBlock.Link_Balance.agbalance_value = Formater.FormatNumber4(dr["agentBalance"], langKey);
                        tplBlock.Link_Balance.custid = dr["custid"].ToString();
                    }
                }
                else
                {
                    tplBlock.Link_Balance.Hide();
                    if (yesAgentBalance == decimal.Zero)
                    {
                        tplBlock.disable = "disabled";
                        tplBlock.Link_Yes_Balance.Hide();
                    }
                    else
                    {
                        tplBlock.Link_Yes_Balance.Show();
                        tplBlock.Link_Yes_Balance.agbalance_yes_value = Formater.FormatNumber4(dr["yesterday_agentBalance"], langKey);
                        tplBlock.Link_Yes_Balance.custid = dr["custid"].ToString();
                    }
                }

                if (agentBalance < decimal.Zero)
                {
                    tplBlock.color_agbalance_value = "trans_favorite";
                }
                else
                {
                    tplBlock.color_agbalance_value = "r";
                }

                if (yesAgentBalance < decimal.Zero)
                {
                    tplBlock.color_agbalance_yes_value = "trans_favorite";
                }
                else
                {
                    tplBlock.color_agbalance_yes_value = "r";
                }

                if (Convert.ToDecimal(dr["agentCashBalance"]) < decimal.Zero)
                {
                    tplBlock.color_agcash_value = "trans_favorite";
                }
                else
                {
                    tplBlock.color_agcash_value = "r";
                }

                if (Convert.ToDecimal(dr["yesterday_agentCashBalance"]) < decimal.Zero)
                {
                    tplBlock.color_agcash_yes_value = "trans_favorite";
                }
                else
                {
                    tplBlock.color_agcash_yes_value = "r";
                }
                ////start bgcolor user
                enable = Convert.ToBoolean(dr["enabled"]);
                isClosed = Convert.ToBoolean(dr["closed"]);
                isSuspended = Convert.ToBoolean(dr["suspended"]);
                dictStatus.Clear();
                dictStatus.Add("Enabled", enable);
                dictStatus.Add("Closed", isClosed);
                dictStatus.Add("Suspended", isSuspended);
                this.RenderBackgroundColorUser(ref bgColor, dictStatus);
                tplBlock.color = bgColor;
                ////end bgcolor user

                this.tpl.Content.Append(tplBlock);
            }

            string pagingControlName_top = "_PagingTop";
            string pagingControlName_bottom = "_PagingBottom";

            ////Paging on Top

            this.tpl.PagingHeader.selected = null;
            this.tpl.PagingHeader.pagingControl = new Paging_Control(this.tpl, pagingControlName_top, pageSize, Request.Url.AbsoluteUri, totalPage * pageSize, false);
            this.tpl.PagingHeader.selPageSize = RenderPageSize(pagingControlName_top, pageSize);
            this.tpl.PagingHeader.button.ButtonText = new IcoTransferMulti(this.tpl);
            if (totalPage == 0)
            {
                this.tpl.pagingControl = Resources.Lang.noInfo;
            }
            else
            {
                this.tpl.pagingControl = new Paging_Control(this.tpl, pagingControlName_bottom, pageSize, Request.Url.AbsoluteUri, totalPage * pageSize, false);
            }
        }

        public void RenderTotalBlock(DataTable dtb)
        {
            AgentList_Total_Block tplBlockTotal = new AgentList_Total_Block();
            if (dtb.Rows.Count > 0)
            {
                tplBlockTotal.mabalance_yes_total = Formater.FormatNumber2(dtb.Compute("SUM(yesterday_agentBalance)", string.Empty), langKey);
                tplBlockTotal.mabalance_total = Formater.FormatNumber2(dtb.Compute("SUM(agentBalance)", string.Empty), langKey);
                tplBlockTotal.macash_yes_total = Formater.FormatNumber2(dtb.Compute("SUM(yesterday_agentCashBalance)", string.Empty), langKey);
                tplBlockTotal.macash_total = Formater.FormatNumber2(dtb.Compute("SUM(agentCashBalance)", string.Empty), langKey);
                tplBlockTotal.outstanding_total = Formater.FormatNumber2(dtb.Compute("SUM(agentOutstanding)", string.Empty), langKey);
                tplBlockTotal.usedcredit_total = Formater.FormatNumber2(dtb.Compute("SUM(agentUsedCredit)", string.Empty), langKey);
                tplBlockTotal.givencredit_total = Formater.FormatNumber2(dtb.Compute("SUM(agentCredit)", string.Empty), langKey);

                this.tpl.Total.Append(tplBlockTotal);
            }
        }

        public void RenderBackgroundColorUser(ref string bgColor, Dictionary<object, bool> dictStatus)
        {
            if (dictStatus["Enabled"])
            {
                if (dictStatus["Closed"])
                {
                    bgColor = "RowBgClosed";
                }
                else if (dictStatus["Suspended"])
                {
                    bgColor = "RowBgSuspended";
                }
                else
                {
                    bgColor = "RowBgOpen";
                }
            }
            else
            {
                bgColor = "RowBgDisabled";
            }
        }

        private void InitPopupTransferMulti()
        {
            this.disabledPopupMenu = new object[2];
            this.textPopupMenu = new object[2];
            this.idPopupMenu = new object[2];

            this.disabledPopupMenu[0] = 0;
            this.textPopupMenu[0] = Resources.Lang.onlythispage;
            this.idPopupMenu[0] = "transfer";

            this.disabledPopupMenu[1] = 0;
            this.textPopupMenu[1] = Resources.Lang.allpages;
            this.idPopupMenu[1] = "transferAll";

            this.RenderPopupTransferMulti(this.textPopupMenu, this.disabledPopupMenu, this.idPopupMenu);
        }

        #endregion Utility
    }
}