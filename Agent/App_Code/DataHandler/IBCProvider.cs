using System;
using System.Collections.Generic;
using System.Web;
using Sunplus.Agent.Utilities;

namespace DAL
{
    public static class IBCProvider
    {
        public static CasinoProvider Casino
        {
            get
            {
                return CasinoProvider.Instance;
            }
        }

        public static IBCCustomerProvider IBCCustomer
        {
            get
            {
                return IBCCustomerProvider.Instance;
            }
        }
        
    }

}

