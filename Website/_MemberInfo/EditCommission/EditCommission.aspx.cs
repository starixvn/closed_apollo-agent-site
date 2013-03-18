namespace Sunplus.Agent.UI._MemberInfo.EditCommission
{
    using System.Web.UI;
    using Fanex.Agent.UI._MemberInfo.EditCommission;

    public partial class EditCommission_Page : Fanex.Age.UI.TplPage
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            var template = new EditCommission_Template();
            template.actionName = Resources.Lang.update;
            template.TargetCustName = Request.QueryString["username"];
            template.dataSource = "Handlers/GetCustomerCommission.ashx";
            template.targetRoleId = Request.QueryString["roleid"];
            template.targetCustId = Request.QueryString["custid"];
            template.updateUrl = "Handlers/UpdateCustomerCommission.ashx";
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../../../_GlobalResources/Js/Core.js?" + TplCore.TplSettings.UrlVersion));
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../Resources/MemberInfo.js?" + TplCore.TplSettings.UrlVersion));
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("Resources/EditCommission.js?" + TplCore.TplSettings.UrlVersion));
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