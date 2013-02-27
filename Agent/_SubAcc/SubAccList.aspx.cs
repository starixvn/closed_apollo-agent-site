using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;

using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL.SubAcc;

using Fanex.Age.UI;

using Fanex.Age.UI.Components;

using Sunplus.Agent.Model.BL.Base;
using Sunplus.Agent.Model.BL.Definitions;
using System.Text;


/*
 * Created 20091224@Lee - Render sub-account listing page
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._SubAcc
{
    public partial class SubAccList_Page : Fanex.Age.UI.TplPage
    {
        public override void ProcessRequest(ref Hashtable p)
        {
            // No params in this url
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

            return bo.GetSubAccountList();
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
            List<SubAccount> subAccList = View as List<SubAccount>;

            // Render html from data and template

            #region Role Filter

            #endregion

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

            rowContent.overcolor = Fanex.Age.UI.Common.RowBgOver;

            for (int i = 0; i < c; i++)
            {
                SubAccount subAcc = subAccList[i];
                RenderSubAccount(rowContent, i, subAcc);
                tpl.Content.Append(rowContent);
            }

            writer.Write(tpl);
        }

        const string CHECKED = Fanex.Age.UI.Common.CHECKED;
        const string DISABLED = Fanex.Age.UI.Common.DISABLED;

        private void RenderSubAccount(SubAccList_Content_Block rowContent, int i, SubAccount subAcc)
        {
            rowContent.no_value = (i + 1).ToString();
            rowContent.username_value = HttpUtility.HtmlEncode(subAcc.UserName);
            rowContent.firstname_value = HttpUtility.HtmlEncode(subAcc.FirstName);
            rowContent.lastname_value = HttpUtility.HtmlEncode(subAcc.LastName);
            rowContent.createdate_value = subAcc.CreateDate.ToString();
            rowContent.id_value = subAcc.Id.ToString();

            PageAccess pa = subAcc.PageAccess;

            var rowDetail = new StringBuilder();

            var perms = Permissions.GetPageAccess(pa);
            var permsRow = new PermissionRow_Template();            

            rowContent.chkValueD = perms[0].Value ? "checked" : string.Empty;
            rowContent.chkValueE = perms[1].Value ? "checked" : string.Empty;

            if (perms[2].Value)
            {
                rowContent.PermissionName = Resources.Lang.FullControl;
                rowContent.lblPermission = "[A][B][H]";
            }
            else if (perms[3].Value)
            {
                rowContent.PermissionName = Resources.Lang.update;
                rowContent.lblPermission = "[B][H]";
            }
            else if (perms[4].Value)
            {
                rowContent.PermissionName = Resources.Lang.view;
                rowContent.lblPermission = "[H]";
            }
            else
            {
                rowContent.PermissionName = Resources.Lang.none;
                rowContent.lblPermission = string.Empty;
            }

            rowContent.chkValueC = perms[5].Value ? "checked" : string.Empty;
            rowContent.chkValueG = perms[6].Value ? "checked" : string.Empty;

            if (subAcc.IsClosed)
            {
                rowContent.color = Fanex.Age.UI.Common.RowBgClosed;
            }
            else if (0 == i % 2)
            {
                rowContent.color = Fanex.Age.UI.Common.RowBgEven;
            }
            else
            {
                rowContent.color = Fanex.Age.UI.Common.RowBgOdd;
            }
        }
    }
}
