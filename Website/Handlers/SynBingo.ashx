<%@ WebHandler Language="C#" Class="UpdateBingoSetting_Handler" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Sunplus.Bingo.Entities;
using Sunplus.Bingo.Services;
using Sunplus.Bingo.Core;

using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Model.BL.Authorization;

public class UpdateBingoSetting_Handler : Fanex.Age.UI.TplPage, IReadOnlySessionState
{
    #region Variable
    protected const int SYSTEM_ID = 2;
    protected int _errorCode = 0;
    protected string _errorMessage = "";
    protected int _custId = 0;
    protected int _roleId = 0;
    protected bool _isNoBackGround = false;
    private bool _isInsert = false;
    /// <summary>
    /// Use for multiple updating
    /// </summary>
    protected List<int> _custIdList = new List<int>();
    protected List<string> _custNameList = new List<string>();
    private ICustomerService customerService = BingoContainer.Current.Resolve<ICustomerService>();
    private Customer _customerSetting = null;
    #endregion

    public UpdateBingoSetting_Handler()
    {

    }

    public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
    {

        if (_errorCode != 0)
        {
            writer.WriteLine("<script> parent.showErrMsg('" + FormatMsg(_errorMessage) + "', 0); </script>");

            writer.WriteLine(@"
                                <script>
                                if(parent && parent.parent && parent!=window && parent.parent.ageWnd)
                                {
                                    parent.parent.ageWnd.onClosing = function()
                                    {
                                        parent.parent.DelayReloadPage();
                                        return true;
                                    };
                               }
                                </script>");
            return;
        }
        else
        {
            bool bSuperMasterUpdMember = (CurrentUserSession.RoleId >= 3 && _roleId == 1);

            if (!_isInsert) // Not in new customer case
            {
                if (!bSuperMasterUpdMember)
                {
                    if (_isNoBackGround)// Not from AccountInfo
                    {
                        writer.WriteLine(@"<script>
                        if(parent && parent.parent && parent!=window && parent.parent.ageWnd)
                        {
                            parent.parent.DelayReloadPage();
                        }
                        else
                        {
                            parent.DelayReloadPage('../MemberInfo/PositionTakingBingo.aspx?custid=" + _custId + @"&issucc=1&isupdate=true');
                        }
                        </script>
                        ");
                    }
                    else // From AccountInfo tab
                    {
                        writer.WriteLine(String.Format("<script>parent.DelayReloadPage('../MemberInfo/PositionTakingBingo.aspx?custid={0}&nobackground=1&issucc=1&isupdate=false'); parent.top.menu.reloadMenuUsrInfo();</script>", _custId));
                    }
                }
                else
                {
                    if (_isNoBackGround)// Not from AccountInfo
                    {

                        writer.WriteLine(@"<script>
                            if(parent && parent.parent && parent!=window && parent.parent.ageWnd)
                            {
                                parent.parent.DelayReloadPage();
                            }
                            </script>");
                    }
                    else // From AccountInfo tab
                    {
                        writer.WriteLine(String.Format("<script>parent.DelayReloadPage('../MemberInfo/PositionTakingBingo.aspx?custid={0}&nobackground=1&issucc=1&isupdate=false'); parent.top.menu.reloadMenuUsrInfo();</script>", _custId));
                    }
                }
            }
            else // In new customer case
            {
                writer.WriteLine(String.Format("<script>parent.DelayReloadPage('../MemberInfo/PositionTakingBingo.aspx?custid={0}&issucc=1&isupdate=true');</script>", _custId));
            }
        }
    }

    public override void ProcessRequest(ref System.Collections.Hashtable p)
    {
        _custId = GetIntRequest("custid", 0);
        _roleId = GetIntRequest("hidRoleId", 0);
        _isNoBackGround = String.IsNullOrEmpty(Request["nobackground"]) ? true : false;
        _isInsert = Request["insert"] == "true" ? true : false;

        // Check if multiple updating
        bool isMultiple = GetBooleanRequest("bIsMult", false);
        if (isMultiple)
        {
            ParseStringList(Request["arrayUserName"], '^', _custNameList);
            ParseIntList(Request["custids"], '^', _custIdList);
        }
        else
        {
            _custIdList.Add(_custId);
        }
    }

    public override object ValidateRequest(System.Collections.Hashtable p)
    {
        AuthorizationBO authorizationBO = new AuthorizationBO(CurrentUserSession.NetSessionId);

        string updateType = _isInsert ? PageAccessType.CreateAccount : PageAccessType.UpdateAccount;
        bool canAccess = authorizationBO.CheckPageAccess(updateType, out _errorCode, out _errorMessage);

        if (CurrentUserSession.AdminId != 0 || !UserPermission.IsShowBingo)
        {
            _errorCode = 105;
            _errorMessage = Resources.Lang.lblNotAuthorized;
        }

        return null;
    }

    public override object InvokeData(System.Collections.Hashtable p)
    {
        string fromIP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP();
        if (_errorCode == 0)
        {
            for (int i = 0, customerCount = _custIdList.Count; i < customerCount; i++)
            {
                // Assign value for customer
                IResult result = new Result();
                _customerSetting = customerService.GetCustomerSetting(_custIdList[i], _roleId, ref result) as Customer;
                if (_isInsert) _customerSetting.IsEnabledBingo = GetBooleanRequest("chkDisabledBingo", false);
                _customerSetting.MaxTransferCredit = GetDecimalRequest("txtMaxTransferCredit", _customerSetting.MaxTransferCredit);
                if (CanChangeMaxTranferValue && IsUpdateDirectDownline) _customerSetting.IsAutoReset = GetBooleanRequest("cbxAutoReset", false);
                AnalyzeGameKindSetting(_customerSetting);

                // Check bet setting limit first, the value 0 means no limit
                decimal maxTranferCredit = GetDecimalRequest("txtMaxTransferCredit", _customerSetting.MaxTransferCredit);
                if ((_customerSetting.MaxLimit != 0 && (maxTranferCredit < _customerSetting.MinLimit || maxTranferCredit > _customerSetting.MaxLimit))
                    || (_customerSetting.MaxLimit == 0 && maxTranferCredit < _customerSetting.MinLimit && maxTranferCredit > 0))
                {
                    if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoMaxTransfer)
                    {
                        _errorCode = 1;
                        _errorMessage += (_custNameList.Count > 0) ? (_custNameList[i] + ": ") : (string.Empty) + Resources.Lang.transferCreditMsg + "<br/>";
                    }
                }
                else
                { // Process update setting
                    try
                    {
                        result = customerService.UpdateCustomerSetting(_customerSetting, CurrentUserSession.RoleId, fromIP, CurrentUserSession.UserId, CurrentUserSession.UserName, SYSTEM_ID) as Result;
                        if (result.ErrorCode != 0)
                        {
                            _errorCode = result.ErrorCode;
                            _errorMessage += result.ErrorMessage + "<br/>";
                        }
                    }
                    catch
                    {
                        _errorCode = 1;
                        _errorMessage += (_custNameList.Count > 0) ? (_custNameList[i] + ": ") : (string.Empty) + "Not Bingo's Customer" + "<br/>";
                    }
                }
            }
        }

        return null;
    }

    #region Utility
    /// <summary>
    /// Parse the string to int list
    /// </summary>
    /// <param name="value"></param>
    /// <param name="separator"></param>
    /// <param name="intList"></param>
    private void ParseIntList(string value, char separator, List<int> intList)
    {
        string[] stringItemList = value.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in stringItemList)
        {
            int intValue = 0;
            if (int.TryParse(item, out intValue)) intList.Add(intValue);
        }
    }

    private void ParseStringList(string value, char separator, List<string> listUserName)
    {
        string[] stringItemList = value.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in stringItemList)
        {
            if (!string.IsNullOrEmpty(item))
            {
                listUserName.Add(item);
            }
        }
    }

    /// <summary>
    /// Parse request["hidBGData"] to get id of position taking, commission, stacklevel input
    /// </summary>
    /// <returns>
    /// </returns>
    private void AnalyzeGameKindSetting(Customer customer)
    {
        string settingIdList = GetStringRequest("hidBGData", "");
        string[] gameKindList = settingIdList.Split(new string[] { BingoDef.SEP_GAMEKIND }, StringSplitOptions.RemoveEmptyEntries);
        // Process each game kind
        foreach (string gameKind in gameKindList)
        {
            int gameKindId = GetGameKindId(gameKind);
            // Get the associate game kind to update its value
            GameKind gameKindItem = GetGameKindFromList(gameKindId, customer.GameKindList);

            if (CurrentUserSession.RoleId == 2)
            {
                AnalyzeGameKindForMember(gameKindItem);
            }
            else if (CurrentUserSession.RoleId == 3)
            {
                // Master edit member
                if (_roleId == 1) AnalyzeGameKindForMasterEditMember(gameKindItem);
                // Master edit agent
                else AnalyzeGameKindForAgent(gameKindItem);
            }
            else if (CurrentUserSession.RoleId == 4)
            {
                // Super edit membet
                if (_roleId == 1) AnalyzeGameKindForSuperEditMember(gameKindItem);
                // Super update master
                else AnalyzeGameKindForMaster(gameKindItem);
            }
            // Process each game type
            string[] gameTypeList = GetGameTypeListData(gameKind);
            foreach (string gameType in gameTypeList)
            {
                int gameTypeId = GetGameTypeId(gameType);
                // Get the associate game type to update its value
                GameType gameTypeItem = GetGameTypeFromList(gameTypeId, gameKindItem.GameTypeList);
                if (CurrentUserSession.RoleId == 2)
                {
                    AnalyzeGameTypeForMember(gameKindId, gameTypeItem);
                }
                else if (CurrentUserSession.RoleId == 3)
                {
                    // Master edit member
                    if (_roleId == 1) AnalyzeGameTypeForMasterEditMember(gameKindId, gameTypeItem);
                    else AnalyzeGameTypeForAgent(gameKindId, gameTypeItem);
                }
                else if (CurrentUserSession.RoleId == 4)
                {
                    // Super update agent
                    if (_roleId == 1) AnalyzeGameTypeForSuperEditMember(gameKindId, gameTypeItem);
                    else AnalyzeGameTypeForMaster(gameKindId, gameTypeItem);
                }
                // Process each stack level
                string[] stackLevelIdList = GetStackLevelIdList(gameType);
                foreach (string stackLevel in stackLevelIdList)
                {
                    int stackLevelId = Convert.ToInt32(stackLevel); ;
                    BettingRange bettingRange = GetBettingRangeFromList(stackLevelId, gameTypeItem.BettingRangeList);
                    bettingRange.IsChecked = GetBooleanRequest(gameKindId + BingoDef.SEP_GAMEKIND_GAMETYPE + gameTypeId + BingoDef.SEP_GAMEKIND_GAMETYPE + "ChkStake" + BingoDef.SEP_GAMEKIND_GAMETYPE + stackLevelId, false);
                }
            }
        }
    }

    #region AnalyzeDataForMember
    private void AnalyzeGameKindForMember(GameKind gameKind)
    {
        string gameKindParamName = gameKind.ID + BingoDef.SEP_GAMEKIND_GAMETYPE + "0" + BingoDef.SEP_GAMEKIND_GAMETYPE;
        gameKind.PositionTaking = GetDoubleRequest(gameKindParamName + BingoDef.Agent + BingoDef.strPresetPT, gameKind.PositionTaking);
        gameKind.Commission = GetDoubleRequest(gameKindParamName + BingoDef.COMM_PT, gameKind.Commission);
    }
    private void AnalyzeGameTypeForMember(int gameKindId, GameType gameType)
    {
        string gameTypeParamName = gameKindId + BingoDef.SEP_GAMEKIND_GAMETYPE + gameType.ID + BingoDef.SEP_GAMEKIND_GAMETYPE;
        gameType.PositionTaking = GetDoubleRequest(gameTypeParamName + BingoDef.Agent + BingoDef.strPresetPT, gameType.PositionTaking);
        gameType.Commission = GetDoubleRequest(gameTypeParamName + BingoDef.COMM_PT, gameType.Commission);
    }
    #endregion

    #region AnalyzeDataForAgent
    private void AnalyzeGameKindForAgent(GameKind gameKind)
    {
        string gameKindParamName = gameKind.ID + BingoDef.SEP_GAMEKIND_GAMETYPE + "0" + BingoDef.SEP_GAMEKIND_GAMETYPE;

        gameKind.PositionTakingUpline = GetDoubleRequest(gameKindParamName + BingoDef.Master + BingoDef.strPresetPT, 0);
        gameKind.PositionTaking = GetDoubleRequest(gameKindParamName + BingoDef.Agent + BingoDef.strPresetPT, gameKind.PositionTaking);
        gameKind.MinPositionTaking = GetDoubleRequest(gameKindParamName + BingoDef.Agent + BingoDef.strMinPT, gameKind.MinPositionTaking);
        gameKind.Commission = GetDoubleRequest(gameKindParamName + BingoDef.COMM_PT, gameKind.Commission);
    }
    private void AnalyzeGameTypeForAgent(int gameKindId, GameType gameType)
    {
        string gameTypeParamName = gameKindId + BingoDef.SEP_GAMEKIND_GAMETYPE + gameType.ID + BingoDef.SEP_GAMEKIND_GAMETYPE;
        gameType.PositionTakingUpline = GetDoubleRequest(gameTypeParamName + BingoDef.Master + BingoDef.strPresetPT, 0);
        gameType.PositionTaking = GetDoubleRequest(gameTypeParamName + BingoDef.Agent + BingoDef.strPresetPT, gameType.PositionTaking);
        gameType.MinPositionTaking = GetDoubleRequest(gameTypeParamName + BingoDef.Agent + BingoDef.strMinPT, gameType.MinPositionTaking);
        gameType.Commission = GetDoubleRequest(gameTypeParamName + BingoDef.COMM_PT, gameType.Commission);
    }
    #endregion

    #region AnalyzeDataForAgent
    private void AnalyzeGameKindForMaster(GameKind gameKind)
    {
        string gameKindParamName = gameKind.ID + BingoDef.SEP_GAMEKIND_GAMETYPE + "0" + BingoDef.SEP_GAMEKIND_GAMETYPE;

        gameKind.IsAutoPositionTaking = GetBooleanRequest(gameKindParamName + BingoDef.strCHK + BingoDef.Super + BingoDef.strAutoPT, false);
        gameKind.AutoPositionTaking = GetDoubleRequest(gameKindParamName + BingoDef.Super + BingoDef.strAutoPT, 0);
        gameKind.PositionTakingUpline = GetDoubleRequest(gameKindParamName + BingoDef.Super + BingoDef.strPresetPT, 0);
        gameKind.PositionTaking = GetDoubleRequest(gameKindParamName + BingoDef.Master + BingoDef.strPresetPT, gameKind.PositionTaking);
        gameKind.MinPositionTaking = GetDoubleRequest(gameKindParamName + BingoDef.Master + BingoDef.strMinPT, gameKind.MinPositionTaking);
        gameKind.Commission = GetDoubleRequest(gameKindParamName + BingoDef.COMM_PT, gameKind.Commission);
    }
    private void AnalyzeGameTypeForMaster(int gameKindId, GameType gameType)
    {
        string gameTypeParamName = gameKindId + BingoDef.SEP_GAMEKIND_GAMETYPE + gameType.ID + BingoDef.SEP_GAMEKIND_GAMETYPE;

        gameType.IsAutoPositionTaking = GetBooleanRequest(gameTypeParamName + BingoDef.strCHK + BingoDef.Super + BingoDef.strAutoPT, false);
        gameType.AutoPositionTaking = GetDoubleRequest(gameTypeParamName + BingoDef.Super + BingoDef.strAutoPT, 0);
        gameType.PositionTakingUpline = GetDoubleRequest(gameTypeParamName + BingoDef.Super + BingoDef.strPresetPT, 0);
        gameType.PositionTaking = GetDoubleRequest(gameTypeParamName + BingoDef.Master + BingoDef.strPresetPT, gameType.PositionTaking);
        gameType.MinPositionTaking = GetDoubleRequest(gameTypeParamName + BingoDef.Master + BingoDef.strMinPT, gameType.MinPositionTaking);
        gameType.Commission = GetDoubleRequest(gameTypeParamName + BingoDef.COMM_PT, gameType.Commission);

    }
    #endregion

    #region AnalyzeDataForSuperEditMember
    private void AnalyzeGameKindForSuperEditMember(GameKind gameKind)
    {
        string gameKindParamName = gameKind.ID + BingoDef.SEP_GAMEKIND_GAMETYPE + "0" + BingoDef.SEP_GAMEKIND_GAMETYPE;

        gameKind.IsAutoPositionTaking = GetBooleanRequest(gameKindParamName + BingoDef.strCHK + BingoDef.Super + BingoDef.strAutoPT, false);
        gameKind.AutoPositionTaking = GetDoubleRequest(gameKindParamName + BingoDef.Super + BingoDef.strAutoPT, 0);
        gameKind.PositionTaking = GetDoubleRequest(gameKindParamName + BingoDef.Super + BingoDef.strPresetPT, 0);

    }
    private void AnalyzeGameTypeForSuperEditMember(int gameKindId, GameType gameType)
    {
        string gameTypeParamName = gameKindId + BingoDef.SEP_GAMEKIND_GAMETYPE + gameType.ID + BingoDef.SEP_GAMEKIND_GAMETYPE;

        gameType.IsAutoPositionTaking = GetBooleanRequest(gameTypeParamName + BingoDef.strCHK + BingoDef.Super + BingoDef.strAutoPT, false);
        gameType.AutoPositionTaking = GetDoubleRequest(gameTypeParamName + BingoDef.Super + BingoDef.strAutoPT, 0);
        gameType.PositionTaking = GetDoubleRequest(gameTypeParamName + BingoDef.Super + BingoDef.strPresetPT, 0);
    }
    #endregion

    #region AnalyzeDataForMasterEditMember
    private void AnalyzeGameKindForMasterEditMember(GameKind gameKind)
    {
        string gameKindParamName = gameKind.ID + BingoDef.SEP_GAMEKIND_GAMETYPE + "0" + BingoDef.SEP_GAMEKIND_GAMETYPE;

        gameKind.PositionTaking = GetDoubleRequest(gameKindParamName + BingoDef.Master + BingoDef.strPresetPT, 0);

    }
    private void AnalyzeGameTypeForMasterEditMember(int gameKindId, GameType gameType)
    {
        string gameTypeParamName = gameKindId + BingoDef.SEP_GAMEKIND_GAMETYPE + gameType.ID + BingoDef.SEP_GAMEKIND_GAMETYPE;
        gameType.PositionTaking = GetDoubleRequest(gameTypeParamName + BingoDef.Master + BingoDef.strPresetPT, 0);
    }
    #endregion
    /// <summary>
    /// Get GameKindId from game kind data
    /// </summary>
    /// <param name="str">Ex: GameKindData=159[30(1380)-32(1382)-31(1381)-33(1428)-34(1444)]</param>
    /// <returns>159</returns>
    private int GetGameKindId(string str)
    {
        return Convert.ToInt32(str.Substring(0, str.IndexOf(BingoDef.OPEN_GAMETYPELIST_MARK)));
    }
    /// <summary>
    /// Get GameTypeListData
    /// </summary>
    /// <param name="str">Ex: GameKindData=159[30(1380)-32(1382)-31(1381)-33(1428)-34(1444)]<</param>
    /// <returns>
    ///         Array={30(1380)
    ///                 32(1382)
    ///                 31(1381)
    ///                 33(1428)
    ///                 34(1444)}
    /// </returns>
    private string[] GetGameTypeListData(string str)
    {
        int startIndex = str.IndexOf(BingoDef.OPEN_GAMETYPELIST_MARK);
        int endIndex = str.IndexOf(BingoDef.CLOSE_GAMETYPELIST_MARK);
        string gamgeTypeListData = str.Substring(startIndex + 1, endIndex - startIndex - 1);
        return gamgeTypeListData.Split(new string[] { BingoDef.SEP_GAMETYPE }, StringSplitOptions.RemoveEmptyEntries);
    }
    /// <summary>
    /// Get game type id from game type data
    /// </summary>
    /// <param name="str">Ex: GameTypeData=12(23, 24, 25, 33)</param>
    /// <returns>12</returns>
    private int GetGameTypeId(string str)
    {
        return Convert.ToInt32(str.Substring(0, str.IndexOf(BingoDef.OPEN_STAKELIST_MARK)));
    }
    /// <summary>
    /// Get stack level id list from game type data
    /// </summary>
    /// <param name="str">Ex: GameTypeData=12(23, 24, 25, 33)</param>
    /// <returns>
    ///     Array(
    ///         23
    ///         24
    ///         25
    ///         33
    ///         )
    /// </returns>
    private string[] GetStackLevelIdList(string str)
    {
        int startIndex = str.IndexOf(BingoDef.OPEN_STAKELIST_MARK);
        int endIndex = str.IndexOf(BingoDef.CLOSE_STAKELIST_MARK);
        string stackLevelListData = str.Substring(startIndex + 1, endIndex - startIndex - 1);
        return stackLevelListData.Split(new string[] { BingoDef.SEP_STAKE }, StringSplitOptions.RemoveEmptyEntries);
    }

    public string FormatMsg(string msg)
    {
        return msg.Replace("'", "\\'").Replace("\\n", "<br />");
    }

    /// <summary>
    /// Get game kind item from list
    /// if not found, create new
    /// </summary>
    /// <param name="id"></param>
    /// <param name="gameKindList"></param>
    /// <returns></returns>
    private GameKind GetGameKindFromList(int id, List<GameKind> gameKindList)
    {
        GameKind gameKindItem = gameKindList.Where(g => g.ID == id).FirstOrDefault();
        if (gameKindItem == null || gameKindItem.ID == 0)
        {
            gameKindItem = new GameKind();
            gameKindItem.ID = id;
            gameKindItem.GameTypeList = new List<GameType>();
            gameKindList.Add(gameKindItem);
        }
        return gameKindItem;
    }
    /// <summary>
    /// Get game type item from list
    /// if not found, create new
    /// </summary>
    /// <param name="id"></param>
    /// <param name="gameTypeList"></param>
    /// <returns></returns>
    private GameType GetGameTypeFromList(int id, List<GameType> gameTypeList)
    {
        GameType gameTypeItem = gameTypeList.Where(g => g.ID == id).FirstOrDefault();
        if (gameTypeItem == null || gameTypeItem.ID == 0)
        {
            gameTypeItem = new GameType();
            gameTypeItem.ID = id;
            gameTypeItem.BettingRangeList = new List<BettingRange>();
            gameTypeList.Add(gameTypeItem);
        }
        return gameTypeItem;
    }
    /// <summary>
    /// Get betting range item from list
    /// if not found, create new
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bettingRangeList"></param>
    /// <returns></returns>
    private BettingRange GetBettingRangeFromList(int id, List<BettingRange> bettingRangeList)
    {
        BettingRange bettingRangeItem = bettingRangeList.Where(g => g.ID == id).FirstOrDefault();
        if (bettingRangeItem == null || bettingRangeItem.ID == 0)
        {
            bettingRangeItem = new BettingRange();
            bettingRangeItem.ID = id;
            bettingRangeList.Add(bettingRangeItem);
        }
        return bettingRangeItem;
    }
    /// <summary>
    /// The user can change bingo max tranfer when Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoMaxTransfer = false
    /// </summary>
    /// <returns></returns>
    private bool CanChangeMaxTranferValue
    {
        get
        {
            return !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoMaxTransfer;
        }
    }

    private string GameKindParamName(string gameKindId)
    {
        return gameKindId + BingoDef.SEP_GAMEKIND_GAMETYPE + "0" + BingoDef.SEP_GAMEKIND_GAMETYPE;
    }

    private string GameTypeParamName(string gameKindId, string gameTypeId)
    {
        return gameKindId + BingoDef.SEP_GAMEKIND_GAMETYPE + gameTypeId + BingoDef.SEP_GAMEKIND_GAMETYPE;
    }

    /// <summary>
    /// Return true if super edit master, master edit agent, agent edit member
    /// </summary>
    private bool IsUpdateDirectDownline
    {
        get
        {
            return CurrentUserSession.RoleId - _roleId == 1 ? true : false;
        }
    }
    #endregion
}