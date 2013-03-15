using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Agent.UI._MemberInfo.CustomerSetting.Pages;

public partial class _MemberInfo_CustomerSetting_Pages_EditMemberPositionTaking : Fanex.Age.UI.TplPage
{
    public override void RenderContent(HtmlTextWriter writer)
    {
        var template = new EditMemberPositionTaking_Template();
        template.actionName = Resources.Lang.update;
        template.title = string.Format("{0} - {1}", Resources.Lang.lblEditMember, this.Request.QueryString["username"]);
        if (this.Request.QueryString["showheader"] == "1")
        {
            template.headerText = string.Format("<div class='margin-top-12' id='header_main'>{0} {1}</div>", Resources.Lang.lblEditPT, this.Request.QueryString["username"]);
            template.isReloadParent = "0";
        }
        else
        {
            template.headerText = string.Empty;
            template.isReloadParent = "1";
        }
        template.TargetAgentId = Context.Request.QueryString["custid"].ToString();
        template.UGroup = Context.Request.QueryString["ugroup"].ToString();
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