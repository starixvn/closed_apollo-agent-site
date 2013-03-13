using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sunplus.Agent.Model.DL.AccountInfo;
using System.Data;

public partial class _AccountInfo_TreeList : System.Web.UI.Page
{
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

        AccountInfoDAO dao = new AccountInfoDAO();
        //AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);

        string scontent = string.Empty;

        if (root == "source") //for the first time the JQuery tree will send the root param value as "source"
        {
            var ds = dao.Age_Customer_GetDownLine(0);
            GenerateTree(ds.Tables[0], 0);
        }
        else if (int.TryParse(root, out parentID)) //subsequent requests will send the ID
        {
            if (parentID > 0)
            {
                var ds = dao.Age_Customer_GetDownLine(parentID);
                GenerateTree(ds.Tables[0], parentID);
            }
        }

        //return scontent;
    }

    private void GenerateTree(DataTable dtParent, int parentID)
    {
        System.Text.StringBuilder tree = new System.Text.StringBuilder();

        DataView dv = new DataView(dtParent);

        dv.RowFilter = string.Format("ParentId = {0}", parentID);

        tree.Append("[");
        //loop through each leaf
        for (int i = 0; i < dv.Count; i++)
        {
            tree.Append("{");
            tree.Append("\"text\": \"").Append(dv[i]["Username"]).Append("\",");
            tree.Append("\"id\": \"").Append(dv[i]["AgentId"]).Append("\",");

            //check whether this node has child nodes
            DataView dvChild = new DataView(dtParent);
            dvChild.RowFilter = string.Format("ParentId = {0}", dv[i]["ParentId"]);
            if (dvChild.Count > 0) //if so mark it in the return node array
                tree.Append("\"hasChildren\": true");
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
        //return tree.ToString();
    }
}