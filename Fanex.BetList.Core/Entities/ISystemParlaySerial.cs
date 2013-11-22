// --------------------------------------------------------------------------
// <copyright file="ISystemParlaySerial.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.5">Created</change>
// </history>
namespace Fanex.BetList.Core.Entities
{
    public interface ISystemParlaySerial
    {
        int AwayId { get; set; }

        int HomeId { get; set; }

        long Serial { get; set; }
    }
}