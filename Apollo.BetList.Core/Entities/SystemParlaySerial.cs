// --------------------------------------------------------------------------
// <copyright file="SystemParlaySerial.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.5">Created</change>
// </history>
namespace Apollo.BetList.Core.Entities
{
    using System;

    /// <summary>
    /// System Parlay Serial.
    /// </summary>
    [Serializable]
    public class SystemParlaySerial : ISystemParlaySerial
    {
        public int AwayId { get; set; }

        public int HomeId { get; set; }

        public long Serial { get; set; }
    }
}