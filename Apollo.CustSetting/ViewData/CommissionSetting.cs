// --------------------------------------------------------------------------
// <copyright file="CommissionSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.6">Created</change>
// </history>

namespace Apollo.CustSetting.ViewData
{
    using System.Collections.Generic;

    /// <summary>
    /// Class name.
    /// </summary>
    public class CommissionSetting
    {
        public IList<CommissionItem> CommissionItems { get; set; }

        public string UserGroup { get; set; }
    }
}