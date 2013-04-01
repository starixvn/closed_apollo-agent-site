// --------------------------------------------------------------------------
// <copyright file="NumberGameViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.1">Created</change>
// </history>

namespace Apollo.Age.TotalBets.ViewModels
{
    using Apollo.Age.Services;

    public class NumberGameViewModel
    {
        public string ShowTime { get; set; }

        public int LeagueId { get; set; }

        public string MatchCode { get; set; }

        public int MatchId { get; set; }

        public decimal Over_1stBall { get; set; }

        public decimal Under_1stBall { get; set; }

        public decimal Over_LastBall { get; set; }

        public decimal Under_LastBall { get; set; }

        public decimal Odd_1stBall { get; set; }

        public decimal Even_1stBall { get; set; }

        public decimal Odd_LastBall { get; set; }

        public decimal Even_LastBall { get; set; }

        public decimal Odd_FullTime { get; set; }

        public decimal Even_FullTime { get; set; }

        public decimal Over_Next { get; set; }

        public decimal Under_Next { get; set; }

        public decimal Odd_Next { get; set; }

        public decimal Even_Next { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Warrior_2ndBall { get; set; }

        public decimal Warrior_3rdBall { get; set; }

        public decimal NextCombo_OverOdd { get; set; }

        public decimal NextCombo_OverEven { get; set; }

        public decimal NextCombo_UnderOdd { get; set; }

        public decimal NextCombo_UnderEven { get; set; }

        public decimal NumberGroupPre { get; set; }

        public decimal NumberGroupLive { get; set; }

        public decimal BetCount_OU1stBall { get; set; }

        public decimal BetCount_OULastBall { get; set; }

        public decimal BetCount_OE1stBall { get; set; }

        public decimal BetCount_OELastBall { get; set; }

        public decimal BetCount_OEFullTime { get; set; }

        public decimal BetCount_OUNext { get; set; }

        public decimal BetCount_OENext { get; set; }

        public decimal BetCount_HighLow { get; set; }

        public decimal BetCount_Warrior_2ndBall { get; set; }

        public decimal BetCount_Warrior_3rdBall { get; set; }

        public decimal BetCount_NextCombo_OverOdd { get; set; }

        public decimal BetCount_NextCombo_OverEven { get; set; }

        public decimal BetCount_NextCombo_UnderOdd { get; set; }

        public decimal BetCount_NextCombo_UnderEven { get; set; }

        public decimal BetCount_NumberGroupPre { get; set; }

        public decimal BetCount_NumberGroupLive { get; set; }

        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }
    }
}
