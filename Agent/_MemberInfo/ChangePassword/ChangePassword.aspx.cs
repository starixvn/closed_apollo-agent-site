// --------------------------------------------------------------------------
// <copyright file="ChangePassword.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry" date="2010.02.26">Render change password page</change>
//   <change who="Kaden.Trinh" date="2012.05.30">Refactor code and check StyleCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.ChangePassword
{
    using System.Web.UI;

    public partial class ChangePassword_Page : Fanex.Age.UI.TplPage
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            this.RegisterJavascriptObject("alertInputOldPwd", Resources.Lang.alertInputOldPwd);
            this.RegisterJavascriptObject("alertPasswordEmpty", Resources.Lang.alertPasswordEmpty);
            this.RegisterJavascriptObject("alertErrorConfirm", Resources.Lang.alertErrorConfirm);
            this.RegisterJavascriptObject("alertPasswordRequire", Resources.Lang.lblPasswordRequire);
            this.RegisterJavascriptObject("alertPasswordSucc", Resources.Lang.lblPasswordChanged1);

            if (!CurrentUserSession.IsLogged)
            {
                this.RegisterJavascriptObject("firstLogged", true);
                this.RegisterJavascriptObject("forcechangepwd", Resources.Lang.forcechangepwd);
            }

            ChangePassword_Template tpl = new ChangePassword_Template();
            writer.Write(tpl);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }
    }
}