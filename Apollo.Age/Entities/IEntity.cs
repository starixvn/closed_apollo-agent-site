// --------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>
namespace Apollo.Age.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IEntity<TId, TName>
    {
        TId Id { get; set; }

        TName Username { get; set; }        
    }
}
