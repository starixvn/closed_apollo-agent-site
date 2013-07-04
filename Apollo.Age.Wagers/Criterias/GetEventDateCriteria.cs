// --------------------------------------------------------------------------
// <copyright file="GetEventDateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.7.4">StyleCop</change>
// </history>

namespace Apollo.Age.Wagers.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class GetEventDateCriteria : CriteriaBase
    {
        public string Sporttypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Wagers_SelDateBySports";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
