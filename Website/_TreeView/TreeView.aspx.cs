using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Agent.UI._TreeView;

public partial class _TreeView_TreeView : Fanex.Age.UI.TplPage
{
    public override void ProcessRequest(ref System.Collections.Hashtable p)
    {
    }

    public override object ValidateRequest(System.Collections.Hashtable p)
    {
        //pageAccess = CurrentUserSession.SubPageAccess;

        return null;
    }

    public override object InvokeData(System.Collections.Hashtable p)
    {
        return null;
    }

    public override void RenderContent(HtmlTextWriter writer)
    {
        TreeView_Template tpl = new TreeView_Template();
        string htmlFrame = "<iframe id='frmDownlineList' name='frmDownlineList' src='../_AccountInfo/DownLineTreeView.aspx?root={0}' style='width:98%;height:100%;'></iframe>";

        Fanex.Age.DTO.Reports.TreeView.TreeViewNode.SearchingID = CurrentUserSession.CustId;
        Fanex.Age.DTO.Reports.TreeView.TreeViewNode.Type = 0; //0: tree in tab treeview

        tpl.TreeViewDownLineFrame = string.Format(htmlFrame, CurrentUserSession.CustId);

        writer.Write(tpl);
    }
}