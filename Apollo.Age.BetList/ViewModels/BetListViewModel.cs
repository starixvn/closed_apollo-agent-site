// --------------------------------------------------------------------------
// <copyright file="BetListViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.07.01">Add properties SportList and BettypeList</change>
// </history>

namespace Apollo.Age.BetList.ViewModels
{
    using System.Collections.Generic;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.Entities;

    /// <summary>
    /// Bet list view model.
    /// </summary>
    public class BetListViewModel
    {
        public string Title { get; set; }

        public int CurrentMainShareHolderId { get; set; }

        public int CurrentAgentId { get; set; }

        public IEnumerable<Agent> ListMainShareHolders { get; set; }

        public IEnumerable<Agent> DownlinePath { get; set; }

        public List<TicketText> TicketList { get; set; }
    }
}