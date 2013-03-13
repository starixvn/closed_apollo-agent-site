using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Common;
using Fanex.Agent.UI._MemberInfo.CustomerSetting.Pages;

public partial class _MemberInfo_CustomerSetting_Pages_EditAgentCommission : Fanex.Age.UI.TplPage
{
    public override void RenderContent(HtmlTextWriter writer)
    {
        var template = new EditAgentCommission_Template();
        template.TargetAgentId = Context.Request.QueryString["custid"].ToString();
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
