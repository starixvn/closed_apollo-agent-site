using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Sunplus.Agent.UI._AccountInfo.AccountInfoBase;
using Sunplus.Agent.Model.BL.AccountInfo;
using Sunplus.Agent.Model.DL.AccountInfo;
using Sunplus.Agent.UI._AccountInfo;

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