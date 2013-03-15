<%@ WebHandler Language="C#" Class="GetAgentPathList" %>

using System;
using System.Web;
using System.Data;
using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.AccountInfo;

public class GetAgentPathList : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
{
    string agentId = string.Empty;
    
    public GetAgentPathList()
    {
        IsValidateRequest = false;
    }

    public override void RenderResult(ref JSONData data)
    {
        DataTable dt = View as DataTable;
        
        if (dt != null)
        {
            int count = dt.Rows.Count;
            object[] agentslist = new object[count];
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                agentslist[i] = new object[] { dt.Rows[i]["AgentId"].ToString(), HttpUtility.HtmlEncode(dt.Rows[i]["UserName"].ToString()) };
            }

            data.Add("Agents", agentslist);
        }
    }

    
    public override void ProcessRequest(ref System.Collections.Hashtable p)
    {
        this.agentId = Request["root"];
    }

    public override object ValidateRequest(System.Collections.Hashtable p)
    {
        if (string.IsNullOrEmpty(this.agentId))
        {
            return new Result(ErrorType.AccessDenied, string.Empty);
        }

        return null;
    }

    public override object InvokeData(System.Collections.Hashtable p)
    {
        AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
        return bo.Age_Customer_GetAgentDownLinePath(int.Parse(this.agentId)).Tables[0];
    }

}