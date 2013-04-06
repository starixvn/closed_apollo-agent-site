using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DNAide.Web;
using Fanex.Data.DNAide;
using DNAide.Core;
public partial class Popup_ResetPass : TplPage
{
    public bool IsNoBackground
    {
        get { return String.IsNullOrEmpty(Request.QueryString["nobackground"]); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "popup";

        string username = (Request["usn"] == null) ? "" : Request["usn"];
        string custid = (Request["custid"] == null) ? "" : Request["custid"];
        string _main = TemplateHandler.LoadTemplate("Popup/ResetPass.html");
        string errorTemplate = TemplateHandler.LoadTemplate("ErrorMsg.html");

        string tblAlign = "left";
        bool isNoBgr = String.IsNullOrEmpty(Request.QueryString["nobackground"]);
        if (isNoBgr)
        {
            tblAlign = "center";
            TemplateHandler.RemoveBlock(ref _main, "nobackground"); //remove override background!            
        }

        TemplateHandler.Assign(ref _main, "error-template", errorTemplate);
        TemplateHandler.Assign(ref _main, "tblalign", tblAlign);
        TemplateHandler.Assign(ref _main, "username", Resources.Lang.lblChangePassword + " " + username);
        TemplateHandler.Assign(ref _main, "password", Resources.Lang.password);
        TemplateHandler.Assign(ref _main, "confirmpassword", Resources.Lang.lblConfirmPassword);
        TemplateHandler.Assign(ref _main, "submit", Resources.Lang.submit);
        TemplateHandler.Assign(ref _main, "cancel", isNoBgr ? Resources.Lang.cancel : Resources.Lang.reset);
        TemplateHandler.Assign(ref _main, "changepass", Resources.Lang.lblChangePassword);

        TemplateHandler.Assign(ref _main, "errmsgoldpass", Resources.Lang.alertInputOldPwd);
        TemplateHandler.Assign(ref _main, "errmsgpassempty", Resources.Lang.alertPasswordEmpty);
        TemplateHandler.Assign(ref _main, "errmsgconfirmpassempty", Resources.Lang.alertConfirmPasswordEmpty);

        TemplateHandler.Assign(ref _main, "ermsgconfirm", Resources.Lang.alertErrorConfirm);
        TemplateHandler.Assign(ref _main, "errmsgpassvalidate", Resources.Lang.lblPasswordRequire);
        TemplateHandler.Assign(ref _main, "succmsg", Resources.Lang.lblPasswordChanged1);
        TemplateHandler.Assign(ref _main, "custid", custid);

        this.TplSetContent(_main);

        AddJsVariable("IsNoBackground", IsNoBackground.ToString());
        AddJsVariable("lblconfirmclosesubacc", Resources.Lang.lblconfirmclosesubacc);
        AddJsVariable("lblConfirmClosed", Resources.Lang.lblConfirmClosed);
    }
}
