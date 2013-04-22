// ---------------------------------------------------------------------------------
// <copyright file="BettypeCmd.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.19">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListCmd
{
    using System;
    using System.Data;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Services;
    using Fanex.BetList.Age.Definition;

    /// <summary>
    /// Summary description for BettypeCmd
    /// </summary>
    public class BettypeCmd : BetListCmd
    {
        private DataTable result = null;

        private string betType = Convert.ToString((int)BetTypes.None);

        public BettypeCmd(string sessionId)
        {
            this.result = new DataTable();
            Title = Resources.BetList.correctscore;
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            this.betType = hash["bettype"].ToString();
            Title = CacheRef.GetBettype(this.betType).ToUpper();
            this.BetListType = BetListType.Member;
        }

        public override object Execute()
        {
            this.result = this.betList.GetRunningByBettype(this.betType);
            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return null;
        }
    }
}