// --------------------------------------------------------------------------
// <copyright file="ISystemParlayTicket.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.5">Created</change>
// </history>
namespace Fanex.BetList.Core.Entities
{
    using System;

    public interface ISystemParlayTicket
    {
        decimal MOdds { get; set; }

        decimal PlayerComm { get; set; }

        long Serial { get; set; }

        decimal Stake { get; set; }

        string Status { get; set; }

        int StatusId { get; set; }

        long TransId { get; set; }

        decimal WinLost { get; set; }

        DateTime WinLostDate { get; set; }
    }
}