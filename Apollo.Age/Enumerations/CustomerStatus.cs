// --------------------------------------------------------------------------
// <copyright file="CustomerStatues.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.7">Created</change>
// </history>

namespace Apollo.Age.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum CustomerStatus : int
    {
        Enable = 1,
        Suspended = 2,
        Closed = 3
    }
}
