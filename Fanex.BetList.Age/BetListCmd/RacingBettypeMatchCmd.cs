// ---------------------------------------------------------------------------------
// <copyright file="RacingBettypeMatchCmd.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.19">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListCmd
{
    using System.Data;
    using Fanex.BetList.Age.Definition;
    using Sunplus.Agent.Model.BL.BetList;

    /// <summary>
    /// Summary description for RacingBettypeMatchCmd
    /// </summary>
    public class RacingBettypeMatchCmd : BetListCmd
    {
        private DataTable result = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;

        private int matchId;
        private int betType;
        private int liveIndicator = 0;
        private string betTeam = string.Empty;

        public RacingBettypeMatchCmd(string sessionId)
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

            Title = Resources.Lang.hr;
            BetListType = BetListType.Member | BetListType.Winlost;
        }

        public override object Execute()
        {
            switch (this.betType)
            {
                case 31:
                case 32:
                case 33:
                case 41:
                case 42:
                case 43:
                    this.result = this.betList.GetAllRacingByMatch(this.betType, this.matchId, out this.outDataTable);
                    break;
                default:
                    this.result = this.betList.GetRunningByBettypeAndMatch(this.matchId, this.betType, this.liveIndicator, this.betTeam, out this.outDataTable);
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