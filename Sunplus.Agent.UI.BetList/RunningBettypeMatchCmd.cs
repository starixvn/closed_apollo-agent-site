namespace Sunplus.Agent.UI.BetList
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.BetList;

    /// <summary>
    /// Summary description for RunningBettypeMatchCmd
    /// </summary>
    public class RunningBettypeMatchCmd : BetListCmd
    {
        private DataTable result = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;

        private int matchId;
        private int betType;
        private int liveIndicator = 0;
        private string betTeam = string.Empty;

        public RunningBettypeMatchCmd(string sessionId)
        {
            this.result = new DataTable();
            this.betList = new BetListBO(sessionId);
            this.outDataTable = new DataTable();
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            this.matchId = (int)hash["matchid"];
            this.betType = (int)hash["bettype"];
            this.liveIndicator = (int)hash["islive"];
            this.betTeam = hash["betteam"].ToString();

            BetListType = BetListType.Member;
        }

        public override object Execute()
        {
            this.result = this.betList.GetRunningByBettypeAndMatch(this.matchId, this.betType, this.liveIndicator, this.betTeam, out this.outDataTable);
            if (this.result != null && this.result.Rows.Count > 0)
            {
                if (this.result.Columns.Contains("leagueid"))
                {
                    DataRow row = this.result.Rows[0];
                    int leagueId = Convert.ToInt32(row["leagueid"]);
                    Title = Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(leagueId) + " " + Sunplus.Agent.Model.BL.Cache.CacheRef.GetBettype(this.betType);
                }
            }

            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return this.outDataTable;
        }
    }
}