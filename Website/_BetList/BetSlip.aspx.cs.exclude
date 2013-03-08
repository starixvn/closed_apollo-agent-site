using System;
using System.Data;
using System.Text;
using System.Web.UI;
using Fanex.Age.UI.Components;
using Sunplus.Agent.Model.BL.BetList;
using Sunplus.Agent.Model.BL.Reports;
using Sunplus.Agent.UI.BetSlip.Factories;
using Sunplus.Agent.Utilities;

/*
 * Created 20111104@Kaden - Render BetSlip page
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._BetList
{
    public partial class BetSlip_Page : Fanex.Age.UI.TplPage
    {
        #region Define variable

        private Int64 transID = 0;
        private DateTime winlostDate = DateTime.Now;
        private bool isRunning = false;

        #endregion Define variable

        private void RenderHeader(BetSlip_Template template, DataRow dataRow)
        {
            template.UserName = Resources.Lang.username + ": " + dataRow["username"].ToString();
            template.RefNo = Resources.Lang.refno.ToUpper() + ": " + transID;
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            var template = new BetSlip_Template();
            DataTable betSlipData = View as DataTable;

            if (betSlipData == null || betSlipData.Rows.Count == 0) this.RenderNoInfo();

            var content = RenderingFactory.Render(betSlipData);

            RenderHeader(template, betSlipData.Rows[0]);

            template.content = content.ToString();
            writer.Write(template);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            Int64.TryParse(Request["TransID"], out transID);
            DateTime.TryParse(Request["winlostdate"], out winlostDate);
            bool.TryParse(Request["IsRunning"], out isRunning);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (transID == 0) this.RenderNoInfo();
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            BetListBO bo = new BetListBO(CurrentUserSession.NetSessionId);

            if (isRunning || winlostDate >= ReportsBO.BalanceUpDate)
            {
                return bo.GetBetSlip(transID);
            }
            else
            {
                return bo.GetBetSlip_Old(transID);
            }
        }

        private object RenderNoInfo()
        {
            return new ErrorPage_Page(Resources.Lang.noInfo);
        }
    }
}