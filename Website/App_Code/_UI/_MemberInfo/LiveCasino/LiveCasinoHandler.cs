// --------------------------------------------------------------------------
// <copyright file="LiveCasinoHandler.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.4.13">Format styles</change>
// </history>

namespace Fanex.Age.UI.LiveCasino
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Fanex.LiveCasinoSetting;
    using Fanex.LiveCasinoSetting.Business;
    using Fanex.LiveCasinoSetting.Model;
    using Fanex.LiveCasinoSetting.Model.Log;
    using Fanex.LiveCasinoSetting.Request;
    using Resources;
    using Sunplus.Agent.Model.BL.AccountInfo;

    /// <summary>
    /// Base class for process live casino setting
    /// </summary>
    public abstract class LiveCasinoHandler : TplHandler
    {
        /// <summary>
        /// Constant object
        /// </summary>
        protected const string NoChangePtErrorCode = "CS0000";

        /// <summary>
        /// Constant object
        /// </summary>
        protected const string NoChangeGameErrorCode = "GS0000";

        /// <summary>
        /// Constant object
        /// </summary>
        protected const string BeginDivSucc = "<span class=\"EnrichSuccmsg\">";

        /// <summary>
        /// Constant object
        /// </summary>
        protected const string BeginDivErr = "<span class=\"EnrichErrmsg\">";

        /// <summary>
        /// Constant object
        /// </summary>
        protected const string EndDiv = "</span><br />";

        /// <summary>
        /// Locker object
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Locker sync object
        /// </summary>
        private static object lockSync = new object();

        /// <summary>
        /// Private property
        /// </summary>
        private List<int> unsyncIds = new List<int>();

        /// <summary>
        /// Private property
        /// </summary>
        private List<int> unsyncTmp = new List<int>();

        /// <summary>
        /// Private property
        /// </summary>
        private List<int> custids = new List<int>();

        /// <summary>
        /// Private property
        /// </summary>
        private List<string[]> errMsgs = new List<string[]>();

        /// <summary>
        /// Private property
        /// </summary>
        private List<GameType> gameTypes = new List<GameType>();

        /// <summary>
        /// Private property
        /// </summary>
        private List<string[]> errCodes = new List<string[]>();

        /// <summary>
        /// Private property
        /// </summary>
        private List<int> wrongPtIds = new List<int>();

        /// <summary>
        /// Private property
        /// </summary>
        private Dictionary<int, string> tmpMsgs = new Dictionary<int, string>();

        /// <summary>
        /// Private property
        /// </summary>
        private Dictionary<int, Dictionary<int, LiveCasinoSetting.Model.Result>> gameResult
            = new Dictionary<int, Dictionary<int, LiveCasinoSetting.Model.Result>>();

        /// <summary>
        /// Private property
        /// </summary>
        private SettingService settingPT = null;

        /// <summary>
        /// Private property
        /// </summary>
        private GameSettingService gameSetting = null;

        /// <summary>
        /// Private property
        /// </summary>
        private AccountInfoBO accInfo = null;

        /// <summary>
        /// Private property
        /// </summary>
        private decimal presetPT = 0m;

        /// <summary>
        /// Private property
        /// </summary>
        private decimal autoPresetPT = 0m;

        /// <summary>
        /// Private property
        /// </summary>
        private decimal minPT = 0m;

        /// <summary>
        /// Private property
        /// </summary>
        private decimal maxPT = 0m;

        /// <summary>
        /// Private property
        /// </summary>
        private decimal comm = 0m;

        /// <summary>
        /// Private property
        /// </summary>
        private decimal newMemberPT = 0.0m;

        /// <summary>
        /// Private property
        /// </summary>
        private int requestCustRoleId = 0;

        /// <summary>
        /// Private property
        /// </summary>
        private int requestCustId = 0;

        /// <summary>
        /// Private property
        /// </summary>
        private bool isSyncLiveCasino = false;

        /// <summary>
        /// Private property
        /// </summary>
        private bool isUpdateAllMember = false;

        /// <summary>
        /// Private property
        /// </summary>
        private bool isAutoPresetPT = false;

        /// <summary>
        /// Private property
        /// </summary>
        private bool isSubmit = false;

        /// <summary>
        /// Private property
        /// </summary>
        private string msgTmp = string.Empty;

        /// <summary>
        /// Private property
        /// </summary>
        private string errCode = string.Empty;

        /// <summary>
        /// Private property
        /// </summary>
        private string errMsg = string.Empty;

        /// <summary>
        /// Private property
        /// </summary>
        private string errCodeStrs = string.Empty;

        /// <summary>
        /// Private property
        /// </summary>
        private bool enableLiveCS = false;

        /// <summary>
        /// Private property
        /// </summary>
        private bool disableCheckBox = false;

        /// <summary>
        /// Gets or sets a value indicating whether upline live casino is disabled or not
        /// </summary>
        public bool DisableCheckBox
        {
            get { return this.disableCheckBox; }
            set { this.disableCheckBox = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether live casino is enabled or not
        /// </summary>
        protected bool EnableLiveCS
        {
            get { return this.enableLiveCS; }
            set { this.enableLiveCS = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected List<int> WrongPtIds
        {
            get { return this.wrongPtIds; }
            set { this.wrongPtIds = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected List<int> UnsyncIds
        {
            get { return this.unsyncIds; }
            set { this.unsyncIds = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected List<int> UnsyncTmp
        {
            get { return this.unsyncTmp; }
            set { this.unsyncTmp = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected List<int> Custids
        {
            get { return this.custids; }
            set { this.custids = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected List<string[]> ErrCodes
        {
            get { return this.errCodes; }
            set { this.errCodes = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected List<string[]> ErrMsgs
        {
            get { return this.errMsgs; }
            set { this.errMsgs = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected List<GameType> GameTypes
        {
            get { return this.gameTypes; }
            set { this.gameTypes = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected Dictionary<int, string> TmpMsgs
        {
            get { return this.tmpMsgs; }
            set { this.tmpMsgs = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected Dictionary<int, Dictionary<int, LiveCasinoSetting.Model.Result>> GameResult
        {
            get { return this.gameResult; }
            set { this.gameResult = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected SettingService PtSetting
        {
            get { return this.settingPT; }
            set { this.settingPT = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected GameSettingService GameSetting
        {
            get { return this.gameSetting; }
            set { this.gameSetting = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected AccountInfoBO AccInfo
        {
            get { return this.accInfo; }
            set { this.accInfo = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected decimal PresetPT
        {
            get { return this.presetPT; }
            set { this.presetPT = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected decimal AutoPresetPT
        {
            get { return this.autoPresetPT; }
            set { this.autoPresetPT = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected decimal MinPT
        {
            get { return this.minPT; }
            set { this.minPT = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected decimal MaxPT
        {
            get { return this.maxPT; }
            set { this.maxPT = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected decimal Comm
        {
            get { return this.comm; }
            set { this.comm = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected decimal NewMemberPT
        {
            get { return this.newMemberPT; }
            set { this.newMemberPT = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected int RequestCustRoleId
        {
            get { return this.requestCustRoleId; }
            set { this.requestCustRoleId = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected int RequestCustId
        {
            get { return this.requestCustId; }
            set { this.requestCustId = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether live casino is synced
        /// </summary>
        protected bool IsSyncLiveCasino
        {
            get { return this.isSyncLiveCasino; }
            set { this.isSyncLiveCasino = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether update all member or not
        /// </summary>
        protected bool IsUpdateAllMember
        {
            get { return this.isUpdateAllMember; }
            set { this.isUpdateAllMember = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is auto preset pt
        /// </summary>
        protected bool IsAutoPresetPT
        {
            get { return this.isAutoPresetPT; }
            set { this.isAutoPresetPT = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the request is submit button clicked
        /// </summary>
        protected bool IsSubmit
        {
            get { return this.isSubmit; }
            set { this.isSubmit = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected string MsgTmp
        {
            get { return this.msgTmp; }
            set { this.msgTmp = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected string ErrCode
        {
            get { return this.errCode; }
            set { this.errCode = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected string ErrMsg
        {
            get { return this.errMsg; }
            set { this.errMsg = value; }
        }

        /// <summary>
        /// Gets or sets property
        /// </summary>
        protected string ErrCodeStrs
        {
            get { return this.errCodeStrs; }
            set { this.errCodeStrs = value; }
        }

        /// <summary>
        /// Gets property
        /// </summary>
        protected LogActions LogActionUpdateStakeLevel
        {
            get
            {
                switch (this.RequestCustRoleId)
                {
                    case 3:
                        return LogActions.UpdateStakeLevel_Master;
                    case 2:
                        return LogActions.UpdateStakeLevel_Agent;
                    case 1:
                        return LogActions.UpdateStakeLevel_Member;
                }

                return LogActions.UpdateStakeLevel_Super;
            }
        }

        /// <summary>
        /// Gets property
        /// </summary>
        private LogActions LogActionManual
        {
            get
            {
                switch (this.RequestCustRoleId)
                {
                    case 1:
                        return LogActions.ManualInsert_Member;
                    case 2:
                        return LogActions.ManualInsert_Agent;
                    case 3:
                        return LogActions.ManualInsert_Master;
                }

                return LogActions.ManualInsert_Super;
            }
        }

        /// <summary>
        /// Render result to JSON
        /// </summary>
        /// <param name="data">JSON data</param>
        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            var msgs = this.ErrMsg.TrimEnd('$').Split('$');
            var err = string.Empty;
            var count = 0;

            foreach (var custId in this.Custids)
            {
                foreach (var tmp in this.TmpMsgs)
                {
                    if (tmp.Key == custId && !msgs[count].Contains(tmp.Value))
                    {
                        err += tmp.Value + msgs[count] + "$";
                        break;
                    }
                }

                count++;
            }

            data.Add("errMsg", this.ErrCode.TrimEnd('$') + "|" + (string.IsNullOrWhiteSpace(err) ? this.ErrMsg.TrimEnd('$') : err.TrimEnd('$')));
        }

        /// <summary>
        /// Invoke data
        /// </summary>
        /// <param name="p">Request params</param>
        /// <returns>Throw exception if not overridden</returns>
        public override object InvokeData(System.Collections.Hashtable p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process request
        /// </summary>
        /// <param name="p">Request params</param>
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            lock (locker)
            {
                this.AccInfo = new AccountInfoBO(CurrentUserSession.NetSessionId);
                this.PtSetting = new SettingService();
                this.GameSetting = new GameSettingService();
                this.GameTypes = new List<GameType>();

                var users = Request.QueryString["custids"];

                this.RequestCustRoleId = Convert.ToInt32(Request.QueryString["roleid"]);

                if (users != null && users.Contains("^"))
                {
                    var userIds = users.Split('^');
                    this.Custids = new List<int>();

                    foreach (string id in userIds)
                    {
                        if (!string.IsNullOrWhiteSpace(id.Trim()))
                        {
                            var custId = Convert.ToInt32(id.Trim());

                            var sync = this.AccInfo.GetCustInfo(custId).LiveCasinoSync;

                            if (!sync)
                            {
                                this.UnsyncIds.Add(custId);
                                this.UnsyncTmp.Add(custId);
                            }

                            this.Custids.Add(custId);
                        }
                    }

                    this.RequestCustId = this.Custids[0];
                }
                else
                {
                    this.RequestCustId = Convert.ToInt32(Request.QueryString["custids"]);

                    var sync = this.AccInfo.GetCustInfo(this.RequestCustId).LiveCasinoSync;

                    if (!sync)
                    {
                        this.UnsyncIds.Add(this.RequestCustId);
                        this.UnsyncTmp.Add(this.RequestCustId);
                    }
                }

                this.IsSyncLiveCasino = this.AccInfo.GetCustInfo(this.RequestCustId).LiveCasinoSync;

                // Set params for attributes of PT/Comm
                decimal.TryParse(Request.QueryString["presetPT"], out this.presetPT);
                decimal.TryParse(Request.QueryString["autoPresetPT"], out this.autoPresetPT);
                decimal.TryParse(Request.QueryString["minPT"], out this.minPT);
                decimal.TryParse(Request.QueryString["maxPT"], out this.maxPT);
                decimal.TryParse(Request.QueryString["comm"], out this.comm);
                decimal.TryParse(Request.QueryString["newMemberPT"], out this.newMemberPT);

                bool.TryParse(Request.QueryString["submit"], out this.isSubmit);
                bool.TryParse(Request.QueryString["isUpdateAllMember"], out this.isUpdateAllMember);
                bool.TryParse(Request.QueryString["isAutoPresetPT"], out this.isAutoPresetPT);
                bool.TryParse(Request.QueryString["enable"], out this.enableLiveCS);
                bool.TryParse(Request.QueryString["disable"], out this.disableCheckBox);

                // Set params for attributes of Game Setting
                // pars = [{1^1101, 'Baccarat', (1^ '10-100'^ true$ 2^ '20-200'^ false)}; {2^1102, 'Sicbo', (3^ '20-200'^ false$ 4^ '20-200'^ false)}]
                var par = Request.QueryString["pars"];

                if (par != "[]")
                {
                    var pars = par != null ? par.Replace("[", string.Empty).Replace("]", string.Empty).Split(';') : new string[0];

                    for (int i = 0; i < pars.Length; i++)
                    {
                        var type = new GameType();
                        var bets = new List<StakeLevel>();

                        var items = pars[i].Replace("{", string.Empty).Replace("}", string.Empty).Trim(); // Gets game setting items
                        var gameType = items.Split('|'); // Gets attributes of game setting
                        var stakes = gameType.ElementAt(gameType.Length - 1).Replace("(", string.Empty).Replace(")", string.Empty).Split('$'); // Gets stake levels

                        type = new GameType()
                        {
                            Id = Convert.ToInt32(gameType.ElementAt(0)),
                            BetTypeId = Convert.ToInt32(gameType.ElementAt(1)), // Id of game type
                            Name = gameType.ElementAt(2) // Name of game type
                        };

                        foreach (var item in stakes)
                        {
                            var stakeAtts = item.Split('^');

                            try
                            {
                                var stake = new StakeLevel()
                                {
                                    Id = Convert.ToInt32(stakeAtts.ElementAt(0).Trim()),
                                    Text = stakeAtts.ElementAt(1).Trim(),
                                    IsSelected = Convert.ToBoolean(stakeAtts.ElementAt(2).Trim())
                                };

                                bets.Add(stake);
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        type.BettingLimit = bets;

                        this.GameTypes.Add(type);
                    }
                }
            }
        }

        /// <summary>
        /// Validate request
        /// </summary>
        /// <param name="p">Request params</param>
        /// <returns>Null if request is valid</returns>
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (CurrentUserSession.SubAccId > 0 && !CurrentUserSession.SubPageAccess.UpdateAccount)
            {
                return new Fanex.Age.UI.Result(-1, this.FormatMsg("-1", BeginDivErr + Lang.strNotAccessPage + EndDiv));
            }

            if (CurrentUserSession.IsInternal/* || CurrentUserSession.IsLiveCasinoDisabled*/)
            {
                return new Fanex.Age.UI.Result(-1, this.FormatMsg("-1", BeginDivErr + Lang.strNotAccessPage + EndDiv));
            }

            var roleDownline = CurrentUserSession.RoleId - this.RequestCustRoleId;

            if (CurrentUserSession.RoleId == 4)
            {
                // Super edits master/member
                if (this.RequestCustId <= 0 || (roleDownline != 1 && roleDownline != 3))
                {
                    return new Fanex.Age.UI.Result(-1, this.FormatMsg("-1", BeginDivErr + Lang.strNotAccessPage + EndDiv));
                }
            }
            else if (CurrentUserSession.RoleId == 3)
            {
                // Master edits agent/member
                if (this.RequestCustId <= 0 || (roleDownline != 1 && roleDownline != 2))
                {
                    return new Fanex.Age.UI.Result(-1, this.FormatMsg("-1", BeginDivErr + Lang.strNotAccessPage + EndDiv));
                }
            }
            else
            {
                // Agent edits member
                if (this.RequestCustId <= 0 || roleDownline != 1)
                {
                    return new Fanex.Age.UI.Result(-1, this.FormatMsg("-1", BeginDivErr + Lang.strNotAccessPage + EndDiv));
                }
            }

            return null;
        }

        /// <summary>
        /// Formats message
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="msg">Error message</param>
        /// <returns>Formatted message</returns>
        protected string FormatMsg(string code, string msg)
        {
            if (this.ErrCodeStrs.Length > 0)
            {
                return this.ErrCodeStrs.Substring(0, this.ErrCodeStrs.Length - 1) + "|" + msg.Substring(0, msg.Length - 1);
            }

            return code + "|" + msg;
        }

        /// <summary>
        /// Process message
        /// </summary>
        /// <param name="results">Result of update action</param>
        /// <param name="commChanged">Whether commission is changed or not</param>
        /// <param name="positionTakingChanged">Whether position taking is changed or not</param>
        /// <param name="minPtChanged">Whether min position taking changed or not</param>
        protected void ProcessMessage(Dictionary<int, Fanex.LiveCasinoSetting.Model.Result> results, bool commChanged, bool positionTakingChanged, bool minPtChanged)
        {
            var users = this.Custids.Count > 0 ? this.Custids : new List<int>() { this.RequestCustId };

            var code0 = -1;
            var code1 = -1;
            var code2 = -1;

            var roleId = CurrentUserSession.RoleId;

            users.ForEach(user =>
            {
                var message = string.Empty;

                var def = results == null || results.Count == 0 ||
                    (results.Count == 1 && (results.ElementAt(0).Value.ErrorCode == ErrorCode.NOT_UPDATE_PT_SETTING || results.ElementAt(0).Value.ErrorCode == ErrorCode.NOT_UPDATE_BETTING_LIMIT));

                var codes = def && UnsyncTmp.Count == 0 ?
                    new string[] { "0" } : def && UnsyncTmp.Count > 0 ?
                    new string[] { "0", "0", "0" } : results[user].ErrorCode.TrimEnd('\a').Split('\a');

                var msgs = new List<string>();

                var tmpMsg = def && UnsyncTmp.Count == 0 ?
                    new string[] { null } : def && UnsyncTmp.Count > 0 ?
                    new string[] { null, null, null } : results[user].ErrorMessage.TrimEnd('\a').Split('\a');

                var lengMsg = tmpMsg.Length - 1;

                for (int i = 0; i < codes.Length; i++)
                {
                    if (codes[i] == ErrorCode.NOT_UPDATE_BETTING_LIMIT ||
                        codes[i] == ErrorCode.NOT_UPDATE_PT_SETTING ||
                        Convert.ToInt32(codes[i]) <= 0)
                    {
                        msgs.Add(string.Empty);
                    }
                    else
                    {
                        if (i <= tmpMsg.Length)
                        {
                            if (i - 1 > lengMsg)
                            {
                                msgs.Add(tmpMsg[lengMsg]);
                            }
                            else
                            {
                                msgs.Add(tmpMsg[i]);
                            }
                        }
                        else
                        {
                            msgs.Add(string.Empty);
                        }
                    }
                }

                if ((commChanged && positionTakingChanged && minPtChanged) || msgs.Count == 3)
                {
                    int.TryParse(codes[0], out code0); // Comm

                    if (codes.Length > 1)
                    {
                        int.TryParse(codes[1], out code1); // PT
                    }
                    else
                    {
                        code1 = 0;
                        msgs.Add(string.Empty);
                    }

                    if (codes.Length > 2)
                    {
                        int.TryParse(codes[2], out code2); // PT
                    }
                    else
                    {
                        code2 = 0;
                        msgs.Add(string.Empty);
                    }

                    this.ErrCodes.Add(new string[]
                                {
                                    code0 < 0 ? "-1" : code0.ToString(),
                                    code1 < 0 ? "-1" : code1.ToString(),
                                    code2 < 0 && string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : code2.ToString()
                                });

                    var msg = code2 < 0 ? BeginDivSucc + Lang.positiontaking + ": " + Lang.updatesuccessfull + EndDiv + MsgTmp :
                                            codes.Length > 2 && string.IsNullOrWhiteSpace(msgs[2]) ? string.Empty :
                                            codes.Length <= 2 ? string.Empty : BeginDivErr + Lang.positiontaking + ": " + msgs[2] + EndDiv + MsgTmp;
                    msg = string.IsNullOrWhiteSpace(msg) && !string.IsNullOrWhiteSpace(MsgTmp) ? MsgTmp : msg;

                    this.ErrMsgs.Add(new string[]
                            {
                                code0 < 0 ? BeginDivSucc + Lang.commission + ": " + Lang.updatesuccessfull + EndDiv :
                                            BeginDivErr + Lang.commission + ": " + (string.IsNullOrWhiteSpace(msgs[0]) ? Lang.validateErrMsg : msgs[0]) + EndDiv,
                                code1 < 0 ? BeginDivSucc + Lang.minposition + ": " + Lang.updatesuccessfull + EndDiv :
                                            string.IsNullOrWhiteSpace(msgs[1]) ? string.Empty : BeginDivErr + Lang.minposition + ": " + msgs[1] + EndDiv,
                                msg
                            });
                }
                else if (commChanged && positionTakingChanged && codes[0] != ErrorCode.NOT_UPDATE_PT_SETTING)
                {
                    int.TryParse(codes[0], out code0);

                    if (codes.Length > 1)
                    {
                        int.TryParse(codes[1], out code1);
                    }
                    else
                    {
                        code1 = 0;
                        msgs.Add(string.Empty);
                    }

                    this.ErrCodes.Add(new string[]
                                {
                                    code0 < 0 ? "-1" : code0.ToString(),
                                    code1 < 0 && string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : code1.ToString()
                                });

                    var msg = code1 < 0 ? BeginDivSucc + Lang.positiontaking + ": " + Lang.updatesuccessfull + EndDiv + MsgTmp :
                                            string.IsNullOrWhiteSpace(msgs[1]) ? string.Empty : BeginDivErr + Lang.positiontaking + ": " + msgs[1] + EndDiv + MsgTmp;
                    msg = string.IsNullOrWhiteSpace(msg) && !string.IsNullOrWhiteSpace(MsgTmp) ? MsgTmp : msg;

                    this.ErrMsgs.Add(new string[]
                            {
                                code0 < 0 ? BeginDivSucc + Lang.commission + ": " + Lang.updatesuccessfull  + EndDiv :
                                            BeginDivErr + Lang.commission + ": " + (string.IsNullOrWhiteSpace(msgs[0]) ? Lang.validateErrMsg : msgs[0]) + EndDiv,
                                msg
                            });
                }
                else if (commChanged && minPtChanged)
                {
                    int.TryParse(codes[0], out code0);

                    if (codes.Length > 1)
                    {
                        int.TryParse(codes[1], out code1);
                    }
                    else
                    {
                        code1 = 0;
                        msgs.Add(string.Empty);
                    }

                    this.ErrCodes.Add(new string[]
                               {
                                    code0 < 0 ? "-1" : code0.ToString(),
                                    code1 < 0 && string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : code1.ToString()
                               });

                    var msg = code1 < 0 ? BeginDivSucc + Lang.minposition + ": " + Lang.updatesuccessfull + EndDiv + MsgTmp :
                                            string.IsNullOrWhiteSpace(msgs[1]) ? string.Empty : BeginDivErr + Lang.minposition + ": " + msgs[1] + EndDiv + MsgTmp;
                    msg = string.IsNullOrWhiteSpace(msg) && !string.IsNullOrWhiteSpace(MsgTmp) ? MsgTmp : msg;

                    this.ErrMsgs.Add(new string[]
                            {
                                code0 < 0 ? BeginDivSucc + Lang.commission + ": " + Lang.updatesuccessfull  + EndDiv :
                                            BeginDivErr + Lang.commission + ": " + (string.IsNullOrWhiteSpace(msgs[0]) ? Lang.validateErrMsg : msgs[0]) + EndDiv,
                                msg
                            });
                }
                else if (minPtChanged && positionTakingChanged)
                {
                    int.TryParse(codes[0], out code0);

                    if (codes.Length > 1)
                    {
                        int.TryParse(codes[1], out code1);
                    }
                    else
                    {
                        code1 = 0;
                        msgs.Add(string.Empty);
                    }

                    this.ErrCodes.Add(new string[]
                            {
                                code0 < 0 ? "-1" : code0.ToString(),
                                code1 < 0 && string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : code1.ToString()
                            });

                    var msg = code1 < 0 ? BeginDivSucc + Lang.positiontaking + ": " + Lang.updatesuccessfull + EndDiv + MsgTmp :
                                            string.IsNullOrWhiteSpace(msgs[1]) ? string.Empty : BeginDivErr + Lang.positiontaking + ": " + msgs[1] + EndDiv + MsgTmp;
                    msg = string.IsNullOrWhiteSpace(msg) && !string.IsNullOrWhiteSpace(MsgTmp) ? MsgTmp : msg;

                    this.ErrMsgs.Add(new string[]
                            {
                                code0 < 0 ? BeginDivSucc + Lang.minposition + ": " + Lang.updatesuccessfull  + EndDiv :
                                            BeginDivErr + Lang.commission + ": " + (string.IsNullOrWhiteSpace(msgs[0]) ? Lang.validateErrMsg : msgs[0]) + EndDiv,
                                msg
                            });
                }
                else if (commChanged)
                {
                    int.TryParse(codes[0], out code0);

                    codes.AsParallel().ForAll(o =>
                    {
                        if (Convert.ToInt32(o) > 0)
                        {
                            code0 = Convert.ToInt32(o);
                        }
                    });

                    msgs.AsParallel().ForAll(o =>
                    {
                        if (!string.IsNullOrWhiteSpace(o))
                        {
                            message = o;
                        }
                    });

                    this.ErrCodes.Add(new string[] { code0 < 0 && string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : code0.ToString() });
                    this.ErrMsgs.Add(new string[]
                            {
                                code0 < 0 ? BeginDivSucc + Lang.commission + ": " + Lang.updatesuccessfull  + EndDiv + MsgTmp :
                                            !string.IsNullOrWhiteSpace(message) ? BeginDivErr + Lang.commission + ": " + message + EndDiv :
                                            BeginDivErr + Lang.commission + ": " + (string.IsNullOrWhiteSpace(msgs[0]) ? Lang.validateErrMsg : msgs[0]) + EndDiv + MsgTmp,
                            });
                }
                else if (positionTakingChanged)
                {
                    int.TryParse(codes[0], out code0);

                    codes.AsParallel().ForAll(o =>
                    {
                        if (Convert.ToInt32(o) > 0)
                        {
                            code0 = Convert.ToInt32(o);
                        }
                    });

                    msgs.AsParallel().ForAll(o =>
                    {
                        if (!string.IsNullOrWhiteSpace(o))
                        {
                            message = o;
                        }
                    });

                    this.ErrCodes.Add(new string[] { code0 < 0 && string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : code0.ToString() });
                    this.ErrMsgs.Add(new string[]
                            {
                                code0 < 0 ? BeginDivSucc + Lang.positiontaking + ": " + Lang.updatesuccessfull + EndDiv + MsgTmp :
                                            !string.IsNullOrWhiteSpace(message) ? BeginDivErr + Lang.positiontaking + ": " + message + EndDiv :
                                            BeginDivErr + Lang.positiontaking + ": " + msgs[0] + EndDiv + this.MsgTmp
                            });
                }
                else if (minPtChanged)
                {
                    int.TryParse(codes[0], out code0);

                    codes.AsParallel().ForAll(o =>
                    {
                        if (Convert.ToInt32(o) > 0)
                        {
                            code0 = Convert.ToInt32(o);
                        }
                    });

                    msgs.AsParallel().ForAll(o =>
                    {
                        if (!string.IsNullOrWhiteSpace(o))
                        {
                            message = o;
                        }
                    });

                    this.ErrCodes.Add(new string[] { code0 < 0 && string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : code0.ToString() });
                    this.ErrMsgs.Add(new string[]
                            {
                                code0 < 0 ? BeginDivSucc + Lang.minposition + ": " + Lang.updatesuccessfull + EndDiv + MsgTmp :
                                            !string.IsNullOrWhiteSpace(message) ? BeginDivErr + Lang.minposition + ": " + message + EndDiv :
                                            BeginDivErr + Lang.minposition + ": " + (string.IsNullOrWhiteSpace(msgs[0]) ? Lang.invalidvalue : msgs[0]) + EndDiv + MsgTmp
                            });
                }
                else
                {
                    // Not changed tracking value
                    if (roleId != 2 && RequestCustRoleId != 1)
                    {
                        this.ErrCodes.Add(new string[]
                                    {
                                        "-1",
                                        "-1",
                                        string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : "1"
                                    });

                        this.ErrMsgs.Add(new string[]
                                {
                                    BeginDivSucc + Lang.commission + ": " + Lang.updatesuccessfull + EndDiv,
                                    BeginDivSucc + Lang.minposition + ": " + Lang.updatesuccessfull + EndDiv,
                                    BeginDivSucc + Lang.positiontaking + ": " + Lang.updatesuccessfull + EndDiv + MsgTmp
                                });
                    }
                    else if (CurrentUserSession.RoleId - RequestCustRoleId == 3 ||
                        CurrentUserSession.RoleId - RequestCustRoleId == 2)
                    {
                        this.ErrCodes.Add(new string[]
                                    {
                                        string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : "1"
                                    });

                        this.ErrMsgs.Add(new string[]
                                {
                                    BeginDivSucc + Lang.positiontaking + ": " + Lang.updatesuccessfull + EndDiv + MsgTmp
                                });
                    }
                    else
                    {
                        this.ErrCodes.Add(new string[]
                                    {
                                        "-1",
                                        string.IsNullOrWhiteSpace(MsgTmp) ? "-1" : "1"
                                    });

                        this.ErrMsgs.Add(new string[]
                                {
                                    BeginDivSucc + Lang.commission + ": " + Lang.updatesuccessfull + EndDiv,
                                    BeginDivSucc + Lang.positiontaking + ": " + Lang.updatesuccessfull + EndDiv + MsgTmp
                                });
                    }
                }
            });

            this.ProcessError();
        }

        /// <summary>
        /// Gets manual insert account request
        /// </summary>
        /// <param name="custId">Customer id need to get info</param>
        /// <returns>InsertAccountRequest object</returns>
        protected virtual InsertAccountRequest GetManualInsertAccountRequest(int custId)
        {
            var custInfo = this.AccInfo.GetCustInfo(custId);

            var result = new Fanex.LiveCasinoSetting.Request.InsertAccountRequest()
            {
                AgentId = custInfo.AIBCCustID,
                CustId = custId,
                CustUserName = custInfo.UserName,
                ExchangeId = CurrentUserSession.CurrencyId,
                SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                IsDisabledLiveCasino = CurrentUserSession.IsLiveCasinoDisabled,
                IsEnableDownline = false,
                Language = CurrentUserSession.LangKey,
                MasterId = custInfo.MIBCCustID,
                RoleId = custInfo.RoleId,
                SignUpForm = 2, // Agency system
                SubAccId = CurrentUserSession.SubAccId,
                SubAccName = CurrentUserSession.SubUserName,
                SuperId = custInfo.SIBCCustID,
                SyncLiveCasino = CurrentUserSession.IsLiveCasinoSync,
                UserId = CurrentUserSession.UserId,
                UserName = CurrentUserSession.UserName,
                IsAutoInsert = false // Manually insert account
            };

            return result;
        }

        /// <summary>
        /// Synchronize customers
        /// </summary>
        /// <param name="isGameSetting">Whether or not a game setting request</param>
        /// <param name="result">Result of update action</param>
        protected virtual void SynManually(bool isGameSetting = false, Dictionary<int, Fanex.LiveCasinoSetting.Model.Result> result = null)
        {
            if (CurrentUserSession.RoleId - this.RequestCustRoleId != 1)
            {
                this.UnsyncIds.Clear();
                return;
            }

            var pt = new PTSetting();

            lock (lockSync)
            {
                var log = new Fanex.LiveCasinoSetting.Business.LogWriterService();
                var r = new Dictionary<int, LiveCasinoSetting.Model.Result>();

                foreach (var custId in this.UnsyncIds)
                {
                    var error = 0;
                    var results = new LiveCasinoSetting.Model.Result();

                    if (isGameSetting)
                    {
                        if (CurrentUserSession.IsSuper)
                        {
                            results = this.SuperUpdateMasterPT(custId, out pt);
                            r.Add(custId, results);
                        }
                        else if (CurrentUserSession.IsMaster)
                        {
                            results = this.MasterUpdateAgentPT(custId, out pt);
                            r.Add(custId, results);
                        }
                        else if (CurrentUserSession.IsAgent)
                        {
                            results = this.AgentUpdateMemberPT(custId, out pt);
                            r.Add(custId, results);
                        }

                        if (results == null || !results.ErrorCode.TrimEnd('\a').Split('\a').ToList().TrueForAll(o => int.TryParse(o, out error) == true && error < 0))
                        {
                            this.WrongPtIds.Add(custId);
                        }
                    }

                    var request = this.GetManualInsertAccountRequest(custId);

                    var res = new LiveCasinoSetting.Model.Result();

                    if (results != null && isGameSetting && (string.IsNullOrWhiteSpace(results.ErrorCode) || results.ErrorCode.TrimEnd('\a').Split('\a').ToList().TrueForAll(o => int.TryParse(o, out error) == true && error < 0)))
                    {
                        res = this.GameSetting.InsertAccount(request);
                    }
                    else if (result != null && !isGameSetting && (string.IsNullOrWhiteSpace(result[custId].ErrorCode) || result[custId].ErrorCode.TrimEnd('\a').Split('\a').ToList().TrueForAll(o => int.TryParse(o, out error) == true && error < 0)))
                    {
                        res = this.GameSetting.InsertAccount(request);
                    }

                    if (string.IsNullOrWhiteSpace(res.ErrorCode) && this.GameTypes.Count > 0)
                    {
                        var oldValues = this.GameSetting.GetByCustomerId(custId);

                        if (oldValues == null || oldValues.Count == 0)
                        {
                            oldValues = this.GameSetting.GetByCustomerId(CurrentUserSession.CustId);
                        }

                        oldValues.ForEach(itemGet => itemGet.BettingLimit.ForEach(item =>
                        {
                            this.GameTypes.ForEach(g =>
                            {
                                if (g.BetTypeId == itemGet.BetTypeId)
                                {
                                    g.BettingLimit.ForEach(b =>
                                    {
                                        if (b.Id == item.Id && ((b.IsSelected != item.IsSelected) || b.IsSelected))
                                        {
                                            b.IsDirty = true;
                                        }
                                    });
                                }
                            });
                        }));

                        var resultList = this.GameSetting.UpdateStakeLevel(custId, this.RequestCustRoleId, this.GameTypes);
                        this.GameResult.Add(custId, resultList);

                        if (string.IsNullOrWhiteSpace(resultList[this.GameTypes.ElementAt(0).BetTypeId].ErrorCode))
                        {
                            // Update sync status for acc
                            this.GameSetting.UpdateSyncLiveCasinoInfo(request);

                            // Write log manual insert account
                            var custList = new List<KeyValuePair<int, string>>();
                            custList.Add(new KeyValuePair<int, string>(request.CustId, request.CustUserName));

                            log.WriteLog(new Fanex.LiveCasinoSetting.Model.Log.LogManualInsertAccount()
                                    {
                                        Action = this.LogActionManual,
                                        AgentId = request.AgentId,
                                        CustId = request.CustId,
                                        CustUserName = request.CustUserName,
                                        ExchangeId = request.ExchangeId,
                                        SignUpIp = request.SignUpIp,
                                        Language = request.Language,
                                        MasterId = request.MasterId,
                                        RoleId = request.RoleId,
                                        SignUpForm = request.SignUpForm,
                                        SubAccId = request.SubAccId,
                                        SubAccName = request.SubAccName,
                                        SuperId = request.SuperId,
                                        UserId = request.UserId,
                                        UserName = request.UserName,
                                        ErrorCode = res.ErrorCode,
                                        ErrorMessage = res.ErrorMessage,
                                        CustomerList = custList
                                    });

                            if (!isGameSetting && !this.IsSubmit)
                            {
                                // Write log update stake level
                                log.WriteLog(new LogUpdateStakeLevel(oldValues, this.GameTypes, resultList)
                                            {
                                                Action = this.LogActionUpdateStakeLevel,
                                                ErrorCode = resultList[this.GameTypes.ElementAt(0).BetTypeId].ErrorCode,
                                                ErrorMessage = resultList[this.GameTypes.ElementAt(0).BetTypeId].ErrorMessage,
                                                RoleId = this.RequestCustRoleId,
                                                SubAccId = CurrentUserSession.SubAccId,
                                                SubAccName = CurrentUserSession.SubUserName,
                                                CustId = custId,
                                                SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                                                Language = CurrentUserSession.LangKey,
                                                UserId = CurrentUserSession.UserId
                                            });

                                this.MsgTmp = BeginDivSucc + Lang.gameSetting + ": " + Lang.updatesuccessfull + EndDiv;
                                this.TmpMsgs.Add(custId, this.MsgTmp);
                            }

                            if (isGameSetting && !this.IsSubmit)
                            {
                                this.MsgTmp = BeginDivSucc + Lang.gameSetting + ": " + Lang.updatesuccessfull + EndDiv;
                                this.TmpMsgs.Add(custId, this.MsgTmp);
                            }
                        }
                        else
                        {
                            if (!isGameSetting)
                            {
                                this.MsgTmp = BeginDivErr + Lang.gameSetting + ": " + Lang.gameSettingErrMsgStake + EndDiv;
                            }
                            else
                            {
                                var dirty = false;

                                foreach (var game in this.GameTypes)
                                {
                                    if (game.IsDirty)
                                    {
                                        dirty = true;
                                        break;
                                    }
                                }

                                if (!dirty)
                                {
                                    this.MsgTmp = BeginDivErr + Lang.gameSetting + ": " + Lang.gameSettingErrMsgStake + EndDiv;
                                }
                            }

                            this.TmpMsgs.Add(custId, this.MsgTmp);
                        }
                    }
                    else
                    {
                        this.MsgTmp = BeginDivErr + Lang.gameSetting + ": " + Lang.gameSettingErrMsg + EndDiv;
                        this.TmpMsgs.Add(custId, this.MsgTmp);
                    }
                }

                if (this.UnsyncIds.Count > 0 && r.Count > 0)
                {
                    if (CurrentUserSession.IsSuper)
                    {
                        this.ProcessMessage(
                                r,
                                pt.IsCommChanged,
                                pt.IsMaxPTChanged || pt.Upline.IsIsAutoPresetPTChanged || pt.Upline.IsAutoPresetPTChanged || pt.Upline.IsPresetPTChanged,
                                pt.IsMinPTChanged);
                    }
                    else if (CurrentUserSession.IsMaster)
                    {
                        this.ProcessMessage(
                                r,
                                pt.IsCommChanged,
                                pt.IsMaxPTChanged || pt.Upline.IsIsAutoPresetPTChanged || pt.Upline.IsAutoPresetPTChanged || pt.Upline.IsPresetPTChanged,
                                pt.IsMinPTChanged);
                    }
                    else if (CurrentUserSession.IsAgent)
                    {
                        this.ProcessMessage(
                                r,
                                pt.IsCommChanged,
                                pt.Upline.IsPresetPTChanged,
                                false);
                    }
                }

                this.UnsyncIds.Clear();
            }
        }

        /// <summary>
        /// Process error message
        /// </summary>
        private void ProcessError()
        {
            var success = true;
            var tmp = string.Empty;

            this.ErrCodes.ForEach(o =>
            {
                tmp = string.Empty;
                success = true;

                o.AsParallel().ForAll(s =>
                {
                    if (int.Parse(s) >= 0)
                    {
                        success = false;
                        tmp += "-1$";
                    }
                });

                this.ErrCode += success ? "0$" : tmp;
            });

            tmp = string.Empty;

            this.ErrMsgs.ForEach(o =>
            {
                tmp = string.Empty;

                o.AsParallel().ForAll(s =>
                {
                    tmp += s;
                });

                this.ErrMsg += tmp + "$";
            });
        }

        /// <summary>
        /// Super edits master pt - comm
        /// </summary>
        /// <param name="custId">Customer id</param>
        /// <param name="resultPT">Out result of this update action</param>
        /// <returns>Result object</returns>
        private LiveCasinoSetting.Model.Result SuperUpdateMasterPT(int custId, out PTSetting resultPT)
        {
            resultPT = null;

            var pt = this.PtSetting.GetMasterPTSeting(this.RequestCustId, CurrentUserSession.CustId);

            if ((this.Custids.Count == 0 && !this.AccInfo.GetCustInfo(custId).LiveCasinoSync) || this.Custids.Count > 0)
            {
                pt.PropertiesChangedList.Add("MaxPT");
                pt.PropertiesChangedList.Add("MinPT");
                pt.PropertiesChangedList.Add("Comm");

                ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add("PresetPT");
                ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add("AutoPresetPT");
                ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add("IsAutoPresetPT");
            }
            else
            {
                // Handles Master values changed
                ((INotifyPropertyChanged)pt).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!pt.PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         pt.PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };

                // Handles Super values changed
                ((INotifyPropertyChanged)pt.Upline).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!((SuperPTSetting)pt.Upline).PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };
            }

            // Super values
            ((SuperPTSetting)pt.Upline).PresetPT = this.PresetPT;
            ((SuperPTSetting)pt.Upline).AutoPresetPT = this.AutoPresetPT;
            ((SuperPTSetting)pt.Upline).IsAutoPresetPT = this.IsAutoPresetPT;
            ((SuperPTSetting)pt.Upline).IsUpdateAllMember = this.IsUpdateAllMember;
            ((SuperPTSetting)pt.Upline).NewMemberPT = this.NewMemberPT;

            // Master values
            pt.MaxPT = this.MaxPT;
            pt.MinPT = this.MinPT;
            pt.Comm = this.Comm;

            resultPT = pt;

            return this.PtSetting.UpdateMasterPTSetting(
                new RequestContext()
                {
                    CustId = custId,
                    SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    Language = CurrentUserSession.LangKey,
                    SubAccId = CurrentUserSession.SubAccId,
                    UplineId = CurrentUserSession.CustId,
                    UserId = CurrentUserSession.UserId
                },
                pt);
        }

        /// <summary>
        /// Master edits agent pt - comm
        /// </summary>
        /// <param name="custId">Customer id</param>
        /// <param name="resultPT">Out result of this update action</param>
        /// <returns>Result object</returns>
        private LiveCasinoSetting.Model.Result MasterUpdateAgentPT(int custId, out PTSetting resultPT)
        {
            resultPT = null;

            var pt = this.PtSetting.GetAgentPTSetting(this.RequestCustId, CurrentUserSession.CustId);

            if ((this.Custids.Count == 0 && !this.AccInfo.GetCustInfo(custId).LiveCasinoSync) || this.Custids.Count > 0)
            {
                pt.PropertiesChangedList.Add("MaxPT");
                pt.PropertiesChangedList.Add("MinPT");
                pt.PropertiesChangedList.Add("Comm");

                ((PTSetting)pt.Upline).PropertiesChangedList.Add("PresetPT");
                ((PTSetting)pt.Upline).PropertiesChangedList.Add("AutoPresetPT");
                ((PTSetting)pt.Upline).PropertiesChangedList.Add("IsAutoPresetPT");
            }
            else
            {
                // Handles Agent values changed
                ((INotifyPropertyChanged)pt).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!pt.PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         pt.PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };

                // Handles Master values changed
                ((INotifyPropertyChanged)pt.Upline).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!((PTSetting)pt.Upline).PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         ((PTSetting)pt.Upline).PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };
            }

            // Master values
            ((PTSetting)pt.Upline).PresetPT = this.PresetPT;
            ((PTSetting)pt.Upline).AutoPresetPT = this.AutoPresetPT;
            ((PTSetting)pt.Upline).IsAutoPresetPT = this.IsAutoPresetPT;
            ((PTSetting)pt.Upline).IsUpdateAllMember = this.IsUpdateAllMember;
            ((PTSetting)pt.Upline).NewMemberPT = this.NewMemberPT;

            // Agent values
            pt.Comm = this.Comm;
            pt.MinPT = this.MinPT;
            pt.MaxPT = this.MaxPT;

            resultPT = pt;

            return this.PtSetting.UpdateAgentPTSetting(
                new RequestContext()
                {
                    CustId = custId,
                    SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    Language = CurrentUserSession.LangKey,
                    SubAccId = CurrentUserSession.SubAccId,
                    UplineId = CurrentUserSession.CustId,
                    UserId = CurrentUserSession.UserId
                },
                pt);
        }

        /// <summary>
        /// Agent edits member pt - comm
        /// </summary>
        /// <param name="custId">Customer id</param>
        /// <param name="resultPT">Out result of this update action</param>
        /// <returns>Result object</returns>
        private LiveCasinoSetting.Model.Result AgentUpdateMemberPT(int custId, out PTSetting resultPT)
        {
            resultPT = null;

            var pt = this.PtSetting.GetMemberPTSetting(this.RequestCustId, CurrentUserSession.CustId);

            if ((this.Custids.Count == 0 && !this.AccInfo.GetCustInfo(custId).LiveCasinoSync) || this.Custids.Count > 0)
            {
                pt.PropertiesChangedList.Add("Comm");
                ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add("PresetPT");
            }
            else
            {
                ((INotifyPropertyChanged)pt).PropertyChanged
                                 += (o, arg) =>
                                 {
                                     if (!pt.PropertiesChangedList.Contains(arg.PropertyName))
                                     {
                                         pt.PropertiesChangedList.Add(arg.PropertyName);
                                     }
                                 };

                ((INotifyPropertyChanged)pt.Upline).PropertyChanged
                         += (o, arg) =>
                         {
                             if (!((SuperPTSetting)pt.Upline).PropertiesChangedList.Contains(arg.PropertyName))
                             {
                                 ((SuperPTSetting)pt.Upline).PropertiesChangedList.Add(arg.PropertyName);
                             }
                         };
            }

            ((SuperPTSetting)pt.Upline).PresetPT = this.PresetPT;
            pt.Comm = this.Comm;

            resultPT = pt;

            return this.PtSetting.UpdateMemberPTSetting(
                new RequestContext()
                {
                    CustId = custId,
                    SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    Language = CurrentUserSession.LangKey,
                    SubAccId = CurrentUserSession.SubAccId,
                    UplineId = CurrentUserSession.CustId,
                    UserId = CurrentUserSession.UserId
                },
                pt);
        }
    }
}