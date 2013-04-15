// --------------------------------------------------------------------------
// <copyright file="BetStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Tring" date="2013.4.12">Created</change>
// </history>
namespace Apollo.Age.Enumerations
{
    public enum BetStatus : int
    {
        Running = 0,
        Waiting = 1,
        Reject = 101,
        Void = 102,
        Refund = 103,
        Draw = 111,
        Won = 112,
        Lose = 113
    }
}