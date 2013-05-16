
namespace Apollo.Age.Account.Models
{
    using System;

    public class AgentLogLoginModel
    {
        public Int64 Logid { get; set; }      

        public string Ip { get; set; }

        public DateTime LogDate { get; set; }

        public int ActionId { get; set; }

        public string Doing { get; set; }

        public string DomainName { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }
    }
}