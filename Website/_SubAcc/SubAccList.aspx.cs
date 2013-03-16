namespace Sunplus.Agent.UI._SubAcc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using Fanex.Age.UI.Components;
    using Fanex.Agent.UI._SubAcc;
    using Sunplus.Agent.Model.BL.SubAcc;
    using Sunplus.Agent.Model.Entities;

    /*
     * Created 20091224@Lee - Render sub-account listing page
     * Revision:
     * ?@? - ...
     *
     */

    public partial class SubAccList_Page : Fanex.Age.UI.TplPage
    {
        private int _AgentId = 0;
        const string CHECKED = Fanex.Age.UI.Common.CHECKED;
        const string DISABLED = Fanex.Age.UI.Common.DISABLED;

        private int AgentId
        {
            get
            {
                return _AgentId;
            }
            set
            {
                _AgentId = value;
            }
        }

        public override void ProcessRequest(ref Hashtable p)
        {
            this.AgentId = string.IsNullOrEmpty(Request["AgentId"]) ? CurrentUserSession.CustId : Convert.ToInt32(Request["AgentId"]);
        }

        public override object ValidateRequest(Hashtable p)
        {
            // Sub-account does not have permission to access SubAcc function
            if (CurrentUserSession.SubAccId > 0)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            SubAccBO bo = new SubAccBO(CurrentUserSession.NetSessionId);

            return bo.GetSubAccountList(this.AgentId);
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            // Add data to _jsData variables, access _jsData as JSON format
            RegisterJavascriptObject("lblconfirmclosesubacc", Resources.Lang.lblconfirmclosesubacc);
            RegisterJavascriptObject("lblConfirmClosed", Resources.Lang.lblConfirmClosed);
            RegisterJavascriptObject("FullControlPM", Resources.Lang.FullControl);
            RegisterJavascriptObject("UpdateViewPM", Resources.Lang.update);
            RegisterJavascriptObject("ViewPM", Resources.Lang.view);
            RegisterJavascriptObject("NonePM", Resources.Lang.none);

            SubAccList_Template tpl = new SubAccList_Template();
            List<ShadowAccount> subAccList = View as List<ShadowAccount>;

            // Render html from data and template

            #region Role Filter

            #endregion Role Filter

            int c = subAccList.Count;

            if (0 == c)
            {
                // Remove content section
                tpl.Content = null;
                writer.Write(tpl);
                return;
            }
            else
            {
                // Remove no information section
                tpl.NoInfo = null;
            }

            tpl.Content.Clear();
            SubAccList_Content_Block rowContent = new SubAccList_Content_Block();
            rowContent.overcolor = "#f8ff8d";
            rowContent.color = "#faf9ee";

            for (int i = 0; i < c; i++)
            {
                ShadowAccount subAcc = subAccList[i];
                RenderSubAccount(rowContent, i, subAcc);
                tpl.Content.Append(rowContent);
            }

            writer.Write(tpl);
        }

        private void RenderSubAccount(SubAccList_Content_Block rowContent, int i, ShadowAccount subAcc)
        {
            rowContent.no_value = (i + 1).ToString();
            rowContent.shadow_value = HttpUtility.HtmlEncode(subAcc.Username);
            rowContent.agentuserid_value = subAcc.AgentUserId.ToString();
            rowContent.agentid_value = this.AgentId.ToString();

            rowContent.totalbet_forecast_checked = rowContent.report_checked = rowContent.accountsetting_checked_read = rowContent.shadowmanagement_checked_read = rowContent.admin_checked_read = string.Empty;
            rowContent.shadowmanagement_checked_host = rowContent.accountsetting_checked_host = rowContent.admin_checked_host = "disabled=true";


            if (subAcc.PageAccess != "")
            {
                string[] permissions = subAcc.PageAccess.Split("-".ToCharArray());
                foreach (string item in permissions)
                {
                    if (item.Contains(ShadowAccess.TotalBet_ForeCast))
                    {
                        rowContent.totalbet_forecast_checked = "Checked=true";
                    }
                    else if (item.Contains(ShadowAccess.Report))
                    {
                        rowContent.report_checked = "Checked=true";
                    }
                    else if (item.Contains(ShadowAccess.AccountSeting))
                    {
                        if (item.Contains(ShadowAccess.Host))
                        {
                            rowContent.accountsetting_checked_host = "Checked=true";
                            rowContent.accountsetting_checked_read = "Checked=true disabled=true ";
                        }
                        else
                        {
                            rowContent.accountsetting_checked_read = "Checked=true";
                        }
                    }
                    else if (item.Contains(ShadowAccess.ShadowManagement))
                    {
                        if (item.Contains(ShadowAccess.Host))
                        {
                            rowContent.shadowmanagement_checked_host = "Checked=true";
                            rowContent.shadowmanagement_checked_read = "Checked=true disabled=true ";
                        }
                        else
                        {
                            rowContent.shadowmanagement_checked_read = "Checked=true";
                        }
                    }
                    else
                    {
                        if (item.Contains(ShadowAccess.Host))
                        {
                            rowContent.admin_checked_host = "Checked=true";
                            rowContent.admin_checked_read = "Checked=true disabled=true";
                        }
                        else
                        {
                            rowContent.admin_checked_read = "Checked=true";
                        }
                    }
                }
            }
        }
    }
}