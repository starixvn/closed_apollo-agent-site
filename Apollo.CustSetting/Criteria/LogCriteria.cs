// --------------------------------------------------------------------------
// <copyright file="LogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.11">Created</change>
// </history>

namespace Apollo.CustSetting.Criteria
{
    using Apollo.CustSetting.Core;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Criteria information.
    /// </summary>
    public class LogCriteria : CriteriaBase
    {
        public LogCriteria()
        {
            Language = Constant.DEFAULT_LANGUAGE;
        }

        public string IP { get; set; }

        public string Language { get; set; }

        /// <summary>
        /// Get Setting key.
        /// </summary>
        /// <returns>Current setting key.</returns>
        public override string GetSettingKey()
        {
            return string.Empty;
        }
    }
}