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

    public class League : IComparable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is League)
            {
                if (this.Id == ((League)obj).Id)
                {
                    return 0;
                }
            }

            return -1;
        }
    }
}