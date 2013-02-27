using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL.SubAcc;
using Sunplus.Agent.Model.BL.Definitions;

using Fanex.Age.UI;

using Fanex.Age.UI.Components;
/*
 * Created 20091224@Lee - Render sub-account adding page
 * Revision:
 * 20111227@Andy - Re-design for permissions which inherited from SubAccPermissionPage
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._SubAcc
{
    public partial class AddSubAcc_Page : SubAccPermissionPage
    {
        public override void ProcessRequest(ref Hashtable p)
        {
            base.ProcessRequest(ref p);
        }

        public override object ValidateRequest(Hashtable p)
        {
            return base.ValidateRequest(p);
        }

        public override object InvokeData(Hashtable p)
        {            
            return base.InvokeData(p);
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            base.RenderContent(writer);

            var tpl = new AddSubAcc_Template();
            tpl.username_value = CurrentUserSession.UserName + Sunplus.Agent.Model.BL.Definitions.AppSettings.SUB_ACC_SUFFIX;

            RegisterJavascriptObject("FullControlPM", Resources.Lang.FullControl);
            RegisterJavascriptObject("UpdateViewPM", Resources.Lang.update);
            RegisterJavascriptObject("ViewPM", Resources.Lang.view);
            RegisterJavascriptObject("NonePM", Resources.Lang.none);

            writer.Write(tpl);
        }
    }
}