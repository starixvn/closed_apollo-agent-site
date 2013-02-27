/*
 * Created 20100605@Simon - Render Index page
 * Revision:
 * ?@? - ... 
 * 
 */

using Sunplus.Agent.Model.Entities;
using System;
namespace Sunplus.Agent.UI._UserGuide
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
            //Response.Cache.SetMaxAge(TimeSpan.FromDays(1));

            Index_Template tpl = new Index_Template();
            tpl.UserGuide = Resources.HowToUse.UserGuide;

            if (CurrentUserSession.IsAgent && !Sunplus.Agent.Model.Entities.Permission.UserPermission.IsNeutral)
            {
                if (UserSession.Current.LangKey == "CS" || UserSession.Current.LangKey == "CT")
                    tpl.urlLink = "Agent/UserGuide_" + UserSession.Current.LangKey + "/" + "index.aspx";
                else
                    tpl.urlLink = "Agent/UserGuide_E/index.aspx";
            }
            else if (CurrentUserSession.IsMaster && !Sunplus.Agent.Model.Entities.Permission.UserPermission.IsNeutral)
            {
                if (UserSession.Current.LangKey == "CS" || UserSession.Current.LangKey == "CT")
                    tpl.urlLink = "Master/UserGuide_" + UserSession.Current.LangKey + "/" + "index.aspx";
                else
                    tpl.urlLink = "Master/UserGuide_E/index.aspx";
            }

            writer.Write(tpl);
        }

    }
}
