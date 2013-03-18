/*
 * Created 20100605@Simon - Render Index page
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._HowToUse
{
    using Fanex.Agent.UI._HowToUse;

    public partial class Index_Page : Fanex.Age.UI.TplPage
    {
        string MainSource = string.Empty;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            string url = Request["src"];
            if (!string.IsNullOrEmpty(url))
            {
                string parameter = url + string.Empty;
                parameter = parameter.Replace(' ', '+');
                parameter = Sunplus.Agent.Utilities.Encryption.CfsDeCrypt(parameter);
                MainSource = "Content.aspx?" + parameter;
            }
            else if (Request.QueryString["lang"] != null)
            {
                MainSource = "Resources/HTML/WinLimit/WinLimit_" + CurrentUserSession.Language + ".html";
            }
            else
            {
                MainSource = "Intro/Intro_" + Sunplus.Agent.Model.Entities.UserSession.Current.Language + ".html";
            }
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
            var site = CurrentUserSession.Site.ToLower();
            if (site != "789y")
            {
                Index_Template tpl = new Index_Template();
                tpl.mainFrameSrc = MainSource;
                writer.Write(tpl);
            }
            else
            {
                writer.Write("<h1>This function is under construction</h1>");
            }
        }
    }
}