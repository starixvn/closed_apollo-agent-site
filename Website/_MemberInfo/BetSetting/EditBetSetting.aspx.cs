namespace Sunplus.Agent.UI._MemberInfo.BetSetting
{
    using System.Web.UI;
    using Fanex.Agent.UI._MemberInfo.BetSetting;

    public partial class EditBetSetting_Page : Fanex.Age.UI.TplPage
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            var template = new EditBetSetting_Template();

            template.actionName = Resources.Lang.update;
            template.targetCustId = this.Request.QueryString["custid"];
            template.lbleditbetsetting = Resources.Lang.lbleditbetsetting + " " + this.Request.QueryString["username"];
            template.Title = Resources.Lang.edit + " " + this.Request.QueryString["username"];
            template.actionForm = "Handlers/UpdateBetSetting.ashx";
            template.dataSource = "Handlers/GetCustomerBetSetting.ashx";

            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../../../_GlobalResources/Js/Core.js?" + TplCore.TplSettings.UrlVersion));
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../Handlers/Language.ashx"));
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../Resources/MemberInfo.js?" + TplCore.TplSettings.UrlVersion));
            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("Resources/BetSetting.js?" + TplCore.TplSettings.UrlVersion));
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
    }
}