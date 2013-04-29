// --------------------------------------------------------------------------
// <copyright file="Sport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.10">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Sport
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ResourceKey { get; set; }

        public List<BetType> BetTypeList { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Sport)
            {
                return ((Sport)obj).Id == this.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }
    }
}
