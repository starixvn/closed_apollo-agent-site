using System;
using System.Web.UI;

public partial class _Default : Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/sso.aspx");
        }
    }
}