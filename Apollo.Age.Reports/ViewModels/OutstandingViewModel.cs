// --------------------------------------------------------------------------
// <copyright file="OutStandingViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutstandingViewModel
    {
        public int CurrentAgentId { get; set; }

        public Agent MainShareHolder { get; set; }

        public int AgentLevel { get; set; }

        public IEnumerable<OutstandingModel> ListOutStanding { get; set; }

        public IEnumerable<Agent> DownlinePath { get; set; }
    }
}