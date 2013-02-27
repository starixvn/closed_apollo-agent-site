// --------------------------------------------------------------------------
// <copyright file="MasterList.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.05.30">Level Super - View list Master </change>
//   <change who="Kaden.Trinh" date="2012.05.30">Refactor code and check StyleCop</change>
//   <change who="Marc.Bui" date="2012.10.11">Add Double Commission feature</change>
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
    using Fanex.PTVirtualSports.Extension;
    using Fanex.Age.UI.Components.DoubleCommissionSearchUserName_Control;

    public partial class MasterList_Page : CustomerList_Page
    {
        #region Variable

        private static string cssHideUpdateMulti = "class='HideCol'";
        private string langKey = string.Empty;
        private MasterList_Template template = new MasterList_Template();
        private string superId = string.Empty;
        private string masterid = string.Empty;

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
            this.InitPopupStatus(true);
            this.InitPopupMulti();
            this.InitPopupOther();

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

            this.template.Content.Clear();
            DataTable customerData = (View as Dictionary<string, DataTable>)["CustomerData"];

            this.RenderSearchControl();
            this.RenderHeader(this.template.Header);
            this.RenderContentBlock(customerData, this.template);

            TemplateMain.content = this.template;
            writer.Write(this.TemplateMain);
        }

        public override object InvokeData(Hashtable p)
        {
            this.superId = CurrentUserSession.CustId.ToString();
            CustomerListBO bo = new CustomerListBO(CurrentUserSession.NetSessionId);
            int totalPage = 0;

            DataTable dtb = bo.GetMasterCollection(CurrentUserSession.CustId, (UserName == Resources.Lang.enterusername) ? string.Empty : UserName, PageIndex, PageSize, Status, DCommStatus, ref totalPage);

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
            string getCustomer_onClick = "GetCustomer('MasterList.aspx')";
            this.template.searchControl = new DoubleCommissionSearchUserName(TemplateMain, UserName, Status, DCommStatus, getCustomer_onClick);

            // Paging
            string pagingControlName_top = "_PagingTop";
            string pagingControlName_bottom = CurrentUserSession.SiteName + "_PagingBottom";

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

            if (!CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                this.template.PagingHeader.selected.Hide();
            }

            this.template.PagingHeader.selected.function_all = "CheckAll_onClick(true);";
            this.template.PagingHeader.selected.function_none = "CheckAll_onClick(false);";
        }

        public void RenderHeader(TplCore.Element.IElement e)
        {
            MasterList_Header_Block tplBlock = e as MasterList_Header_Block;

            if (!CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                tplBlock.DisplayUpdMulti = cssHideUpdateMulti;
            }

            tplBlock.isShowDCom = UserPermission.IsShowDoubleCommission.ToString();

            string renderMulti = "RenderMulti(this,'MultiPopup','','" + this.superId + "')";
            tplBlock.editMult = new IcoEditMulti(TemplateMain, renderMulti);
        }

        public void RenderContentBlock(DataTable dtb, TplCore.Element.IElement e)
        {
            if (dtb.Rows.Count > 0)
            {
                MasterList_Content_Block tplBlock = new MasterList_Content_Block();
                foreach (DataRow dr in dtb.Rows)
                {
                    this.RenderDetail(dr, tplBlock);
                    this.RenderRacing(dr, tplBlock);
                    this.RenderCasino(dr, tplBlock);
                    this.RenderBingo(dr, tplBlock);
                    this.RenderP2P(dr, tplBlock);
                    this.RenderLiveCasino(dr, tplBlock);
                    this.RenderVirtualSport(dr, tplBlock);

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
            MasterList_Content_Block tplBlock = e as MasterList_Content_Block;

            this.masterid = dr["custid"].ToString();
            bool enable = Convert.ToBoolean(dr["enabled"]);
            bool isClosed = Convert.ToBoolean(dr["closed"]);
            bool isSuspended = Convert.ToBoolean(dr["suspended"]);
            bool isAllowAutoPT = !Convert.ToBoolean(dr["DisableAutoMPT"]);
            bool isOutright = !Convert.ToBoolean(dr["DisableOutright"]);

            tplBlock.custId = this.masterid;
            tplBlock.chkid = "chkid_" + this.masterid;
            tplBlock.chkid_value = dr["username"] as string;

            tplBlock.id = dr["RowNumber"].ToString();
            tplBlock.username = dr["username"] as string;

            // Render Status and Status Menu Popup
            string editMaster_Single = "EditMaster_Single('" + this.masterid + "','" + dr["username"].ToString() + "', " + this.superId + ")";
            string renderStatus = "RenderStatus(this,'Popup','" + this.masterid + "'," + isClosed.ToString().ToLower() + "," + isSuspended.ToString().ToLower() + "," + isAllowAutoPT.ToString().ToLower() + "," + isOutright.ToString().ToLower() + ", '" + tplBlock.chkid + "')";
            Dictionary<object, bool> dictStatus = new Dictionary<object, bool>();
            string backgroundColor = string.Empty;
            dictStatus.Add("Enabled", enable);
            dictStatus.Add("Closed", isClosed);
            dictStatus.Add("Suspended", isSuspended);
            IcoStatus status = new IcoStatus(TemplateMain, dictStatus, ref backgroundColor, editMaster_Single, renderStatus);
            tplBlock.color = backgroundColor;
            tplBlock.status = status;

            string setDoubleCommission = string.Format("SetDoubleCommission('{0}',this,true);", this.masterid);
            if (!UserPermission.IsShowDoubleCommission)
            {
                setDoubleCommission = string.Empty;
            }

            bool isDoubleCommission = Convert.ToBoolean(dr["DoubleCommLosing"]);

            IcoDComm dcomm = new IcoDComm(TemplateMain, isDoubleCommission, setDoubleCommission);
            tplBlock.DCom.doublecommission = dcomm;

            // Render Other menu
            string otherOnclick = "ShowFrmUpdOthers(this,'divUpdOthers', '" + this.masterid + "', '" + dr["username"].ToString() + "', 'Master')";
            tplBlock.other_Option = new IcoOther(TemplateMain, otherOnclick);

            if (!CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                tplBlock.DisplayUpdMulti = cssHideUpdateMulti;
            }

            tplBlock.LnkUserName = CurrentUserSession.SubPageAccess.ViewReports ? "class='text' onclick=\"viewStatement(" + this.masterid + ",'Master')\"" : string.Empty;

            // View downline agent level
            string viewDownlineClick = "ViewDownLine('AgentList.aspx?custId=" + this.masterid.ToString() + "')";
            IcoViewDownline viewDownline = new IcoViewDownline(TemplateMain, viewDownlineClick);
            tplBlock.view = viewDownline;

            tplBlock.firstName = dr["firstname"] as string;
            tplBlock.lastName = dr["lastname"] as string;
            tplBlock.groupA = Formater.FormatNumber4(dr["groupa"], this.langKey);
            tplBlock.groupB = Formater.FormatNumber4(dr["groupb"], this.langKey);
            tplBlock.groupC = Formater.FormatNumber4(dr["groupc"], this.langKey);
            tplBlock.groupD = Formater.FormatNumber4(dr["groupd"], this.langKey);
            tplBlock.other = Formater.FormatNumber4(dr["discountcs"], this.langKey);
            tplBlock.lb1x2 = Formater.FormatNumber4(dr["discount1x2"], this.langKey);
            tplBlock.game = Formater.FormatNumber4(dr["discountnumber"], this.langKey);
            tplBlock.createdate = Formater.FormatDateTimeEn12(dr["createdate"]);
            tplBlock.lastLoginIp = Sunplus.Agent.Model.Common.AgentIPFormat.FormatIP(dr["lastloginip"].ToString());
        }

        private void RenderRacing(DataRow dr, TplCore.Element.IElement e)
        {
            MasterList_Content_Block tplBlock = e as MasterList_Content_Block;
            bool disStatus = DisabledRacing(dr);
            int status = 1;
            int iconHR = 1;

            if (disStatus)
            {
                status = 0;
                iconHR = 2;
            }

            tplBlock.statusRacing = status.ToString();
            tplBlock.iconhr = iconHR.ToString();
        }

        private void RenderP2P(DataRow dr, TplCore.Element.IElement e)
        {
            MasterList_Content_Block tplBlock = e as MasterList_Content_Block;
            int statusP2P = -1;
            int iconP2P = 0;
            int isDisabledP2P = 0;

            if (UserPermission.IsShowP2P)
            {
                statusP2P = 1;
                iconP2P = 1;

                if (Convert.ToBoolean(dr["disablep2p"]))
                {
                    iconP2P = 2;
                }
            }

            if (CurrentUserSession.IsInternal == true || Convert.ToBoolean(dr["disablep2p"]) == true)
            {
                isDisabledP2P = 1;
            }

            tplBlock.statusP2P = statusP2P.ToString();
            tplBlock.iconp2p = iconP2P.ToString();
            tplBlock.isDisabledP2P = isDisabledP2P.ToString();
        }

        private void RenderCasino(DataRow dr, TplCore.Element.IElement e)
        {
            MasterList_Content_Block tplBlock = e as MasterList_Content_Block;
            int statusCS = -1;
            int iconCS = 0;

            if (UserPermission.IsShowCasino)
            {
                statusCS = 0; // Not syn
                iconCS = 3;

                if (Convert.ToBoolean(dr["synccasino"]))
                {
                    statusCS = 1;
                    iconCS = 1;

                    if (Convert.ToBoolean(dr["disablecasino"]))
                    {
                        // Edit/Sync Casino button
                        iconCS = 2;
                    }
                }
            }

            tplBlock.statusCS = statusCS.ToString();
            tplBlock.iconcs = iconCS.ToString();
        }

        private void RenderBingo(DataRow dr, TplCore.Element.IElement e)
        {
            MasterList_Content_Block tplBlock = e as MasterList_Content_Block;

            int statusBG = -1;
            int iconBG = 0;

            if (UserPermission.IsShowBingo)
            {
                statusBG = 0; // Not syn
                iconBG = 3;

                if (Convert.ToBoolean(dr["syncbingo"]))
                {
                    statusBG = 1;
                    iconBG = 1;

                    if (Convert.ToBoolean(dr["disablebingo"]))
                    {
                        // Edit/Sync Casino button
                        iconBG = 2;
                    }
                }
            }

            tplBlock.statusBingo = statusBG.ToString();
            tplBlock.iconbg = iconBG.ToString();
        }

        private void RenderLiveCasino(DataRow dr, TplCore.Element.IElement e)
        {
            MasterList_Content_Block tplBlock = e as MasterList_Content_Block;

            int statusLiveCasino = -1;
            int iconLiveCasino = 0;
            int isDisabledLiveCS = 0;

            if (UserPermission.IsShowLiveCasino)
            {
                statusLiveCasino = 0; // Not syn
                iconLiveCasino = 3;

                if (Convert.ToBoolean(dr["synclivecasino"]))
                {
                    statusLiveCasino = 1;
                    iconLiveCasino = 1;

                    if (Convert.ToBoolean(dr["disablelivecasino"]))
                    {
                        // Edit Sync Casino button
                        iconLiveCasino = 2;
                    }
                }
            }

            if (CurrentUserSession.IsInternal == true || Convert.ToBoolean(dr["disablelivecasino"]) == true)
            {
                isDisabledLiveCS = 1;
            }

            tplBlock.statusLiveCasino = statusLiveCasino.ToString();
            tplBlock.iconLiveCasino = iconLiveCasino.ToString();
            tplBlock.isDisabledLiveCS = isDisabledLiveCS.ToString();
        }

        private void RenderVirtualSport(DataRow dr, TplCore.Element.IElement e)
        {
            MasterList_Content_Block tplBlock = e as MasterList_Content_Block;
            int statusVirtualSport = -1;
            int iconVirtualSport = 0;
            int isDisabledVS = 0;

            if (DataRowExtension.HasColumn(dr, "disablevirtualsports"))
            {
                if (UserPermission.IsShowVirtualSports)
                {
                    statusVirtualSport = 1;
                    iconVirtualSport = 1;

                    if (Convert.ToBoolean(dr["disablevirtualsports"]))
                    {
                        iconVirtualSport = 2;
                    }
                }

                if (CurrentUserSession.IsInternal == true || Convert.ToBoolean(DataRowExtension.HasColumn(dr, "disablevirtualsports")) == true)
                {
                    isDisabledVS = 1;
                }
            }

            tplBlock.isDisabledVS = isDisabledVS.ToString();
            tplBlock.statusVirtualSport = statusVirtualSport.ToString();
            tplBlock.iconVirtualSport = iconVirtualSport.ToString();
        }
    }
}