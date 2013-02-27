namespace Sunplus.Agent.UI._MemberInfo.PositionTaking.Agent
{
    using System;
    using System.Web.UI;
    using Fanex.PTEngine.Age.Templates;
    using Fanex.PTEngine.BusinessObjects;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Security;

    public partial class AddNewMember_Page : Fanex.Age.UI.TplPage
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            var template = new AddNewMember_Template();
            // Set Captcha
            HttpFrequenceRule rule = new HttpFrequenceRule("SaveNewMember.ashx", 19, TimeSpan.FromDays(2));
            if (rule.IsViolated)
            {
                template.NoCaptcha.SetValue(string.Empty);
            }
            else
            {
                template.Captcha.SetValue(string.Empty);
            }
            // End Captcha
            template.LabelGroup.Hide();
            template.actionName = Resources.Lang.btnAdd;
            template.Title = Resources.Lang.lblAddNewMember;
            template.targetCustId = "0";
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