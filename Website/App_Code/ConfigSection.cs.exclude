using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using DNAide.Core;

/// <summary>
/// Summary description for ConfigSection
/// </summary>
namespace ConfigSection
{

    public class IBCAccounting : ConfigurationSection
    {
        
        [ConfigurationProperty("Casino", IsRequired = true)]
        public CasinoElements Casino
        {
            get { return (CasinoElements)base["Casino"]; }
        }
        [ConfigurationProperty("IBCCustomer", IsRequired = true)]
        public IBCCustomerElements IBCCustomer
        {
            get { return (IBCCustomerElements)base["IBCCustomer"]; }
        }
              
    }

   

    public class CasinoElements: ConfigurationElement
    {
        private bool _UseWebConfigProviderType = true;
        private string _ProviderType;
        public bool UseWebConfigProviderType
        {
            get { return _UseWebConfigProviderType; }
            set { _UseWebConfigProviderType = value; }
        }


        [ConfigurationProperty("providerType", DefaultValue = "DAL.APIClient.APICasinoProvider")]
        public string ProviderType
        {
            get 
            {
                return (_UseWebConfigProviderType) ? (string)base["providerType"] : _ProviderType; 
                    
            }
            set 
            {
                if (_UseWebConfigProviderType)
                    base["providerType"] = value;
                else
                    _ProviderType = value;
            }
        }

        [ConfigurationProperty("showBetSetting", DefaultValue = false)]
        public bool ShowBetSetting
        {
            get
            {
                return true;// CastAide.AsBoolean(base["showBetSetting"], false);

            }
            set
            {
                base["showBetSetting"] = value;
            }
        }
    }

    public class IBCCustomerElements : ConfigurationElement
    {
        [ConfigurationProperty("providerType", DefaultValue = "DAL.IBCClient.IBCLibProvider")]
        public string ProviderType
        {
            get
            {
                return (string)base["providerType"];
            }
            set
            {
                base["providerType"] = value;
            }
        }
    }

   
}

