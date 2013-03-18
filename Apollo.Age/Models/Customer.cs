// -----------------------------------------------------------------------
// <copyright file="Customer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Apollo.Age.Models
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

        public int Currency { get; set; }

        public Agent Agent { get; set; }
    }
}
