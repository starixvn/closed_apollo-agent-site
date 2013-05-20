// --------------------------------------------------------------------------
// <copyright file="DiscountModel.cs" company="Nexcel Solutions Vietnam">
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
    public class DiscountModel
    {
        public int SportType { get; set; }

        public int BetType { get; set; }

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

        public decimal GroupA { get; set; }

        public decimal GroupB { get; set; }

        public decimal GroupC { get; set; }

        public decimal GroupD { get; set; }

        public decimal OldGroupA { get; set; }

        public decimal OldGroupB { get; set; }

        public decimal OldGroupC { get; set; }

        public decimal OldGroupD { get; set; }
    }
}
