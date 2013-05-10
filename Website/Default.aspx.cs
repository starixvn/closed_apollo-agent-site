using System;
using System.Web.UI;
using Apollo.Age.Web;
using System.Web.Security;

public partial class _Default : Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!User.Identity.IsAuthenticated || CurrentSession == null)
        {
            FormsAuthentication.RedirectToLoginPage();
        }  
    }

    protected ICurrentSession CurrentSession
    {
        get { return SessionManager.CurrentSession; }
    }
}