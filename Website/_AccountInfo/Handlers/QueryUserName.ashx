<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.QueryUserName_Handler" %>

using System;
using System.Web;
using System.Data;
using Fanex.Age.UI;

using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._AccountInfo
{

    public class QueryUserName_Handler : Fanex.Age.UI.TplHandler,System.Web.SessionState.IReadOnlySessionState
    {
        string query = string.Empty;
        bool isMember = false;
        
        public QueryUserName_Handler()
        {
            IsValidateRequest = false;
        }

        public override void RenderResult(ref JSONData data)
        {
            DataTable result = View as DataTable; // username, firstname, lastname

            DataRowCollection rows = result.Rows;
            int count = rows.Count;

            object[] suggessions = new object[count];
            
            for (int i = 0; i < count; i++)
            {
                suggessions[i] = new object[]{HttpUtility.HtmlEncode(rows[i]["username"].ToString())};
            }

            data.Add("query", query);
            data.Add("suggestions", suggessions);
        }

        
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            query = Request.QueryString["query"];
            isMember = Request.QueryString["isMember"] == "0" ? false : true; 
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new Result(ErrorType.AccessDenied, string.Empty);
            }
            
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            return bo.Age_Customer_GetUserSearch(query, isMember);
        }
    }

}