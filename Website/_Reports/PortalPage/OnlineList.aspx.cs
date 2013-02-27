using System;
using System.Collections.Generic;
using System.Web.UI;
using Fanex.Age.UI;
using Fanex.Age.UI.Reports.PortalPage;
using System.Data;
using Resources;
using Sunplus.Agent.Model.Entities;
using Fanex.Age.UI.Components;
using Sunplus.Agent.Model.BL.Reports;
using Sunplus.Core.Common.IPs;
using System.Linq;

namespace Fanex.Age.UI.Reports
{
    public partial class OnlineList : TplPage
    {
        private ReportsBO report = null;
        private int uid = 0;

        private const int DEFAULT_PAGE_SIZE = 5;
        private const int DEFAULT_PAGE_INDEX = 1;

        private int _pageSize = DEFAULT_PAGE_SIZE;
        private int _pageIndex = DEFAULT_PAGE_INDEX;

        public override void RenderContent(HtmlTextWriter writer)
        {
            Response.Cache.SetMaxAge(new TimeSpan(0, 10, 0));

            var tpl = new OnlineList_Template();
            var dt = View as DataTable;

            this._pageIndex = Convert.ToInt32(Request["pageIndex"]);
            this._pageSize = Convert.ToInt32(Request["pageSize"]);

            if (this._pageSize == 0)
                this._pageSize = DEFAULT_PAGE_SIZE;

            if (this._pageIndex == 0)
                this._pageIndex = DEFAULT_PAGE_INDEX;

            tpl.OnlineList = Lang.onlinelist.ToUpper();
            tpl.UserName = Lang.username;
            tpl.LoggedIn = Lang.lastlogin;
            //tpl.LastTransaction = Lang.lasttransaction;
            tpl.IP = Lang.ip;

            tpl.Rows.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                tpl.NoRow.Clear();
                var rows = dt.AsEnumerable().Skip((this._pageIndex - 1) * this._pageSize).Take(this._pageSize);
                var row = new OnlineList_Rows_Block();

                var index = 0;

                for (int i = 0; i < rows.Count(); i++)
                {
                    index++;

                    var dr = rows.ElementAt(i);

                    row.oddEvenRow = index % 2 == 0 ? "evenRow" : "oddRow";
                    row.UserName_vl = dr["username"].ToString();
                    row.LoggedIn_vl = dr["reload_time"].ToString();
                    //row.LastTransaction_vl = dr["last_transdate"] == DBNull.Value ? "-" : dr["last_transdate"].ToString();
                    var ip = FormatIp(dr["IP"].ToString());
                    row.IP_vl = ip;
                    row.currentIP = ip;

                    tpl.Rows.Append(row);                    
                }
            }
            else
                tpl.NoRow.NoRowInfo = Lang.noInfo;

            // Paging
            string pagingControlName = "PortalPage_Paging_OL";
            tpl.Paging.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                var paging = new OnlineList_Paging_Block();
                paging.PagingControl = new Paging_Control(tpl, pagingControlName, _pageSize, this.Request.Url.AbsoluteUri, dt.Rows.Count, new List<object> { 5, 10, 20 });
                tpl.Paging.Append(paging); 
            }

            writer.Write(tpl);
        }

        public string FormatIp(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return string.Empty;

            bool canViewVNIp = CurrentUserSession.IsAdminCanViewVNIP;

            if (!canViewVNIp && IpUtils.IsVnIp(ip))
            {
                return string.Empty;
            }

            return ip;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            try
            {
                return report.GetOnlineList(uid);
            }
            catch
            {
                return null;
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            if (this.report == null)
                this.report = new ReportsBO(UserSession.Current.NetSessionId);

            if (this.uid == 0)
                this.uid = UserSession.Current.CustId;
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }
    }
}
