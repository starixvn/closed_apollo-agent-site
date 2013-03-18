<%@ WebHandler Language="C#" Class="GetMemberList" %>

using System;
using System.Web;
using System.Data;
using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.AccountInfo;

public class GetMemberList : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
{
    string agentId = string.Empty;
    
    public GetMemberList()
    {
        IsValidateRequest = false;
    }

    public override void RenderResult(ref JSONData data)
    {
        DataTable dt = View as DataTable;

        if (dt != null)
        {
            int count = dt.Rows.Count;
            object[] memberslist = new object[count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                memberslist[i] = new object[] { dt.Rows[i]["CustId"].ToString(), HttpUtility.HtmlEncode(dt.Rows[i]["UserName"].ToString()) };
            }

            data.Add("Members", memberslist);
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
        return bo.Age_Customer_GetMemberDownLine(int.Parse(this.agentId)).Tables[0];
    }

}