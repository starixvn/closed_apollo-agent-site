// --------------------------------------------------------------------------
// <copyright file="SingbetSport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.6.26">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SingbetSport
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SingbetSport> GenerateSingbetSport
        {
            get
            {
                List<SingbetSport> listSport = new List<SingbetSport>() { 
                    new SingbetSport{
                    Id  = "",
                    Name = ""
                    }
                };
                return listSport;
            }
        }
    }


}
