namespace Sunplus.Agent.UI._MemberInfo.TransferCondition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Fanex.PTEngine.Age.Templates;
    using Fanex.PTEngine.BusinessObjects;
    using Sunplus.Agent.UI._MemberInfo.TransferCondition;

    public partial class EditTransferCondition_Page : Fanex.Age.UI.TplPage
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            var template = new EditTransferCondition_Template();
            var tplTransferCondition = new TransferCondition_Template();

            template.transferCondition = tplTransferCondition;
            template.actionName = Resources.Lang.update;
            template.TargetCustName = Request.QueryString["username"];
            template.targetCustId = Request.QueryString["custid"];
            template.dataSource = "Handlers/GetTransferCondition.ashx";

            template.ScriptBlock.Append(Fanex.PTEngine.HttpContextExtension.GenerateScriptLink("../Handlers/Language.ashx"));
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