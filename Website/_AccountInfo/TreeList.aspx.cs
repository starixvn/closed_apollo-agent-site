using System;
using System.Data;
using System.Web.UI;
using Sunplus.Agent.Model.BL.AccountInfo;

public partial class _AccountInfo_TreeList : Fanex.Age.UI.TplPage
{
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

    public override void RenderContent(HtmlTextWriter writer)
    {
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //the tree view will Post a parameter root - so if the param 'root' is present generate the Tree view 
            if (Request.Params["root"] != null)
            {
                AsyncTree();
            }
        }
    }

    public void AsyncTree()
    {
        string root = Request.Params["root"];
        int parentID = 0;

        AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
        string scontent = string.Empty;

        if (root == "source") //for the first time the JQuery tree will send the root param value as "source"
        {
            parentID = Fanex.Age.DTO.Reports.TreeView.TreeViewNode.SearchingID;
            var ds = bo.Age_Customer_GetAgentDownLine(parentID);
            GenerateTree(ds.Tables[0], parentID);
        }
        else if (int.TryParse(root, out parentID)) //subsequent requests will send the ID
        {
            if (parentID > 0)
            {
                var ds = bo.Age_Customer_GetAgentDownLine(parentID);
                GenerateTree(ds.Tables[0], parentID);
            }
        }
    }

    private void GenerateTree(DataTable dtParent, int parentID)
    {
        System.Text.StringBuilder tree = new System.Text.StringBuilder();

        DataView dv = new DataView(dtParent);

        string url = string.Empty;
        string target = string.Empty;

        tree.Append("[");
        //loop through each leaf
        for (int i = 0; i < dv.Count; i++)
        {
            if (Fanex.Age.DTO.Reports.TreeView.TreeViewNode.Type == 1)
            {
                url = "../_AccountInfo/AccountInfo.aspx?IsMember=0&custid=" + dv[i]["AgentId"] + "&txtUsername=" + dv[i]["Username"];
                target = "target='menu'";
            }
            else
            {
                url = "../_MemberInfo/CustomerList/AgentList.aspx?custid=" + dv[i]["AgentId"] + "&us=" + dv[i]["Username"];
                target = "target='main'";
            }

            tree.Append("{");
            tree.Append("\"Text\": \"").Append("<a href='" + url + "' " + target  + ">" + dv[i]["Username"] + "</a>").Append("\",");
            tree.Append("\"Id\": \"").Append(dv[i]["AgentId"]).Append("\",");
            tree.Append("\"HasChildren\": " + dv[i]["hasChildren"]);
            tree.Append("},");
        }

        //remove the ending comma from the result
        if (tree.Length > 1)
        {
            tree = tree.Remove(tree.Length - 1, 1);
        }
        tree.Append("]");

        //put the result in Response and end the Response
        Response.Write(tree.ToString());
        Response.End();
    }
}