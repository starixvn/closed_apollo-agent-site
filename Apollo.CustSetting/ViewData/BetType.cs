// --------------------------------------------------------------------------
// <copyright file="BetType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
// </history>

namespace Apollo.CustSetting.ViewData
{
    /// <summary>
    /// Class name.
    /// </summary>
    public class BetType
    {
        public BetType()
        {
            Pt = new PositionTaking();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string ResourceKey { get; set; }

        public PositionTaking Pt { get; set; }

        public int? MinWidth { get; set; }
    }
}