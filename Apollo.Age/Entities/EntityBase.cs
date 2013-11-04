// --------------------------------------------------------------------------
// <copyright file="EntityBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    /// <summary>
    /// Entity base.
    /// </summary>
    public abstract class EntityBase : IEntity<int, string>
    {
        public int Id { get; set; }

        public string Username { get; set; }
    }
}
