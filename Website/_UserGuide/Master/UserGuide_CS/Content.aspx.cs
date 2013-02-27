using System.Web.UI;
using System;
using DNAide.Web;

namespace Sunplus.Agent.UI._UserGuide.Master.UserGuide_CS
{
    public partial class Content_Page : Fanex.Age.UI.TplPage
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
            if (CurrentUserSession.IsMaster && !string.IsNullOrEmpty(Request["Url"]))
            {
                string UrlRequest = Request["Url"].Replace(' ', '+');
                UrlRequest = Utilities.Encryption.CfsDeCrypt(UrlRequest);
                string[] arrUrlRequest = UrlRequest.Split('/');

                string html = TemplateHandler.LoadTemplate("../_UserGuide/Master/UserGuide_CS/" + UrlRequest, false);

                html = html.Replace("href=\"../css/", "href=\"css/");
                html = html.Replace("src=\"images/", "src=\"" + arrUrlRequest[0] + "/images/");
                writer.Write(html);
            }
            else
                Response.Redirect("~/Error.aspx");
        }
    }
}
