using System;
using Fanex.Agent.UI._AccountInfo;
using Sunplus.Agent.UI._AccountInfo.AccountInfoBase;


public partial class _AccountInfo_DownLineTreeView : AccountInfoBase_Page
{
    public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
    {
        DownLineTreeView_Template tpl = new DownLineTreeView_Template();
        writer.Write(tpl);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }
}