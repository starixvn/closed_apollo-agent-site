/* ----------------------------------------------------------------------------------
 * File: ChangePass.aspx.cs
 * Desc: Change Password
 * Writer: hiepvt
 * Template: App_Templates\AccountInfo\ChangePassword.tpl
 * ---------------------------------------------------------------------------------- */
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
using DNAide.Utils;
using DNAide.Core;
public partial class AccountInfo_ChangePass : TplPage
{
    string strChangePassTemplate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main"; //nqminh - 15Oct08

        strChangePassTemplate = TemplateHandler.LoadTemplate("AccountInfo/ChangePassword.html");

        //Common.SetGUIByLang(ref strChangePassTemplate);

        TemplateHandler.Assign(ref strChangePassTemplate, "lblChangePassword", Resources.Lang.lblChangePassword);

        if (Request.QueryString["Message"] == null)
        {
            if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsLogged)
            {
                TemplateHandler.Assign(ref strChangePassTemplate, "{TrMsgDisplay}", "block");
                TemplateHandler.Assign(ref strChangePassTemplate, "{MsgDisplay}", "none");
                TemplateHandler.Assign(ref strChangePassTemplate, "{Message-Error}", Resources.Lang.forcechangepwd);
                TemplateHandler.Assign(ref strChangePassTemplate, "{ErrDisplay}", "block");
            }
            else
            {
                TemplateHandler.Assign(ref strChangePassTemplate, "{TrMsgDisplay}", "none");
                TemplateHandler.Assign(ref strChangePassTemplate, "{MsgDisplay}", "none");                  
            }
            //this.HtmlContent = BuildHtml(strChangePassTemplate);
            this.TplSetContent(BuildHtml(strChangePassTemplate));
        }
        else
        {
            TemplateHandler.Assign(ref strChangePassTemplate, "{TrMsgDisplay}", "block");
            if (Request.QueryString["Message"].ToString().Trim() == "PasswordChanged")
            {
                TemplateHandler.Assign(ref strChangePassTemplate, "{Message}", Resources.Lang.lblPasswordChanged1);
                TemplateHandler.Assign(ref strChangePassTemplate, "{MsgDisplay}", "block");
                TemplateHandler.Assign(ref strChangePassTemplate, "{ErrDisplay}", "none");
            }
            else
            {
                TemplateHandler.Assign(ref strChangePassTemplate, "{Message-Error}", Request.QueryString["Message"].ToString());
                TemplateHandler.Assign(ref strChangePassTemplate, "{MsgDisplay}", "none");
                TemplateHandler.Assign(ref strChangePassTemplate, "{ErrDisplay}", "block");
            }
            //this.HtmlContent = BuildHtml(strChangePassTemplate);
            this.TplSetContent(BuildHtml(strChangePassTemplate));
        }       
         
    }
    private string BuildHtml(string strTemplate)
    {
        TemplateHandler.Assign(ref strTemplate, "{Header}", Resources.Lang.lblChangePassword);
        TemplateHandler.Assign(ref strTemplate, "{OldPassword}", Resources.Lang.lblOldPassword);
        TemplateHandler.Assign(ref strTemplate, "{NewPassword}", Resources.Lang.password);
        TemplateHandler.Assign(ref strTemplate, "{ConfirmPassword}", Resources.Lang.lblConfirmPassword);
        TemplateHandler.Assign(ref strTemplate, "{btnSubmit}", Resources.Lang.submit);
        TemplateHandler.Assign(ref strTemplate, "{btnReset}", Resources.Lang.reset);
        TemplateHandler.Assign(ref strTemplate, "{alertInputOldPwd}", Resources.Lang.alertInputOldPwd);
        TemplateHandler.Assign(ref strTemplate, "{alertPasswordEmpty}", Resources.Lang.alertPasswordEmpty);
        TemplateHandler.Assign(ref strTemplate, "{alertErrorConfirm}", Resources.Lang.alertErrorConfirm);
        TemplateHandler.Assign(ref strTemplate, "{alertPasswordRequire}", Resources.Lang.lblPasswordRequire);
        TemplateHandler.Assign(ref strTemplate, "{alertPasswordSucc}", Resources.Lang.lblPasswordChanged1);
        
        return strTemplate;
    }
}
