// --------------------------------------------------------------------------
// <copyright file="OutstandingDownlinePathDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Reports.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.ViewModels;

    public class OutstandingDownlinePathDTO
    {
        public int CurrentAgentId { get; set; }

        public IEnumerable<OutstandingViewModel> ListOutStanding { get; set; }

        public IEnumerable<Agent> DownlinePath { get; set; }
    }
}