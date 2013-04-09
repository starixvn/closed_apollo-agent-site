// --------------------------------------------------------------------------
// <copyright file="ExchRate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.3.18">Created</change>
// </history>

namespace Apollo.Age.DTO.Admin.ExchangeRate
{
    public class ExchRate
    {
        public int ExchangeId { get; set; }

        public string Currency { get; set; }

        public int? MultipliedBy { get; set; }

        public double BaseRate { get; set; }

        public double MinBet { get; set; }

        public double MinBetLive { get; set; }

        public string Description { get; set; }
    }
}