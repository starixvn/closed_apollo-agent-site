/*
 * Created 20101110@Simon - Render Monitor page
 * Revision:
 * 20120104@Kaden: Refactor code. Get Odds, OddsType in the code
 *
 */

namespace Fanex.Age.UI.Reports.Monitor
{
    using System;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using Sunplus.Agent.Model.BL.Reports;
    using Sunplus.Agent.UI.BetList;
    using Sunplus.Agent.UI.BetList.Factories;

    public partial class MonitorBetList_Page : Reports_Page
    {
        private int betTypeGroup = 0, sportTypeGroup = 0, limitRowCount = 10;
        private long transId = 0;

        public string TransIdCookieKey
        {
            get
            {
                string name = string.Join(null, new string[] { CurrentUserSession.CustId.ToString(), "lastTransId", CurrentUserSession.UserId.ToString() });
                return Sunplus.Agent.Utilities.Encryption.CfsEnCode(name, 6);
            }
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable bets = View as DataTable;

            StringBuilder content = RenderingFactory.Render(BetListType.Member, ref bets, Sunplus.Agent.Model.BL.Definitions.UserTypes.Member);

            writer.Write(content.ToString());
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(this.Request["sportTypeGroup"], out this.sportTypeGroup);
            int.TryParse(this.Request["betTypeGroup"], out this.betTypeGroup);
            int.TryParse(this.Request["limitRowCount"], out this.limitRowCount);

            long.TryParse(Request.Cookies[this.TransIdCookieKey] != null ? Request.Cookies[this.TransIdCookieKey].Value : "0", out this.transId);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            ReportsBO report = new ReportsBO(CurrentUserSession.NetSessionId);

            DataTable bets = null;
            try
            {
                bets = report.GetLastBetList(this.sportTypeGroup, this.betTypeGroup, this.transId, this.limitRowCount);
            }
            catch
            {
                throw new HttpException("Our server is too busy at the moment so this function is turned off temporarily. Please try again later and sorry for the inconvenience.");
            }

            int c = bets.Rows.Count;
            if (c > 0)
            {
                this.transId = Convert.ToInt64(bets.Rows[c - 1]["transid"]) - 1; // Get last top rows
            }

            HttpCookie cookie = new HttpCookie(this.TransIdCookieKey, this.transId.ToString());
            Response.Cookies.Set(cookie);

            return bets;
        }
    }
}