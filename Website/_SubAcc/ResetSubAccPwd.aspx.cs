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
 * Created 20091224@Lee - Render sub-account reseting password page
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._SubAcc
{
    public partial class ResetSubAccPwd_Page : Fanex.Age.UI.TplPage
    {
        public override void ProcessRequest(ref Hashtable p)
        {
            p.AddHttpParameters(new string[]{
                "CallBack", "SubAccId"
            }, Request);
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
            return null;
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            // Add data to _page variables, access it as JSON format
            RegisterJavascriptObject("errMsgEmpty", Resources.Lang.alertPasswordEmpty);
            RegisterJavascriptObject("errMsgRequire", Resources.Lang.lblPasswordRequire);
            RegisterJavascriptObject("errMsgConfirm", Resources.Lang.alertErrorConfirm);
            RegisterJavascriptObject("SubAccId", Parameters["SubAccId"]);
            RegisterJavascriptObject("CallBack", Parameters["CallBack"]);

            ResetSubAccPwd_Template tpl = new ResetSubAccPwd_Template();

            writer.Write(tpl);
        }
    }

}