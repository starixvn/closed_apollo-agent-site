// --------------------------------------------------------------------------
// <copyright file="AcountInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.22">Created</change>
// </history>

namespace Apollo.Age.Criterias.Accounts
{
    using Fanex.NAP.Framework.Data;

    public class AcountInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public bool IsMember { get; set; }

        public int SearchId { get; set; }

        public string SearchName { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Age_Customer_GetAllInfo";
        }
    }
}
