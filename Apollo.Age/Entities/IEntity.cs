// --------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.9.5">Created</change>
// </history>
namespace Apollo.Age.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IEntity<TId, TName>
    {
        TId Id { get; set; }

        TName Username { get; set; }
    }
}