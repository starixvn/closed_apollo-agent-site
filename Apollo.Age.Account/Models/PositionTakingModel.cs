// --------------------------------------------------------------------------
// <copyright file="PositionTakingModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.17">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System.Diagnostics.CodeAnalysis;
    using System.Resources;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Resources;
    using Apollo.Age.Services;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class PositionTakingModel
    {
        public int SportType { get; set; }

        public string SportName
        {
            get
            {
                if (this.SportType == (int)SportTypes.OtherSports && this.BetType == (int)BetTypes.Mix_Parlay)
                {
                    return (new ResourceManager(typeof(Lang)) { IgnoreCase = true }).GetString("mixsports");
                }

                return CacheRef.GetSportName(this.SportType);
            }
        }

        public int BetType { get; set; }

        public string BetName
        {
            get
            {
                return CacheRef.GetBettype(this.BetType);
            }
        }

        public bool IsLive { get; set; }

        public decimal MaxUpper { get; set; }

        public decimal MaxGiven { get; set; }

        public decimal MinGiven { get; set; }

        public decimal OldMaxUpper { get; set; }

        public decimal OldMaxGiven { get; set; }

        public decimal OldMinGiven { get; set; }
    }
}
