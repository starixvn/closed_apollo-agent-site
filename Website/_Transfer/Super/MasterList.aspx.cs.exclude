// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.05.30">Improve performance, check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._Transfer.Super
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

    public partial class MasterList_Page : Transfer_Page
    {
        #region Variable

        private MasterList_Template tpl = new MasterList_Template();
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
            userName = Resources.Lang.enterusername;
            userName = string.IsNullOrEmpty(Request["username"]) == false ? Request["username"] : userName;
            isCheckAllYesBalance = GetStringRequest("allYesBal", isCheckAllYesBalance);
            if (string.Compare(isCheckAllYesBalance, "yes") == 0)
            {
                isYesterdayBalance = true;
            }

            pageIndex = GetIntRequest("pageindex", pageIndex);
            status = GetIntRequest("status", status);

            htbQueryString.Add("custid", custId.ToString());
            htbQueryString.Add("username", userName);
            htbQueryString.Add("status", status);
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

            if (!CurrentUserSession.SubPageAccess.Transfer || CurrentUserSession.RoleId != 4)
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

        private void RenderSearchControl()
        {
            string getCustomer = "GetTransferList('MasterList.aspx')";
            if (string.Compare(isCheckAllYesBalance, "yes") == 0)
            {
                this.tpl.searchControl = new SearchUserName(this.tpl, userName, status, getCustomer, true, true);
                this.tpl.allowTransfer = new AllowTransfer_Control(false, false, true);
                isYesterdayBalance = true;
            }
            else
            {
                this.tpl.searchControl = new SearchUserName(this.tpl, userName, status, getCustomer, true, false);
                this.tpl.allowTransfer = new AllowTransfer_Control(false, false, true);
            }

            ////Printer
            string printClick = "getPrint('print')";
            this.tpl.print = new PrintControl(this.tpl, printClick);
        }

        private void RenderContentBlock(DataTable dtb)
        {
            MasterList_Content_Block tplBlock = new MasterList_Content_Block();
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
                tplBlock.mabalance_yes_value = Formater.FormatNumber2(dr["yesterday_magentBalance"], langKey);
                tplBlock.mabalance_value = Formater.FormatNumber2(dr["magentbalance"], langKey);
                tplBlock.macash_yes_value = Formater.FormatNumber2(dr["yesterday_magentCashBalance"], langKey);
                tplBlock.macash_value = Formater.FormatNumber2(dr["magentCashBalance"], langKey);
                tplBlock.outstading_value = Formater.FormatNumber2(dr["magentOutstanding"], langKey);
                tplBlock.givencredit_value = Formater.FormatNumber2(dr["magentCredit"], langKey);
                tplBlock.usedcredit_value = Formater.FormatNumber2(dr["magentUsedCredit"], langKey);
                tplBlock.firstname_value = dr["firstname"].ToString();
                tplBlock.lastname_value = dr["lastname"].ToString();
                tplBlock.custid = dr["custid"].ToString();

                decimal amt = Convert.ToDecimal(Formater.FormatNumber4(dr["yesterday_magentBalance"], langKey)) * (-1);
                decimal magentBalance = Convert.ToDecimal(dr["magentBalance"]);
                decimal yesMagentBalance = Convert.ToDecimal(dr["yesterday_magentBalance"]);
                tplBlock.balance_yesterday_value = amt.ToString();
                tplBlock.disable = string.Empty;

                if (Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowedTotalBalanceTransfer)
                {
                    tplBlock.Link_Yes_Balance.Hide();
                    if (magentBalance == decimal.Zero)
                    {
                        tplBlock.disable = "disabled";
                        tplBlock.Link_Balance.Hide();
                    }
                    else
                    {
                        tplBlock.Link_Balance.Show();
                        tplBlock.Link_Balance.mabalance_value = Formater.FormatNumber4(dr["magentBalance"], langKey);
                        tplBlock.Link_Balance.custid = dr["custid"].ToString();
                    }
                }
                else
                {
                    tplBlock.Link_Balance.Hide();
                    if (yesMagentBalance == decimal.Zero)
                    {
                        tplBlock.disable = "disabled";
                        tplBlock.Link_Yes_Balance.Hide();
                    }
                    else
                    {
                        tplBlock.Link_Yes_Balance.Show();
                        tplBlock.Link_Yes_Balance.mabalance_yes_value = Formater.FormatNumber4(dr["yesterday_magentBalance"], langKey);
                        tplBlock.Link_Yes_Balance.custid = dr["custid"].ToString();
                    }
                }

                if (magentBalance < decimal.Zero)
                {
                    tplBlock.color_mabalance_value = "trans_favorite";
                }
                else
                {
                    tplBlock.color_mabalance_value = "r";
                }

                if (yesMagentBalance < decimal.Zero)
                {
                    tplBlock.color_mabalance_yes_value = "trans_favorite";
                }
                else
                {
                    tplBlock.color_mabalance_yes_value = "r";
                }

                if (Convert.ToDecimal(dr["magentCashBalance"]) < decimal.Zero)
                {
                    tplBlock.color_macash_value = "trans_favorite";
                }
                else
                {
                    tplBlock.color_macash_value = "r";
                }

                if (Convert.ToDecimal(dr["yesterday_magentCashBalance"]) < decimal.Zero)
                {
                    tplBlock.color_macash_yes_value = "trans_favorite";
                }
                else
                {
                    tplBlock.color_macash_yes_value = "r";
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
                this.tpl.Paging.pagingControl = Resources.Lang.noInfo;
            }
            else
            {
                this.tpl.Paging.pagingControl = new Paging_Control(this.tpl, pagingControlName_bottom, pageSize, Request.Url.AbsoluteUri, totalPage * pageSize, false);
            }
        }

        private void RenderTotalBlock(DataTable dtb)
        {
            MasterList_Total_Block tplBlockTotal = new MasterList_Total_Block();
            if (dtb.Rows.Count > 0)
            {
                tplBlockTotal.mabalance_yes_total = Formater.FormatNumber2(dtb.Compute("SUM(yesterday_magentBalance)", string.Empty), langKey);
                tplBlockTotal.mabalance_total = Formater.FormatNumber2(dtb.Compute("SUM(magentbalance)", string.Empty), langKey);
                tplBlockTotal.macash_yes_total = Formater.FormatNumber2(dtb.Compute("SUM(yesterday_magentCashBalance)", string.Empty), langKey);
                tplBlockTotal.macash_total = Formater.FormatNumber2(dtb.Compute("SUM(magentCashBalance)", string.Empty), langKey);
                tplBlockTotal.outstanding_total = Formater.FormatNumber2(dtb.Compute("SUM(magentOutstanding)", string.Empty), langKey);
                tplBlockTotal.usedcredit_total = Formater.FormatNumber2(dtb.Compute("SUM(magentUsedCredit)", string.Empty), langKey);
                tplBlockTotal.givencredit_total = Formater.FormatNumber2(dtb.Compute("SUM(magentCredit)", string.Empty), langKey);

                this.tpl.Total.Append(tplBlockTotal);
            }
        }

        private void RenderBackgroundColorUser(ref string bgColor, Dictionary<object, bool> dictStatus)
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