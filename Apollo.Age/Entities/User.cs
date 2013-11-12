// --------------------------------------------------------------------------
// <copyright file="User.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
//   <change who="Anders.Le" date="2013.4.9">Add rule check use super level</change>
// </history>

namespace Apollo.Age.Entities
{
    using System;

    /// <summary>
    /// Agent user entity.
    /// </summary>
    [Serializable]
    public class User : EntityBase
    {
        public bool IsMaster { get; set; }

        public DateTime CreatedDate { get; set; }

        public byte Status { get; set; }

        public Agent Agent { get; set; }

        public bool IsSuper
        {
            get
            {
                return this.Agent.Level == 0;
            }
        }
    }
}