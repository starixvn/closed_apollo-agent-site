// --------------------------------------------------------------------------
// <copyright file="AgentList.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.05.30">Level Super - View list Agent</change>
//   <change who="Kaden.Trinh" date="2012.05.30">Refactor code and check StyleCop</change>
//   <change who="Marc.Bui" date="2012.10.15">Add Double Commission feature</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.CustomerList.Super
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model.BL.MemberInfo.CustomerList;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberInfo.CustomerList.Components;
    using Sunplus.Agent.Utilities;
    using Fanex.Age.UI.Components.DoubleCommissionSearchUserName_Control;

    public partial class AgentList_Page : CustomerList_Page
    {
        #region Variable

        private AgentList_Template template = new AgentList_Template();
        private string langKey = string.Empty;
        private string agentId = string.Empty;

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
            this.InitPopupStatus(false);
            this.InitPopupOther_EditAgent();

            this.RequestQueryString = this.HashTableQueryString;
            this.JavaScriptRegister("custid", this.CustId);
            this.MasterId = this.CustId;
            this.JavaScriptRegister("roleid", CurrentUserSession.RoleId);
            this.JavaScriptRegister("roleidP2P", CurrentUserSession.RoleId - 2);
            this.JavaScriptRegister("disableP2P", 1);
            this.JavaScriptRegister("isAgentLevel", 0);
            this.JavaScriptRegister("usernamedefault", Resources.Lang.enterusername);

            // Double Commission
            this.JavaScriptRegister("strConfirmDisableDoubleComm", Resources.Lang.confirmDisableDoubleComm);
            this.JavaScriptRegister("lblAlertDisableDoubleCommMember", Resources.Lang.strConfirmDisableDoubleCommMember);
            this.JavaScriptRegister("strConfirmEnableDoubleComm", Resources.Lang.strConfirmEnableDoubleComm);
            this.JavaScriptRegister("strConfirmEnableDoubleCommMember", Resources.Lang.strConfirmEnableDoubleCommMember);
            this.JavaScriptRegister("strAllowed", Resources.Lang.allowed);
            this.JavaScriptRegister("strDisallowed", Resources.Lang.disallowed);

            this.RegisterClientVariables();

            this.langKey = CurrentUserSession.LangKey;
            DataTable customerData = (View as Dictionary<string, DataTable>)["CustomerData"];

            this.RenderContentBlock(customerData, this.template.Content);
            this.RenderSearchControl();

            this.TemplateMain.content = this.template;
            writer.Write(this.TemplateMain);
        }

        public override object InvokeData(Hashtable p)
        {
            CustomerListBO bo = new CustomerListBO(CurrentUserSession.NetSessionId);

            // CustId is Master Level
            int totalPage = 0;
            DataTable dtb = bo.GetAgentCollection(CustId, (UserName == Resources.Lang.enterusername) ? string.Empty : UserName, PageIndex, PageSize, Status, DCommStatus, ref totalPage);
            this.TotalPage = totalPage;
            this.ViewData.Add("CustomerData", dtb);
            return this.ViewData;
        }

        public void RenderSearchControl()
        {
            // Printer
            string printClick = "getPrint('page_main')";
            this.template.print = new PrintControl(this.template, printClick);

            // Searching
            string getCustomer = "GetCustomer('AgentList.aspx')";
            this.template.searchControl = new DoubleCommissionSearchUserName(TemplateMain, UserName, Status, DCommStatus, getCustomer);            

            string pagingControlName_top = "_PagingTop";
            string pagingControlName_bottom = "MemberList" + CurrentUserSession.SiteName + "_PagingBottom";

            // Paging on Top
            this.template.PagingHeader.pagingControl = new Paging_Control(TemplateMain, pagingControlName_top, PageSize, Request.Url.AbsoluteUri, TotalPage * PageSize, false);
            this.template.PagingHeader.selPageSize = this.RenderPageSize(pagingControlName_top, this.PageSize);

            if (this.TotalPage == 0)
            {
                this.template.pagingControl = string.Empty;
            }
            else
            {
                this.template.pagingControl = new Paging_Control(TemplateMain, pagingControlName_bottom, PageSize, Request.Url.AbsoluteUri, TotalPage * PageSize, false);
            }

            this.template.PagingHeader.button.Hide();
            this.template.PagingHeader.selected = null;
        }

        public void RenderContentBlock(DataTable dtb, TplCore.Element.IElement e)
        {
            if (dtb.Rows.Count > 0)
            {
                AgentList_Content_Block tplBlock = new AgentList_Content_Block();

                this.template.Content.Clear();

                foreach (DataRow dr in dtb.Rows)
                {
                    this.RenderDetail(dr, tplBlock);
                    this.RenderRacing(dr, tplBlock);
                    this.RenderCasino(dr, tplBlock);
                    this.RenderBingo(dr, tplBlock);
                    this.RenderP2P(dr, tplBlock);

                    this.template.Content.Append(tplBlock);
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

                this.template.Content.Clear();
                this.template.Content.Append(noInfo.ToString());
            }
        }

        public void RenderDetail(DataRow dr, TplCore.Element.IElement e)
        {
            AgentList_Content_Block tplBlock = e as AgentList_Content_Block;

            bool enable = Convert.ToBoolean(dr["enabled"]);
            bool isClosed = Convert.ToBoolean(dr["closed"]);
            bool isSuspended = Convert.ToBoolean(dr["suspended"]);
            bool disableOutright = !Convert.ToBoolean(dr["DisableOutright"]);

            this.agentId = dr["custid"].ToString();
            tplBlock.LnkUserName = CurrentUserSession.SubPageAccess.ViewReports ? "class='text' onclick=\"viewStatement(" + this.agentId + ",'Agent')\"" : string.Empty;

            tplBlock.custId = this.agentId;
            tplBlock.id = dr["RowNumber"].ToString();
            tplBlock.username = dr["username"] as string;

            string renderStatus = "RenderStatus(this,'Popup','" + this.agentId + "'," + isClosed.ToString().ToLower() + "," + isSuspended.ToString().ToLower() + ",0," + disableOutright.ToString().ToLower() + ", '" + dr["custid"].ToString() + "')";
            Dictionary<object, bool> dictStatus = new Dictionary<object, bool>();
            string backgroundColor = string.Empty;
            dictStatus.Add("Enabled", enable);
            dictStatus.Add("Closed", isClosed);
            dictStatus.Add("Suspended", isSuspended);
            IcoStatus status = new IcoStatus(TemplateMain, dictStatus, ref backgroundColor, string.Empty, renderStatus, true);
            tplBlock.color = backgroundColor;
            tplBlock.status = status;


            bool isDoubleCommission = Convert.ToBoolean(dr["DoubleCommLosing"]);
            string setDoubleCommission = string.Empty;

            IcoDComm dcomm = new IcoDComm(TemplateMain, isDoubleCommission, setDoubleCommission);
            tplBlock.DCom.doublecommission = dcomm;

            // Render Other menu
            string otherOnclick = "ShowFrmUpdOthers(this,'divUpdOthers', '" + this.agentId + "', '" + dr["username"].ToString() + "', 'ViewAgent')";
            tplBlock.other = new IcoOther(TemplateMain, otherOnclick);

            string viewDonwlineClick = "ViewDownLine('MemberList.aspx?custid=" + this.agentId + "&masterId=" + this.MasterId + "')";
            IcoViewDownline viewDownline = new IcoViewDownline(tplBlock, viewDonwlineClick);
            tplBlock.view = viewDownline;

            tplBlock.firstName = dr["firstname"] as string;
            tplBlock.lastName = dr["lastname"] as string;
            tplBlock.createDate = Formater.FormatDateTimeEn12(dr["createdate"]);
            tplBlock.lastLoginIp = Sunplus.Agent.Model.Common.AgentIPFormat.FormatIP(dr["lastloginip"].ToString());
            tplBlock.groupA = Formater.FormatNumber4(dr["groupa"], this.langKey);
            tplBlock.groupB = Formater.FormatNumber4(dr["groupb"], this.langKey);
            tplBlock.groupC = Formater.FormatNumber4(dr["groupc"], this.langKey);
            tplBlock.groupD = Formater.FormatNumber4(dr["groupd"], this.langKey);
            tplBlock.discountOther = Formater.FormatNumber4(dr["discountcs"], this.langKey);
            tplBlock.discount1x2 = Formater.FormatNumber4(dr["discount1x2"], this.langKey);
            tplBlock.discountGame = Formater.FormatNumber4(dr["discountnumber"], this.langKey);
        }

        private void RenderRacing(DataRow dr, TplCore.Element.IElement e)
        {
            AgentList_Content_Block tplBlock = e as AgentList_Content_Block;
            bool disStatus = DisabledRacing(dr);
            int iconHR = 0;

            if (!disStatus)
            {
                iconHR = 4;
            }

            tplBlock.iconhr = iconHR.ToString();
        }

        private void RenderP2P(DataRow dr, TplCore.Element.IElement e)
        {
            AgentList_Content_Block tplBlock = e as AgentList_Content_Block;
            int iconP2P = 0;

            if (UserPermission.IsShowP2P)
            {
                iconP2P = 4;
            }

            tplBlock.iconp2p = iconP2P.ToString();
        }

        private void RenderCasino(DataRow dr, TplCore.Element.IElement e)
        {
            AgentList_Content_Block tplBlock = e as AgentList_Content_Block;
            int iconCS = 0;

            if (UserPermission.IsShowCasino)
            {
                if (Convert.ToBoolean(dr["synccasino"]))
                {
                    // Edit/Sync Casino button
                    iconCS = 4;
                }
            }

            tplBlock.iconcs = iconCS.ToString();
        }

        private void RenderBingo(DataRow dr, TplCore.Element.IElement e)
        {
            AgentList_Content_Block tplBlock = e as AgentList_Content_Block;
            int iconBG = 0;

            if (UserPermission.IsShowBingo)
            {
                if (Convert.ToBoolean(dr["syncbingo"]))
                {
                    iconBG = 4; // Not syn
                }
            }

            tplBlock.iconbg = iconBG.ToString();
        }
    }
}