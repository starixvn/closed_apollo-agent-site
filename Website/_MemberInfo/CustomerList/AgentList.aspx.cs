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

        private AgentList_Template template = new AgentList_Template();
        private string langKey = string.Empty;
        private string agentId = string.Empty;

        #endregion Variable

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
        }

        public void RenderNavigation()
        {
            // Get data
            CustomerListBO bo = new CustomerListBO(CurrentUserSession.NetSessionId);
            DataTable dtbNavigation = bo.GetAgentDownlinePath(CurrentUserSession.CustId, this.CustId);

            // Init template
            AgentList_nav_link_container_Block nav_link_Block = new AgentList_nav_link_container_Block();
            this.template.nav_link_container.nav_link.Clear();
            string url = "AgentList.aspx?custid={0}&us={1}";
            int counter = dtbNavigation.Rows.Count;
            if (counter > 0)
            {
                foreach (DataRow dr in dtbNavigation.Rows)
                {
                    counter--;
                    string us = dr["username"].ToString();
                    nav_link_Block.nav_link.navigateName = us;
                    nav_link_Block.nav_link.url = string.Format(url, dr["agentid"], us);
                    if (counter == 0)
                    {
                        nav_link_Block.nav_link.SetValue(us);
                    }
                    else
                    {
                        nav_link_Block.nav_link.narrow = "<img height=\"12\" width=\"12\" src=\"Resources/Images/breadcrumb.gif\"/>";
                    }
                    this.template.nav_link_container.nav_link.Append(nav_link_Block.nav_link);
                }
            }
            if (CurrentUserSession.CustId == this.CustId)
            {
                this.template.IconEditSetting = string.Empty;
            }
            else
            {
                string editMenuControl = "EditAgentMenuControl(this,'editControl', '" + this.CustId + "', '" + this.QueryUserName + "')";
                string onClickEvent = "EditAgentSetting(this,'" + this.CustId + "', '" + this.QueryUserName + "')";
                this.template.IconEditSetting = new IcoOther(TemplateMain, "edit_" + this.CustId, "icon-edit", onClickEvent, editMenuControl);
            }

            this.template.AgentName = this.QueryUserName;
            this.template.urlMemberList = "MemberList.aspx?custid=" + this.CustId + "&us=" + this.QueryUserName;
            this.template.urlShadowList = "#";
        }

        /// <summary>
        /// Render Content
        /// </summary>
        /// <param name="writer">writer</param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            this.InitJsLang();
            this.InitEditMenuControl();

            this.JavaScriptRegister("custid", this.CustId);
            this.JavaScriptRegister("roleid", CurrentUserSession.RoleId);
            this.JavaScriptRegister("usernamedefault", Resources.Lang.enterusername);

            this.RegisterClientVariables();

            this.langKey = CurrentUserSession.LangKey;
            DataTable customerData = (View as Dictionary<string, DataTable>)["CustomerData"];

            this.RenderHeader(this.template.Header);
            this.RenderContentBlock(customerData, this.template.Content);
            this.RenderSearchControl();
            RenderNavigation();
            TemplateMain.content = this.template;
            writer.Write(this.TemplateMain);
        }

        public override object InvokeData(Hashtable p)
        {
            CustomerListBO bo = new CustomerListBO(CurrentUserSession.NetSessionId);
            int totalPage = 0;
            DataTable dtb = bo.GetAgentCollection(this.CustId, (this.UserName == Resources.Lang.enterusername) ? string.Empty : UserName, PageIndex, PageSize, ref totalPage);
            this.TotalPage = totalPage;
            this.ViewData.Add("CustomerData", dtb);
            return this.ViewData;
        }

        public void RenderSearchControl()
        {
            // Searching
            string getCustomer = "GetCustomer('AgentList.aspx?custid=" + this.CustId + "&us=" + this.QueryUserName + "')";
            this.template.searchControl = new DoubleCommissionSearchUserName(TemplateMain, this.UserName, Status, DCommStatus, getCustomer);

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
        }

        public void RenderHeader(TplCore.Element.IElement e)
        {
            AgentList_Header_Block tplBlock = e as AgentList_Header_Block;
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

                    this.template.Content.Append(tplBlock);
                }
            }
            else
            {
                StringBuilder noInfo = new StringBuilder();
                noInfo.Append("<tr>");
                noInfo.Append("<td colspan='17'>");
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

            //// Render Other
            string editMenuControl = "EditAgentMenuControl(this,'editControl', '" + this.agentId + "', '" + dr["username"].ToString() + "')";
            string onClickEvent = "EditAgentSetting(this,'" + this.agentId + "', '" + dr["username"].ToString() + "')";
            tplBlock.edit_Option = new IcoOther(TemplateMain, "edit_" + this.agentId, "divOther", onClickEvent, editMenuControl);

            int totalSubAgents = Convert.ToInt32(dr["NumberOfAgent"]);

            if (totalSubAgents > 0)
            {
                string viewAgentDonwlineClick = "ViewDownLine('AgentList.aspx?custid=" + this.agentId + "&us=" + dr["username"].ToString() + "')";
                IcoViewDownline viewAgentDownline = new IcoViewDownline(TemplateMain, "view people-icon", viewAgentDonwlineClick, "[" + totalSubAgents.ToString() + "]");
                tplBlock.viewAgentList = viewAgentDownline;
            }
            else
            {
                tplBlock.viewAgentList = "0";
            }

            int totalMembers = Convert.ToInt32(dr["NumberOfMember"]);
            if (totalMembers > 0)
            {
                string viewPlayerDonwlineClick = "ViewDownLine('MemberList.aspx?custid=" + this.agentId + "&us=" + dr["username"].ToString() + "')";
                IcoViewDownline viewPlayerDownline = new IcoViewDownline(TemplateMain, "view people-icon", viewPlayerDonwlineClick, "[" + totalMembers.ToString() + "]");
                tplBlock.viewPlayerList = viewPlayerDownline;
            }
            else
            {
                tplBlock.viewPlayerList = "0";
            }

            tplBlock.groupA = Formater.FormatNumber4((Convert.ToDecimal(dr["groupa"]) * 100), this.langKey);
            tplBlock.groupB = Formater.FormatNumber4((Convert.ToDecimal(dr["groupb"]) * 100), this.langKey);
            tplBlock.groupC = Formater.FormatNumber4((Convert.ToDecimal(dr["groupc"]) * 100), this.langKey);
            tplBlock.groupD = Formater.FormatNumber4((Convert.ToDecimal(dr["groupd"]) * 100), this.langKey);
            tplBlock.other = Formater.FormatNumber4((Convert.ToDecimal(dr["DiscountOther"]) * 100), this.langKey);
            tplBlock.onextwo = Formater.FormatNumber4((Convert.ToDecimal(dr["Discount1x2"]) * 100), this.langKey);
            tplBlock.game = Formater.FormatNumber4((Convert.ToDecimal(dr["DiscountNumberGame"]) * 100), this.langKey);

            tplBlock.HDP = Formater.FormatNumber4((Convert.ToDecimal(dr["HDP_PositionTaking"]) / 100), this.langKey);
            tplBlock.OU = Formater.FormatNumber4((Convert.ToDecimal(dr["OU_PositionTaking"]) / 100), this.langKey);
            tplBlock.HDPLive = Formater.FormatNumber4((Convert.ToDecimal(dr["HDP_PositionTaking_Live"]) / 100), this.langKey);
            tplBlock.OULive = Formater.FormatNumber4((Convert.ToDecimal(dr["OU_PositionTaking_Live"]) / 100), this.langKey);
        }
    }
}