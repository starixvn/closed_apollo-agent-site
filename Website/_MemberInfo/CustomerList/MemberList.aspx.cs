// --------------------------------------------------------------------------
// <copyright file="MemberList.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.05.30">Level Agent - View Member List</change>
//   <change who="Kaden.Trinh" date="2012.05.30">Refactor code and check StyleCop</change>
//   <change who="Marc.Bui" date="2012.10.11">Add Double Commission feature</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.CustomerList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Components.DoubleCommissionSearchUserName_Control;
    using Fanex.Agent.UI._MemberInfo.CustomerList;
    using Fanex.PTVirtualSports.Extension;
    using Sunplus.Agent.Model.BL.MemberInfo.CustomerList;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberInfo.CustomerList.Components;
    using Sunplus.Agent.Utilities;

    public partial class MemberList_Page : CustomerList_Page
    {
        #region Variable

        private static string cssHideUpdateMulti = "class='HideCol'";
        private MemberList_Template tempate = new MemberList_Template();
        private string langKey = string.Empty;
        private string agentId = string.Empty;
        private string memberId = string.Empty;

        #endregion Variable

        #region Properties

        private Hashtable RequestQueryString
        {
            get { return HttpContext.Current.Session["RequestQueryString"] == null ? new Hashtable() : (Hashtable)HttpContext.Current.Session["RequestQueryString"]; }
            set { HttpContext.Current.Session["RequestQueryString"] = value; }
        }

        #endregion Properties

        public override void RenderContent(HtmlTextWriter writer)
        {
            this.InitJsLang();
            this.InitEditMenuControl();

            this.RequestQueryString = this.HashTableQueryString;
            this.JavaScriptRegister("custid", this.CustId);
            this.JavaScriptRegister("roleid", CurrentUserSession.RoleId);
            this.JavaScriptRegister("usernamedefault", Resources.Lang.enterusername);

            // Double Commission
            this.JavaScriptRegister("strConfirmDisableDoubleComm", Resources.Lang.confirmDisableDoubleComm);
            this.JavaScriptRegister("lblAlertDisableDoubleCommMember", Resources.Lang.strConfirmDisableDoubleCommMember);
            this.JavaScriptRegister("strConfirmEnableDoubleComm", Resources.Lang.strConfirmEnableDoubleComm);
            this.JavaScriptRegister("strConfirmEnableDoubleCommMember", Resources.Lang.strConfirmEnableDoubleCommMember);
            this.JavaScriptRegister("strAllowed", Resources.Lang.doublecommissionON);
            this.JavaScriptRegister("strDisallowed", Resources.Lang.doublecommissionOFF);

            this.RegisterClientVariables();

            this.langKey = CurrentUserSession.LangKey;
            DataTable customerData = (View as Dictionary<string, DataTable>)["CustomerData"];

            this.RenderHeader(this.tempate.Header);
            this.RenderContentBlock(customerData, this.tempate.Content);
            this.RenderSearchControl();

            TemplateMain.content = this.tempate;
            writer.Write(this.TemplateMain);
        }

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            //HashTableQueryString.Add("AgentId", agentId.ToString());
            //HashTableQueryString.Add("FromAccount", string.Empty);
            //HashTableQueryString.Add("ParentForm", "CustomerList/Agent/MemberList.aspx");
        }

        public override object InvokeData(Hashtable p)
        {
            CustomerListBO bo = new CustomerListBO(CurrentUserSession.NetSessionId);
            int totalPage = 0;
            DataTable dtb = bo.GetMemberCollection(this.CustId, (this.UserName == Resources.Lang.enterusername || this.UserName == CurrentUserSession.UserName) ? string.Empty : UserName, PageIndex, PageSize, ref totalPage);
            this.TotalPage = totalPage;
            this.ViewData.Add("CustomerData", dtb);
            return this.ViewData;
        }

        public void RenderSearchControl()
        {
            // Printer
            string printClick = "getPrint('page_main')";
            this.tempate.print = new PrintControl(this.tempate, printClick);

            // Searching
            Dictionary<object, int> dcommFilterDict = new Dictionary<object, int>() {
                {Resources.Lang.all, 2},{Resources.Lang.doublecommissionON, 1},
                {Resources.Lang.doublecommissionOFF, 0}
            };

            DoubleCommissionSearchUserName dcomControl = new DoubleCommissionSearchUserName(TemplateMain, (this.UserName == Resources.Lang.enterusername || this.UserName == CurrentUserSession.UserName) ? Resources.Lang.enterusername : UserName, Status, DCommStatus, "GetCustomer('MemberList.aspx')", dcommFilterDict);
            this.tempate.searchControl = dcomControl;

            string pagingControlName_top = "_PagingTop";
            string pagingControlName_bottom = "MemberList" + CurrentUserSession.SiteName + "_PagingBottom";

            // Paging on Top
            this.tempate.PagingHeader.pagingControl = new Paging_Control(TemplateMain, pagingControlName_top, PageSize, Request.Url.AbsoluteUri, TotalPage * PageSize, false);
            this.tempate.PagingHeader.selPageSize = this.RenderPageSize(pagingControlName_top, this.PageSize);

            if (this.TotalPage == 0)
            {
                this.tempate.pagingControl = string.Empty;
            }
            else
            {
                this.tempate.pagingControl = new Paging_Control(TemplateMain, pagingControlName_bottom, PageSize, Request.Url.AbsoluteUri, TotalPage * PageSize, false);
            }

            this.tempate.PagingHeader.button.Hide();

            if (!CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                this.tempate.PagingHeader.selected.Hide();
            }

            //this.tempate.PagingHeader.selected.function_all = "CheckAll_onClick(true);";
            //this.tempate.PagingHeader.selected.function_none = "CheckAll_onClick(false);";
        }

        public void RenderHeader(TplCore.Element.IElement e)
        {
            // Render MultiEdit Icon
            MemberList_Header_Block tplBlock = e as MemberList_Header_Block;

            //if (!CurrentUserSession.SubPageAccess.UpdateAccount)
            //{
            //    tplBlock.DisplayUpdMulti = cssHideUpdateMulti;
            //}

            //tplBlock.isShowDCom = UserPermission.IsShowDoubleCommission.ToString();

            //string renderMulti = "RenderMulti(this,'MultiPopup','Agent','" + this.agentId + "','" + MasterId.ToString() + "')";
            //tplBlock.editmult = new IcoEditMulti(TemplateMain, renderMulti);
        }

        public void RenderContentBlock(DataTable dtb, TplCore.Element.IElement e)
        {
            if (dtb.Rows.Count > 0)
            {
                MemberList_Content_Block tplBlock = new MemberList_Content_Block();
                this.tempate.Content.Clear();
                foreach (DataRow dr in dtb.Rows)
                {
                    this.RenderDetail(dr, tplBlock);
                    //this.RenderRacing(dr, tplBlock);
                    //this.RenderCasino(dr, tplBlock);
                    //this.RenderBingo(dr, tplBlock);
                    //this.RenderP2P(dr, tplBlock);
                    //this.RenderLiveCasino(dr, tplBlock);
                    //this.RenderVirtualSport(dr, tplBlock);

                    this.tempate.Content.Append(tplBlock);
                }
            }
            else
            {
                StringBuilder noInfo = new StringBuilder();
                noInfo.Append("<tr>");
                noInfo.Append("<td colspan='23'>");
                noInfo.Append(Resources.Lang.noInfo);
                noInfo.Append("</td>");
                noInfo.Append("</tr>");

                this.tempate.Content.Clear();
                this.tempate.Content.Append(noInfo.ToString());
            }
        }

        public void RenderDetail(DataRow dr, TplCore.Element.IElement e)
        {
            MemberList_Content_Block tplBlock = e as MemberList_Content_Block;
            //bool enable = Convert.ToBoolean(dr["enabled"]);
            //bool isClosed = Convert.ToBoolean(dr["closed"]);
            //bool isSuspended = Convert.ToBoolean(dr["suspended"]);
            //bool disableOutright = !Convert.ToBoolean(dr["DisableOutright"]);
            //bool isDisableNumber = dr.Table.Columns.Contains("disablenumber") ? Convert.ToBoolean(dr["disablenumber"]) : true;

            tplBlock.chkid = dr["CustId"].ToString();
            tplBlock.chkid_value = dr["username"] as string;

            this.memberId = dr["CustId"].ToString();
            //tplBlock.LnkUserName = CurrentUserSession.SubPageAccess.ViewReports ? "class='text' onclick=\"viewCustOutSt('" + dr["username"].ToString() + "'," + this.memberId + ")\"" : string.Empty;

            //tplBlock.custId = this.memberId;
            tplBlock.id = dr["RowNumber"].ToString();
            tplBlock.username = dr["username"] as string;

            //string editMember_Single = "EditMember_Single('" + this.memberId + "','" + dr["username"].ToString() + "', " + this.agentId + ", '" + "Agent" + "')";
            //string renderStatus = "RenderStatus(this,'Popup','" + this.memberId + "'," + isClosed.ToString().ToLower() + "," + isSuspended.ToString().ToLower() + ",0," + disableOutright.ToString().ToLower() + ", '" + tplBlock.chkid + "')";
            //Dictionary<object, bool> dictStatus = new Dictionary<object, bool>();
            //string backgroundColor = string.Empty;

            //dictStatus.Add("Enabled", enable);
            //dictStatus.Add("Closed", isClosed);
            //dictStatus.Add("Suspended", isSuspended);
            //IcoStatus status = new IcoStatus(TemplateMain, dictStatus, ref backgroundColor, editMember_Single, renderStatus);
            //tplBlock.color = backgroundColor;
            ////tplBlock.status = status;

            //string setDoubleCommission = string.Format("SetDoubleCommission('{0}',this);", this.memberId);
            //if (!UserPermission.IsShowDoubleCommission)
            //{
            //    setDoubleCommission = string.Empty;
            //}

            // bool isDoubleCommission = Convert.ToBoolean(dr["DoubleCommLosing"]);

            // MemberIcoDComm dcomm = new MemberIcoDComm(TemplateMain, isDoubleCommission, setDoubleCommission);
            //tplBlock.DCom.doublecommission = dcomm;

            // Render Other menu
            string editMenuControl = "EditMenuControl(this,'editControl', '" + this.memberId + "', '" + dr["username"].ToString() + "')";
            tplBlock.edit_Option = new IcoOther(TemplateMain, "edit_" + this.memberId, string.Empty, editMenuControl);

            tplBlock.group = dr["ugroup"] as string;

            tplBlock.groupAgent = Formater.FormatNumber4(dr["agentgroup"], this.langKey);
            tplBlock.oneXtwoAgent = Formater.FormatNumber4(dr["agentdiscount1x2"], this.langKey);
            tplBlock.otherAgent = Formater.FormatNumber4(dr["agentdiscountother"], this.langKey);
            tplBlock.gameAgent = Formater.FormatNumber4(dr["agentdiscountnumbergame"], this.langKey);

            tplBlock.groupMember = Formater.FormatNumber4(dr["CustGroup"], this.langKey);
            tplBlock.oneXtwoMember = Formater.FormatNumber4(dr["CustDiscount1x2"], this.langKey);
            tplBlock.otherMember = Formater.FormatNumber4(dr["CustDiscountOther"], this.langKey);
            tplBlock.gameMember = Formater.FormatNumber4(dr["CustDiscountNumberGame"], this.langKey);

            tplBlock.HDP = Formater.DecFormat(dr["HDP_Soccer_PT"], 2);
            tplBlock.OU = Formater.DecFormat(dr["OU_Soccer_PT"], 2);
            tplBlock.HDPLive = Formater.DecFormat(dr["HDP_Soccer_PT_Live"], 2);
            tplBlock.OULive = Formater.DecFormat(dr["OU_Soccer_PT_Live"], 2);
        }
    }
}