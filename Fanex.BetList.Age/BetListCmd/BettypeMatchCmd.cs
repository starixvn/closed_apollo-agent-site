// ---------------------------------------------------------------------------------
// <copyright file="BettypeMatchCmd.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.19">Create</change>
// </history>
// ---------------------------------------------------------------------------------

using System;
using System.Data;
using Fanex.BetList.Age.Definition;
using Sunplus.Agent.Model.BL.BetList;
using Sunplus.Agent.Model.BL.Cache;

namespace Fanex.BetList.Age.BetListCmd
{
    /// <summary>
    /// Summary description for BettypeMatchCmd
    /// </summary>
    public class BettypeMatchCmd : BetListCmd
    {
        private DataTable result = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;

        private int matchId = 0;
        private int betType = 0;
        private int liveIndicator = 0;
        private string betTeam = string.Empty;

        public BettypeMatchCmd(string sessionId)
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

            this.BetListType = BetListType.Member;
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
                    //if (!UserPermission.IsShowRacing)
                    //{
                    //    return new DataTable();
                    //}

                    this.result = this.betList.GetAllRacingByMatch(this.betType, this.matchId, out this.outDataTable);
                    break;
                default:
                    this.result = this.betList.GetRunningByBettypeAndMatch(this.matchId, this.betType, this.liveIndicator, this.betTeam, out this.outDataTable);
                    break;
            }

            // set title
            if (this.result != null && this.result.Rows.Count > 0)
            {
                // outright
                if (this.betType == 10)
                {
                    Title = CacheRef.GetBettype(this.betType).ToUpper();
                }
                else if (this.betType == 31 || this.betType == 32 || this.betType == 33)
                {
                    Title = Resources.Racing.horseracing;
                }
                else if (this.result.Columns.Contains("leagueid"))
                {
                    DataRow row = this.result.Rows[0];
                    int leagueId = Convert.ToInt32(row["leagueid"]);
                    Title = CacheRef.GetLeagueName(leagueId) + " " + CacheRef.GetBettype(this.betType);
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