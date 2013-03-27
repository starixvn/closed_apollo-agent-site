<%@ WebHandler Language="C#" Class="GetAgentPathList" %>

using System;
using System.Web;
using System.Data;
using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.AccountInfo;
using Apollo.Age.Services.Interface;
using Apollo.Age.Services.Impl;
using Apollo.Age.Criterias.Accounts;
using System.Collections.Generic;
using System.Web.SessionState;
using Apollo.Age.Entities;
using System.Collections;
using Fanex.Common;

public class GetAgentPathList : TplHandler, IReadOnlySessionState
{
    private int agentId = -1;

    public int AgentId 
    {
        get
        {
            return Converter.ToInt(Request["agentId"]);
        }  
    }
    
    public GetAgentPathList()
    {
        IsValidateRequest = false;
    }

    public override void RenderResult(ref JSONData data)
    {
        List<Agent> lstAgents = View as List<Agent>;

        if (lstAgents != null)
        {
            int count = lstAgents.Count;
            object[] agentslist = new object[count];

            for (int i = 0; i < count; i++)
            {
                agentslist[i] = new object[] { lstAgents[i].Id, HttpUtility.HtmlEncode(lstAgents[i].Username) };
            }

            data.Add("Agents", agentslist);
        }
    }
    
    public override void ProcessRequest(ref Hashtable p)
    {
        this.agentId = AgentId;
    }

    public override object ValidateRequest(Hashtable p)
    {
        if (this.agentId == -1)
        {
            return new Result(ErrorType.AccessDenied, string.Empty);
        }

        return null;
    }

    public override object InvokeData(Hashtable p)
    {
        IAgentService service = new AgentService();

        AgentDownlinePathCriteria criteria = new AgentDownlinePathCriteria()
        {
            ParentId = CurrentUserSession.AgentId,
            AgentId = this.agentId
        };

        var lstAgents = service.GetAgentDownlinePath(criteria);

        return lstAgents;
    }

}