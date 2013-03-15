/*
 * Created 20111227@Andy - Render sub-account permissions page
 * Revision:
 * ?@? - ...
 *
 */

using System.Collections.Generic;
using System.Text;
using Fanex.Age.UI;
using Fanex.Age.UI.Components;
using Resources;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.UI._SubAcc;

namespace Sunplus.Agent.UI._SubAcc
{
    /// <summary>
    /// Defines permissions for sub-account
    /// </summary>
    public abstract class SubAccPermissionPage : TplPage
    {
        private IList<IExtObject<string, string>> FunctionNames = new List<IExtObject<string, string>>();

        private StringBuilder _permissions = new StringBuilder();

        protected StringBuilder Permissions
        {
            get
            {
                return _permissions;
            }
        }

        public SubAccPermissionPage()
        {
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            // Add data to _page variables, access it as JSON format
            RegisterJavascriptObject("errMsgEmpty", Resources.Lang.alertPasswordEmpty);
            RegisterJavascriptObject("errMsgRequire", Resources.Lang.lblPasswordRequire);
            RegisterJavascriptObject("CallBack", Parameters["CallBack"]);

            var checkBoxes = new List<string>();

            var numBreak = 0;

            //foreach (IExtObject<string, string> data in FunctionNames)
            //{
            //    perm = new SubAccPermission_Template();

            //    perm.chkID = data.Key;
            //    perm.FunctionName = data.Value;

            //    _permissions.Append(perm);

            //    checkBoxes.Add(data.Key);

            //    if (numBreak == 3)
            //    {
            //        _permissions.Append("<br />");

            //        numBreak = 0;
            //        continue;
            //    }

            //    numBreak++;
            //}

            RegisterJavascriptObject("checkBoxes", checkBoxes.ToArray());
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            FunctionNames = Sunplus.Agent.UI._SubAcc.Permissions.GetPermissions();

            p.AddHttpParameters(
                    "CallBack",
                    Request);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            // Sub-account does not have permission to access SubAcc function
            if (CurrentUserSession.SubAccId > 0)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }
    }
}