// --------------------------------------------------------------------------
// <copyright file="MemberWinlossModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.12">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;

    /// <summary>
    /// Member win loss model class.
    /// </summary>
    [Serializable]
    public class MemberWinLossModel : AgentWinLossModel
    {
        public int MemberId { get; set; }

        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets Outstanding Amount.
        /// </summary>
        /// <value>The OutstandingAmount.</value>
        public decimal OutstandingAmount { get; set; }

        /// <summary>
        /// Gets or sets Outstanding Share Amount.
        /// </summary>
        /// <value>The OutstandingShareAmount.</value>
        public decimal OutstandingShareAmount { get; set; }
    }
}
