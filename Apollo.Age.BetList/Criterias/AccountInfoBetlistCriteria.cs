// --------------------------------------------------------------------------
// <copyright file="AccountInfoBetlistCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.2.21">Created</change>
// </history>

namespace Apollo.Age.BetList.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Account information bet list criteria class.
    /// </summary>
    public class AccountInfoBetlistCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime Todate { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelByMem";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}