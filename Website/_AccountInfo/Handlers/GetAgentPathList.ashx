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
using Apollo.Age.DTO.Accounts;
using Apollo.Age.Entities;

public class GetAgentPathList : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
{
    private int agentId = -1;
    
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
    
    public override void ProcessRequest(ref System.Collections.Hashtable p)
    {
        this.agentId = string.IsNullOrEmpty(Request["root"]) ? this.agentId : int.Parse(Request["root"].ToString());
    }

    public override object ValidateRequest(System.Collections.Hashtable p)
    {
        if (this.agentId == -1)
        {
            return new Result(ErrorType.AccessDenied, string.Empty);
        }

        return null;
    }

    public override object InvokeData(System.Collections.Hashtable p)
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