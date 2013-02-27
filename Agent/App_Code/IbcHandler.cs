using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

[Obsolete]
public static class IbcHandler
{
    public static HttpContext InitializeCulture(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);        
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Sunplus.Agent.Model.Entities.UserSession.Current.Language);
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(Sunplus.Agent.Model.Entities.UserSession.Current.Language);
        return context;
    }
}
