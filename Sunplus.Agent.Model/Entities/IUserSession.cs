/*
 * Created 20091229@Lee - User session interface
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using Sunplus.Agent.Model.Entities.Base;

    public interface IUserSession : IEntity, ISerializable
    {
        #region Apollo-Session
     
        int AgentId { get; set; }

        short AgentLevel { get; set; }

        string AgentName { get; set; }

        byte CurrencyId { get; set; }

        int UserId { get; set; }   

        string Username { get; set; }         

        bool IsUserMaster { get; set; }

        decimal MatchLimit { get; set; }

        decimal VirtualMultiplier { get; set; }

        bool DoubleCommLosing { get; set; }


        #endregion Apollo-Session

        #region Misc

        /// <summary>
        /// Default: en-US
        /// </summary>
        string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Values: E,J,CT,CS,TH,KO.
        /// </summary>
        string LangKey
        {
            get;
            set;
        }

        CultureInfo Culture
        {
            get;
        }

        /// <summary>
        /// Current captchar in login page.
        /// </summary>
        string CaptchaCode
        {
            get;
            set;
        }

        /// <summary>
        /// True that mean current currency is in set: RM,SD$,RMB,HK$,US$,EURO,TB,THB,IDD,IDR,IN,IND.
        /// </summary>
        bool IsBelongTo9Currency
        {
            get;
            set;
        }

        #endregion Misc

        #region Logged-in informations

        /// <summary>
        /// Id of admin account (logging from accounting site).
        /// </summary>
        int AdminId
        {
            get;
        }

        /// <summary>
        /// User name of admin account (logging from accounting site).
        /// </summary>
        string AdminName
        {
            get;
        }

        /// <summary>
        /// Id of main logged-in account.
        /// Return 0 if it is not available.
        /// </summary>

        [Obsolete("use agentId instead")]
        int CustId
        {
            get;
        }

        /// <summary>
        /// Role id of main logged-in account.
        /// Return 0 if it is not available.
        /// </summary>
        byte RoleId
        {
            get;
        }

        /// <summary>
        /// Server id of main logged-in account.
        /// Return 0 if it is not available.
        /// </summary>
        int ServerId
        {
            get;
        }

        ///// <summary>
        ///// User name of main logged-in account.
        ///// </summary>
        //string Username
        //{
        //    get;
        //}

        /// <summary>
        /// Nick name of main logged-in account.
        /// </summary>
        string NickName
        {
            get;
            set;
        }

        /// <summary>
        /// DisplayNickname of main logged-in account.
        /// </summary>
        string DisplayNickName
        {
            get;
            set;
        }

        /// <summary>
        /// Return 0 if it is not available.
        /// </summary>
        bool IsNicknameLoginOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Return 0 if it is not available.
        /// </summary>
        bool IsNickname
        {
            get;
            set;
        }

        /// <summary>
        /// Logged-in indentify on OnUser database, use for ReCheckin and Kichout feature.
        /// Return 0 if it is not available.
        /// </summary>
        int OnUserId
        {
            get;
        }

        /// <summary>
        /// Id of logged-in sub-account.
        /// If logged-in account is main, return 0.
        /// </summary>
        int SubAccId
        {
            get;
        }

        /// <summary>
        /// True if logged-in is sub-account.
        /// </summary>
        bool IsSubAcc
        {
            get;
        }

        /// <summary>
        /// True if logged-in is administrator.
        /// </summary>
        bool IsInternal
        {
            get;
        }

        /// <summary>
        /// 12bet,789y,indo...
        /// </summary>
        string Site
        {
            get;
        }

        /// <summary>
        /// Logged-in sub-account access.
        /// If logged-in account is main, return null.
        /// </summary>
        PageAccess SubPageAccess
        {
            get;
        }

        /// <summary>
        /// User name of logged-in sub-account.
        /// If logged-in account is main, return null.
        /// </summary>
        string SubUserName
        {
            get;
        }

        /// <summary>
        /// Current site name: Agent, Master, Super.
        /// </summary>
        string SiteName
        {
            get;
        }

        /// <summary>
        /// User is using system: AdminId if logged-in user is administrator, otherwise is CustId.
        /// </summary>
        //int UserId
        //{
        //    get;
        //}

        /// <summary>
        /// Current currency name.
        /// </summary>
        string Currency
        {
            get;
        }

        ///// <summary>
        ///// Current currency id.
        ///// </summary>
        //int CurrencyId
        //{
        //    get;
        //}

        /// <summary>
        /// False that mean first logged-in time.
        /// </summary>
        bool IsLogged
        {
            get;
            set;
        }

        /// <summary>
        /// True indicate admin user can view VN ip.
        /// </summary>
        bool IsAdminCanViewVNIP
        {
            get;
        }

        /// <summary>
        /// True indicate current user is agent
        /// </summary>
        bool IsAgent
        {
            get;
        }

        /// <summary>
        /// True indicate current user is agent
        /// </summary>
        bool IsMaster
        {
            get;
        }

        /// <summary>
        /// True indicate current user is super
        /// </summary>
        bool IsSuper
        {
            get;
        }

        /// <summary>
        /// True indicate current user is group header
        /// </summary>
        bool IsShowConsolidatedReport
        {
            get;
            set;
        }

        #endregion Logged-in informations

        #region Casino

        /// <summary>
        /// Disabled casino status of logged-in user.
        /// </summary>
        bool IsCasinoDisabled
        {
            get;
            set;
        }

        /// <summary>
        /// Synchronized casino status of logged-in user.
        /// </summary>
        bool IsCasinoSync
        {
            get;
            set;
        }

        /// <summary>
        /// True that mean current currency is casino currency.
        /// </summary>
        bool IsNotCurrencyCasino
        {
            get;
            set;
        }

        #endregion Casino

        #region P2P

        bool IsP2PDisabled
        {
            get;
            set;
        }

        #endregion P2P

        #region LiveCasino

        bool IsLiveCasinoDisabled
        {
            get;
            set;
        }

        bool IsLiveCasinoSync
        {
            get;
            set;
        }

        #endregion LiveCasino

        #region VirtualSports

        bool IsHideVirtualSports
        {
            get;
            set;
        }

        bool IsDisabledVirtualSports
        {
            get;
            set;
        }

        #endregion VirtualSports

        #region Double Commission

        bool IsDoubleCommLosing
        {
            get;
            set;
        }

        #endregion Double Commission

        #region Number Game
        
        /// <summary>
        /// Disabled Number Game status of logged-in user.
        /// </summary>
        bool IsNumberGameDisabled
        {
            get;
        }

        #endregion Number Game

        #region Will be removed when Racing was launched

        bool IsDisabledHR
        {
            get;
        }

        bool IsDisabledFI
        {
            get;
        }

        #endregion Will be removed when Racing was launched

        #region Racing

        bool IsAllowHRCurrency
        {
            get;
        }

        /// <summary>
        /// HR-SA
        /// </summary>
        bool IsDisable151_31
        {
            get;
        }

        /// <summary>
        /// HR-UK
        /// </summary>
        bool IsDisable151_41
        {
            get;
        }

        /// <summary>
        /// HR-AU
        /// </summary>
        bool IsDisable151_51
        {
            get;
        }

        /// <summary>
        /// Greyhound-UK
        /// </summary>
        bool IsDisable152_41
        {
            get;
        }

        /// <summary>
        /// Greyhound-AU
        /// </summary>
        bool IsDisable152_51
        {
            get;
        }

        /// <summary>
        /// Harness-AU
        /// </summary>
        bool IsDisable153_51
        {
            get;
        }

        #endregion Racing

        #region PostionTaking

        bool IsDisabledAutoMPT
        {
            get;
        }

        #endregion PostionTaking

        #region UplineInformations

        /// <summary>
        /// Super Id. Return 0 if it is not available.
        /// </summary>
        int SRecommendId
        {
            get;
        }

        /// <summary>
        /// Master Id. Return 0 if it is not available.
        /// </summary>
        int MRecommendId
        {
            get;
        }

        /// <summary>
        /// Agent Id. Return 0 if it is not available.
        /// </summary>
        int ARecommendId
        {
            get;
        }

        #endregion UplineInformations

        #region TotalBalanceTransfer

        /// <summary>
        /// Under supers which allow transfer on Total Balance.
        /// </summary>
        bool IsAllowedTotalBalanceTransfer
        {
            get;
        }

        #endregion TotalBalanceTransfer

        /// <summary>
        /// Current session id.
        /// </summary>
        string NetSessionId
        {
            get;
        }
    }
}