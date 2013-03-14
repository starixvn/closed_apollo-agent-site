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

        public void RenderNavigation()
        {
            if (this.CustId == CurrentUserSession.CustId)
            {
                CurrentUserSession.NavigateUserName = new Dictionary<object, object>();
                CurrentUserSession.NavigateUserName.Add(this.CustId, this.QueryUserName);
                this.tempate.IconEditSetting = string.Empty;
                this.tempate.nav_link_container.SetValue(string.Empty);
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
                string editMenuControl = "EditAgentMenuControl(this,'editControl', '" + this.CustId + "', '" + this.QueryUserName + "')";
                this.tempate.IconEditSetting = new IcoOther(TemplateMain, "edit_" + this.CustId, "icon-edit", string.Empty, editMenuControl);
            }

            MemberList_nav_link_container_Block nav_link_Block = new MemberList_nav_link_container_Block();

            StringBuilder nav = new StringBuilder();
            this.tempate.nav_link_container.nav_link.Clear();
            string url = "AgentList.aspx?custid={0}&us={1}";
            int counter = CurrentUserSession.NavigateUserName.Count;
            foreach (object key in CurrentUserSession.NavigateUserName.Keys)
            {
                counter--;
                string navusername = CurrentUserSession.NavigateUserName[key].ToString();
                nav_link_Block.nav_link.navigateName = navusername;
                nav_link_Block.nav_link.url = string.Format(url, key, navusername);
                if (counter == 0)
                {
                    nav_link_Block.nav_link.SetValue(navusername);
                }
                else
                {
                    nav_link_Block.nav_link.narrow = "<img height=\"12\" width=\"12\" src=\"Resources/Images/breadcrumb.gif\"/>";
                }
                this.tempate.nav_link_container.nav_link.Append(nav_link_Block.nav_link);
            }
            this.tempate.AgentName = this.QueryUserName;
            this.tempate.urlAgentList = "AgentList.aspx?custid=" + this.CustId + "&us=" + this.QueryUserName;
            this.tempate.urlShadowList = "#";
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
            RenderNavigation();
            TemplateMain.content = this.tempate;
            writer.Write(this.TemplateMain);
        }

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
        }

        public override object InvokeData(Hashtable p)
        {
            CustomerListBO bo = new CustomerListBO(CurrentUserSession.NetSessionId);
            int totalPage = 0;
            DataTable dtb = bo.GetMemberCollection(this.CustId, (this.UserName == Resources.Lang.enterusername) ? string.Empty : UserName, PageIndex, PageSize, ref totalPage);
            this.TotalPage = totalPage;
            this.ViewData.Add("CustomerData", dtb);
            return this.ViewData;
        }

        public void RenderSearchControl()
        {
            // Searching
            Dictionary<object, int> dcommFilterDict = new Dictionary<object, int>() {
                {Resources.Lang.all, 2},{Resources.Lang.doublecommissionON, 1},
                {Resources.Lang.doublecommissionOFF, 0}
            };

            DoubleCommissionSearchUserName dcomControl = new DoubleCommissionSearchUserName(TemplateMain, this.UserName, Status, DCommStatus, "GetCustomer('MemberList.aspx')", dcommFilterDict);
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
        }

        public void RenderHeader(TplCore.Element.IElement e)
        {
            // Render MultiEdit Icon
            MemberList_Header_Block tplBlock = e as MemberList_Header_Block;
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

            tplBlock.chkid = dr["CustId"].ToString();
            tplBlock.chkid_value = dr["username"] as string;

            this.memberId = dr["CustId"].ToString();

            //tplBlock.custId = this.memberId;
            tplBlock.id = dr["RowNumber"].ToString();
            tplBlock.username = dr["username"] as string;

            // Render Other menu
            string userGroup = dr["ugroup"] as string;
            string editMenuControl = "EditMemberMenuControl(this,'editControl', '" + this.memberId + "', '" + dr["username"].ToString() + "','" + userGroup + "')";
            string onClickEvent = "EditMemberSetting(this,'" + this.memberId + "', '" + dr["username"].ToString() + "')";
            tplBlock.edit_Option = new IcoOther(TemplateMain, "edit_" + this.memberId, "divOther", onClickEvent, editMenuControl);

            tplBlock.group = userGroup;

            tplBlock.groupAgent = Formater.FormatNumber4((Convert.ToDecimal(dr["agentgroup"]) * 100), this.langKey); 
            tplBlock.oneXtwoAgent = Formater.FormatNumber4((Convert.ToDecimal(dr["agentdiscount1x2"]) * 100), this.langKey); 
            tplBlock.otherAgent = Formater.FormatNumber4((Convert.ToDecimal(dr["agentdiscountother"]) * 100), this.langKey); 
            tplBlock.gameAgent = Formater.FormatNumber4((Convert.ToDecimal(dr["agentdiscountnumbergame"]) * 100), this.langKey); 

            tplBlock.groupMember = Formater.FormatNumber4((Convert.ToDecimal(dr["CustGroup"]) * 100), this.langKey); 
            tplBlock.oneXtwoMember = Formater.FormatNumber4((Convert.ToDecimal(dr["CustDiscount1x2"]) * 100), this.langKey); 
            tplBlock.otherMember = Formater.FormatNumber4((Convert.ToDecimal(dr["CustDiscountOther"]) * 100), this.langKey); 
            tplBlock.gameMember = Formater.FormatNumber4((Convert.ToDecimal(dr["CustDiscountNumberGame"]) * 100), this.langKey);

            tplBlock.HDP = Formater.FormatNumber4((Convert.ToDecimal(dr["HDP_Soccer_PT"]) / 100), this.langKey); 
            tplBlock.OU = Formater.FormatNumber4((Convert.ToDecimal(dr["OU_Soccer_PT"]) / 100), this.langKey);
            tplBlock.HDPLive = Formater.FormatNumber4((Convert.ToDecimal(dr["HDP_Soccer_PT_Live"]) / 100), this.langKey); 
            tplBlock.OULive = Formater.FormatNumber4((Convert.ToDecimal(dr["OU_Soccer_PT_Live"]) / 100), this.langKey); 
        }
    }
}