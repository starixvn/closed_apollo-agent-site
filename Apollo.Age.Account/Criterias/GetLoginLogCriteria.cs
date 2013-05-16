using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;

namespace Apollo.Age.Account.Criterias
{
    public class GetLoginLogCriteria : CriteriaBase
    {
        public int AgentId { get; set; }
        public int AgentUserId { get; set; }

        public override string GetSettingKey()
        {
            return "ReadLog_Login";
        }
        public override bool IsValid()
        {
            return AgentId > 0 || AgentUserId > 0;
        }
    }
}
