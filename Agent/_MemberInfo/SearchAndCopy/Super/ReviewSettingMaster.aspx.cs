// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.01.18">Improve styles</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Super
{
    using System;
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
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Utilities;

    public partial class ReviewSettingMaster_Page : SearchAndCopy_Page
    {
        #region Variable

        private ReviewSettingMaster_Template tpl = new ReviewSettingMaster_Template();
        private CSCustomerDetail custCS = new CSCustomerDetail();
        private int active = 0;
        private string msgErrorCS = "Not casino's Customer";
        private bool errorCS = false;

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
            this.tpl.back_function = "SearchAndCopy.Master.BackSearch()";
            this.tpl.next_function = "SearchAndCopy.NextInfo(4)";
            RenderHeaderStep(out this.tpl.Header.header, 2);

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

            RegisterJavascriptObject("userName", userName);
            RegisterJavascriptObject("custId", custId);
            RegisterJavascriptObject("errorCS", this.errorCS);
            RegisterJavascriptObject("MsgErrorCS", this.msgErrorCS);

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            SearchAndCopyBO bo = new SearchAndCopyBO(CurrentUserSession.NetSessionId);
            DataTable dtb = new DataTable();

            switch (this.active)
            {
                case 0:
                    dtb = bo.GetSportBookCloneSettingMaster(custId);
                    break;
                case 1:
                    dtb = bo.GetRacingCloneSettingMaster(custId);
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
                    dtb = bo.GetVirtualSportsSettingMaster(custId);
                    break;
                case 4:
                    dtb = bo.GetVirtualSportsSettingMaster(custId);
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

            bool allowAutoPT = !Convert.ToBoolean(dr["DisableAutoMPT"]);
            if (allowAutoPT)
            {
                tplSportBookBlock.AllowAutoPT = Resources.Lang.lblYes;
            }
            else
            {
                tplSportBookBlock.AllowAutoPT = Resources.Lang.no0;
            }

            tplSportBookBlock.groupA = Formater.FormatNumber4(dr["groupA"], langKey);
            tplSportBookBlock.groupB = Formater.FormatNumber4(dr["groupB"], langKey);
            tplSportBookBlock.groupC = Formater.FormatNumber4(dr["groupC"], langKey);
            tplSportBookBlock.groupD = Formater.FormatNumber4(dr["groupD"], langKey);
            tplSportBookBlock.v1x2 = Formater.FormatNumber4(dr["v1discount_5"], langKey);
            tplSportBookBlock.others = Formater.FormatNumber4(dr["v1discount_4"], langKey);
            tplSportBookBlock.gameComm = Formater.FormatNumber4(dr["v161discount_81"], langKey);

            FormatTransferCondition(dr["TransferOption"], ref tplSportBookBlock.condition, ref tplSportBookBlock.day);

            tplSportBookBlock.minBetSoccer = Formater.FormatNumber1(dr["v1minbet_1"], langKey);
            tplSportBookBlock.maxBetSoccer = Formater.FormatNumber1(dr["v1maxbet_1"], langKey);
            tplSportBookBlock.maxPerMatchSoccer = Formater.FormatNumber1(dr["v1maxpermatch_1"], langKey);
            tplSportBookBlock.minBetBasketball = Formater.FormatNumber1(dr["v2minbet_1"], langKey);
            tplSportBookBlock.maxBetBasketball = Formater.FormatNumber1(dr["v2maxbet_1"], langKey);
            tplSportBookBlock.maxPerMatchBasketball = Formater.FormatNumber1(dr["v2maxpermatch_1"], langKey);
            tplSportBookBlock.minBetNumberGame = Formater.FormatNumber1(dr["v161minbet_81"], langKey);
            tplSportBookBlock.maxBetNumberGame = Formater.FormatNumber1(dr["v161maxbet_81"], langKey);
            tplSportBookBlock.maxPerMatchNumberGame = Formater.FormatNumber1(dr["v161maxpermatch_81"], langKey);

            tplSportBookBlock.soccerAgentPT = Formater.FormatNumber2(dr["v1mpositiontaking_1"], langKey);
            tplSportBookBlock.soccerMasterPreset = Formater.FormatNumber2(dr["v1spositiontaking_1"], langKey);
            tplSportBookBlock.soccerAutoMasterPreset = Formater.FormatNumber2(dr["v1AutoPT_1"], langKey);
            tplSportBookBlock.soccerIsAuto = FormatIsAuto(dr["v1isAutoPT_1"]);

            tplSportBookBlock.BasketBallAgentPT = Formater.FormatNumber2(dr["v2mpositiontaking_1"], langKey);
            tplSportBookBlock.BasketBallMasterPreset = Formater.FormatNumber2(dr["v2spositiontaking_1"], langKey);
            tplSportBookBlock.BasketBallAutoMasterPreset = Formater.FormatNumber2(dr["v2AutoPT_1"], langKey);
            tplSportBookBlock.BasketBallIsAuto = FormatIsAuto(dr["v2isAutoPT_1"]);

            tplSportBookBlock.numberGameAgentPT = Formater.FormatNumber2(dr["v161mpositiontaking_81"], langKey);
            tplSportBookBlock.numberGameMasterPreset = Formater.FormatNumber2(dr["v161spositiontaking_81"], langKey);
            tplSportBookBlock.numberGameAutoMasterPreset = Formater.FormatNumber2(dr["v161AutoPT_81"], langKey);
            tplSportBookBlock.numberGameIsAuto = FormatIsAuto(dr["v161isAutoPT_81"]);

            tplSportBookBlock.soccerDeadBall = Formater.FormatNumber2(dr["v1MinPT_1"], langKey);
            tplSportBookBlock.soccerLiveBall = Formater.FormatNumber2(dr["v1MinLivePT_1"], langKey);
            tplSportBookBlock.basketBallDeadball = Formater.FormatNumber2(dr["v2MinPT_1"], langKey);
            tplSportBookBlock.numberGameDeadball = Formater.FormatNumber2(dr["v161MinPT_81"], langKey);
            tplSportBookBlock.numberGameLiveball = Formater.FormatNumber2(dr["v161MinLivePT_85"], langKey);

            this.tpl.ActiveTabHeader.showSetting = tplSportBookBlock;
        }

        private void RenderRacingBlock(DataTable dtb)
        {
            ReviewSettingRacing_Template tplRacingBlock = new ReviewSettingRacing_Template();
            DataRow dr = dtb.Rows[0];

            tplRacingBlock.CommissionMaster = Formater.FormatNumber4(dr["v151discount_41"], langKey);
            tplRacingBlock.minBet = Formater.FormatNumber4(dr["v151minbet_41"], langKey);
            tplRacingBlock.maxBet = Formater.FormatNumber4(dr["v151maxbet_41"], langKey);
            tplRacingBlock.maxPerMatch = Formater.FormatNumber4(dr["v151maxpermatch_41"], langKey);

            tplRacingBlock.MasterPreset = Formater.FormatNumber4(dr["v151mpositiontaking_41"], langKey);
            tplRacingBlock.SuperPreset = Formater.FormatNumber4(dr["v151spositiontaking_41"], langKey);
            tplRacingBlock.AutoSuperPreset = Formater.FormatNumber4(dr["v151autospositiontaking_41"], langKey);
            tplRacingBlock.IsAuto = FormatIsAuto(dr["v151isAutosPT_41"]);
            tplRacingBlock.Win = Formater.FormatNumber4(dr["v151MinPT_41"], langKey);

            this.tpl.ActiveTabHeader.showSetting = tplRacingBlock;
        }

        private void RenderCasinoBlock()
        {
            if (!this.custCS.NoCSData)
            {
                ReviewSettingCasino_Template tplCasinoBlock = new ReviewSettingCasino_Template();
                ////Bet Setting block
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

                GameTypeDetail baccarat = null;
                GameTypeDetail miniBaccarat = null;

                // Position Taking block
                tplCasinoBlock.PositionTaking_Row.Clear();
                foreach (GameKindDetail gameKindDetail in lsGameKind)
                {
                    if (gameKindDetail.CodeID == 9 || gameKindDetail.CodeID == 159)
                    {
                        var rowBlock = new ReviewSettingCasino_PositionTaking_Row_Block();
                        rowBlock.CodeName = gameKindDetail.CodeName;

                        rowBlock.SuperPresetPT = Formater.FormatNumber2(gameKindDetail.SPositionTaking, langKey);
                        rowBlock.SuperAutoPT = Formater.FormatNumber2(gameKindDetail.AutoPositionTaking, langKey);
                        rowBlock.isAuto = FormatIsAuto(gameKindDetail.IsAutoPositionTaking);
                        rowBlock.MasterMaxPT = Formater.FormatNumber2(gameKindDetail.MaxPositionTaking, langKey);
                        rowBlock.MasterMinPT = Formater.FormatNumber2(gameKindDetail.MinPositionTaking, langKey);
                        double comm = this.custCS.CasinoSync ? gameKindDetail.MAgentRollingRate : gameKindDetail.SMAgentRollingRate;
                        rowBlock.MasterCommission = Formater.FormatNumber4(comm, langKey);
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

            VirtualSportBlock.superPT = Formater.FormatNumber4(dr["v180spositiontaking_1201"], langKey);
            VirtualSportBlock.autoPTSuper = Formater.FormatNumber4(dr["v180autospositiontaking_1201"], langKey);
            VirtualSportBlock.isAuto = FormatIsAuto(dr["v180isAutosPT_1201"]);
            VirtualSportBlock.masterMaxPT = Formater.FormatNumber4(dr["v180mpositiontaking_1201"], langKey);
            VirtualSportBlock.masterMinPT = Formater.FormatNumber4(dr["v180MinPT_1201"], langKey);

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
                    this.tpl.ActiveTabHeader.virtualSports_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.liveCasino_class = "buttonDisable";
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