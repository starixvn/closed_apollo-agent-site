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
            if (this.CustId == CurrentUserSession.CustId)
            {
                CurrentUserSession.NavigateUserName = new Dictionary<object, object>();
                CurrentUserSession.NavigateUserName.Add(this.CustId, this.QueryUserName);
            }
            else
            {
                if (!CurrentUserSession.NavigateUserName.ContainsKey(this.CustId))
                {
                    CurrentUserSession.NavigateUserName.Add(this.CustId, this.QueryUserName);
                }
                else
                {
                    this.NavUserName = CurrentUserSession.NavigateUserName;
                    CurrentUserSession.NavigateUserName = new Dictionary<object, object>();
                    foreach (object key in this.NavUserName.Keys)
                    {
                        if ((int)key == this.CustId)
                        {
                            CurrentUserSession.NavigateUserName.Add(key, this.NavUserName[key]);
                            break;
                        }
                        CurrentUserSession.NavigateUserName.Add(key, this.NavUserName[key]);
                    }
                    this.NavUserName = new Dictionary<object, object>();
                }
            }

            AgentList_nav_link_Block nav_link_Block = new AgentList_nav_link_Block();

            StringBuilder nav = new StringBuilder();
            this.template.nav_link.Clear();
            string url = "AgentList.aspx?custid={0}&us={1}";
            int counter = CurrentUserSession.NavigateUserName.Count;
            foreach (object key in CurrentUserSession.NavigateUserName.Keys)
            {
                counter--;
                string navusername = CurrentUserSession.NavigateUserName[key].ToString();
                nav_link_Block.navigateName = navusername;
                nav_link_Block.url = string.Format(url, key, navusername);
                if (counter == 0)
                {
                    nav_link_Block.SetValue(navusername);
                }
                else
                {
                    nav_link_Block.narrow = " ->";
                }
                this.template.nav_link.Append(nav_link_Block);
            }
            this.template.AgentName = this.QueryUserName;
            string editMenuControl = "EditMenuControl(this,'editControl', '" + this.CustId + "', '" + this.QueryUserName + "')";
            this.template.IconEditSetting = new IcoOther(TemplateMain, "edit_" + this.CustId, string.Empty, editMenuControl);
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
            string getCustomer = "GetCustomer('AgentList.aspx')";
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
            tplBlock.edit_Option = new IcoOther(TemplateMain, "edit_" + this.agentId, string.Empty, editMenuControl);

            if (Convert.ToInt32(dr["NumberOfAgent"]) > 0)
            {
                string viewAgentDonwlineClick = "ViewDownLine('AgentList.aspx?custid=" + this.agentId + "&us=" + dr["username"].ToString() + "')";
                IcoViewDownline viewAgentDownline = new IcoViewDownline(TemplateMain, "view", viewAgentDonwlineClick);
                tplBlock.viewAgentList = viewAgentDownline;
            }
            else
            {
                tplBlock.viewAgentList = string.Empty;
            }

            if (Convert.ToInt32(dr["NumberOfMember"]) > 0)
            {
                string viewPlayerDonwlineClick = "ViewDownLine('MemberList.aspx?custid=" + this.agentId + "&us=" + dr["username"].ToString() + "')";
                IcoViewDownline viewPlayerDownline = new IcoViewDownline(TemplateMain, "view", viewPlayerDonwlineClick);
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