

namespace Apollo.Age.Wagers.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    public class GetEventDateCriteria : CriteriaBase
    {
        public string Sporttypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Wagers_SelDateBySports";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
