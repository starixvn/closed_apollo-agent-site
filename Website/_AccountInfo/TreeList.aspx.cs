using System;
using System.Collections.Generic;
using System.Web.UI;
using Apollo.Age.Criterias.Accounts;
using Apollo.Age.DTO.Accounts;
using Apollo.Age.Services.Impl;
using Apollo.Age.Services.Interface;
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

        IAgentService service = new AgentService();
        AgentDownlineCriteria criteria = null;


        AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
        string scontent = string.Empty;

        if (root == "source") //for the first time the JQuery tree will send the root param value as "source"
        {
            criteria = new AgentDownlineCriteria()
            {
                AgentId = TreeViewNodeDTO.Root
            };

            var lstNodes = service.GetTreeViewNode(criteria);

            GenerateTree(lstNodes);
        }
        else if (int.TryParse(root, out parentID)) //subsequent requests will send the ID
        {
            if (parentID > 0)
            {
                criteria = new AgentDownlineCriteria()
                {
                    AgentId = parentID
                };

                var lstNodes = service.GetTreeViewNode(criteria);

                GenerateTree(lstNodes);
            }
        }
    }

    private void GenerateTree(List<TreeViewNodeDTO> lstNodes)
    {
        System.Text.StringBuilder tree = new System.Text.StringBuilder();
        tree.Append("[");

        foreach (var node in lstNodes)
        {
            tree.Append("{");
            tree.Append("\"Text\": \"").Append(node.Text).Append("\",");
            tree.Append("\"Id\": \"").Append(node.Id).Append("\",");
            tree.Append("\"HasChildren\": " + (node.HasChildren ? "true" : "false"));
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