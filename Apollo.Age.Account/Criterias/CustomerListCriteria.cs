// --------------------------------------------------------------------------
// <copyright file="CustomerListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>
namespace Apollo.Age.Account.Criterias
{
    using System.Configuration;
    using Fanex.NAP.Framework.Data;

    public class CustomerListCriteria : CriteriaBase
    {
        private string _username = string.Empty;

        public int AgentId { get; set; }

        public int PageIndex { get; set; }

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

                int.TryParse(ConfigurationManager.AppSettings["PageSizeMemberList"].ToString(), out pageSize);

                return pageSize;
            }
        }

        public int Status { get; set; }

        [SpParam(Ignored = true)]
        public int OldPageIndex { get; set; }

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
        public byte CustomerSettingPolicy { get; set; }

        [SpParam(Ignored = true)]
        public bool HasDownlineDetailsPermission { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Customer_GetByAgent";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}