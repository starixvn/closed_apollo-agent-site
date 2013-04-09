// --------------------------------------------------------------------------
// <copyright file="ForecastMatchWinLost.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.4">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">StyleCop</change>
// </history>

namespace Apollo.Age.Forecast.ViewModels
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ForecastMatchWinLostViewModel
    {
        public decimal Value0 { get; set; }

        public decimal Value1 { get; set; }

        public decimal Value2 { get; set; }

        public decimal Value3 { get; set; }

        public decimal Value4 { get; set; }
    }
}