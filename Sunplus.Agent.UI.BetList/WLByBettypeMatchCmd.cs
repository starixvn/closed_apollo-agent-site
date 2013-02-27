namespace Sunplus.Agent.UI.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Reports;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// Summary description for WLByBettypeMatchCmd
    /// </summary>
    public class WLByBettypeMatchCmd : BetListCmd
    {
        private DataTable result = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;
        private ICollection<KeyValuePair<string, bool>> filterParams = null;
        
        private int matchId = 0;
        private int betType = 0;
        private int custId = 0;
        private string userName = string.Empty;
        private DateTime fdate, tdate;
        private int leagueId = 0;
        private string race = string.Empty;
        private int liveIndicator = 0;

        public WLByBettypeMatchCmd(string sessionId)
        {
            this.result = new DataTable();
            this.betList = new BetListBO(sessionId);
            this.outDataTable = new DataTable();
            this.filterParams = new Dictionary<string, bool>();            
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            this.custId = Sunplus.Agent.Model.Entities.UserSession.Current.CustId;
            this.matchId = (int)hash["matchid"];
            this.betType = (int)hash["bettype"];            
            this.userName = hash["username"].ToString();
            this.fdate = (DateTime)hash["fdate"];
            this.tdate = (DateTime)hash["tdate"];
            this.leagueId = (int)hash["leagueid"];
            this.race = hash["race"].ToString();
            this.liveIndicator = (int)hash["islive"];    

            if (hash["DateIsNull"] != null)
            {
                Title = Resources.BetList.lblMemberBetList;
            }
            else
            {
                Title = string.Join(null, new string[] { Resources.BetList.lblMemberBetList, " ", this.fdate.ToShortDateString(), " --> ", this.tdate.ToShortDateString() });
            }

            BetListType = BetListType.Member | BetListType.Winlost;
        }

        public override object Execute()
        {
            switch (this.betType)
            {
                case 10: // outright                    
                    this.result = this.betList.GetWinlossOutright(this.fdate, this.tdate, ReportsBO.MoveBetsDate);  
                    break;
                case 31: // horseracing
                case 32:
                case 33:
                case 41:
                case 42:
                case 43:
                    this.fdate = Cast.AsDateTime(this.fdate);
                    this.tdate = Cast.AsDateTime(this.tdate);

                    this.result = this.betList.GetWinlossHR(this.betType, this.leagueId, this.race, this.fdate, this.tdate, ReportsBO.MoveBetsDate, out this.outDataTable);  
                    break;
                default:                    
                    this.result = this.betList.GetWinlossByBettypeMatch(this.matchId, this.betType, this.userName, this.liveIndicator, out this.outDataTable);                    
                    break;
            }

            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return this.outDataTable;
        }
    }
}