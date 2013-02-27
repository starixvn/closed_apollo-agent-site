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

namespace Sunplus.Agent.UI._UserGuide.Agent.UserGuide_E
{
    public partial class Menu_Page : Fanex.Age.UI.TplPage
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
            Menu_Template tpl = new Menu_Template();
            writer.Write(tpl);
        }

    }
}