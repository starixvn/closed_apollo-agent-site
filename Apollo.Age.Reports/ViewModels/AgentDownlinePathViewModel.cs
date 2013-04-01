namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AgentDownlinePathViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public Int16 AgentLevel { get; set; }

        public int ParentId { get; set; }

        public bool Enabled { get; set; }
    }
}