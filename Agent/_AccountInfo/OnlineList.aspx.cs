using System;
using System.Data;
using Sunplus.Agent.Model.BL.AccountInfo;
using Sunplus.Agent.Utilities;

/*
 * Created 20100715@Simon - Render Account Info page
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._AccountInfo
{

    public partial class OnlineList_Page : Fanex.Age.UI.TplPage
    {
        int custId = 0;
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            custId =  (string.IsNullOrEmpty(Request["custId"]))? 0 : Convert.ToInt32(Request["custId"]);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            return bo.CustomerSelOnline(custId);
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            OnlineList_Template tpl = new OnlineList_Template();
            if (custId > 0)
            {
                DataTable tblDownline = View as DataTable;
                DataRowCollection rows = tblDownline.Rows;
                int userCount = rows.Count;
                if (userCount > 0)
                {
                    string row = string.Empty;
                    for (int i = 0; i < userCount; ++i)
                    {
                        OnlineList_RowLastLogin_Block tplLog = new OnlineList_RowLastLogin_Block();
                        tplLog.no = (i + 1).ToString();                        
                        tplLog.ip = Sunplus.Agent.Model.Common.AgentIPFormat.FormatIP(rows[i]["IP"].ToString());
                        tplLog.date = ToReadableString(rows[i]["reload_time"]);

                        row += tplLog.ToString();
                    }
                    tpl.RowLastLogin.SetValue(row);
                    writer.Write(tpl.ToString());
                }
                else
                    writer.Write("<center>" + Resources.Lang.userisnotonline + "</center>");
            }
        }


        public string ToReadableString(object dateTime)
        {
            DateTime loginTime = DateTime.MinValue;
            bool isDateTime = DateTime.TryParse(dateTime + string.Empty, out loginTime);

            if (!isDateTime)
            {
                return "-";
            }

            return loginTime.ToString("dd MMM hh:mm tt");
        }

    }
}
