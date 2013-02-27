/*
 * Created 20100605@Simon - Render Index page
 * Revision:
 * ?@? - ... 
 * 
 */

using System;
namespace Sunplus.Agent.UI._UserGuide.Master.UserGuide_CT
{
    public partial class Index_Page : Fanex.Age.UI.TplPage
    {

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            if (CurrentUserSession.IsMaster)
            {
                index_Template tpl = new index_Template();
                writer.Write(tpl);
            }
            else
                Response.Redirect("~/Error.aspx");
        }

    }
}
