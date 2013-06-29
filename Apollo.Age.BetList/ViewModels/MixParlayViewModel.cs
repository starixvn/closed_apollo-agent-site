// --------------------------------------------------------------------------
// <copyright file="MixParlayViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.28">Created and Implement</change>
// </history>

namespace Apollo.Age.BetList.ViewModels
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    public class MixParlayViewModel
    {
        public IList<ITicket> TicketList { get; set; }
    }
}