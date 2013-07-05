// --------------------------------------------------------------------------
// <copyright file="NumberGameModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.1">Created</change>
// </history>

namespace Apollo.Age.TotalBets.Models
{
    using Apollo.Age.Services;

    /// <summary>
    /// Number Game Model.
    /// </summary>
    public class NumberGameModel
    {
        public string ShowTime { get; set; }

        public int LeagueId { get; set; }

        public string MatchCode { get; set; }

        public int MatchId { get; set; }

        public decimal Over1stBall { get; set; }

        public decimal Under1stBall { get; set; }

        public decimal OverLastBall { get; set; }

        public decimal UnderLastBall { get; set; }

        public decimal Odd_1stBall { get; set; }

        public decimal Even1stBall { get; set; }

        public decimal OddLastBall { get; set; }

        public decimal EvenLastBall { get; set; }

        public decimal OddFullTime { get; set; }

        public decimal EvenFullTime { get; set; }

        public decimal OverNext { get; set; }

        public decimal UnderNext { get; set; }

        public decimal OddNext { get; set; }

        public decimal EvenNext { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Warrior2ndBall { get; set; }

        public decimal Warrior3rdBall { get; set; }

        public decimal NextComboOverOdd { get; set; }

        public decimal NextComboOverEven { get; set; }

        public decimal NextComboUnderOdd { get; set; }

        public decimal NextComboUnderEven { get; set; }

        public decimal NumberGroupPre { get; set; }

        public decimal NumberGroupLive { get; set; }

        public int BetCountOU1stBall { get; set; }

        public int BetCountOULastBall { get; set; }

        public int BetCountOE1stBall { get; set; }

        public int BetCountOELastBall { get; set; }

        public int BetCountOEFullTime { get; set; }

        public int BetCountOUNext { get; set; }

        public int BetCountOENext { get; set; }

        public int BetCountHighLow { get; set; }

        public int BetCountWarrior2ndBall { get; set; }

        public int BetCountWarrior3rdBall { get; set; }

        public int BetCountNextComboOverOdd { get; set; }

        public int BetCountNextComboOverEven { get; set; }

        public int BetCountNextComboUnderOdd { get; set; }

        public int BetCountNextComboUnderEven { get; set; }

        public int BetCountNumberGroupPre { get; set; }

        public int BetCountNumberGroupLive { get; set; }

        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }
    }
}
