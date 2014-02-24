// --------------------------------------------------------------------------
// <copyright file="OutstandingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.2.24">Created</change>
// </history>

namespace Apollo.Age.BetList.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Out standing bet list criteria class.
    /// </summary>
    public class OutstandingCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public int CustId { get; set; }

        public string Sporttypes { get; set; }

        public string Bettypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelRunning";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}