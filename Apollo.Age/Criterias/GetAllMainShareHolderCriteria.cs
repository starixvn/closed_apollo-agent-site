// --------------------------------------------------------------------------
// <copyright file="GetAllMainShareHolderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.25">Created</change>
// </history>


namespace Apollo.Age.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetAllMainShareHolderCriteria : CriteriaBase
    {
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
