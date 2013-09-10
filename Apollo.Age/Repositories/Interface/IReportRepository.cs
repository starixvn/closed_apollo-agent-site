// --------------------------------------------------------------------------
// <copyright file="IReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.26">Created</change>
// </history>

namespace Apollo.Age.Repositories.Interface
{
    using Fanex.NAP.Framework.Data;

    public interface IReportRepository<T> : IReadonlyRepository<T>
    {
    }
}