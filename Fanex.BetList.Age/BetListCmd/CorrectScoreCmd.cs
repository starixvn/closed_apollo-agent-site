// ---------------------------------------------------------------------------------
// <copyright file="CorrectScoreCmd.cs" company="Nexcel Solutions Vietnam">
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
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    /// Summary description for BettypeCmd
    /// </summary>
    public class CorrectScoreCmd : BetListCmd
    {
        private DataTable result = null;
        private BetListBO betList = null;

        public CorrectScoreCmd(string sessionId)
        {
            this.result = new DataTable();
            this.betList = new BetListBO(sessionId);
            Title = Resources.BetList.correctscore;
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            Title = CacheRef.GetBettype(4).ToUpper(); // Bettype = 4 is Correct Score
            BetListType = BetListType.Member;
        }

        public override object Execute()
        {
            this.result = this.betList.GetRunningCorrectScore();
            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return null;
        }
    }
}