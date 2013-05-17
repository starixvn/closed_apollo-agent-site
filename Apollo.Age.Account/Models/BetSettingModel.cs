// --------------------------------------------------------------------------
// <copyright file="DiscountModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.16">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System.Resources;
    using Apollo.Age.Resources;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetSettingModel
    {
        public int SportType { get; set; }

        public int BetType { get; set; }

        public string SportName
        {
            get
            {
                if (this.SportType == 99 && this.BetType == 9)
                {
                    return (new ResourceManager(typeof(Lang)) { IgnoreCase = true }).GetString("mixsports");
                }

                return CacheRef.GetSportName(this.SportType);
            }
        }

        public decimal MinBet { get; set; }

        public decimal OldMinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal OldMaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal OldMaxPerMatch { get; set; }
    }
}
