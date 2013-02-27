using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Sunplus.Agent.Model.BL;
using Sunplus.Agent.Model.BL.Definitions;

using Sunplus.Agent.Model.Entities;

using Fanex.Age.UI;

using Fanex.Age.UI.Components;


/*
 * Created 20100126@Lee - Sitemap view
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Sunplus.Agent.UI.Navigation
{
    public partial class Sitemap_Page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IUserSession session = Sunplus.Agent.Model.Entities.UserSession.Current;
            SiteMapNodeCollection data = null;

            if (session.RoleId == (int)UserTypes.Admin)
            {
                data = SiteMap.RootNode.ChildNodes;
                TreeView1.DataSource = data;
                TreeView1.DataBind();
                TreeView1.CollapseAll();
            }
        }

        protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            TreeNode node = e.Node;
            SiteMapNode dataItem = e.Node.DataItem as SiteMapNode;

            node.NavigateUrl = dataItem.GetAttribute("href");

            if (!string.IsNullOrEmpty(node.NavigateUrl))
            {
                node.SelectAction = TreeNodeSelectAction.Select;
            }
            else
            {
                node.SelectAction = TreeNodeSelectAction.Expand;
            }

            node.Text = GetGlobalResourceObject("Lang", dataItem.Title) as string;
        }

    }
}