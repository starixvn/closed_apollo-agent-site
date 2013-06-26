namespace Apollo.Age.Account.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    using System.Configuration;

    public class CustomerListCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public string UserName { get; set; }

        public int PageSize
        {
            get
            {
                int pageSize = 0;

                int.TryParse(ConfigurationManager.AppSettings["PageSizeAgentMemberList"].ToString(), out pageSize);

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
                return TotalItems % PageSize == 0 ? TotalItems / PageSize : TotalItems / PageSize + 1;
            }
        }

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