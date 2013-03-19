// --------------------------------------------------------------------------
// <copyright file="Member.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Customer
    /// </summary>
    public class Member : Customer
    {       
        public string Group { get; set; }
    }
}
