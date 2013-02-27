// --------------------------------------------------------------------------
// <copyright file="P2P_Page.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.4.17">Format styles</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.P2P
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Components.ErrorMsg;
    using Resources;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.MemberInfo.P2P;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberInfo.P2P;

    /// <summary>
    /// P2P UI for user to edit p2p setting, commission and max tramsfer limit
    /// </summary>
    public partial class P2P_Page : TplPage
    {
        /// <summary>
        /// Role id of current editted user
        /// </summary>
        private int requestCustRoleId = 0;

        /// <summary>
        /// User id of current editted user
        /// </summary>
        private int requestCustId = 0;

        /// <summary>
        /// Error code of update
        /// </summary>
        private int errCode = 0;

        /// <summary>
        /// Numbers of user is editted
        /// </summary>
        private int count = 0;

        /// <summary>
        /// User name who is editted
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// Is this form edit multi user?
        /// </summary>
        private string isMulti = string.Empty;

        /// <summary>
        /// List of user names editted
        /// </summary>
        private string userNames = string.Empty;

        /// <summary>
        /// Is this form disabled?
        /// </summary>
        private string disable = string.Empty;

        /// <summary>
        /// List of customer id
        /// </summary>
        private IList<int> custids = null;

        /// <summary>
        /// Business object of p2p project
        /// </summary>
        private P2PBO bo = null;

        /// <summary>
        /// Error message of update
        /// </summary>
        private string errMsg = null;

        /// <summary>
        /// String ids of customer
        /// </summary>
        private string ids = string.Empty;

        /// <summary>
        /// Indicate whether to load screen name or not
        /// </summary>
        private bool loadScreenName = false;

        /// <summary>
        /// Gets Lobby Balance
        /// </summary>
        private decimal LobbyBalance
        {
            get
            {
                var retValue = 0.0m;

                try
                {
                    using (var service = new Fanex.Age.Business.BetList.P2P.WebServiceHandle())
                    {
                        var info = service.getP2PCustInfo(AppSettings.P2PSiteId, Request.QueryString["custids"]);

                        retValue = Convert.ToDecimal(info.LobbyAmount);
                    }
                }
                catch (Exception ex)
                {
                    Fanex.Logging.LogService.Error(ex.Message, ex);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Render UI
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            if (Request.QueryString.Count == 0)
            {
                writer.Write("<script>javascript:history.back(-1);</script>");
                return;
            }

            var tpl = new P2P_Page_Template();
            var data = View;
            var selected = false;

            tpl.WarningMsg = Lang.NoCurrencyPlay;

            tpl.Submit = Lang.submit;
            tpl.saveBetSetting = Lang.saveBetSetting;
            tpl.saveGameSetting = Lang.saveGameSetting;
            tpl.saveCommission = Lang.saveCommission;

            var disabled = //CurrentUserSession.IsInternal ||
                !CurrentUserSession.SubPageAccess.UpdateAccount ||
                ////(this.disable != null && this.disable.Equals("1")) ||
                (CurrentUserSession.RoleId - this.requestCustRoleId) != 1 ||
                !UserPermission.IsShowP2P ||
                this.bo.IsDisabledAccount(this.requestCustId, out this.errMsg);

            tpl.btndisabled = disabled ?
                "disabled='disabled'" : string.Empty;

            this.RegisterJavascriptObject("custRoleId", this.requestCustRoleId);
            this.RegisterJavascriptObject("custId", this.requestCustId);
            this.RegisterJavascriptObject("updateError", Lang.noInfo);
            this.RegisterJavascriptObject("noAccess", Lang.strNotAccessPage);

            var ds = data as DataSet;

            var tblInfo = ds.Tables[0];
            var tblGame = ds.Tables[1];
            var tblComm = ds.Tables[2];

            var infoRow = tblInfo.Rows[0];
            var gameRow = tblGame.Rows;
            var commRow = tblComm.Rows;

            if (!CurrentUserSession.IsAgent)
            {
                tpl.ATA_Item.Clear();
            }
            else
            {
                var ataValue = this.ToDecimal(tblInfo.Rows[0].Field<decimal>("transferp2p") + tblInfo.Rows[0].Field<decimal>("todayWinlostP2P") - this.LobbyBalance);
                tpl.ATA_Item.ATA = Lang.lblAccumulatedWinLoss;
                tpl.ATA_Item.ATA_vl = ataValue.ToString("#,##0.##");
            }

            tpl.BetSetting = Lang.lblBetSetting;
            tpl.MemberMaxTranCredit = Lang.lblMaxTransferCredit;
            tpl.MinTransfer = this.ToDecimal(infoRow.Field<decimal>("mintransferp2p")).ToString("#,##0.##");
            tpl.MaxTransfer = this.ToDecimal(infoRow.Field<decimal>("maxtransferp2p")).ToString("#,##0.##");
            tpl.maxTranCreditValue = this.ToDecimal(infoRow.Field<decimal>("transferp2p")).ToString("#,##0.##");
            tpl.disabledReset = infoRow.Field<bool>("uplineisautoresetp2p") ? string.Empty : "disabled='disabled'";
            tpl.checked_vl = !infoRow.Field<bool>("uplineisautoresetp2p") ? string.Empty : infoRow.Field<bool>("isautoresetp2p") ? "checked='checked'" : string.Empty;
            tpl.DailyResetLimit = Lang.autoReset;
            tpl.FullMulTranResetLimit = Lang.resetmaxtranfer;

            tpl.GameSetting = Lang.gameSetting;
            tpl.GameType = Lang.lblGameType;
            tpl.StakeLevel = Lang.stakeLevel;

            var count = 0;
            var index = 0;
            var gameRows = new P2P_Page_GameRow_Block();
            tpl.GameRow.Clear();

            foreach (DataRow dr in gameRow)
            {
                count++;

                if (count >= 3)
                {
                    index++;

                    gameRows.disabledLow = index % 2 != 0 ?
                        (!gameRow[0].Field<bool>("StakeLevel2") ? "disabled='disabled'" : string.Empty) :
                        (!gameRow[1].Field<bool>("StakeLevel2") ? "disabled='disabled'" : string.Empty);
                    gameRows.disabledMed = index % 2 != 0 ?
                        (!gameRow[0].Field<bool>("StakeLevel3") ? "disabled='disabled'" : string.Empty) :
                        (!gameRow[1].Field<bool>("StakeLevel3") ? "disabled='disabled'" : string.Empty);
                    gameRows.disabledHigh = index % 2 != 0 ?
                        (!gameRow[0].Field<bool>("StakeLevel4") ? "disabled='disabled'" : string.Empty) :
                        (!gameRow[1].Field<bool>("StakeLevel4") ? "disabled='disabled'" : string.Empty);
                    gameRows.disabledVip = index % 2 != 0 ?
                        (!gameRow[0].Field<bool>("StakeLevel5") ? "disabled='disabled'" : string.Empty) :
                        (!gameRow[1].Field<bool>("StakeLevel5") ? "disabled='disabled'" : string.Empty);

                    gameRows.ID = index.ToString();

                    gameRows.GameType_vl = CacheRef.GetBettype(dr.Field<short>("bettype"));
                    gameRows.bettypeID = index.ToString();
                    gameRows.BettypeValue = dr.Field<short>("bettype").ToString();

                    gameRows.FunID = index.ToString();
                    gameRows.LowID = index.ToString();
                    gameRows.MedID = index.ToString();
                    gameRows.HighID = index.ToString();
                    gameRows.VipID = index.ToString();

                    gameRows.funValue = dr.Field<bool>("StakeLevel1").ToString().ToLower();
                    gameRows.checkedFun = dr.Field<bool>("StakeLevel1") ? "checked='checked'" : string.Empty;
                    gameRows.checkedLow = dr.Field<bool>("StakeLevel2") ? "checked='checked'" : string.Empty;
                    gameRows.checkedMed = dr.Field<bool>("StakeLevel3") ? "checked='checked'" : string.Empty;
                    gameRows.checkedHigh = dr.Field<bool>("StakeLevel4") ? "checked='checked'" : string.Empty;
                    gameRows.checkedVip = dr.Field<bool>("StakeLevel5") ? "checked='checked'" : string.Empty;

                    gameRows.Fun_vl = Lang.playForFun;
                    gameRows.Low_vl = Lang.low;
                    gameRows.Med_vl = Lang.medium;
                    gameRows.High_vl = Lang.high;
                    gameRows.Vip_vl = Lang.vip;

                    tpl.GameRow.Append(gameRows);
                }
            }

            this.RegisterJavascriptObject("gameRows", index);

            tpl.Commission = Lang.commission;

            if (this.requestCustRoleId == Convert.ToInt32(UserTypes.Super))
            {
                tpl.CommUplineLevel = Lang.lblCommPTSuper;
                tpl.CommLevel = string.Empty;

                var countComm = 0;
                var commRows = new P2P_Page_CommRow_Block();
                tpl.CommRow.Clear();

                foreach (DataRow dr in commRow)
                {
                    countComm++;

                    commRows.UpID = countComm.ToString();
                    commRows.ID = countComm.ToString();

                    commRows.Games = dr.Field<int>("groupid") == 1 ? Lang.games : Lang.privateGames;
                    commRows.Games_vl = string.Empty;

                    var cbo = new P2P_Comm_ComboBox_Template();
                    var maxDiscount = this.ToDecimal(dr.Field<decimal>("Maxdiscount"));
                    var discount = this.ToDecimal(dr.Field<decimal>("discount"));
                    var uplineDiscount = this.ToDecimal(dr.Field<decimal>("Uplinediscount"));

                    cbo.CommID = "Comm" + countComm.ToString();
                    cbo.rowId = countComm.ToString();
                    cbo.upValue = uplineDiscount.ToString();

                    var cboItems = new P2P_Comm_ComboBox_Items_Block();
                    cbo.Items.Clear();

                    for (var i = maxDiscount; i >= 0.0m; i -= 0.01m)
                    {
                        cboItems.ItemText = this.ToDecimal(i).ToString();
                        cboItems.ItemValue = this.ToDecimal(i).ToString();

                        if (i == discount)
                        {
                            selected = true;
                        }

                        cboItems.selected = i == discount ? "selected='selected'" : string.Empty;

                        cbo.Items.Append(cboItems);
                    }

                    if (!selected)
                    {
                        cbo.Items.Append(new P2P_Comm_ComboBox_Items_Block()
                                {
                                    ItemText = discount.ToString(),
                                    ItemValue = discount.ToString(),
                                    selected = "selected='selected'"
                                });
                    }

                    commRows.GamesUpline_vl = cbo;
                    tpl.CommRow.Append(commRows);
                }
            }
            else if (this.requestCustRoleId == Convert.ToInt32(UserTypes.Master))
            {
                tpl.CommUplineLevel = Lang.lblCommPTSuper;
                tpl.CommLevel = Lang.lblCommPTMaster;
            }
            else if (this.requestCustRoleId == Convert.ToInt32(UserTypes.Agent))
            {
                tpl.CommUplineLevel = Lang.lblCommPTMaster;
                tpl.CommLevel = Lang.lblCommPTAgent;
            }
            else if (this.requestCustRoleId == Convert.ToInt32(UserTypes.Member))
            {
                tpl.CommUplineLevel = Lang.lblCommPTAgent;
                tpl.CommLevel = Lang.lblMemberCommission;
            }

            if (this.requestCustRoleId != Convert.ToInt32(UserTypes.Super))
            {
                var countComm = 0;
                var commRows = new P2P_Page_CommRow_Block();
                tpl.CommRow.Clear();

                foreach (DataRow dr in commRow)
                {
                    selected = false;
                    countComm++;

                    commRows.UpID = countComm.ToString();
                    commRows.ID = countComm.ToString();

                    commRows.Games = dr.Field<int>("groupid") == 1 ? Lang.games : Lang.privateGames;
                    commRows.Games_vl = string.Empty;

                    var cbo = new P2P_Comm_ComboBox_Template();
                    var maxDiscount = this.ToDecimal(dr.Field<decimal>("Maxdiscount"));
                    var discount = this.ToDecimal(dr.Field<decimal>("discount"));
                    var uplineDiscount = this.ToDecimal(dr.Field<decimal>("Uplinediscount"));

                    cbo.CommID = "Comm" + countComm.ToString();
                    cbo.rowId = countComm.ToString();
                    cbo.upValue = uplineDiscount.ToString();

                    var cboItems = new P2P_Comm_ComboBox_Items_Block();
                    cbo.Items.Clear();

                    commRows.GamesUpline_vl = (uplineDiscount - discount).ToString();

                    for (var i = maxDiscount; i >= 0.0m; i -= 0.01m)
                    {
                        cboItems.ItemText = this.ToDecimal(i).ToString();
                        cboItems.ItemValue = this.ToDecimal(i).ToString();

                        if (i == discount)
                        {
                            selected = true;
                        }

                        cboItems.selected = i == discount ? "selected='selected'" : string.Empty;

                        cbo.Items.Append(cboItems);
                    }

                    if (!selected)
                    {
                        cbo.Items.Append(new P2P_Comm_ComboBox_Items_Block()
                                {
                                    ItemText = discount.ToString(),
                                    ItemValue = discount.ToString(),
                                    selected = "selected='selected'"
                                });
                    }

                    commRows.Games_vl = cbo;

                    tpl.CommRow.Append(commRows);
                }
            }

            if ((this.isMulti == null || !this.userNames.Contains(',')) && this.count == 0)
            {
                this.loadScreenName = true;

                if (ds != null && ds.Tables.Count > 2)
                {
                    tpl.UserInfos.UserName = Lang.username;
                    tpl.UserInfos.UserName_vl = infoRow.Field<string>("username");
                    tpl.UserInfos.FirstName = Lang.firstname;
                    tpl.UserInfos.FirstName_vl = infoRow.Field<string>("firstname");
                    tpl.UserInfos.Phone = Lang.lblPhone;
                    tpl.UserInfos.Phone_vl = infoRow.Field<string>("phone");
                    tpl.UserInfos.MobilePhone = Lang.lblMobilePhone;
                    tpl.UserInfos.MobilePhone_vl = infoRow.Field<string>("mobilephone");
                    tpl.UserInfos.LastName = Lang.lastname;
                    tpl.UserInfos.LastName_vl = infoRow.Field<string>("lastname");
                    tpl.UserInfos.ScreenName = Lang.screenName;
                    tpl.UserInfos.ScreenName_vl = "--";
                }
            }
            else
            {
                this.loadScreenName = false;

                tpl.UserInfos.Clear();
            }

            tpl.UserNames = this.userNames;
            tpl.UserIDs = this.ids == string.Empty ? this.requestCustId.ToString() : this.ids;

            var errMsgTpl = new ErrorMsg_Template();
            tpl.Msg.ErrorMsg = errMsgTpl;

            tpl.PageTitle = this.userNames.Contains(',') ? Lang.editmultp2p : Lang.editP2P;
            tpl.EditGame = tpl.PageTitle;

            this.RegisterJavascriptObject("loadScreenName", this.loadScreenName);
            this.RegisterJavascriptObject("minTransfer", tpl.MinTransfer.Replace(",", string.Empty));
            this.RegisterJavascriptObject("maxTransfer", tpl.MaxTransfer.Replace(",", string.Empty));
            this.RegisterJavascriptObject("invalidValue", Lang.invalidvalue);
            this.RegisterJavascriptObject("userName", this.userName == null || this.userName == string.Empty ? infoRow.Field<string>("username") : this.userName);
            this.RegisterJavascriptObject("disabled", disabled);
            this.RegisterJavascriptObject("disabledMsg", Lang.accdisabled);

            if (CurrentUserSession.IsAgent)
            {
                this.RegisterJavascriptObject("todayWinlost", this.ToDecimal(tblInfo.Rows[0].Field<decimal>("todayWinlostP2P")));
                this.RegisterJavascriptObject("lobbyBalance", this.ToDecimal(this.LobbyBalance));
            }

            writer.Write(tpl);
        }

        /// <summary>
        /// Invoke data
        /// </summary>
        /// <param name="p">Request params</param>
        /// <returns>Object value</returns>
        public override object InvokeData(System.Collections.Hashtable p)
        {
            return this.bo.GetP2PInfo(this.requestCustId, this.requestCustRoleId, out this.errCode, out this.errMsg);
        }

        /// <summary>
        /// Process request
        /// </summary>
        /// <param name="p">Request params</param>
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.isMulti = Request.QueryString["isMult"];
            this.disable = Request.QueryString["disable"];
            this.count = Convert.ToInt32(Request.QueryString["count"]);

            var users = Request.QueryString["custnames"];
            string[] userList = null;

            if (users != null)
            {
                userList = users.Split('^');
                var userIds = Request.QueryString["custids"].Split('^');
                this.custids = new List<int>();

                foreach (string id in userIds)
                {
                    if (!string.IsNullOrEmpty(id.Trim()))
                    {
                        this.custids.Add(Convert.ToInt32(id));
                    }
                }

                foreach (string s in userList)
                {
                    if (!string.IsNullOrEmpty(s.Trim()))
                    {
                        this.userNames += s + ", ";
                    }
                }

                this.userNames = this.userNames.Substring(0, this.userNames.Length - 2);
            }

            if (this.isMulti != null && this.isMulti.Equals("1"))
            {
                this.ids = Request.QueryString["custids"].Trim();
                this.ids = this.ids.Substring(0, this.ids.Length - 1);

                this.requestCustId = this.custids.ElementAt(0);
                this.userName = userList.ElementAt(0);
            }
            else
            {
                if (Request.QueryString["custids"] != null && Request.QueryString["custids"].Contains('^'))
                {
                    this.requestCustId = Convert.ToInt32(Request.QueryString["custids"].Split('^').ElementAt(0));
                    this.userName = string.Empty;
                }
                else
                {
                    this.requestCustId = Convert.ToInt32(Request.QueryString["custids"]);
                    this.userName = Request.QueryString["username"];
                }
            }

            this.requestCustRoleId = Convert.ToInt32(Request.QueryString["roleid"]);

            this.bo = this.bo == null ? new P2PBO(CurrentUserSession.NetSessionId) : this.bo;
        }

        /// <summary>
        /// Validate request
        /// </summary>
        /// <param name="p">Request params</param>
        /// <returns>Object value</returns>
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (CurrentUserSession.IsP2PDisabled)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }

        /// <summary>
        /// Converts a decimal value to formated decimal value
        /// </summary>
        /// <param name="dec">Decimal value to format</param>
        /// <returns>Formatted decimal value</returns>
        private decimal ToDecimal(decimal dec)
        {
            return Convert.ToDecimal(decimal.Round(dec, 2).ToString("#,##0.##"));
        }
    }
}