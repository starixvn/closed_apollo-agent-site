// --------------------------------------------------------------------------
// <copyright file="ICurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.6">Created</change>
// </history>

namespace Apollo.Age.Web
{
    using System;

    /// <summary>
    /// It contains all properties of an authenticated user
    /// </summary>
    public interface ICurrentSession
    {
        int AgentId { get; set; }

        short AgentLevel { get; set; }

        string AgentName { get; set; }

        int UserId { get; set; }

        string Username { get; set; }

        bool IsMaster { get; set; }

        int CurrencyId { get; set; }

        bool DoubleCommLosing { get; set; }

        string Language { get; set; }

        decimal MatchLimit { get; set; }

        decimal VirtualMultiplier { get; set; }

        bool IsSuper { get; set; }
    }
}
