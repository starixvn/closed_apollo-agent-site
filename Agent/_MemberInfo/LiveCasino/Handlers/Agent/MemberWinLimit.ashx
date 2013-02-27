<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.MemberWinLimit" %>

using System;
using System.Web;
using System.Linq;
using System.Data;

namespace Fanex.Age.UI.LiveCasino
{
    public class MemberWinLimit : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private int custId = 0;
        private string userName = string.Empty;
        private int status = 1;
        private int pageSize = 500;
        private int pageIndex = 1;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            var users = View as System.Data.DataTable;

            var userList = (from c in users.AsEnumerable()
                            select new
                                {
                                    CustId = c.Field<int>("custid"),
                                    Username = c.Field<string>("username"),
                                    WinLimit = this.FormatLCS(c.Field<decimal>("lcswinpercent"))
                                }).ToArray();

            data.AsResult(new Fanex.Age.UI.Result(0, Newtonsoft.Json.JsonConvert.SerializeObject(userList)), true);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var bo = new Sunplus.Agent.Model.BL.MemberInfo.CreditBalance.CreditBalanceBO(CurrentUserSession.NetSessionId);

            return bo.GetCreditBalanceListLCSWinLimit(this.custId, this.userName, this.status, this.pageIndex, this.pageSize);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.userName = (this.Request["username"] == null || this.Request["username"] == "All" || this.Request["username"] == Resources.Lang.enterusername) ? string.Empty : HttpUtility.HtmlDecode(this.Request["username"]);
            this.userName = this.userName.Length == 0 ? string.Empty : this.userName;

            this.custId = (this.Request["custid"] == null) ? CurrentUserSession.CustId : Convert.ToInt32(this.Request["custid"]);
            this.status = (this.Request["status"] == null) ? 1 : Convert.ToInt32(this.Request["status"]);

            this.pageSize = this.GetIntRequest("pagesize", this.pageSize);
            this.pageIndex = this.GetIntRequest("pageindex", this.pageIndex);

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        private string FormatLCS(decimal value)
        {
            var ret = value == -1m ? Resources.Lang.Unlimited : value >= 100m ? "<font color='Red'>" + value.ToString("#,##0.##") + "%</font>" : value.ToString("#,##0.##") + "%";

            return ret;
        }
    }
}