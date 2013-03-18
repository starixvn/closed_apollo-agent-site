// --------------------------------------------------------------------------
// <copyright file="CustomerRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>


namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        public ICommandResult Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(Customer entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(Customer entity)
        {
            throw new NotImplementedException();
        }

        public int Count(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> Fetch(ICriteria criteria, Action<Orderable<Customer>> order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> Fetch(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> Fetch()
        {
            throw new NotImplementedException();
        }

        public Customer Get(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public Customer Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Length(dynamic paramaters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> Query(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        public Customer Single(dynamic parameters)
        {
            throw new NotImplementedException();
        }
    }
}
