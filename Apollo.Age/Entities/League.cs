// --------------------------------------------------------------------------
// <copyright file="League.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services;

    /// <summary>
    /// This class to override equals.
    /// </summary>
    public class League
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Override equals.
        /// </summary>
        /// <param name="obj">Input a object.</param>
        /// <returns>Return true or false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is League)
            {
                return ((League)obj).Id == this.Id;
            }

            return false;
        }

        /// <summary>
        /// Override get hash code.
        /// </summary>
        /// <returns>Return a integer value.</returns>
        public override int GetHashCode()
        {
            return (int)Id;
        }
    }
}