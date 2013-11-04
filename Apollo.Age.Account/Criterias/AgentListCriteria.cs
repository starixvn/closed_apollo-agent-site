// --------------------------------------------------------------------------
// <copyright file="AgentListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.26">Add parameter for Singbet</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using System.Configuration;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Agent list criteria.
    /// </summary>
    public class AgentListCriteria : CriteriaBase
    {
        private string _username = string.Empty;

        public int AgentId { get; set; }

        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int OldPageIndex { get; set; }

        public int TotalItems { get; set; }

        public string UserName
        {
            get
            {
                return _username != null ? _username.Trim() : string.Empty;
            }

            set
            {
                _username = value;
            }
        }

        public int PageSize
        {
            get
            {
                int pageSize = 0;

                int.TryParse(ConfigurationManager.AppSettings["PageSizeAgentList"].ToString(), out pageSize);

                return pageSize;
            }
        }

        public int Status { get; set; }

        [SpParam(Ignored = true)]
        public int OrderBy { get; set; }

        [SpParam(Ignored = true)]
        public int OrderType { get; set; }

        [SpParam(Ignored = true)]
        public int ParentAgentId { get; set; }

        [SpParam(Ignored = true)]
        public int UplineAgentId { get; set; }

        [SpParam(Ignored = true)]
        public int NumPages
        {
            get
            {
                return TotalItems % PageSize == 0 ? TotalItems / PageSize : (TotalItems / PageSize) + 1;
            }
        }

        [SpParam(Ignored = true)]
        public byte PermissionControlPolicy { get; set; }

        [SpParam(Ignored = true)]
        public byte ShadowPermissionPolicy { get; set; }

        [SpParam(Ignored = true)]
        public byte CustomerSettingPolicy { get; set; }

        [SpParam(Ignored = true)]
        public bool HasDownlineDetailsPermission { get; set; }

        /// <summary>
        /// Get setting key of agent list.
        /// </summary>
        /// <returns>Return setting key Age_Agent_GetByAgent.</returns>
        public override string GetSettingKey()
        {
            return "Age_Agent_GetByAgent";
        }

        /// <summary>
        /// Check criteria is valid.
        /// </summary>
        /// <returns>Return true.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}