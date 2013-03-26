// --------------------------------------------------------------------------
// <copyright file="DownLineTreeView.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

using System;

using Fanex.Agent.UI._AccountInfo;
using Sunplus.Agent.UI._AccountInfo.AccountInfoBase;

public partial class _AccountInfo_DownLineTreeView : AccountInfoBase_Page
{
    const string CLASS_TREEVIEW_TAB = "tree-tab";
    const string CLASS_TREEVIEW_SUB = "tree-sub";

    public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
    {
        string agentId = Request.QueryString["agentid"];

        DownLineTreeView_Template tpl = new DownLineTreeView_Template();

        tpl.AgentId = agentId.ToString();

        if (agentId == CurrentUserSession.AgentId.ToString())
        {
            tpl.TreeClass = CLASS_TREEVIEW_TAB;
        }
        else
        {
            tpl.TreeClass = CLASS_TREEVIEW_SUB;
        }

        writer.Write(tpl);
    }
}