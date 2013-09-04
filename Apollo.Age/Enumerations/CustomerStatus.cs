// --------------------------------------------------------------------------
// <copyright file="CustomerStatues.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.7">Created</change>
//   <change who="Ricky.Nguyen" date="2013.5.6">Update Rename</change>
// </history>

namespace Apollo.Age.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum CustomerStatus : int
    {
        Active = 1,
        Stopped = 2,
        Locked = 3,
        CLock = 4
    }
}
