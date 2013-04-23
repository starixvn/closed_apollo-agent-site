using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using Sunplus.Agent.Utilities;

namespace DAL
{
    public abstract class IBCCustomerProvider
    {
        private static IBCCustomerProvider _Instance = null;

        public static IBCCustomerProvider Instance
        {
            get
            {
                if (null == _Instance)
                {
                    _Instance = (IBCCustomerProvider)Activator.CreateInstance(Type.GetType(Globals.IBCSetting.IBCCustomer.ProviderType));
                }
                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }


        public virtual bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS)
        {
            return false;
        }
        public virtual bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline)
        {
            return false;
        }
        public virtual CSCustomerDetail GetCustInfoIBC(string custid, Hashtable r)
        {
            return null;
        }
        public virtual CSCustomerDetail GetCustInfoIBC(int _IBCCustId)
        {
            return null;
        }
    }
}
