using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Age.Integration;
using Apollo.Age.Services.Impl;
using Apollo.Age.Web;
using System.Web.Security;

public partial class sso : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // get authorization code returned by authorization server 
        string authCode = Request.QueryString["code"];

        if (!string.IsNullOrWhiteSpace(authCode))
        {
            Autheticate(authCode);
        }
    }

    private void Autheticate(string code)
    {
        AuthenticationService authService = new AuthenticationService();
        // get authenticated info from authorization server
        AuthModel authModel = authService.GetAuthInfo(code);

        if (authModel != null)
        {
            UserService userService = new UserService();
            Apollo.Age.Entities.User user = userService.GetUserById(authModel.UserId);


            if (user != null)
            {
                ICurrentSession currentSession = new CurrentSession(user);
              //  Session["Apollo.Age.Web.CurrentSession"] = currentSession;              
                FormsAuthentication.RedirectFromLoginPage(user.Username, false);
            }
            else
            { 
                // what should we do
            }
        }
    }

    private string Language
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Request.QueryString["lang"]))
            { 
                return Request.QueryString["lang"];
            }

            return "en-US";
        }
    }
}