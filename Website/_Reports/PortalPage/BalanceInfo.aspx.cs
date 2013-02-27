using System;
using System.Web.UI;
using Fanex.Age.UI;
using Fanex.Age.UI.Reports.PortalPage;
using System.Data;
using Resources;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL.Reports;

namespace Fanex.Age.UI.Reports
{
    public partial class BalanceInfo : TplPage
    {
        private ReportsBO report = null;
        private int uid = 0;
        private string currency = null;

        public override void RenderContent(HtmlTextWriter writer)
        {
            var tpl = new BalanceInfo_Template();
            var dt = View as DataTable;

            if (dt == null) return;

            tpl.BalanceInfo = Lang.balanceinfo.ToUpper();
            tpl.Currency = Lang.currency;
            tpl.Currency_vl = currency;
            tpl.CashBalance = Lang.lblCashBalance;
            tpl.YesterdayBalance = Lang.yesterdaycashbalance;
            tpl.TotalBalance = Lang.lblTotalBalance;
            tpl.YesterdayTotalBalance = Lang.yesterdaytotalbalance;
            tpl.TodayWinLoss = Lang.todaywinloss + " (" + DateTime.Now.ToString("MM/dd/yyyy") + ")";
            tpl.YesterdayWinLoss = Lang.yesterdaywinloss + " (" + DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy") + ")";

            if (UserSession.Current.IsSuper)
            {
                tpl.UserName = Lang.super;
                tpl.AgentGivenCredit = Lang.supergivencredit;
                tpl.TotalMemberCredit = Lang.lblTotalMasterCredit;
            }
            else if (UserSession.Current.IsMaster)
            {
                tpl.UserName = Lang.master;
                tpl.AgentGivenCredit = Lang.mastergivencredit;
                tpl.TotalMemberCredit = Lang.lblTotalAgentCredit;
            }
            else if (UserSession.Current.IsAgent)
            {
                tpl.UserName = Lang.agent;
                tpl.AgentGivenCredit = Lang.agentgivencredit;
                tpl.TotalMemberCredit = Lang.lblTotalMemberCredit;
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];

                tpl.AgentName_vl = UserSession.Current.UserName;
                tpl.CashBalance_vl = this.ConvertToDecimal(row["cashBalance"]);
                tpl.YesterdayBalance_vl = this.ConvertToDecimal(row["yesterday_CashBalance"]);
                tpl.TotalBalance_vl = this.ConvertToDecimal(row["totalBalance"]);
                tpl.YesterdayTotalBalance_vl = this.ConvertToDecimal(row["yesterday_totalBalance"]);
                tpl.TodayWinLoss_vl = this.ConvertToDecimal(row["todayWinloss"]);
                tpl.YesterdayWinLoss_vl = this.ConvertToDecimal(row["yesterdayWinloss"]);
                tpl.AgentGivenCredit_vl = this.ConvertToDecimal(row["givenCredit"]);
                tpl.TotalMemberCredit_vl = this.ConvertToDecimal(row["memberCredit"]);
            }

            writer.Write(tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            try
            {
                return report.GetBalanceInfo(uid);
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

            if (this.currency == null)
                this.currency = UserSession.Current.Currency + " ";
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        private string ConvertToDecimal(object value)
        {
            var format = "<span class='{0}'>{1}</span>";
            var num = decimal.Round(Convert.ToDecimal(value == DBNull.Value ? 0 : value), 2);
            var style = num < .00m ? "bl_favorite" : "bl_underdog";

            var ret = string.Format(format, style, num.ToString("#,##0.00"));

            return ret;
        }
    }
}