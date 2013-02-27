using System;
using System.Collections.Generic;
using System.Web;

using Sunplus.Agent.Utilities;

/// <summary>
/// Summary description for CustomerDetail
/// </summary>
namespace DAL
{
    [Serializable]
    public class CSCustomerDetail
    {
        private int _IBCCustID;
        private int _CSCustID;
        private string _UserPwd;
        private bool _IsEnablePT = true;
        private double _MaxWin;
        private double _MaxLose;
        private double _MinCreditBase;
        private double _MaxTransferCredit;
        private bool _IsMaxWin;
        private bool _IsMaxLose;
        private bool _PTByGameKind;
        private bool _PTByGameType;
        private bool _PTByGameTypeUpline;
        private double _TotalWinLoss;
        private int _RoleId;
        private int _UpperCustID;
        private bool _NoCSData = true;
        private bool _NoIBCData = true;

        private string _UserName2;
        private string _UserName;
        private string _FirstName;
        private string _LastName;
        private string _Phone;
        private string _MobilePhone;
        private string _Email;
        private int _LangID;
        private int _ExchangeID;
        private string _CurrencyName;
        private int _CountryID;
        private string _Fax;
        private bool _Closed = false;
        private bool _Suspend = false;
        private int _SIBCCustID;
        private int _MIBCCustID;
        private int _AIBCCustID;
        private int _Currency;

        private bool _CasinoSync = false;
        private bool _CasinoDisabled = false;

        private bool _LiveCasinoDisabled = false;
        private bool _LiveCasinoSync = false;

        private bool _BingoSync = false;
        private bool _BingoDisabled = false;

        public float GivenCredit = 0;
        public bool IsMultipleEdit = false;
        public bool IsAutoReset = false;
        public bool IsIncludeTodayWL = false;

        private double _MinTransferCreditLimit = 0;

        public double MinTransferCreditLimit
        {
            get {
                return _MinTransferCreditLimit;
            }
            set {
                _MinTransferCreditLimit = value;
            }
        }

        private double _MaxTransferCreditLimit = 0;

        public double MaxTransferCreditLimit
        {
            get
            {
                return _MaxTransferCreditLimit;
            }
            set
            {
                _MaxTransferCreditLimit = value;
            }
        }

        private double _DefaultTransferCredit = 0;
        public double DefaultTransferCredit
        {
            get
            {
                return _DefaultTransferCredit;
            }
            set
            {
                _DefaultTransferCredit = value;
            }
        }

        public string CurrencyName
        {
            get { return _CurrencyName; }
            set { _CurrencyName = value; }
        }


        public string UserPwd
        {
            get { return _UserPwd; }
            set { _UserPwd = value; }
        }

        public bool CasinoDisabled
        {
            get { return _CasinoDisabled; }
            set { _CasinoDisabled = value; }
        }

        public bool CasinoSync
        {
            get { return _CasinoSync; }
            set { _CasinoSync = value; }
        }

        public bool LiveCasinoDisabled
        {
            get { return _LiveCasinoDisabled; }
            set { _LiveCasinoDisabled = value; }
        }

        public bool LiveCasinoSync
        {
            get { return _LiveCasinoSync; }
            set { _LiveCasinoSync = value; }
        }

        public bool BingoDisabled
        {
            get { return _BingoDisabled; }
            set { _BingoDisabled = value; }
        }

        public bool BingoSync
        {
            get { return _BingoSync; }
            set { _BingoSync = value; }
        }

        public int Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public string UserName2
        {
            get { return _UserName2; }
            set { _UserName2 = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        public string MobilePhone
        {
            get { return _MobilePhone; }
            set { _MobilePhone = value; }
        }
        public int LangID
        {
            get { return _LangID; }
            set { _LangID = value; }
        }
        public int ExchangeID
        {
            get { return _ExchangeID; }
            set { _ExchangeID = value; }
        }
        public int CountryID
        {
            get { return _CountryID; }
            set { _CountryID = value; }
        }
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        public bool Closed
        {
            get { return _Closed; }
            set { _Closed = value; }
        }
        public bool Suspend
        {
            get { return _Suspend; }
            set { _Suspend = value; }
        }
        public int SIBCCustID
        {
            get { return _SIBCCustID; }
            set { _SIBCCustID = value; }
        }
        public int MIBCCustID
        {
            get { return _MIBCCustID; }
            set { _MIBCCustID = value; }
        }
        public int AIBCCustID
        {
            get { return _AIBCCustID; }
            set { _AIBCCustID = value; }
        }
        

        public CSCustomerDetail()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public int IBCCustID
        {
            get { return _IBCCustID; }
            set { _IBCCustID = value; }
        }

        public int CSCustID
        {
            get { return _CSCustID; }
            set { _CSCustID = value; }
        }

        public bool IsEnablePT
        {
            get { return _IsEnablePT; }
            set { _IsEnablePT = value; }
        }

        public double MaxWin
        {
            get { return _MaxWin; }
            set { _MaxWin = value; }
        }

        public double MaxLose
        {
            get { return _MaxLose; }
            set { _MaxLose = value; }
        }

        public double MinCreditBase
        {
            get { return _MinCreditBase; }
            set { _MinCreditBase = value; }
        }

        public double MaxTransferCredit
        {
            get { return _MaxTransferCredit; }
            set { _MaxTransferCredit = value; }
        }

        public bool IsMaxWin
        {
            get { return _IsMaxWin; }
            set { _IsMaxWin = value; }
        }

        public bool IsMaxLose
        {
            get { return _IsMaxLose; }
            set { _IsMaxLose = value; }
        }

        public bool PTByGameKind
        {
            get { return _PTByGameKind; }
            set { _PTByGameKind = value; }
        }

        public bool PTByGameType
        {
            get { return _PTByGameType; }
            set { _PTByGameType = value; }
        }

        public bool PTByGameTypeUpline
        {
            get { return _PTByGameTypeUpline; }
            set { _PTByGameTypeUpline = value; }
        }

        public double TotalWinLoss
        {
            get { return _TotalWinLoss; }
            set { _TotalWinLoss = value; }
        }

        public int RoleId
        {
            get { return _RoleId; }
            set { _RoleId = value; }
        }

        public int UpperCustID
        {
            get { return _UpperCustID; }
            set { _UpperCustID = value; }
        }

        public bool NoCSData
        {
            get { return _NoCSData; }
            set { _NoCSData = value; }
        }

        public bool NoIBCData
        {
            get { return _NoIBCData; }
            set { _NoIBCData = value; }
        }
    }
}
