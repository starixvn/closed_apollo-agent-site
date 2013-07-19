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

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class DiscountModel
    {
        public int SportType { get; set; }

        public int BetType { get; set; }

        public decimal GroupA { get; set; }

        public decimal GroupB { get; set; }

        public decimal GroupC { get; set; }

        public decimal GroupD { get; set; }

        public decimal? OldGroupA { get; set; }

        public decimal? OldGroupB { get; set; }

        public decimal? OldGroupC { get; set; }

        public decimal? OldGroupD { get; set; }
    }
}
