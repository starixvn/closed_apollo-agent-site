// --------------------------------------------------------------------------
// <copyright file="ForecastMatchModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.4">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">StyleCop</change>
// </history>

namespace Apollo.Age.Forecast.Models
{
    /// <summary>
    /// Forecast match model.
    /// </summary>
    public class ForecastMatchModel
    {
        public decimal Offset { get; set; }

        public decimal HomeTotal { get; set; }

        public decimal AwayTotal { get; set; }

        public decimal HomeWinLost { get; set; }

        public decimal AwayWinLost { get; set; }

        //// Nerver Use
        public bool LiveIndicator { get; set; }

        public string Status { get; set; }
    }
}