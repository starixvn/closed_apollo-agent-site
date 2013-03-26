// --------------------------------------------------------------------------
// <copyright file="DownLineTreeView.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

namespace Sunplus.Agent.UI._AccountInfo
{
    using System;
    using Apollo.Age.DTO.Accounts;
    using Fanex.Agent.UI._AccountInfo;
    using Sunplus.Agent.UI._AccountInfo.AccountInfoBase;

    public partial class _AccountInfo_DownLineTreeView : AccountInfoBase_Page
    {
        const string CLASS_TREEVIEW_TAB = "tree-tab";
        const string CLASS_TREEVIEW_SUB = "tree-sub";

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            DownLineTreeView_Template tpl = new DownLineTreeView_Template();
            if (TreeViewNodeDTO.Type == 1)
            {
                tpl.TreeClass = CLASS_TREEVIEW_SUB;
            }
            else
            {
                tpl.TreeClass = CLASS_TREEVIEW_TAB;
            }

            writer.Write(tpl);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}