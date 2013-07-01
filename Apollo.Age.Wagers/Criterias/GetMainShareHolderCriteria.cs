// --------------------------------------------------------------------------
// <copyright file="GetMainShareHolderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.1">Created</change>
// </history>

namespace Apollo.Age.Wagers.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetMainShareHolderCriteria : CriteriaBase
    {
        public string Bettypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetAllMainShareHolder";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}