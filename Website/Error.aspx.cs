using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Error : System.Web.UI.Page
{
    protected string ErrorMessage = null;
    protected string urlBefore = "";//Sunplus.Agent.Model.BL.Definitions.AppSettings.BeforeLoginUrl;

    public string GetBaseUrl()
    {
        string hostName = Request.Headers["Host"];
        string appPath = Request.ApplicationPath;

        string url = /*Protocol + hostName +*/ appPath;

        if (appPath.Length > 1)
        {
            url += '/';
        }

        return url;
    }

    protected override void OnLoad(EventArgs e)
    {
        string errCode = Request["err"];

        switch (errCode)
        {
            case "403":
                ErrorMessage = Apollo.Age.Resources.Lang.HttpError_403;
                break;
            case "404":
                ErrorMessage = Apollo.Age.Resources.Lang.HttpError_404;
                break;
            default:
                ErrorMessage = Apollo.Age.Resources.Lang.HttpError_Default;
                break;
        }

        base.OnLoad(e);
    }
}