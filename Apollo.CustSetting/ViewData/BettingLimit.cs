// --------------------------------------------------------------------------
// <copyright file="BettingLimit.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
// </history>

namespace Apollo.CustSetting.ViewData
{
    /// <summary>
    /// Class name.
    /// </summary>
    public class BettingLimit
    {
        public int SportId { get; set; }

        public int BetTypeId { get; set; }

        public string Name { get; set; }

        public string ResourceKey { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MaxPerBall { get; set; }

        public decimal MinBetMax { get; set; }

        public decimal MaxBetMin { get; set; }

        public decimal MaxPerMatchMin { get; set; }

        public decimal MaxPerBallMin { get; set; }
    }
}