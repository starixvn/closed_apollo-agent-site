// --------------------------------------------------------------------------
// <copyright file="OutStandingViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutstandingModel
    {
        public int CurrentAgentId { get; set; }

        public int AgentLevel { get; set; }

        public IEnumerable<OutstandingDTO> ListOutStanding { get; set; }

        public IEnumerable<Agent> DownlinePath { get; set; }
    }
}