namespace Sunplus.Agent.UI._MemberInfo.PositionTaking.Agent
{
    using System;
    using System.Web.UI;
    using Fanex.PTEngine.Age.Templates;
    using Fanex.PTEngine.BusinessObjects;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Security;

    public partial class EditMember_Page : Fanex.Age.UI.TplPage
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.Request["CallBack"]))
            {
                RegisterJavascriptObject("CallBack", this.Request["CallBack"]);
            }
            var template = new AddNewMember_Template();
            // Set Captcha
            HttpFrequenceRule rule = new HttpFrequenceRule("SaveNewMember.ashx", 20, TimeSpan.FromSeconds(600));
            template.Captcha.Hide();
            template.SelectGroup.Hide();
            template.actionName = Resources.Lang.update;
            template.Title = string.Format("{0} - {1}", Resources.Lang.lblEditMember, this.Request.QueryString["USER"]);
            template.UserNameAndPasswordBlock.Hide();
            template.targetCustId = this.Request.QueryString["custid"];
            template.targetRoleId = Convert.ToString((int)UserSessionBase.UserTypes.Member);
            template.formType = FormType.EditPT.ToString();

            template.actionForm = "../Handlers/SaveNewMember.ashx";
            template.dataSource = "../Handlers/GetCustomerSetting.ashx";
            template.CssBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateStyleSheetLink("../../../_GlobalResources/Css/Agent.css?" + TplCore.TplSettings.UrlVersion));
            template.CssBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateStyleSheetLink("../../Resources/MemberInfo.css?" + TplCore.TplSettings.UrlVersion));
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../../../_GlobalResources/Js/Core.js?" + TplCore.TplSettings.UrlVersion));
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../../Handlers/Language.ashx"));
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../../Resources/MemberInfo.js?" + TplCore.TplSettings.UrlVersion));
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../Resources/AddNewMember.js?" + TplCore.TplSettings.UrlVersion));
            writer.Write(template);
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

        private object RenderNoInfo()
        {
            return new Fanex.Age.UI.Components.ErrorPage_Page(Resources.Lang.noInfo);
        }
    }
}