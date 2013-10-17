// --------------------------------------------------------------------------
// <copyright file="SportCreditModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.7.31">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;

    /// <summary>
    /// Sport credit model class.
    /// </summary>
    [Serializable]
    public class SportCreditModel
    {
        public short AgentLevel { get; set; }

        public decimal OutstandingCredit { get; set; }

        public decimal RemainingCredit { get; set; }
    }
}
