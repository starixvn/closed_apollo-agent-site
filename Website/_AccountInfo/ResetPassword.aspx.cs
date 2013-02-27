/*
 * Created 20120727@Jeff - Render Reset password page
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._AccountInfo
{
    using System;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Sunplus.Agent.Utilities;
    using System.Collections.Generic;

    public partial class ResetPassword_Page : Fanex.Age.UI.TplPage
    {
        private int custId = 0;
        private string userName = string.Empty;
        private string isSyncCasino = string.Empty;
        private bool isPopup = false;
        private ResetPassword_Template tpl = new ResetPassword_Template();

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.custId = Convert.ToInt32(Request["custId"]);
            this.userName = Request["usn"];
            this.isPopup = String.IsNullOrEmpty(Request.QueryString["nobackground"]);
            this.isSyncCasino = Request.QueryString["isSyncCasino"];
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
            RegisterJavascriptObject("errOldPassword", Resources.Lang.alertInputOldPwd);
            RegisterJavascriptObject("errPasswordEmpty", Resources.Lang.alertPasswordEmpty);
            RegisterJavascriptObject("errConfirmPasswordEmpty", Resources.Lang.alertConfirmPasswordEmpty);
            RegisterJavascriptObject("errConfirm", Resources.Lang.alertErrorConfirm);
            RegisterJavascriptObject("infoSuccess", Resources.Lang.lblPasswordChanged1);
            RegisterJavascriptObject("errPasswordRequire", Resources.Lang.lblPasswordRequire);
            RegisterJavascriptObject("custId", this.custId);
            RegisterJavascriptObject("isSyncCasino", this.isSyncCasino);
            RegisterJavascriptObject("isPopup", this.isPopup);

            this.tpl.TargetCustName = this.userName;

            writer.Write(this.tpl);
        }
    }
}