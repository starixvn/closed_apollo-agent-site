// --------------------------------------------------------------------------
// <copyright file="PositionTakingSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
// </history>

namespace Fanex.CustSetting.ViewData
{
    using System.Collections.Generic;

    /// <summary>
    /// Class name.
    /// </summary>
    public class PositionTakingSetting
    {
        public PositionTakingSetting()
        {
            SportTypes = new List<SportType>();
        }

        public string Name { get; set; }

        public IList<SportType> SportTypes { get; set; }
    }
}