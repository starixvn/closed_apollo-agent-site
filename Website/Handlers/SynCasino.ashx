<%@ WebHandler Language="C#" Class="UpdateCSSetting" %>

using System;
using System.Data;
using System.Web;
using System.Web.SessionState;

using BLL;
using DAL;
using DNAide.Core;

using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Utilities;

using Fanex.Caching;

public class UpdateCSSetting : IHttpHandler, IReadOnlySessionState
{
    private CSCustomerDetail currentCustomer = null;
    private bool isMultiple = false;
    private string[] ids = null;
    private string[] userNames = null;
    private string[] statuses = null;
    private int custId = 0;
    private string currentUserName = null;
    private string currentStatus = null;
    private int maxTransfer = 0;
    private bool isMaxTransfer = true;
    private decimal maxWin = 0;
    private decimal maxLose = 0;
    private bool status = false;
    private string requestData = null;
    private string[] gameKinds = null;
    private int kc = 0;
    private int idx = 0;
    private bool hasError = false;
    private bool hasSuccess = false;
    private bool isSyncFailed = false;
    private string errorMessage = string.Empty;
    private bool autoReset = false;

    private ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);

    private string[] Statuses
    {
        get
        {
            if (cache.Get<string[]>("Statuses") == null) return null;
            return cache.Get<string[]>("Statuses");
        }
        set
        {
            cache.Set<string[]>("Statuses", value);
        }
    }

    private string[] Ids
    {
        get
        {
            if (cache.Get<string[]>("Ids") == null) return null;
            return cache.Get<string[]>("Ids");
        }
        set
        {
            cache.Set<string[]>("Ids", value);
        }
    }

    private string ErrorMessage
    {
        get
        {
            if (cache.Get<string>("ErrorMessage") == null) return null;
            return cache.Get<string>("ErrorMessage");
        }
        set
        {
            cache.Set<string>("ErrorMessage", value);
        }
    }

    /// <summary>
    /// Check 2 arrays of string is equal or not
    /// </summary>
    /// <param name="id1"></param>
    /// <param name="id2"></param>
    /// <returns></returns>
    private bool IsValid(string[] id1, string[] id2)
    {
        if (id1 == null || id2 == null || id1.Length != id2.Length) return false;
        int c = id1.Length;
        for (int i = 0; i < c; i++)
        {
            if (id1[i] != id2[i]) return false;
        }
        return true;
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetNoStore();
        HttpRequest request = context.Request;

        if (!UserSession.Current.SubPageAccess.UpdateAccount)
        {
            context.Response.Write("<script type='text/javascript'>parent.showErrMsg('" + PositionTaking.ReplaceData(Resources.Lang.lblNotAuthorized) + "');</script>");
            return;
        }

        if (UserSession.Current.AdminId != 0)
        {
            context.Response.Write("<script type='text/javascript'>parent.showErrMsg('" + PositionTaking.ReplaceData(Resources.Lang.lblNotAuthorized) + "');</script>");
            return;
        }

        string returnString = "<script>{0}</script>"; // Return script

        // Format gameKind1[gameType1(stake1, stake2)-gameType2(stake3, stake4))];gameKind2...
        requestData = request["hidCSData"]; // Ex: "8[1(24, 25, 29, 30)-2(1, 2, 3, 4)];10[1(29, 30)-2(3, 4)];"
        if (string.IsNullOrEmpty(requestData)) return;
        gameKinds = requestData.Split(new string[] { Globals.SEP_GAMEKIND }, StringSplitOptions.RemoveEmptyEntries); // Ex: 10[1(29, 30)-2(3, 4)]
        kc = gameKinds.Length;

        if (kc == 0)
        {
            context.Response.Write("BAD POST DUE TO UNKNOWN DATA");
            return;
        }

        currentCustomer = CasinoManager.GetCustInfoIBC(CastAide.AsInt32(request["custid"]));
        custId = currentCustomer.IBCCustID;
        currentUserName = currentCustomer.UserName;
        currentStatus = (currentCustomer.CasinoSync) ? "1" : "0"; // "0" indicate that has not been synchronized yet

        autoReset = request["cbxAutoReset"] != null;
        isMultiple = (request["bIsMult"] == "1");

        if (isMultiple)
        {
            ids = request["custids"].Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
            userNames = request["arrayUserName"].Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValid(ids, this.Ids)) // 2 arrays not equal, refresh session
            {
                statuses = request["arrayStatus"].Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                this.Statuses = statuses;
            }
            else
            {
                statuses = this.Statuses;
            }
        }

        // Check max transfer for edit member
        if (!(UserSession.Current.RoleId == 4 && currentCustomer.RoleId == 2) && !(UserSession.Current.RoleId >= 3 && currentCustomer.RoleId == 1) && isMaxTransfer)
        {
            if (!int.TryParse("0" + request[Globals.TXT_MAXTRANSTERCREDIT], out maxTransfer))
            {
                context.Response.Write(@"<script>parent.showErrMsg('" + FormatMessage(Resources.Lang.maxTransferInvalid) + "'); parent.$('" + Globals.TXT_MAXTRANSTERCREDIT + "').focus(); </script>");
                return;
            }

            if (maxTransfer > (int)(currentCustomer.MaxTransferCreditLimit) || maxTransfer < (int)(currentCustomer.MinTransferCreditLimit)) // Show error
            {
                context.Response.Write(@"<script>parent.showErrMsg('" + FormatMessage(Resources.Lang.transferCreditMsg) + "');parent.$('" + Globals.TXT_MAXTRANSTERCREDIT + "').focus();</script>");
                return;
            }
        }

        bool indirectUpdate = (UserSession.Current.RoleId >= 3 && currentCustomer.RoleId == 1);

        if (!isMultiple)
        {
            if (indirectUpdate)
            {
                this.IndirectUpdate(request);
            }
            else if (!(UserSession.Current.RoleId == 4 && currentCustomer.RoleId == 2))
            {
                this.DirectUpdate(request);
            }
        }
        else
        {
            int c = ids.Length;
            for (idx = 0; idx < c; idx++)
            {
                this.custId = int.Parse(ids[idx]);
                this.currentUserName = userNames[idx];
                this.currentStatus = statuses[idx];

                // For loop
                if (indirectUpdate)
                {
                    this.IndirectUpdate(request);
                }
                else if (!(UserSession.Current.RoleId == 4 && currentCustomer.RoleId == 2))
                {
                    if (idx != 0 || currentStatus != "1") // Get custinfo from AGE
                    {
                        currentCustomer = CasinoManager.GetCustInfoIBC(int.Parse(ids[idx]));
                    }

                    this.DirectUpdate(request);
                }
                // End for loop
            }
        }

        if (this.hasError)
        {
            context.Response.Write(@"<script>parent.showErrMsg('" + FormatMessage(errorMessage) + "', 0); </script>");

            // Reload parent window when there is any updating success
            if (hasSuccess)
            {
                context.Response.Write(@"
                                <script>
                                    parent.parent.ageWnd.onClosing = function()
                                    {
                                        parent.parent.DelayReloadPage();
                                        return true;
                                    };
                                </script>");
            }

            return;
        }
        else
        {
            if (request["insert"] != "true") // Not in new customer case
            {
                if (!indirectUpdate)
                {
                    if (this.IsNoBackground(context)) // Not from AccountInfo
                    {
                        context.Response.Write(string.Format(returnString, @"
                        if(parent && parent.parent && parent!=window && parent.parent.ageWnd)
                        {
                            parent.parent.DelayReloadPage();
                        }
                        else
                        {
                            parent.DelayReloadPage('../MemberInfo/PositionTakingCS.aspx?custid=" + currentCustomer.IBCCustID + @"&issucc=1&isupdate=true');
                        }
                        "
                        ));
                    }
                    else // From AccountInfo tab
                    {
                        context.Response.Write(String.Format(@"
                            <script>
                                parent.DelayReloadPage('../MemberInfo/PositionTakingCS.aspx?custid={0}&nobackground=1&issucc=1&isupdate=false');
                                top.menu.reloadMenuUsrInfo();
                            </script>"
                            , context.Request["custid"]));
                    }
                }
                else
                {
                    if (this.IsNoBackground(context)) // Not from AccountInfo
                    {
                        context.Response.Write(string.Format(returnString, @"
                            if(parent && parent.parent && parent!=window && parent.parent.ageWnd)
                            {
                                parent.parent.DelayReloadPage();
                            }"));
                    }
                    else // From AccountInfo tab
                    {
                        context.Response.Write(String.Format(@"
                            <script>
                                parent.DelayReloadPage('../MemberInfo/PositionTakingCS.aspx?custid={0}&nobackground=1&issucc=1&isupdate=false');
                                top.menu.reloadMenuUsrInfo();
                            </script>"
                            , context.Request["custid"]));
                    }
                }
            }
            else // In new customer case
            {
                context.Response.Write(String.Format(@"
                    <script>
                        parent.DelayReloadPage('../MemberInfo/PositionTakingCS.aspx?custid={0}&issucc=1&isupdate=true');
                    </script>"
                    , request["custid"]));
            }
        }
    }

    public void IndirectUpdate(HttpRequest request)
    {
        string error = string.Empty;
        if (!MemberInfo.CustomerCheckEnable(custId.ToString(), out error))
        {
            hasError = true;
            errorMessage += currentUserName + ": " + error + " <br />";
            return;
        }

        string roleid = currentCustomer.RoleId.ToString();
        bool ptByGameType = currentCustomer.PTByGameType;

        bool isS = UserSession.Current.RoleId.ToString() == Globals.SuperLevel.ToString();
        bool isM = UserSession.Current.RoleId.ToString() == Globals.MasterLevel.ToString();
        bool isA = UserSession.Current.RoleId.ToString() == Globals.AgentLevel.ToString();
        bool isC = UserSession.Current.RoleId.ToString() == Globals.MemberLevel.ToString();

        CasinoLibrary.SBAgent.Customer csCustomer = new CasinoLibrary.SBAgent.Customer();

        bool gameKindGroup = request[GetGameKindId(gameKinds[0]) + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CHKGROUP] != null;

        // Update cust game kind and game type
        DataTable dtGameKinds = GameKindsDataTable_Member();
        DataTable dtGameTypes = GameTypesDataTable_Member();
        DataTable dtGameTypesStake = GameTypesStakesDataTable();

        DataTable dtGameKindsReturn = null;
        DataTable dtGameTypesReturn = null;

        string currentError = null;

        try
        {
            for (int i = 0; i < kc; i++)
            {
                int gameKind = GetGameKindId(gameKinds[i]);
                bool isAuto = request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.strCHK + Globals.Super + Globals.strAutoPT] != null;
                // Update cust game kind
                dtGameKinds.Rows.Add(new object[] {
                    custId,
                    gameKind,
                    (!(isS||isM)||isAuto?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strPresetPT]),
                    (!(isM||isA)?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.strPresetPT]),
                    isAuto?"1":"0",
                    (!isAuto?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strAutoPT]),
                    UserSession.Current.RoleId,
                    Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),  UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2
                });

                // Loop and update game types
                string[] gameTypes = GetGameTypesId(gameKinds[i]);
                int tc = gameTypes.Length;
                for (int j = 0; j < tc; j++)
                { // Update stake level
                    string stakeList = string.Empty;
                    bool p2p = !gameTypes[j].Contains(Globals.OPEN_STAKELIST_MARK);
                    if (!p2p)
                    {
                        stakeList = GetStakeLevelList(gameTypes[j]);
                    }
                    int gameTypeId = GetGameTypeId(gameTypes[j]);
                    isAuto = request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.strCHK + Globals.Super + Globals.strAutoPT] != null;

                    dtGameTypesStake.Rows.Add(new object[] {
                    custId,
                    gameTypeId,
                    GetSelectedStakeLevels(stakeList, gameKind, gameTypeId),
                    UserSession.Current.RoleId,
                    Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),  UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2
                });
                }

                if (!gameKindGroup)
                {
                    // Loop and update game types
                    for (int j = 0; j < tc; j++)
                    { // Update game type

                        int gameTypeId = GetGameTypeId(gameTypes[j]);
                        isAuto = request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.strCHK + Globals.Super + Globals.strAutoPT] != null;

                        dtGameTypes.Rows.Add(new object[] {
                        custId,
                        gameTypeId,
                        (!(isS||isM)||isAuto?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strPresetPT]),
                        (!(isM||isA)?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.strPresetPT]),
                        isAuto?"1":"0",
                        (!isAuto?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strAutoPT]),
                        UserSession.Current.RoleId,
                        Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2
                    });
                    }
                }
            }

            if (dtGameKinds.Rows.Count > 0)
            {
                dtGameKindsReturn = csCustomer.UpdateCustGameKind_EditMember(dtGameKinds);
            }

            dtGameTypesReturn = csCustomer.UpdateCustGameTypeStakeLevel(dtGameTypesStake);

            if (!gameKindGroup)
            {
                dtGameTypesReturn = csCustomer.UpdateCustGameType_EditMember(dtGameTypes);
            }

            for (int i = 0; i < dtGameKindsReturn.Rows.Count; i++)
            {

                currentError += dtGameKindsReturn.Rows[i][2].ToString() + "<br />";
            }

            for (int i = 0; i < dtGameTypesReturn.Rows.Count; i++)
            {

                currentError += dtGameTypesReturn.Rows[i][2].ToString() + "<br />";
            }

        }
        catch (Exception e)
        {
            Logs.Write(e);
            currentError += Resources.Lang.lblNotAuthorized + "<br />";
        }

        if (string.IsNullOrEmpty(currentError))
        {
            hasSuccess = true;
        }
        else
        {
            hasError = true;
            errorMessage += currentUserName + ": " + currentError;
        }
    }

    public void DirectUpdate(HttpRequest request)
    {
        string strErr = string.Empty;
        if (!MemberInfo.CustomerCheckEnable(custId.ToString(), out strErr))
        {
            errorMessage += currentUserName + ": " + strErr + " <br />";
            hasError = true;
            return;
        }

        CasinoLibrary.SBAgent.Customer customer = new CasinoLibrary.SBAgent.Customer();

        bool isInserting = !customer.CheckCustSetting(custId); ; // = true indicates that it's being in synchronize case

        string roleId = (UserSession.Current.RoleId - 1).ToString();
        bool ptByGameType = currentCustomer.PTByGameType;

        bool isS = roleId == Globals.SuperLevel.ToString();
        bool isM = roleId == Globals.MasterLevel.ToString();
        bool isA = roleId == Globals.AgentLevel.ToString();
        bool isC = roleId == Globals.MemberLevel.ToString();

        if (isS)
        {
            roleId = Globals.Super;
        }
        else if (isM)
        {
            roleId = Globals.Master;
        }
        else if (isA)
        {
            roleId = Globals.Agent;
        }

        bool gameKindGroup = request[GetGameKindId(gameKinds[0]) + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CHKGROUP] != null;
        string totalError = string.Empty;

        // Upd cust setting max win, max lose
        try
        {
            #region Update Customer Status and BetSetting
            bool isDisableCasino = !(request[Globals.ID_CHECKBOX_DISABLEDCASINO] != null || request["insert"] == "true" || isInserting);
            bool isUpdateDownline = isDisableCasino || (request[Globals.ID_CHECKBOX_DISABLEDCASINO_DL] != null);

            if (UserSession.Current.IsCasinoDisabled)
            {
                isDisableCasino = true;
            }

            if (!isInserting && (!isMultiple || (isMultiple && currentStatus == "1"))) //strCurrentStatus = 1 means that already Sync
            {
                status = false;

                // Sync status enable casino and betsetting
                status = customer.UpdateMaxTransferMisc(
                     custId,
                     !isDisableCasino,
                     isUpdateDownline,
                     isMaxTransfer,
                     maxTransfer, // Max transfer credit
                     autoReset,
                     ptByGameType,
                     Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                     UserSession.Current.CustId,
                     UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName,
                     2);

                if (!status)
                {
                    hasError = true;
                    errorMessage += currentUserName + ": " + FormatMessage(Resources.Lang.updCustSettingFailed) + "<br />";
                    return;
                }
            }
            else if (!isMultiple || (isMultiple && currentStatus == "0")) // 0 = not sync yet
            {
                status = false;
                status = customer.InsertCustSetting(
                     Globals.CASINO_GROUP1,
                     custId,
                     currentCustomer.UserName,
                     currentCustomer.UserName2,
                     currentCustomer.FirstName,
                     currentCustomer.LastName,
                     currentCustomer.UserPwd,
                     currentCustomer.Phone,
                     currentCustomer.MobilePhone,
                     currentCustomer.Currency,
                     currentCustomer.Fax,
                     currentCustomer.Closed,
                     currentCustomer.Suspend,
                     false, // Default is false, later upd UpdateCustEnablePT
                     maxWin,
                     maxLose,
                     isMaxTransfer,
                     maxTransfer, // Max transfer credit
                     currentCustomer.SIBCCustID, // sid
                     currentCustomer.MIBCCustID, // mid
                     currentCustomer.AIBCCustID, // aid
                     ptByGameType,
                     (UserSession.Current.RoleId - 1).ToString(),
                     DateTime.Now,
                     Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                     UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2, autoReset
                     );

                if (!status)
                {
                    hasError = true;
                    errorMessage += currentUserName + ": " + FormatMessage(Resources.Lang.updCustSettingFailed) + "<br />";
                    return;
                }
                else
                {
                    if (isMultiple)
                    {
                        Statuses[idx] = "1"; // Already synchronized
                    }
                }
            }

            // Sync status success now, then update casino status in AGE:
            status = false;
            status = CasinoProvider.Instance.UpdateCSStatusWithoutSync(custId, true, isDisableCasino, request[Globals.ID_CHECKBOX_DISABLEDCASINO_DL] != null);
            #endregion

            #region Update Customer GameKind/Type and Position Taking
            // Update cust game kind and game type
            DataTable dtGameKinds = GameKindsDataTable();
            DataTable dtGameTypes = GameTypesDataTable();
            DataTable dtGameTypesStake = GameTypesStakesDataTable();

            DataTable dtGameKindsReturn = null;
            DataTable dtGameTypesReturn = null;

            for (int i = 0; i < kc; i++)
            {
                int gameKind = GetGameKindId(gameKinds[i]);
                bool isAuto = request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.strCHK + Globals.Super + Globals.strAutoPT] != null;
                // Update cust game kind
                dtGameKinds.Rows.Add(new object[] {
                    custId,
                    gameKind,
                    null,
                    (!(isS||isM)||isAuto?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strPresetPT]),
                    (!(isM||isA)?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.strPresetPT]),
                    (!(isA||isC)?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Agent + Globals.strPresetPT]),
                    isAuto?"1":"0",
                    (!isAuto?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strAutoPT]),
                    (!(isS||isM||isA)?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + roleId + Globals.strMinPT]),
                    0,
                    !isS?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.Comm],
                    !isM?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.Comm],
                    !isA?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Agent + Globals.Comm],
                    !isC?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Member + Globals.Comm],
                    currentCustomer.RoleId,
                    Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2
                });

                if (gameKindGroup)
                {
                    // Loop and update stake level
                    string[] gameTypes = GetGameTypesId(gameKinds[i]);
                    int tc = gameTypes.Length;
                    for (int j = 0; j < tc; j++)
                    { // Update game type
                        string stakeList = string.Empty;
                        bool p2p = !gameTypes[j].Contains(Globals.OPEN_STAKELIST_MARK);
                        if (!p2p)
                        {
                            stakeList = GetStakeLevelList(gameTypes[j]);
                        }
                        int gameTypeId = GetGameTypeId(gameTypes[j]);
                        isAuto = request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.strCHK + Globals.Super + Globals.strAutoPT] != null;

                        dtGameTypesStake.Rows.Add(new object[] {
                        custId,
                        gameTypeId,
                        GetSelectedStakeLevels(stakeList, gameKind, gameTypeId),
                        currentCustomer.RoleId,
                        Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2
                    });
                    }
                }
                else
                {
                    // Loop and update game types
                    string[] gameTypes = GetGameTypesId(gameKinds[i]);
                    int tc = gameTypes.Length;
                    for (int j = 0; j < tc; j++)
                    { // Update game type
                        string stakeList = string.Empty;
                        bool p2p = !gameTypes[j].Contains(Globals.OPEN_STAKELIST_MARK);
                        if (!p2p)
                        {
                            stakeList = GetStakeLevelList(gameTypes[j]);
                        }
                        int gameTypeId = GetGameTypeId(gameTypes[j]);
                        isAuto = request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.strCHK + Globals.Super + Globals.strAutoPT] != null;

                        dtGameTypes.Rows.Add(new object[] {
                        custId,
                        gameTypeId,
                        null,
                        (!(isS||isM)||isAuto?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strPresetPT]),
                        (!(isM||isA)?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.strPresetPT]),
                        (!(isA||isC)?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Agent + Globals.strPresetPT]),
                        isAuto?"1":"0",
                        (!isAuto?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strAutoPT]),
                        (!(isS||isM||isA)?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + roleId + Globals.strMinPT]),
                        0,
                        !isS?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.Comm],
                        !isM?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.Comm],
                        !isA?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Agent + Globals.Comm],
                        !isC?null:request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Member + Globals.Comm],
                        GetSelectedStakeLevels(stakeList, gameKind, gameTypeId),
                        currentCustomer.RoleId,
                        Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2
                    });
                    }
                }
            }

            if (dtGameKinds.Rows.Count > 0)
            {
                dtGameKindsReturn = customer.UpdateCustGameKind(dtGameKinds);
            }
            if (gameKindGroup)
            {
                dtGameTypesReturn = customer.UpdateCustGameTypeStakeLevel(dtGameTypesStake);
            }
            else
            {
                dtGameTypesReturn = customer.UpdateCustGameType(dtGameTypes);
            }

            for (int i = 0; i < dtGameKindsReturn.Rows.Count; i++)
            {
                isSyncFailed = true;
                totalError += dtGameKindsReturn.Rows[i][2].ToString() + "<br />";
            }

            int gameTypeCount = dtGameTypesReturn.Rows.Count;

            if (gameKindGroup)
            {
                for (int i = 0; i < gameTypeCount; i++)
                {
                    if (CastAide.AsBoolean(dtGameTypesReturn.Rows[i][1]).Equals(false))
                    {
                        isSyncFailed = true;
                        totalError += String.Format("{0} {1} <br>", Resources.Lang.errorupdategametype, dtGameTypesReturn.Rows[i][0]);
                    }
                }
            }
            else
            {
                isSyncFailed = (gameTypeCount > 0);
                for (int i = 0; i < gameTypeCount; i++)
                {
                    totalError += dtGameTypesReturn.Rows[i][2].ToString() + "<br />";
                }
            }

            // Rollback customer info from casino when error:
            if (isSyncFailed)
            {
                if (!currentCustomer.CasinoSync)
                {
                    status = CasinoProvider.Instance.UpdateCSStatusWithoutSync(custId, false, true, request[Globals.ID_CHECKBOX_DISABLEDCASINO_DL] != null);
                    status = customer.RemoveAccount(currentUserName, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2);
                }
            }

            else
            {
                customer.UpdateCustEnablePT(custId, true); // Enable PT
            }

            #endregion
        }
        catch (Exception e)
        {
            Logs.Write(e);
            totalError += FormatMessage(e.Message) + "<br />";

            if (!currentCustomer.CasinoSync)
            {
                status = CasinoProvider.Instance.UpdateCSStatusWithoutSync(custId, false, true, request[Globals.ID_CHECKBOX_DISABLEDCASINO_DL] != null);
                status = customer.RemoveAccount(currentUserName, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2);
            }
        }

        if (string.IsNullOrEmpty(totalError))
        {
            hasSuccess = true;
        }
        else
        {
            hasError = true;
            errorMessage += currentUserName + ": " + totalError;
        }
    }

    public DataTable GameKindsDataTable_Member()
    {
        DataTable dtGameKinds = new DataTable();
        dtGameKinds.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameKind"),
            new DataColumn("SPositionTaking"),
            new DataColumn("MPositionTaking"),
            new DataColumn("IsAutoPositionTaking"),
            new DataColumn("AutoPositionTaking"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

        return dtGameKinds;
    }

    public DataTable GameTypesDataTable_Member()
    {
        DataTable dtGameTypes = new DataTable();
        dtGameTypes.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameTypeID"),
            new DataColumn("SPositionTaking"),
            new DataColumn("MPositionTaking"),
            new DataColumn("IsAutoPositionTaking"),
            new DataColumn("AutoPositionTaking"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

        return dtGameTypes;
    }

    public DataTable GameKindsDataTable()
    {
        DataTable dtGameKinds = new DataTable();
        dtGameKinds.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameKind"),
            new DataColumn("HPositionTaking"),
            new DataColumn("SPositionTaking"),
            new DataColumn("MPositionTaking"),
            new DataColumn("APositionTaking"),
            new DataColumn("IsAutoPositionTaking"),
            new DataColumn("AutoPositionTaking"),
            new DataColumn("MinPositionTaking"),
            new DataColumn("MaxPositionTaking"),
            new DataColumn("SMAgentRollingRate"),
            new DataColumn("MAgentRollingRate"),
            new DataColumn("AgentRollingRate"),
            new DataColumn("MemberRollingRate"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

        return dtGameKinds;
    }

    public DataTable GameTypesDataTable()
    {
        DataTable dtGameTypes = new DataTable();
        dtGameTypes.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameTypeID"),
            new DataColumn("HPositionTaking"),
            new DataColumn("SPositionTaking"),
            new DataColumn("MPositionTaking"),
            new DataColumn("APositionTaking"),
            new DataColumn("IsAutoPositionTaking"),
            new DataColumn("AutoPositionTaking"),
            new DataColumn("MinPositionTaking"),
            new DataColumn("MaxPositionTaking"),
            new DataColumn("SMAgentRollingRate"),
            new DataColumn("MAgentRollingRate"),
            new DataColumn("AgentRollingRate"),
            new DataColumn("MemberRollingRate"),
            new DataColumn("StakeLevelList"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

        return dtGameTypes;
    }

    public DataTable GameTypesStakesDataTable()
    {
        DataTable dtGameTypesStake = new DataTable();
        dtGameTypesStake.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameTypeID"),
            new DataColumn("StakeLevelList"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

        return dtGameTypesStake;
    }

    public string GetSelectedStakeLevels(string format, int gameKind, int gameTypeId)
    {
        HttpRequest request = HttpContext.Current.Request;
        string[] stakeLevels = format.Split(new string[] { Globals.SEP_STAKE }, StringSplitOptions.RemoveEmptyEntries);
        string selectedStakeLevels = string.Empty;

        for (int i = 0; i < stakeLevels.Length; i++)
        {
            if (request[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CHK_STAKE + Globals.SEP_GAMEKIND_GAMETYPE + stakeLevels[i]] != null)
                selectedStakeLevels += stakeLevels[i] + ",";
        }

        if (selectedStakeLevels.EndsWith(","))
        {
            selectedStakeLevels = selectedStakeLevels.Remove(selectedStakeLevels.LastIndexOf(","), 1);
        }

        return selectedStakeLevels;
    }

    public string GetStakeLevelList(string format)
    {
        return format.Substring(format.IndexOf(Globals.OPEN_STAKELIST_MARK) + 1, format.IndexOf(Globals.CLOSE_STAKELIST_MARK) - format.IndexOf(Globals.OPEN_STAKELIST_MARK) - 1);
    }

    public int GetGameTypeId(string format)
    {
        if (format.Contains(Globals.OPEN_STAKELIST_MARK))
        {
            return Convert.ToInt32(format.Substring(0, format.IndexOf(Globals.OPEN_STAKELIST_MARK)));
        }
        else
        {
            return Convert.ToInt32(format);
        }
    }

    public int GetGameKindId(string format)
    {
        return Convert.ToInt32(format.Substring(0, format.IndexOf(Globals.OPEN_GAMETYPELIST_MARK)));
    }

    public string[] GetGameTypesId(string format)
    {
        return format.Substring(format.IndexOf(Globals.OPEN_GAMETYPELIST_MARK) + 1, format.IndexOf(Globals.CLOSE_GAMETYPELIST_MARK) - format.IndexOf(Globals.OPEN_GAMETYPELIST_MARK) - 1).Split(new string[] { Globals.SEP_GAMETYPE }, StringSplitOptions.RemoveEmptyEntries);
    }

    public string FormatMessage(string message)
    {
        return message.Replace("'", "\\'").Replace("\\n", "<br />");
    }

    public string RenderTable(DataTable dt)
    {
        string returnString = "<table border=1>";
        returnString += "<tr>";
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            returnString += "<td>" + dt.Columns[i].ColumnName + "</td>";
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            returnString += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                returnString += "<td>" + (dt.Rows[i][j] == null ? "null" : dt.Rows[i][j].ToString()) + "</td>";
            }
            returnString += "</tr>";
        }

        returnString += "</tr>";
        returnString += "</table><br />";

        return returnString;
    }

    /// <summary>
    /// Open in popup or in main page
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected bool IsNoBackground(HttpContext context)
    {
        return (String.IsNullOrEmpty(context.Request["nobackground"]));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}