// --------------------------------------------------------------------------
// <copyright file="TreeList.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Apollo.Age.Entities;
using Apollo.Age.Services.Impl;
using Apollo.Age.Services.Interface;
using Fanex.Common;

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

    private readonly IAgentService _agentService;

    public _AccountInfo_TreeList()
    {
        _agentService = new AgentService();
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

    public int SelectedAgentId
    {
        get
        {
            return Converter.ToInt(Request.QueryString["root"]);
        }
    }

    public int AgentId
    {
        get
        {
            return Converter.ToInt(Request.QueryString["agentid"]);
        }
    }

    public void AsyncTree()
    {
        int agentId = SelectedAgentId;
        agentId =  (agentId < 0) ? AgentId: agentId;

        if (_agentService.IsDescendant(CurrentUserSession.AgentId, agentId))
        {
            var customerList = _agentService.GetDirectDownline(agentId);
            RenderJsonString(ConvertToTreeViewNode(customerList));
        }
    }

    /// <summary>
    /// Convert agent list to ui object model treeview node
    /// </summary>
    /// <param name="customerList"></param>
    /// <returns></returns>
    public List<TreeViewNode> ConvertToTreeViewNode(List<Agent> customerList)
    {
        var listTreeViewNode = new List<TreeViewNode>();

        if (customerList != null && customerList.Any())
        {
            customerList.ForEach(agent =>
            {
                listTreeViewNode.Add(new TreeViewNode
                    {
                        Id = agent.Id,
                        UserName = agent.Username,
                        HasChildren = agent.HasAgentDownline,
                        Type = (CurrentUserSession.AgentId == AgentId) ? 0 : 1
                    });
            });
        }

        return listTreeViewNode;
    }

    private void RenderJsonString(List<TreeViewNode> nodeList)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string json = serializer.Serialize(nodeList);     

        Response.Write(json);
        Response.End();
    }
}