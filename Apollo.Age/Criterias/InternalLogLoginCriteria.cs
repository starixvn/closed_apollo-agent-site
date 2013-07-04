// --------------------------------------------------------------------------
// <copyright file="InternalLogLoginCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.4">Created</change>
// </history>
namespace Apollo.Age.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class InternalLogLoginCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int AgentUserId { get; set; }

        public int ServerId { get; set; }

        public string LoginIp { get; set; }

        public int ActionId { get; set; }

        public string Doing { get; set; }

        public string DomainName { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }        

        public override string GetSettingKey()
        {
            return "WriteLog_AgentLog_Login";
        }

        public override bool IsValid()
        {
            return AgentId >= 0 && AgentUserId >= 0 && ServerId != 0 && ActionId != 0 && !string.IsNullOrWhiteSpace(Doing);
        }
    }
}