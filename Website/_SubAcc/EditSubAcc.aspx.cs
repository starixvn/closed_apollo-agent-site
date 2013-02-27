using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;

using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL.SubAcc;

using Fanex.Age.UI;

using Fanex.Age.UI.Components;

/*
 * Created 20091224@Lee - Render sub-account editing page
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._SubAcc
{
    public partial class EditSubAcc_Page : Fanex.Age.UI.TplPage
    {
        private int subAccId = 0;
        public override void ProcessRequest(ref Hashtable p)
        {
            p.AddHttpParameters(new string[] { 
                "SubAccId", 
                "User", 
                "CallBack" 
            }, Request);
        }

        public override object ValidateRequest(Hashtable p)
        {
            // Sub-account does not have permission to access SubAcc function
            if (CurrentUserSession.SubAccId > 0)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            // Validate sub-account id
            subAccId = int.Parse(p["SubAccId"] as string);
            if (subAccId <= 0)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            SubAccBO bo = new SubAccBO(CurrentUserSession.NetSessionId);
            return bo.GetSubAccount(subAccId);
        }

        const string CHECKED = Fanex.Age.UI.Common.CHECKED;


        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            RegisterJavascriptObject("CallBack", Parameters["CallBack"]);
            RegisterJavascriptObject("SubAccId", Parameters["SubAccId"]);

            EditSubAcc_Template tpl = new EditSubAcc_Template();
            SubAccount subAcc = View as SubAccount;

            tpl.username_value = HttpUtility.HtmlEncode(subAcc.UserName);
            tpl.firstname_value = HttpUtility.HtmlEncode(subAcc.FirstName);
            tpl.lastname_value = HttpUtility.HtmlEncode(subAcc.LastName);
            tpl.closed_value = subAcc.IsClosed ? CHECKED : null;

            writer.Write(tpl);
        }
    }
}
