// --------------------------------------------------------------------------
// <copyright file="AgentList.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.05.30">Level Master - View Agent List</change>
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
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberInfo.CustomerList.Components;
    using Sunplus.Agent.Utilities;

    public partial class AgentList_Page : CustomerList_Page
    {
        #region Variable

        private static string cssHideUpdateMulti = "class='HideCol'";
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

        /// <summary>
        /// Render Content
        /// </summary>
        /// <param name="writer">writer</param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            this.InitJsLang();
            this.InitEditMenuControl();
            ////this.InitPopupStatus(false);
            ////this.InitPopupMulti();
            ////this.InitPopupOther();

            this.RequestQueryString = this.HashTableQueryString;

            this.JavaScriptRegister("custid", this.CustId);
            this.JavaScriptRegister("roleid", CurrentUserSession.RoleId);
            this.JavaScriptRegister("roleidP2P", CurrentUserSession.RoleId - 1);
            this.JavaScriptRegister("roleidLiveCasino", CurrentUserSession.RoleId - 1);
            this.JavaScriptRegister("roleidVirtualSports", CurrentUserSession.RoleId - 1);
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

            this.RenderHeader(this.template.Header);
            this.RenderContentBlock(customerData, this.template.Content);
            this.RenderSearchControl();

            TemplateMain.content = this.template;
            writer.Write(this.TemplateMain);
        }

        public override object InvokeData(Hashtable p)
        {
            CustomerListBO bo = new CustomerListBO(CurrentUserSession.NetSessionId);
            this.MasterId = CurrentUserSession.CustId;
            int totalPage = 0;
            DataTable dtb = bo.GetAgentCollection(this.CustId, (UserName == Resources.Lang.enterusername) ? string.Empty : UserName, PageIndex, PageSize, ref totalPage);
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

            // Paging on bottom
            if (this.TotalPage == 0)
            {
                this.template.pagingControl = string.Empty;
            }
            else
            {
                this.template.pagingControl = new Paging_Control(TemplateMain, pagingControlName_bottom, PageSize, Request.Url.AbsoluteUri, TotalPage * PageSize, false);
            }

            this.template.PagingHeader.button.Hide();

            //if (!CurrentUserSession.SubPageAccess.UpdateAccount)
            //{
            //    this.template.PagingHeader.selected.Hide();
            //}

            //this.template.PagingHeader.selected.function_all = "CheckAll_onClick(true);";
            //this.template.PagingHeader.selected.function_none = "CheckAll_onClick(false);";
        }

        public void RenderHeader(TplCore.Element.IElement e)
        {
            // Render MultiEdit Icon
            //string renderMulti = "RenderMulti(this,'MultiPopup','','" + this.MasterId + "','0')";
            AgentList_Header_Block tplBlock = e as AgentList_Header_Block;
            //if (!CurrentUserSession.SubPageAccess.UpdateAccount)
            //{
            //    tplBlock.DisplayUpdMulti = cssHideUpdateMulti;
            //}

            //tplBlock.isShowDCom = UserPermission.IsShowDoubleCommission.ToString();

            //tplBlock.editMult = new IcoEditMulti(TemplateMain, "");
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
                    //this.RenderRacing(dr, tplBlock);
                    // this.RenderCasino(dr, tplBlock);
                    // this.RenderBingo(dr, tplBlock);
                    //  this.RenderP2P(dr, tplBlock);
                    //  this.RenderLiveCasino(dr, tplBlock);
                    //  this.RenderVirtualSport(dr, tplBlock);

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

            this.agentId = dr["agentid"].ToString();

            tplBlock.chkid = this.agentId;
            tplBlock.chkid_value = dr["username"] as string;

            tplBlock.id = dr["RowNumber"].ToString();
            tplBlock.username = dr["username"] as string;

            //string editAgent_Single = "EditAgent_Single('" + this.agentId + "','" + dr["username"].ToString() + "', " + this.MasterId + ")";
            //string renderStatus = "RenderStatus(this,'Popup','" + this.agentId + "'," + isClosed.ToString().ToLower() + "," + isSuspended.ToString().ToLower() + ",0," + disableOutright.ToString().ToLower() + ", '" + tplBlock.chkid + "')";
            //Dictionary<object, bool> dictStatus = new Dictionary<object, bool>();
            //string backgroundColor = string.Empty;
            //dictStatus.Add("Enabled", enable);
            //dictStatus.Add("Closed", isClosed);
            //dictStatus.Add("Suspended", isSuspended);
            //IcoStatus status = new IcoStatus(TemplateMain, dictStatus, ref backgroundColor, editAgent_Single, renderStatus);
            //tplBlock.color = backgroundColor;
            //tplBlock.status = status;

            //string setDoubleCommission = string.Format("SetDoubleCommission('{0}',this,true);", this.agentId);

            //if (!UserPermission.IsShowDoubleCommission)
            //{
            //    setDoubleCommission = string.Empty;
            //}

            //bool isDoubleCommission = Convert.ToBoolean(dr["DoubleCommLosing"]);

            //IcoDComm dcomm = new IcoDComm(TemplateMain, isDoubleCommission, setDoubleCommission);
            //tplBlock.DCom.doublecommission = dcomm;

            //// Render Other
            string editMenuControl = "EditMenuControl(this,'editControl', '" + this.agentId + "', '" + dr["username"].ToString() + "')";
            tplBlock.edit_Option = new IcoOther(TemplateMain, "edit_" + this.agentId, string.Empty, editMenuControl);

            if (Convert.ToInt32(dr["NumberOfAgent"]) > 0)
            {
                string viewAgentDonwlineClick = "ViewDownLine('AgentList.aspx?custid=" + this.agentId + "')";
                IcoViewDownline viewAgentDownline = new IcoViewDownline(TemplateMain, viewAgentDonwlineClick);
                tplBlock.viewAgentList = viewAgentDownline;
            }
            else
            {
                tplBlock.viewAgentList = string.Empty;
            }

            if (Convert.ToInt32(dr["NumberOfMember"]) > 0)
            {
                IcoViewDownline viewPlayerDownline = new IcoViewDownline(TemplateMain, "");
                tplBlock.viewPlayerList = viewPlayerDownline;
            }
            else
            {
                tplBlock.viewPlayerList = string.Empty;
            }

            tplBlock.groupA = Formater.FormatNumber4(dr["groupa"], this.langKey);
            tplBlock.groupB = Formater.FormatNumber4(dr["groupb"], this.langKey);
            tplBlock.groupC = Formater.FormatNumber4(dr["groupc"], this.langKey);
            tplBlock.groupD = Formater.FormatNumber4(dr["groupd"], this.langKey);
            tplBlock.other = Formater.FormatNumber4(dr["DiscountOther"], this.langKey);
            tplBlock.onextwo = Formater.FormatNumber4(dr["Discount1x2"], this.langKey);
            tplBlock.game = Formater.FormatNumber4(dr["DiscountNumberGame"], this.langKey);

            tplBlock.HDP = dr["HDP_PositionTaking"].ToString();
            tplBlock.OU = dr["OU_PositionTaking"].ToString();
            tplBlock.HDPLive = dr["HDP_PositionTaking_Live"].ToString();
            tplBlock.OULive = dr["OU_PositionTaking_Live"].ToString();
        }
    }
}