<%@ WebHandler Language="C#" Class="GetAgentPathList" %>

using System;
using System.Web;
using System.Data;
using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.AccountInfo;

public class GetAgentPathList : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
{
    string agentID = string.Empty;
    
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
            object[] suggessions = new object[count];
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                suggessions[i] = new object[] { dt.Rows[i]["AgentId"].ToString(), HttpUtility.HtmlEncode(dt.Rows[i]["UserName"].ToString()) };
                //data.Add("ID", dt.Rows[i]["AgentId"].ToString());
                //data.Add("Agent", dt.Rows[i]["UserName"].ToString());
            }

            data.Add("Test", "test");
            data.Add("Agent", suggessions);
        }
    }

    
    public override void ProcessRequest(ref System.Collections.Hashtable p)
    {
        agentID = Request["root"];
    }

    public override object ValidateRequest(System.Collections.Hashtable p)
    {
        if (string.IsNullOrEmpty(agentID))
        {
            return new Result(ErrorType.AccessDenied, string.Empty);
        }

        return null;
    }

    public override object InvokeData(System.Collections.Hashtable p)
    {
        AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
        return bo.Age_Customer_GetDownLine(int.Parse(agentID)).Tables[0];
    }

}