using System.Web;
using System.Web.UI;
using Sunplus.Agent.Model.BL.Definitions;
using Sunplus.Agent.Model.Entities;
using Fanex.Age.UI;


/*
 * Created 20100610@Lee - Render left menu page
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._HowToUse
{
    public partial class Top_Page : Fanex.Age.UI.TplPage
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

        public override void RenderContent(HtmlTextWriter writer)
        {
            Top_Template tpl = new Top_Template();
            if (CurrentUserSession.Language == "zh-CN" || CurrentUserSession.Language == "zh-TW")
                tpl.idLogo = "logo_" + Sunplus.Agent.Model.Entities.UserSession.Current.Language;
            else
                tpl.idLogo = "logo_en-US";
            writer.Write(tpl);
        }
    }
}