using System;

// --------------------------------------------------------------------------
// <copyright file="BetListViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
// </history>
using System.Collections.Generic;
using Fanex.BetList.Core.Entities;

namespace Apollo.Age.BetList.ViewModels
{
    public class BetListViewModel
    {
        public IList<ITicket> ListAgentComm { get; set; }
    }
}