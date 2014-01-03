// --------------------------------------------------------------------------
// <copyright file="CustomerCounterViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.12.25">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;

    /// <summary>
    /// Customer counter model class.
    /// </summary>
    [Serializable]
    public class CustomerCounterViewModel
    {
        public int? AgentId { get; set; }

        public int? IsAgent { get; set; }

        public int? CustCount { get; set; }
    }
}