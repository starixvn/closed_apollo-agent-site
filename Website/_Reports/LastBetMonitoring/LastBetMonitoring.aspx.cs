/*
 * Created 20101110@Simon - Render Monitor page
 * Revision:
 * 20110628@Qui - Turnning peformance by seperate bet type, sport type
 *
 */

namespace Fanex.Age.UI.Reports.LastBetMonitoring
{
    using System.Web;
    using System.Web.UI;

    public partial class LastBetMonitoring_Page : Reports_Page
    {
        public string TransIdCookieKey
        {
            get
            {
                string name = string.Join(null, new string[] { CurrentUserSession.CustId.ToString(), "lastTransId", CurrentUserSession.UserId.ToString() });
                return Sunplus.Agent.Utilities.Encryption.CfsEnCode(name, 6);
            }
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            LastBetMonitoring_Template tpl = new LastBetMonitoring_Template();

            this.RegisterJavascriptObject("sportTypeGroup", 0);
            this.RegisterJavascriptObject("betTypeGroup", 0);
            this.RegisterJavascriptObject("limitRowCount", 10);

            this.RegisterJavascriptObject("TransIdCookieKey", this.TransIdCookieKey);

            this.RegisterJavascriptObject("BetTypes_All", (int)Sunplus.Agent.Model.BL.Definitions.BetType.None);
            this.RegisterJavascriptObject("BetTypes_HDP", (int)Sunplus.Agent.Model.BL.Definitions.BetType.Handicap);
            this.RegisterJavascriptObject("BetTypes_OU", (int)Sunplus.Agent.Model.BL.Definitions.BetType.Over_Under);
            this.RegisterJavascriptObject("BetTypes_Others", (int)Sunplus.Agent.Model.BL.Definitions.BetType.Others);

            writer.Write(tpl);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            HttpCookie cookie = new HttpCookie(this.TransIdCookieKey, "0");
            Response.Cookies.Set(cookie);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }
    }
}