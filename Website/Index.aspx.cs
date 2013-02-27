using System;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Utilities;

public partial class Index : System.Web.UI.Page
{
    private const string DEFAULT_INDEX = "_Reports/PortalPage/PortalPage.aspx";
    protected string mainPage = null;

    protected override void  OnLoad(EventArgs e)
    {
        if (!UserSession.Current.IsLogged)
        {
            mainPage = "_MemberInfo/ChangePassword/ChangePassword.aspx";
        }
        else
        {
            var currentURLCookie = Request.Cookies[CookieCurrentURLKey];
            string url = DEFAULT_INDEX;

            if (currentURLCookie != null && !string.IsNullOrEmpty(currentURLCookie.Value) && !UserSession.Current.IsInternal)
            {
                if (!currentURLCookie.Value.Contains("Error.aspx") && !currentURLCookie.Value.Contains("SignIn.aspx"))
                {
                    url = currentURLCookie.Value;
                }
            }

            mainPage = url;
        }

        // Hot fix for Opera browser (IIS does sniffing user agent)
        Response.ContentType = "text/html";
    }

    public string CookieCurrentURLKey
    {
        get
        {
            string name = string.Join("_", new string[] { 
               UserSession.Current.CustId.ToString(), 
               UserSession.Current.SubAccId.ToString(),
               "URL"
            });

            return Encryption.PearsonEncode(name);
        }
    }
}

