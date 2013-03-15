namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ShadowAccount
    {
        private int _AgentUserId;
        private string _UserName;
        private bool _IsMaster;
        private string _PageAccess;
        private DateTime _CreateDate;
        private bool _Closed;
        private int _Creator;
        private int _LoginCount;

        public int AgentUserId
        {
            set
            {
                _AgentUserId = (int)value;
            }
            get
            {
                return _AgentUserId;
            }
        }

        public string Username
        {
            set
            {
                _UserName = value;
            }
            get
            {
                return _UserName;
            }
        }

        public bool IsMaster
        {
            set
            {
                _IsMaster = (bool)value;
            }
            get
            {
                return _IsMaster;
            }
        }

        public string PageAccess
        {
            set
            {
                _PageAccess = value;
            }
            get
            {
                return _PageAccess;
            }
        }

        public DateTime CreateDate
        {
            set
            {
                _CreateDate = (DateTime)value;
            }
            get
            {
                return _CreateDate;
            }
        }

        public bool Closed
        {
            set
            {
                _Closed = (bool)value;
            }
            get
            {
                return _Closed;
            }
        }

        public int Creator
        {
            set
            {
                _Creator = (int)value;
            }
            get
            {
                return _Creator;
            }
        }

        public int LoginCount
        {
            set
            {
                _LoginCount = (int)value;
            }
            get
            {
                return _LoginCount;
            }
        }
    }
}