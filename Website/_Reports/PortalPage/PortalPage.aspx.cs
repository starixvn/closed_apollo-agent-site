using System.Web.UI;
using Fanex.Age.UI.Reports.PortalPage;
using Fanex.Age.UI;
using System.Collections;
using Fanex.Age.UI.Components;
using Sunplus.Agent.Model.BL.UserMessage;

using Sunplus.Agent.Model.BL.MemberInfo.CreditBalance;
using System.Data;
using System.Threading;
using System.Globalization;
using Sunplus.Agent.Model.Entities.Permission;

namespace Fanex.Age.UI.Reports
{
    public partial class PortalPage_Page : TplPage
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            var cacheKey = Sunplus.Agent.Utilities.Encryption.CfsEnCrypt(CurrentUserSession.Language + CurrentUserSession.CustId.ToString());
            RegisterJavascriptObject("key", cacheKey);
            RegisterJavascriptObject("RemoveOnlineList", Sunplus.Agent.Model.BL.Definitions.AppSettings.IsHideOnlineList);
            RegisterJavascriptObject("more", Resources.Lang.more + "...");

            var tpl = new PortalPage_Template();
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsHideOnlineList)
            {
                tpl.OnlineList.Clear();
            }
            writer.Write(tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {            
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

    }
}
