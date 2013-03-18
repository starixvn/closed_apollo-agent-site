namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AgentUserAccount
    {
        public int AgentUserId
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public bool IsMaster
        {
            get;
            set;
        }

        public string PageAccess
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }

        public bool Closed
        {
            get;
            set;
        }

        public int Creator
        {
            get;
            set;
        }

        public int LoginCount
        {
            get;
            set;
        }
    }
}