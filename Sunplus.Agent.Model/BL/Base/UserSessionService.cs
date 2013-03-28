/*
 * Created 20091229@Lee - User session service.
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.Model.BL
{
    using Apollo.Age.Entities;
    using Sunplus.Agent.Model.Entities;

    public sealed class UserSessionService
    {
        /// <summary>
        /// Get user session from current session id.
        /// </summary>
        /// <param name="netSessionId"></param>
        /// <returns></returns>
        public static IUserSession GetUserSessionInstance(string netSessionId)
        {
            return new UserSessionBase(netSessionId) as IUserSession;
        }

        /// <summary>
        /// Remove an user session.
        /// </summary>
        /// <param name="userSession"></param>
        public static void RemoveUserSession(IUserSession userSession)
        {
            if (null == userSession)
            {
                return;
            }

            UserSessionBase session = userSession as UserSessionBase;
            session.Dispose();
        }

        /// <summary>
        /// Create an empty user session.
        /// </summary>
        /// <param name="netSessionId"></param>
        /// <returns></returns>
        internal static IUserSession CreateUserSessionInstance(string netSessionId)
        {
            return new UserSessionBase(netSessionId) as IUserSession;
        }

        /// <summary>
        /// Create a user session with a session id and values.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="netSessionId"></param>
        /// <returns></returns>
        internal static IUserSession CreateUserSessionInstance(User user, string netSessionId)
        {
            UserSessionBase session = new UserSessionBase(netSessionId);

            SetUserSessionValues(session, user);

            return session as IUserSession;
        }

        /// <summary>
        /// Init user session from a hash table of values:
        /// int custid,
        /// string userName,
        /// string nickname,
        /// int subaccid,
        /// string subaccname,
        /// string subpageaccess,
        /// string sitename,
        /// string site,
        /// string currency,
        /// int currencyid,
        /// int roleid,
        /// int serverid,
        /// int adminid,
        /// string accusername,
        /// string loginip,
        /// bool isallowhrcurrency,
        /// bool iscasinodisabled,
        /// bool iscasinosync,
        /// bool disablenumber,
        /// int on_userid,
        /// bool disablehorseracing,
        /// bool disablefi,
        /// bool disableautompt,
        /// int srecommend,
        /// bool isadmincanviewvnip,
        /// bool isallowedtotalbalancetransfer,
        /// bool IsLogNickname
        /// </summary>
        /// <param name="userSession"></param>
        /// <param name="values"></param>
        public static void SetUserSessionValues(IUserSession userSession, User authenticatedUser)
        {
            if (ReferenceEquals(userSession, null) || ReferenceEquals(authenticatedUser, null))
            {
                return;
            }

            UserSessionBase session = userSession as UserSessionBase;

            session.AgentId = authenticatedUser.Agent.Id;
            session.AgentLevel = authenticatedUser.Agent.Level;
            session.AgentName = authenticatedUser.Agent.Username;
            session.CurrencyId = authenticatedUser.Agent.Currency;
            session.UserId = authenticatedUser.Id;
            session.Username = authenticatedUser.Username;
            session.IsUserMaster = authenticatedUser.IsMaster;
            session.MatchLimit = authenticatedUser.Agent.MatchLimit;
            session.VirtualMultiplier = authenticatedUser.Agent.VirtualMultiplier;
            session.DoubleCommLosing = authenticatedUser.Agent.DoubleCommLosing;
            session.SubPageAccess = (PageAccess)(authenticatedUser.PageAcess as string);
            session.IsLogged = true;

            // temporarily use when permission checking is pending
            session.SubPageAccess.SetFullPermission();


            //session.CustId = (int)values["agentid"];
            //session.UserName = (values["username"] as string).ToUpperInvariant().Trim();
            //session.Currency = values["currency"] as string;
            //session.AgentLevel = (short)values["agentlevel"];
            //session.IsUserMaster = (bool)values["ismaster"];
            //session.RoleId = (byte)(short)values["agentlevel"];
            //session.IsLogged = true;

            ////session.SubUserName = values["subaccname"] as string;
            //session.SubPageAccess = (PageAccess)(values["PageAccess"] as string);
            //if (!session.IsSubAcc)
            //{
            //    session.SubPageAccess.SetFullPermission();
            //}

            //session.SiteName = values["sitename"] as string;
            //session.Site = values["site"] as string;

            //session.CurrencyId = (int)values["currencyid"];
            //session.RoleId = (byte)(int)values["roleid"];
            //session.ServerId = (int)values["serverid"];

            //session.AdminId = (int)values["adminid"];
            //session.AdminName = values["accusername"] as string;

            //session.IsLogged = session.IsSubAcc || session.IsInternal || !string.IsNullOrEmpty(values["loginip"] as string);

            //session.IsAllowHRCurrency = (bool)values["isallowhrcurrency"];

            //session.IsCasinoDisabled = (bool)values["iscasinodisabled"];
            //session.IsCasinoSync = (bool)values["iscasinosync"];

            //if (values.Contains("disablep2p"))
            //{
            //    session.IsP2PDisabled = (bool)values["disablep2p"];
            //}

            //if (values.Contains("disablelivecasino"))
            //{
            //    session.IsLiveCasinoDisabled = (bool)values["disablelivecasino"];
            //}

            //if (values.Contains("synclivecasino"))
            //{
            //    session.IsLiveCasinoSync = (bool)values["synclivecasino"];
            //}

            //if (values.Contains("doublecommlosing"))
            //{
            //    session.IsDoubleCommLosing = (bool)values["doublecommlosing"];
            //}

            //if (values.Contains("disablevirtualsports"))
            //{
            //    session.IsDisabledVirtualSports = (bool)values["disablevirtualsports"];
            //}

            //if (values.Contains("hidevirtualsports"))
            //{
            //    session.IsHideVirtualSports = (bool)values["hidevirtualsports"];
            //}

            //if (values.Contains("nickname"))
            //{
            //    session.NickName = (values["nickname"] + string.Empty).ToUpperInvariant();
            //}

            //if (values.Contains("nicknameloginonly"))
            //{
            //    session.IsNicknameLoginOnly = string.IsNullOrEmpty(values["nicknameloginonly"].ToString()) ? false : (bool)values["nicknameloginonly"];
            //}

            //if (values.Contains("isnickname"))
            //{
            //    session.IsNickname = string.IsNullOrEmpty(values["isnickname"].ToString()) ? false : (bool)values["isnickname"];
            //}

            //session.DisplayNickName = session.IsNickname ? session.NickName : session.UserName;

            //if (values.Contains("disablenumber"))
            //{
            //    session.IsNumberGameDisabled = (bool)values["disablenumber"];
            //}

            //session.OnUserId = (int)values["on_userid"];

            // Extra informations
            //if (values.Contains("disable151_41"))
            //{
            //    session.IsDisable151_31 = (bool)values["disable151_31"];
            //    session.IsDisable151_41 = (bool)values["disable151_41"];
            //    session.IsDisable151_51 = (bool)values["disable151_51"];
            //    session.IsDisable152_41 = (bool)values["disable152_41"];
            //    session.IsDisable152_51 = (bool)values["disable152_51"];
            //    session.IsDisable153_51 = (bool)values["disable153_51"];
            //}

            //// Will be removed when Racing was launched
            //if (values.Contains("disablehorseracing"))
            //{
            //    session.IsDisabledHR = (bool)values["disablehorseracing"];
            //}

            //session.IsDisabledFI = (bool)values["disablefi"];
            //session.IsDisabledAutoMPT = (bool)values["disableautompt"];
            //session.SRecommendId = (int)values["srecommend"];
            //session.MRecommendId = (int)values["mrecommend"];
            //session.ARecommendId = (int)values["arecommend"];
            //session.IsAdminCanViewVNIP = (bool)values["isadmincanviewvnip"];
            //session.IsAllowedTotalBalanceTransfer = (bool)values["isallowedtotalbalancetransfer"];
            //session.IsShowConsolidatedReport = (bool)values["isshowconsolidated"];
        }
    }
}