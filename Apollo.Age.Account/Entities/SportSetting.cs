using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apollo.Age.Account.Entities
{
    public class SportSetting
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public bool IsMixSportParlay { get; set; }

        public string ResourceKey { get; set; }

        public List<BetTypeSetting> BetTypeList { get; set; }
    }
}
