<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.GetRacingStatus_Handler" %>

using System;
using System.Web;

using Sunplus.Agent.Model.BL.AccountInfo;
using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.Racing;

using System.Data;

namespace Sunplus.Agent.UI._AccountInfo
{
    public class GetRacingStatus_Handler : TplHandler, System.Web.SessionState.IRequiresSessionState
    {
        int custId = 0;
        
        public GetRacingStatus_Handler()
        {
            IsValidateRequest = false;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            DataTable dt = View as DataTable;
            DataRow row = dt.Rows[0];
            DataColumnCollection cols = dt.Columns;
            
            // Row to JSON           
            foreach (DataColumn col in cols)
            {
                data.Add(col.ColumnName, row[col]);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["custid"], out custId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (custId <= 0)
            {
                return new Result(ErrorType.Unknown, "Invalid customer id!");
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            RacingBO racing = new RacingBO(CurrentUserSession.NetSessionId);

            return racing.SelectStatus(custId);
        }
    }
}