using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;

using Sunplus.Agent.Model.BL.Forecast;
using Sunplus.Agent.Model.BL.Definitions;

using Fanex.Age.UI;

using Fanex.Age.UI.Components;
using Sunplus.Agent.Utilities;

using System.Text;
using System.Collections;
using Sunplus.Agent.Model.BL.Reports;
using Sunplus.Agent.Model.BL.BetList;
using Sunplus.Agent.UI.BetList;

/*

 * 
 */
namespace Sunplus.Agent.UI._BetList.MixParlay
{
    public partial class CombinationMixParlay_Page : TplPage
    {
        DataTable betListData = null;
        long transid;
        public override void RenderContent(HtmlTextWriter writer)
        {
            betListData = View as DataTable;

            StringBuilder row = Sunplus.Agent.UI.BetList.Factories.RenderingFactory.RenderMixParlay(ref betListData, false);

            writer.Write(row.ToString());
        }

        public override object InvokeData(Hashtable hash)
        {
            DataTable dtResult = null;
            IBetListCmd command = new CombMixParlayBetListCmd(CurrentUserSession.NetSessionId);
            if (command != null)
            {
                hash.Add("transid", transid);
                command.SetParams(hash);
                dtResult = command.Execute() as System.Data.DataTable;
            }
            return dtResult;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            long.TryParse(Request["transid"].ToString(), out transid);
        }

        public override object ValidateRequest(Hashtable p)
        {
            if (transid < 0)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }
            return null;
        }
    }

}