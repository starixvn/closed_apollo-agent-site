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
    /// Entity interface.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TName">The type of the name.</typeparam>
    public interface IEntity<TId, TName>
    {
        TId Id { get; set; }

        TName Username { get; set; }
    }
}