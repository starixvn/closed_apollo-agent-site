// --------------------------------------------------------------------------
// <copyright file="UpdateExchangeRateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Admin.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class UpdateExchangeRateCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string IP { get; set; }

        public int ExchangeId { get; set; }

        public string Description { get; set; }

        public double MultipliedBy { get; set; }

        public double BaseRate { get; set; }

        public double MinBet { get; set; }

        public double MinBetLive { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Age_Exchange_UpdateRate";
        }
    }
}