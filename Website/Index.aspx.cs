using System;
using System.Web;
using System.Web.UI;
using Sunplus.Agent.Utilities;

public partial class Index : Page
{
    private const string DEFAULT_INDEX = "Message/Portal";
    protected string mainPage = null;
    //protected string baseURL = string.Empty;

    protected override void OnLoad(EventArgs e)
    {
        //if (!UserSession.Current.IsLogged)
        //{
        //    mainPage = "_MemberInfo/ChangePassword/ChangePassword.aspx";
        //}

        mainPage = DEFAULT_INDEX;
        string url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
            HttpContext.Current.Request.ApplicationPath;
        //baseURL = url.LastIndexOf("/", url.Length - 1, 1) != -1 ? url : url + "/";

        // Hot fix for Opera browser (IIS does sniffing user agent)
        Response.ContentType = "text/html";
    }

    public string CookieCurrentURLKey
    {
        get
        {
            string name = string.Join("_", new string[] {
               //UserSession.Current.CustId.ToString(),
              // UserSession.Current.SubAccId.ToString(),
               "URL"
            });

            return Encryption.PearsonEncode(name);
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/sso.aspx");
        }
    }
}