// --------------------------------------------------------------------------
// <copyright file="ICustomerRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>

namespace Apollo.Age.Repositories.Interface
{
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface ICustomerRepository : IRepository<Customer>
    {
    }
}