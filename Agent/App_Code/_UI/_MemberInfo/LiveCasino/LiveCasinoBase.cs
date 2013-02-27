// --------------------------------------------------------------------------
// <copyright file="LiveCasinoBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.4.13">Format styles</change>
// </history>

namespace Fanex.Age.UI.LiveCasino
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.LiveCasino.Common;
    using Fanex.LiveCasinoSetting.Business;
    using Fanex.LiveCasinoSetting.Model;
    using Resources;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Sunplus.Agent.Model.BL.Authorization;

    /// <summary>
    /// Base class for live casino.
    /// Already rendered CustInfo, GameSetting, Buttons
    /// </summary>
    public abstract class LiveCasinoBase : TplPage
    {
        /// <summary>
        /// Constant for checked value of combobox or checkbox
        /// </summary>
        protected const string CHECKED = "checked='checked'";

        /// <summary>
        /// Constant for selected value of option
        /// </summary>
        protected const string SELECTED = "selected='selected'";

        /// <summary>
        /// Constant for disabled value of input tag
        /// </summary>
        protected const string DISABLED = "disabled='disabled'";

        /// <summary>
        /// Constant for customer info in hastable
        /// </summary>
        protected const string CustInfo = "custInfo";

        /// <summary>
        /// Constant for position taking of setting page
        /// </summary>
        protected const string PtCommSetting = "ptSetting";

        /// <summary>
        /// Constant for game setting of setting page
        /// </summary>
        protected const string GameSetting = "gameSetting";

        /// <summary>
        /// Constant to specify type of checkbox
        /// </summary>
        protected const string CHECKBOX = "checkbox";

        /// <summary>
        /// Constant to specify type of radio
        /// </summary>
        protected const string RADIO = "radio";

        /// <summary>
        /// Constant of PT step
        /// </summary>
        protected const double IntervalPT = 0.01;

        /// <summary>
        /// Constant of commission step
        /// </summary>
        protected const double IntervalComm = 0.0005;

        /// <summary>
        /// Role id of editted customer
        /// </summary>
        private int requestCustRoleId = 0;

        /// <summary>
        /// Customer id of editted
        /// </summary>
        private int requestCustId = 0;

        /// <summary>
        /// Error code of this page
        /// </summary>
        private int errCode = 0;

        /// <summary>
        /// Numbers of customers are editted
        /// </summary>
        private int count = 0;

        /// <summary>
        /// First username of editted customer
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// "1" if multiple update
        /// </summary>
        private string isMulti = string.Empty;

        /// <summary>
        /// List of username seperated by comma
        /// </summary>
        private string userNames = string.Empty;

        /// <summary>
        /// Disabled state of current editted customer
        /// </summary>
        private string disable = string.Empty;

        /// <summary>
        /// Error message of this page
        /// </summary>
        private string errMsg = null;

        /// <summary>
        /// List of cust id
        /// </summary>
        private string ids = string.Empty;

        /// <summary>
        /// Is this account is disabled?
        /// </summary>
        private bool isDisabledAccount = false;

        /// <summary>
        /// List of customer ids
        /// </summary>
        private List<int> custids = new List<int>();

        /// <summary>
        /// Setting service class
        /// </summary>
        private SettingService pt = new SettingService();

        /// <summary>
        /// Game setting service class
        /// </summary>
        private GameSettingService game = new GameSettingService();

        /// <summary>
        /// Setting page template
        /// </summary>
        private SettingPage_Template settingPage = null;

        /// <summary>
        /// Data of this page
        /// </summary>
        private Hashtable data = new Hashtable();

        /// <summary>
        /// Synchronize status of editted customer
        /// </summary>
        private bool sync = true;

        /// <summary>
        /// Account info business object
        /// </summary>
        private AccountInfoBO accInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveCasinoBase"/> class
        /// </summary>
        public LiveCasinoBase()
        {
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected int RequestCustRoleId
        {
            get { return this.requestCustRoleId; }
            set { this.requestCustRoleId = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected int RequestCustId
        {
            get { return this.requestCustId; }
            set { this.requestCustId = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected int ErrCode
        {
            get { return this.errCode; }
            set { this.errCode = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected string IsMulti
        {
            get { return this.isMulti; }
            set { this.isMulti = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected string UserNames
        {
            get { return this.userNames; }
            set { this.userNames = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected string Disable
        {
            get { return this.disable; }
            set { this.disable = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected string Ids
        {
            get { return this.ids; }
            set { this.ids = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether account is disabled
        /// </summary>
        protected bool IsDisabledAccount
        {
            get { return this.isDisabledAccount; }
            set { this.isDisabledAccount = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected List<int> Custids
        {
            get { return this.custids; }
            set { this.custids = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected SettingService Pt
        {
            get { return this.pt; }
            set { this.pt = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected GameSettingService Game
        {
            get { return this.game; }
            set { this.game = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected SettingPage_Template SettingPage
        {
            get { return this.settingPage; }
            set { this.settingPage = value; }
        }

        /// <summary>
        /// Gets or sets this property
        /// </summary>
        protected Hashtable Data
        {
            get { return this.data; }
            set { this.data = value; }
        }

        /// <summary>
        /// Render UI
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            var gameRowCount = 0;

            // Inits Customer Info
            var custInfo = this.Data[CustInfo] as Fanex.LiveCasinoSetting.Model.CustomerInfo;

            // Inits Setting Page info
            this.SettingPage = CasinoUI.SettingPage;
            this.SettingPage.UserNames = this.IsMulti.Equals("1") ? this.UserNames : null;
            this.SettingPage.UserIDs = this.Ids;
            this.SettingPage.btndisabled = this.IsDisabledAccount ? DISABLED : string.Empty;
            this.SettingPage.Msg.ErrorMsg = new ErrorMsg_Control(this.SettingPage);

            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableMemberWinLimit)
            {
                var comboBox = CasinoUI.CommControl;
                var comboItems = new MemberWinLimit().Limits;
                var itemText = string.Empty;

                switch (Convert.ToInt32(custInfo.Limit.MaxWinLimitUpline))
                {
                    case 3:
                        comboItems.Remove(5);
                        comboItems.Remove(10);
                        comboItems.Remove(0);
                        break;
                    case 5:
                        comboItems.Remove(10);
                        comboItems.Remove(0);
                        break;
                    case 10:
                        comboItems.Remove(0);
                        break;
                    default:
                        break;
                }

                comboBox.Items.Clear();
                comboBox.level = "0";

                for (int i = 0; i < comboItems.Count(); i++)
                {
                    var item = CasinoUI.CommControl_Items;

                    switch (comboItems.ElementAt(i))
                    {
                        case 0:
                            itemText = Lang.Unlimited;
                            break;
                        case 3:
                            itemText = "3X";
                            break;
                        case 5:
                            itemText = "5X";
                            break;
                        case 10:
                            itemText = "10X";
                            break;
                        default:
                            break;
                    }

                    item._Value = itemText.Replace("X", string.Empty).Replace(Lang.Unlimited, "0");
                    item.Text = itemText;
                    item.selected =
                        (Convert.ToInt32(custInfo.Limit.MaxWinLimit).ToString() == itemText.Replace("X", string.Empty) ||
                        Convert.ToInt32(custInfo.Limit.MaxWinLimit) == 0) ? SELECTED : string.Empty;

                    comboBox.Items.Append(item);
                }

                this.SettingPage.MemberWin.detail.MaximumWin = Lang.MaximumWin;
                this.SettingPage.MemberWin.detail.OnGivenCredit = Lang.OnGivenCredit;
                this.SettingPage.MemberWin.detail.MaximumWinValues = comboBox;
                this.SettingPage.MemberWin.detail.autoReset = Lang.autoReset;
                this.SettingPage.MemberWin.MemberWinLimit = Lang.MemberWinLimit;
                this.SettingPage.MemberWin.SaveMemberWinLimit = Lang.SaveMemberWinLimit;
                this.SettingPage.MemberWin.detail.Checked =
                    custInfo.Limit.IsDailyAutoResetUpline && custInfo.Limit.IsDailyAutoReset ? CHECKED : string.Empty;
                this.SettingPage.MemberWin.detail.Disabled =
                    custInfo.Limit.IsDailyAutoResetUpline ? string.Empty : DISABLED;

                if (CurrentUserSession.IsAgent || requestCustRoleId == 1)
                {
                    this.SettingPage.MemberWin.detail.WinLimit.ClickToViewCurrentWinLimit = Lang.ClickToViewCurrentWinLimit;
                }
                else
                {
                    this.SettingPage.MemberWin.detail.WinLimit.Clear();
                }
            }
            else
            {
                this.SettingPage.MemberWin.Clear();
            }

            if (this.IsMulti.Equals("1"))
            {
                this.SettingPage.CustInfo.Clear();

                if (CurrentUserSession.IsAgent && this.RequestCustRoleId == 1)
                {
                    var enableValue = CasinoUI.BettingLimitCtrl;
                    var enableValueItem = CasinoUI.BettingLimitCtrl_Items;

                    enableValueItem.ID = "chkEnableLiveCS";
                    enableValueItem.inputType = CHECKBOX;
                    enableValueItem.isChecked = !custInfo.DisableLiveCasino && !custInfo.UplineDisableLiveCasino ? CHECKED : string.Empty;
                    enableValueItem.order = "0";
                    enableValueItem.Range = Lang.enable;
                    enableValueItem.disabled = custInfo.UplineDisableLiveCasino ? DISABLED : string.Empty;

                    enableValue.ID = "enableLiveCS";
                    enableValue.Items = enableValueItem;

                    this.SettingPage.Enable.Enable_vl = enableValue;
                }
                else
                {
                    this.SettingPage.Enable.Clear();
                }
            }
            else
            {
                this.SettingPage.Enable.Clear();

                if (CurrentUserSession.IsAgent && this.RequestCustRoleId == 1)
                {
                    var enableValue = CasinoUI.BettingLimitCtrl;
                    var enableValueItem = CasinoUI.BettingLimitCtrl_Items;

                    enableValueItem.ID = "chkEnableLiveCS";
                    enableValueItem.inputType = CHECKBOX;
                    enableValueItem.isChecked = !custInfo.DisableLiveCasino && !custInfo.UplineDisableLiveCasino ? CHECKED : string.Empty;
                    enableValueItem.order = "0";
                    enableValueItem.Range = string.Empty;
                    enableValueItem.disabled = custInfo.UplineDisableLiveCasino ? DISABLED : string.Empty;

                    enableValue.ID = "enableLiveCS";
                    enableValue.Items = enableValueItem;

                    this.SettingPage.CustInfo.Enable = Lang.enable;
                    this.SettingPage.CustInfo.Enable_vl = enableValue;
                }
                else
                {
                    this.SettingPage.CustInfo.Enable = string.Empty;
                    this.SettingPage.CustInfo.Enable_vl = string.Empty;
                }

                this.SettingPage.CustInfo.UserName = Lang.username;
                this.SettingPage.CustInfo.NickName = Lang.nickname;
                this.SettingPage.CustInfo.FirstName = Lang.firstname;
                this.SettingPage.CustInfo.Phone = Lang.lblPhone;
                this.SettingPage.CustInfo.LastName = Lang.lastname;
                this.SettingPage.CustInfo.Mobile = Lang.lblMobilePhone;
                this.SettingPage.CustInfo.Fax = Lang.lblFax;

                this.SettingPage.CustInfo.UserName_vl = custInfo.UserName;
                this.SettingPage.CustInfo.NickName_vl = this.RequestCustRoleId == 1 ? custInfo.NickName : string.Empty;
                this.SettingPage.CustInfo.FirstName_vl = custInfo.FirstName;
                this.SettingPage.CustInfo.Phone_vl = custInfo.Phone;
                this.SettingPage.CustInfo.LastName_vl = custInfo.LastName;
                this.SettingPage.CustInfo.Mobile_vl = custInfo.Mobile;
                this.SettingPage.CustInfo.Fax_vl = custInfo.Fax;
            }

            // Load titles for form
            this.SettingPage.GameSetting = Lang.gameSetting;
            this.SettingPage.SaveGameSetting = Lang.saveGameSetting;
            this.SettingPage.PTComm = Lang.PositionTakingCommission;
            this.SettingPage.SavePTComm = Lang.SavePositionTakingCommission;
            this.SettingPage.GameType = Lang.lblGameType;
            this.SettingPage.BettingLimit = Lang.lblBettingLimit;
            this.SettingPage.Submit = Lang.submit;
            this.SettingPage.EditGame = this.IsMulti.Equals("1") ? Lang.EditLiveCasinoMultiple : Lang.EditLiveCasino;

            // Load Game Setting section
            var gameSetting = this.Data[GameSetting] as List<GameType>;

            this.SettingPage.GameRow.Clear();

            var inputType =
                CurrentUserSession.IsAgent || // Agent edits member
                CurrentUserSession.RoleId - this.RequestCustRoleId == 2 || // Master edits member
                CurrentUserSession.RoleId - this.RequestCustRoleId == 3 ? // Super edits member
                RADIO : CHECKBOX;

            var gameSettings = gameSetting.GroupBy(o => o.Id);
            var language = CurrentUserSession.LangKey;
            var allUnchecked = false;
            var idx = 0;

            gameSettings.ToList().ForEach(item =>
            {
                var gameRow = CasinoUI.SettingPage_GameRow;
                var stakeLevel = CasinoUI.BettingLimitCtrl;
                var stakeItem = CasinoUI.BettingLimitCtrl_Items;

                var hasSelected = false;

                stakeLevel.Items.Clear();

                gameRow.GameType_vl = CacheRef.GetBettype(item.ElementAt(0).BetTypeId);
                gameRow.order = idx.ToString();
                gameRow.gameTypeID = item.ElementAt(0).Id.ToString();
                gameRow.betTypeID = item.ElementAt(0).BetTypeId.ToString();

                item.AsParallel().ForAll(gameType =>
                    {
                        var count = 0;
                        var allHasNotChecked = true;

                        allHasNotChecked = gameType.BettingLimit.TrueForAll(o => !o.IsSelected);

                        if (allHasNotChecked)
                        {
                            allUnchecked = true;
                        }

                        stakeLevel.ID = gameType.Id.ToString();

                        gameType.BettingLimit.ForEach(bettingLimit =>
                            {
                                if ((inputType == RADIO && (!custInfo.IsSyncLiveCasino && count == 0)) || (inputType == RADIO && allHasNotChecked && count == 0))
                                {
                                    stakeItem.isChecked = CHECKED;
                                    hasSelected = true;
                                }
                                else
                                {
                                    stakeItem.isChecked = bettingLimit.IsSelected && !hasSelected ? CHECKED : string.Empty;
                                }

                                stakeItem.disabled = string.Empty;
                                stakeItem.order = idx.ToString();
                                stakeItem.ID = bettingLimit.Id.ToString();
                                stakeItem.inputType = inputType;
                                stakeItem.Range = bettingLimit.Text;

                                stakeLevel.Items.Append(stakeItem);

                                count++;
                            });

                        idx++;
                        gameRowCount++;
                    });

                gameRow.BettingLimit_vl = stakeLevel;

                this.SettingPage.GameRow.Append(gameRow);
            });

            // Register script variables
            this.RegisterJavascriptObject("roleId", CurrentUserSession.RoleId);
            this.RegisterJavascriptObject("custRoleId", this.RequestCustRoleId);
            this.RegisterJavascriptObject("custId", this.RequestCustId);
            this.RegisterJavascriptObject("disabled", this.IsDisabledAccount);

            this.RegisterJavascriptObject("noAccess", Lang.strNotAccessPage);
            this.RegisterJavascriptObject("isMult", this.IsMulti.Equals("1"));
            this.RegisterJavascriptObject("userName", this.UserName);
            this.RegisterJavascriptObject("warningMsg", Lang.warningUncheckAll);

            this.RegisterJavascriptObject("gameRows", gameRowCount);
            this.RegisterJavascriptObject("sync", this.sync);
            this.RegisterJavascriptObject("uncheck", allUnchecked);
            this.RegisterJavascriptObject("uncheckMsg", Lang.confirmUncheckAll);

            this.RegisterJavascriptObject("currentWinLimit", Lang.CurrentWinLimit);
            this.RegisterJavascriptObject("of", Lang.of);
            this.RegisterJavascriptObject("showWinLimit", Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableMemberWinLimit);

            this.RegisterJsPT(this.Data[PtCommSetting] as PTSetting);
        }

        /// <summary>
        /// Invoke data
        /// </summary>
        /// <param name="p">Params of this request</param>
        /// <returns>Object, which contains data for current request</returns>
        public override object InvokeData(System.Collections.Hashtable p)
        {
            var ht = new Hashtable();

            var model =
                (CurrentUserSession.IsSuper && this.RequestCustRoleId == 3) ? this.Pt.GetMasterSetting(this.RequestCustId, CurrentUserSession.CustId) :
                (CurrentUserSession.IsSuper && this.RequestCustRoleId == 1) ? this.Pt.SuperGetMemberSetting(this.RequestCustId, 0) :
                (CurrentUserSession.IsMaster && this.RequestCustRoleId == 2) ? this.Pt.GetAgentSetting(this.RequestCustId, CurrentUserSession.CustId) :
                (CurrentUserSession.IsMaster && this.RequestCustRoleId == 1) ? this.Pt.MasterGetMemberSetting(this.RequestCustId, 0) :
                this.Pt.GetMemberSetting(this.RequestCustId, CurrentUserSession.CustId);

            var custInfo = model.CustomerInfo;
            var settingPT = model.PTSetting;
            var gameSetting = model.GameSetting;

            ht.Add(CustInfo, custInfo);
            ht.Add(PtCommSetting, settingPT);
            ht.Add(GameSetting, gameSetting);

            this.Data = ht;

            return ht;
        }

        /// <summary>
        /// Process request
        /// </summary>
        /// <param name="p">Params of request</param>
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.accInfo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            this.IsMulti = Request.QueryString["isMult"];
            this.Disable = Request.QueryString["disable"];
            this.Count = Convert.ToInt32(Request.QueryString["count"]);

            var users = Request.QueryString["custnames"];
            string[] userList = null;

            if (users != null)
            {
                userList = users.Split('^');
                var userIds = Request.QueryString["custids"].Split('^');
                this.Custids = new List<int>();

                userIds.ToList().ForEach(userId =>
                        {
                            var i = 0;

                            if (!string.IsNullOrWhiteSpace(userId.Trim()))
                            {
                                var id = Convert.ToInt32(userId.Trim());

                                this.Custids.Add(id);

                                if (sync)
                                {
                                    sync = accInfo.GetCustInfo(id).LiveCasinoSync;
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(userList.ElementAt(i).Trim()))
                            {
                                this.UserNames += userList.ElementAt(i).Trim() + ", ";
                            }

                            i++;
                        });

                this.UserNames = this.UserNames.Substring(0, this.UserNames.Length - 2);
            }

            if (this.IsMulti != null && this.IsMulti.Equals("1"))
            {
                this.Ids = Request.QueryString["custids"].Trim();
                this.Ids = this.Ids.Substring(0, this.Ids.Length - 1);

                this.RequestCustId = this.Custids.ElementAt(0);
                this.UserName = userList.ElementAt(0);
            }
            else
            {
                if (Request.QueryString["custids"] != null && Request.QueryString["custids"].Contains('^'))
                {
                    this.RequestCustId = Convert.ToInt32(Request.QueryString["custids"].Split('^').ElementAt(0));
                    this.UserName = string.Empty;
                }
                else
                {
                    this.RequestCustId = Convert.ToInt32(Request.QueryString["custids"]);
                    this.UserName = Request.QueryString["username"];
                }

                this.sync = this.accInfo.GetCustInfo(this.RequestCustId).LiveCasinoSync;
            }

            this.RequestCustRoleId = Convert.ToInt32(Request.QueryString["roleid"]);
        }

        /// <summary>
        /// Validates rules
        /// </summary>
        /// <param name="p">Params of request</param>
        /// <returns>Object, null if pass validation rules</returns>
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            var disabled = !new AuthorizationBO(CurrentUserSession.NetSessionId).IsCustomerEnable(this.RequestCustId, out this.errMsg);
            this.IsDisabledAccount = disabled/* || CurrentUserSession.IsInternal*/;

            var custInfo = this.accInfo.GetCustInfo(Convert.ToInt32(Request.QueryString["custids"].Split('^').ElementAt(0)));

            var roleDownline = CurrentUserSession.RoleId - this.RequestCustRoleId;

            if ((!custInfo.LiveCasinoSync && roleDownline != 1) || disabled/* || CurrentUserSession.IsLiveCasinoDisabled*/)
            {
                return new ErrorPage_Page(Lang.strNotAccessPage);
            }

            if (CurrentUserSession.RoleId == 4)
            {
                // Super edits master/member
                if (this.RequestCustId <= 0 || (roleDownline != 1 && roleDownline != 3))
                {
                    return new ErrorPage_Page(Lang.strNotAccessPage);
                }
            }
            else if (CurrentUserSession.RoleId == 3)
            {
                // Master edits agent/member
                if (this.RequestCustId <= 0 || (roleDownline != 1 && roleDownline != 2))
                {
                    return new ErrorPage_Page(Lang.strNotAccessPage);
                }
            }
            else
            {
                // Agent edits member
                if (this.RequestCustId <= 0 || roleDownline != 1)
                {
                    return new ErrorPage_Page(Lang.strNotAccessPage);
                }
            }

            return null;
        }

        /// <summary>
        /// Convert a decimal to formatted decimal
        /// </summary>
        /// <param name="dec">Input decimal</param>
        /// <param name="prec">Precision of decimal</param>
        /// <returns>A formatted decimal</returns>
        protected decimal ToDecimal(decimal dec, int prec = 2)
        {
            return Convert.ToDecimal(decimal.Round(dec, prec).ToString("#,##0.########"));
        }

        /// <summary>
        /// Convert an object to double
        /// </summary>
        /// <param name="obj">Object need to convert</param>
        /// <returns>Double object</returns>
        protected double ToDouble(object obj)
        {
            return Convert.ToDouble(obj);
        }

        /// <summary>
        /// This method is always called to register params for javascript to render PT combobox
        /// </summary>
        /// <param name="pt">PT of upline and downline</param>
        protected abstract void RegisterJsPT(PTSetting pt);
    }
}