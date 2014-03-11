// --------------------------------------------------------------------------
// <copyright file="ICustomer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.9.5">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    /// <summary>
    /// Customer interface.
    /// </summary>
    public interface ICustomer : IEntity<int, string>
    {
        byte Currency { get; set; }

        decimal MatchLimit { get; set; }

        decimal VirtualMultiplier { get; set; }

        bool DoubleCommLosing { get; set; }

        Agent Agent { get; set; }
    }
}
