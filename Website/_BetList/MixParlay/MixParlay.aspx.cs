// ---------------------------------------------------------------------------------
// <copyright file="MixParlay.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.19">Appy CoreBetList</change>
//     <change who="Marc.Bui" date="2013.01.10">Rollback old betlist</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList.MixParlay
{
    using System.Collections;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.UI.BetList;

    public partial class MixParlay_Page : TplPage
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
            IBetListCmd command = new MixParlayBetListCmd(CurrentUserSession.NetSessionId);
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