// --------------------------------------------------------------------------
// <copyright file="DownLineTreeView.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

using Fanex.Agent.UI._AccountInfo;
using Fanex.Common;
using Sunplus.Agent.UI._AccountInfo.AccountInfoBase;

public partial class _AccountInfo_DownLineTreeView : AccountInfoBase_Page
{
    public string AgentId 
    {
        get
        {
            return Request.QueryString["agentid"];
        }
    }

    public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
    {
        DownLineTreeView_Template tpl = new DownLineTreeView_Template();

        tpl.AgentId = AgentId.ToString();

        if (AgentId == CurrentUserSession.AgentId.ToString())
        {
            tpl.ClassTreeViewSub.Hide();
        }
        else
        {
            tpl.ClassTreeViewTab.Hide();
        }

        writer.Write(tpl);
    }
}