// --------------------------------------------------------------------------
// <copyright file="Customer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.4">Created</change>
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
    public class Customer : ICustomer
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public decimal VirtualMultiplier { get; set; }

        public decimal MatchLimit { get; set; }

        public decimal Credit { get; set; }

        public bool DoubleCommLosing { get; set; }

        public byte Currency { get; set; }

        public Agent Agent { get; set; }
    }
}
