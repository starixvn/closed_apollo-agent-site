using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apollo.Age.Account.ViewModel
{
    public class CustomerInfoViewModel
    {
        public int CurrentAgentId { get; set; }

        public bool IsMember { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public short Level { get; set; }

        public bool HasAgentDownline { get; set; }

        public bool HasMemberDownline { get; set; }

        public string Group { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }
    }
}