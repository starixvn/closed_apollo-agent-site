// --------------------------------------------------------------------------
// <copyright file="ReviewSettingMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.01.18">Improve styles</change>
//   <change who="Jeff.Nguyen" date="2012.08.24">add Virtual Sports</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Agent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using BLL;
    using CasinoLibrary.SBAgent;
    using DAL;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Sunplus.Agent.Model.BL.Casino;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.MemberInfo.SearchAndCopy;
    using Sunplus.Agent.Model.BL.Racing;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Utilities;

    public partial class ReviewSettingMember_Page : SearchAndCopy_Page
    {
        #region Variable

        private ReviewSettingMember_Template tpl = new ReviewSettingMember_Template();
        private CSCustomerDetail custCS = new CSCustomerDetail();
        private int active = 0;
        private string msgErrorCS = "Not casino's Customer";
        private bool errorCS = false;
        private Hashtable htbSportbook = new Hashtable();
        private Hashtable htbRacebook = new Hashtable();
        private bool isPTChange = false;

        #endregion Variable

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable searchAndCopyData = new DataTable();
            if (this.active != 2 || !this.custCS.NoCSData)
            {
                searchAndCopyData = (View as Dictionary<string, DataTable>)["Data"];
            }

            this.tpl.MemberSetting.memberSettingInfo = System.Threading.Thread.CurrentThread.CurrentCulture.Name == "vi-VN" ?
                                                            Resources.Lang.settingOf + userName : userName + Resources.Lang.settingOf;
            this.tpl.back_function = "SearchAndCopy.Agent.BackSearch()";
            this.tpl.next_function = "SearchAndCopy.NextInfo(2)";

            if (!UserPermission.IsShowRacing)
            {
                this.tpl.ActiveTabHeader.RaceBook = "Hide";
            }

            if (!UserPermission.IsShowCasino)
            {
                this.tpl.ActiveTabHeader.Casino = "Hide";
            }

            //if (!UserPermission.IsShowLiveCasino)
            //{
            //    this.tpl.ActiveTabHeader.LiveCasino = "Hide";
            //}
            this.tpl.ActiveTabHeader.LiveCasino = "Hide";

            if (!UserPermission.IsShowVirtualSports)
            {
                this.tpl.ActiveTabHeader.VirtualSports = "Hide";
            }

            this.ShowButton(this.active, searchAndCopyData);

            RenderHeaderStep(out this.tpl.Header.header, 2, this.isPTChange);
            RegisterJavascriptObject("userName", userName);
            RegisterJavascriptObject("custId", custId);
            RegisterJavascriptObject("errorCS", this.errorCS);
            RegisterJavascriptObject("MsgErrorCS", this.msgErrorCS);
            RegisterJavascriptObject("msgErrorSB", Resources.Lang.msgContraintData);

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            SearchAndCopyBO bo = new SearchAndCopyBO(CurrentUserSession.NetSessionId);
            DataTable dtb = new DataTable();

            try
            {
                this.custCS = CasinoManager.GetCustInfoIBC(custId);
            }
            catch (Exception ex)
            {
                this.msgErrorCS = ex.Message;
            }

            if (UserPermission.IsShowLiveCasino)
            {
                var checkPTLiveCasino = new CheckConstraintDataForLiveCasino();
                this.isPTChange = checkPTLiveCasino.IsChangedData(custId);
            }

            switch (this.active)
            {
                case 0:
                    dtb = bo.GetSportBookSettingMember(custId);
                    var checkConstraintData = new CheckConstraintDataForSportBook();
                    DataRow settingCustomer = dtb.Rows[0];

                    checkConstraintData.GeneratePositionTaking(settingCustomer, ref this.htbSportbook);
                    checkConstraintData.GenerateBettingLimit(settingCustomer, ref this.htbSportbook);
                    checkConstraintData.GenerateInfoAccount(settingCustomer, ref this.htbSportbook);
                    Session["HashtableSportbook"] = this.htbSportbook;
                    if (!this.isPTChange)
                    {
                        this.isPTChange = checkConstraintData.IsChangedData(settingCustomer);
                    }

                    break;
                case 1:
                    RacingBO racingBO = new RacingBO(CurrentUserSession.NetSessionId);
                    Hashtable outputs = null;
                    DataSet ds = racingBO.GetHRSetting_Member(custId, out outputs);
                    dtb = ds.Tables[0];
                    var checkConstraintData_Racebook = new CheckConstraintDataForRaceBook();

                    checkConstraintData_Racebook.GeneratePositionTaking(dtb.Rows[0], ref this.htbRacebook);
                    if (!this.isPTChange)
                    {
                        this.isPTChange = checkConstraintData_Racebook.IsChangedData(dtb.Rows[0]);
                    }

                    Session["HashtableRacebook"] = this.htbRacebook;

                    break;
                case 2:
                    if (this.custCS.CasinoSync)
                    {
                        try
                        {
                            CasinoManager.GetCustSettingCS(ref this.custCS);
                        }
                        catch (Exception ex)
                        {
                            this.msgErrorCS = ex.Message;
                        }
                    }

                    break;
                case 3:
                    if (!this.isPTChange)
                    {
                        var checkPTVirtualSports = new CheckConstraintDataForVirtualSports();
                        this.isPTChange = checkPTVirtualSports.IsChangedData(custId);
                    }

                    dtb = bo.GetVirtualSportsSettingMember(custId);
                    break;
                case 4:
                    if (!this.isPTChange)
                    {
                        var checkPTVirtualSports = new CheckConstraintDataForVirtualSports();
                        this.isPTChange = checkPTVirtualSports.IsChangedData(custId);
                    }

                    dtb = bo.GetVirtualSportsSettingMember(custId);
                    break;
                default:

                    break;
            }

            viewData.Add("Data", dtb);

            return viewData;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            userName = GetStringRequest("userName", userName);
            custId = GetIntRequest("custId", custId);
            this.active = GetIntRequest("active", this.active);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (custId == 0)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }

        #endregion Override

        #region Utility

        private void RenderSportBookBlock(DataTable dtb)
        {
            ReviewSettingSportsbook_Template tplSportBookBlock = new ReviewSettingSportsbook_Template();
            DataRow dr = dtb.Rows[0];

            int status = GetStatus(Convert.ToBoolean(dr["closed"]), Convert.ToBoolean(dr["suspended"]));
            switch (status)
            {
                case 1:
                    tplSportBookBlock.Status = Resources.Lang.open;
                    break;
                case 2:
                    tplSportBookBlock.Status = Resources.Lang.suspend;
                    break;
                case 3:
                    tplSportBookBlock.Status = Resources.Lang.close;
                    break;
                default:
                    tplSportBookBlock.Status = null;
                    break;
            }

            tplSportBookBlock.CommissionH_OU_OE = Formater.FormatNumber4(dr["discount"], langKey);
            tplSportBookBlock.CommissionH_OU_OEMember = Formater.FormatNumber4(dr["playerdiscount"], langKey);
            tplSportBookBlock.Commission1x2 = Formater.FormatNumber4(dr["discount1x2"], langKey);
            tplSportBookBlock.Commission1x2Member = Formater.FormatNumber4(dr["playerdiscount1x2"], langKey);
            tplSportBookBlock.CommissionOtherType = Formater.FormatNumber4(dr["discountcs"], langKey);
            tplSportBookBlock.CommissionOtherTypeMember = Formater.FormatNumber4(dr["playerdiscountcs"], langKey);
            tplSportBookBlock.CommissionNumberGame = Formater.FormatNumber4(dr["discountnumber"], langKey);
            tplSportBookBlock.CommissionNumberGameMember = Formater.FormatNumber4(dr["playerdiscountnumber"], langKey);

            tplSportBookBlock.minBetSoccer = Formater.FormatNumber4(dr["minbet"], langKey);
            tplSportBookBlock.maxBetSoccer = Formater.FormatNumber4(dr["maxbet"], langKey);
            tplSportBookBlock.maxPerMatchSoccer = Formater.FormatNumber4(dr["maxpermatch"], langKey);
            tplSportBookBlock.minBetBasketball = Formater.FormatNumber4(dr["minbetv2"], langKey);
            tplSportBookBlock.maxBetBasketball = Formater.FormatNumber4(dr["maxbetv2"], langKey);
            tplSportBookBlock.maxPerMatchBasketball = Formater.FormatNumber4(dr["maxpermatchv2"], langKey);
            tplSportBookBlock.minBetNumberGame = Formater.FormatNumber4(dr["minbetv161"], langKey);
            tplSportBookBlock.maxBetNumberGame = Formater.FormatNumber4(dr["maxbetv161"], langKey);
            tplSportBookBlock.maxPerMatchNumberGame = Formater.FormatNumber4(dr["maxpermatchv161"], langKey);

            tplSportBookBlock.ptSoccer = Formater.FormatNumber4(this.htbSportbook["apositiontaking_1"], langKey);
            tplSportBookBlock.ptBasketball = Formater.FormatNumber4(this.htbSportbook["v2apositiontaking_1"], langKey);
            tplSportBookBlock.ptNumberGame = Formater.FormatNumber4(this.htbSportbook["v161apositiontaking_81"], langKey);

            this.tpl.ActiveTabHeader.showSetting = tplSportBookBlock;
        }

        private void RenderRacingBlock(DataTable dtb)
        {
            ReviewSettingRacing_Template tplSportBookBlock = new ReviewSettingRacing_Template();
            DataRow dr = dtb.Rows[0];

            tplSportBookBlock.CommissionAgent = Formater.FormatNumber4(dr["v151discount_41_max"], langKey);
            tplSportBookBlock.CommissionMember = Formater.FormatNumber4(dr["v151discount_41"], langKey);
            tplSportBookBlock.minBet = Formater.FormatNumber4(this.htbRacebook["v151minbet_41"], langKey);
            tplSportBookBlock.maxBet = Formater.FormatNumber4(this.htbRacebook["v151maxbet_41"], langKey);
            tplSportBookBlock.maxPerMatch = Formater.FormatNumber4(this.htbRacebook["v151maxpermatch_41"], langKey);
            tplSportBookBlock.win = Formater.FormatNumber4(this.htbRacebook["v151apositiontaking_41"], langKey);

            this.tpl.ActiveTabHeader.showSetting = tplSportBookBlock;
        }

        private void RenderCasinoBlock()
        {
            if (!this.custCS.NoCSData)
            {
                ReviewSettingCasino_Template tplCasinoBlock = new ReviewSettingCasino_Template();

                // Bet Setting block
                tplCasinoBlock.maxTransferCredit = Formater.FormatNumber4(this.custCS.MaxTransferCredit, langKey);
                tplCasinoBlock.accumulatedWinLoss = Formater.FormatNumber4(this.custCS.TotalWinLoss, langKey);
                if (this.custCS.IsAutoReset)
                {
                    tplCasinoBlock.autoReset = Resources.Lang.lblYes;
                }
                else
                {
                    tplCasinoBlock.autoReset = Resources.Lang.no0;
                }
                ////end

                List<GameKindDetail> lsGameKind = this.custCS.CasinoSync ? CasinoManager.GetGameKinds(this.custCS.IBCCustID) : CasinoManager.GetGameKinds(Sunplus.Agent.Model.Entities.UserSession.Current.CustId);

                double positionTaking = 0;
                GameTypeDetail baccarat = null;
                GameTypeDetail miniBaccarat = null;

                // Position Taking block
                tplCasinoBlock.PositionTaking_Row.Clear();
                foreach (GameKindDetail gameKindDetail in lsGameKind)
                {
                    if (gameKindDetail.CodeID == 9 || gameKindDetail.CodeID == 159)
                    {
                        if (!this.isPTChange)
                        {
                            if (gameKindDetail.MaxPositionTakingUpline < gameKindDetail.APositionTaking)
                            {
                                this.isPTChange = true;
                            }
                        }

                        if (gameKindDetail.MaxPositionTakingUpline < gameKindDetail.APositionTaking)
                        {
                            positionTaking = gameKindDetail.MaxPositionTakingUpline;
                        }
                        else
                        {
                            positionTaking = gameKindDetail.APositionTaking;
                        }

                        var rowBlock = new ReviewSettingCasino_PositionTaking_Row_Block();
                        rowBlock.CodeName = gameKindDetail.CodeName;

                        rowBlock.AgentPresetPT = Formater.FormatNumber2(positionTaking, langKey);
                        double memberComm = this.custCS.CasinoSync ? gameKindDetail.MemberRollingRate : gameKindDetail.AgentRollingRate;
                        rowBlock.MemberCommission = Formater.FormatNumber4(memberComm, langKey);
                        tplCasinoBlock.PositionTaking_Row.Append(rowBlock);

                        if (baccarat == null || miniBaccarat == null)
                        {
                            List<GameTypeDetail> lsGameType = this.custCS.CasinoSync ? CasinoManager.GetGameType(this.custCS.IBCCustID, gameKindDetail.CodeID) : CasinoManager.GetGameType(gameKindDetail.CodeID);
                            if (lsGameType != null && lsGameType.Count > 0)
                            {
                                foreach (var item in lsGameType)
                                {
                                    if (baccarat == null)
                                    {
                                        // Baccarat
                                        if (item.GameTypeID == 7)
                                        {
                                            baccarat = item;
                                        }
                                    }

                                    if (miniBaccarat == null)
                                    {
                                        // Mini Baccarat
                                        if (item.GameTypeID == 30)
                                        {
                                            miniBaccarat = item;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Betting Limit block
                if (baccarat != null)
                {
                    tplCasinoBlock.Baccarat.Baccarat = FormatStackLevel(this.custCS, baccarat);
                }
                else
                {
                    tplCasinoBlock.Baccarat.Hide();
                }

                if (miniBaccarat != null)
                {
                    tplCasinoBlock.MiniBaccarat.MiniBaccarat = FormatStackLevel(this.custCS, miniBaccarat);
                }
                else
                {
                    tplCasinoBlock.MiniBaccarat.Hide();
                }

                this.tpl.ActiveTabHeader.showSetting = tplCasinoBlock;
            }
            else
            {
                this.tpl.ActiveTabHeader.showSetting = null;
                this.errorCS = true;
            }
        }

        private void RenderVirtualSportBlock(DataTable dtb)
        {
            ReviewSettingVirtualSports_Template VirtualSportBlock = new ReviewSettingVirtualSports_Template();
            DataRow dr = dtb.Rows[0];

            VirtualSportBlock.minBetSoccer = Formater.FormatNumber4(dr["v180minbet_1201"], langKey);
            VirtualSportBlock.maxBetSoccer = Formater.FormatNumber4(dr["v180maxbet_1201"], langKey);
            VirtualSportBlock.maxPerMatchSoccer = Formater.FormatNumber4(dr["v180maxpermatch_1201"], langKey);

            VirtualSportBlock.agentPT = Formater.FormatNumber4(dr["v180apositiontaking_1201"], langKey);

            this.tpl.ActiveTabHeader.showSetting = VirtualSportBlock;
        }

        private void ShowButton(int active, DataTable dtb)
        {
            switch (active)
            {
                case 0:
                    this.RenderSportBookBlock(dtb);
                    this.tpl.ActiveTabHeader.sportBook_class = "button";
                    this.tpl.ActiveTabHeader.racebook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.casino_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.liveCasino_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.virtualSports_class = "buttonDisable";
                    break;
                case 1:
                    this.RenderRacingBlock(dtb);
                    this.tpl.ActiveTabHeader.sportBook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.racebook_class = "button";
                    this.tpl.ActiveTabHeader.casino_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.liveCasino_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.virtualSports_class = "buttonDisable";
                    break;
                case 2:
                    this.RenderCasinoBlock();
                    this.tpl.ActiveTabHeader.sportBook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.racebook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.liveCasino_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.virtualSports_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.casino_class = "button";
                    break;
                case 3:
                    this.RenderVirtualSportBlock(dtb);
                    this.tpl.ActiveTabHeader.sportBook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.racebook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.liveCasino_class = "button";
                    this.tpl.ActiveTabHeader.virtualSports_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.casino_class = "buttonDisable";
                    break;
                case 4:
                    this.RenderVirtualSportBlock(dtb);
                    this.tpl.ActiveTabHeader.sportBook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.racebook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.liveCasino_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.virtualSports_class = "button";
                    this.tpl.ActiveTabHeader.casino_class = "buttonDisable";
                    break;
                default:
                    break;
            }
        }
        
        #endregion Utility
    }
}
