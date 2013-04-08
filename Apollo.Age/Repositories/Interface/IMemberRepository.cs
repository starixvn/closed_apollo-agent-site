// --------------------------------------------------------------------------
// <copyright file="IMemberRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

namespace Apollo.Age.Repositories.Interface
{
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface IMemberRepository : IRepository<Member>
    {
    }
}
