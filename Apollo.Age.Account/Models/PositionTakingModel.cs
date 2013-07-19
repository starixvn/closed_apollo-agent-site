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

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class PositionTakingModel
    {
        public int SportType { get; set; }

        public int BetType { get; set; }

        public bool IsLive { get; set; }

        public decimal MaxUpper { get; set; }

        public decimal MaxGiven { get; set; }

        public decimal MinGiven { get; set; }

        public decimal? OldMaxUpper { get; set; }

        public decimal? OldMaxGiven { get; set; }

        public decimal? OldMinGiven { get; set; }
    }
}
