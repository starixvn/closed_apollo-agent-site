// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.03.12">Improve styles</change>
//   <change who="Jeff.Nguyen" date="2012.08.24">add Virtual Sports</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Master
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

    public partial class ReviewSettingAgent_Page : SearchAndCopy_Page
    {
        #region Variable

        private ReviewSettingAgent_Template tpl = new ReviewSettingAgent_Template();
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
            this.tpl.back_function = "SearchAndCopy.Agent.BackSearch()";
            this.tpl.next_function = "SearchAndCopy.NextInfo(3)";
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

            try
            {
                this.custCS = CasinoManager.GetCustInfoIBC(custId);
            }
            catch (Exception ex)
            {
                this.msgErrorCS = ex.Message;
            }

            switch (this.active)
            {
                case 0:
                    dtb = bo.GetSportBookCloneSettingAgent(custId);
                    break;
                case 1:
                    dtb = bo.GetRacingCloneSettingAgent(custId);
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
                    dtb = bo.GetVirtualSportsSettingAgent(custId);
                    break;
                case 4:
                    dtb = bo.GetVirtualSportsSettingAgent(custId);
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

            tplSportBookBlock.groupA = Formater.FormatNumber4(dr["groupA"], langKey);
            tplSportBookBlock.groupB = Formater.FormatNumber4(dr["groupB"], langKey);
            tplSportBookBlock.groupC = Formater.FormatNumber4(dr["groupC"], langKey);
            tplSportBookBlock.groupD = Formater.FormatNumber4(dr["groupD"], langKey);
            tplSportBookBlock.v1x2 = Formater.FormatNumber4(dr["v1discount_5"], langKey);
            tplSportBookBlock.others = Formater.FormatNumber4(dr["v1discount_4"], langKey);
            tplSportBookBlock.gameComm = Formater.FormatNumber4(dr["v161discount_81"], langKey);

            FormatTransferCondition(dr["TransferOption"], ref tplSportBookBlock.condition, ref tplSportBookBlock.day);

            tplSportBookBlock.minBetSoccer = Formater.FormatNumber4(dr["v1minbet_1"], langKey);
            tplSportBookBlock.maxBetSoccer = Formater.FormatNumber4(dr["v1maxbet_1"], langKey);
            tplSportBookBlock.maxPerMatchSoccer = Formater.FormatNumber4(dr["v1maxpermatch_1"], langKey);
            tplSportBookBlock.minBetBasketball = Formater.FormatNumber4(dr["v2minbet_1"], langKey);
            tplSportBookBlock.maxBetBasketball = Formater.FormatNumber4(dr["v2maxbet_1"], langKey);
            tplSportBookBlock.maxPerMatchBasketball = Formater.FormatNumber4(dr["v2maxpermatch_1"], langKey);
            tplSportBookBlock.minBetNumberGame = Formater.FormatNumber4(dr["v161minbet_81"], langKey);
            tplSportBookBlock.maxBetNumberGame = Formater.FormatNumber4(dr["v161maxbet_81"], langKey);
            tplSportBookBlock.maxPerMatchNumberGame = Formater.FormatNumber4(dr["v161maxpermatch_81"], langKey);

            tplSportBookBlock.soccerAgentPT = Formater.FormatNumber4(dr["v1apositiontaking_1"], langKey);
            tplSportBookBlock.soccerMasterPreset = Formater.FormatNumber4(dr["v1mpositiontaking_1"], langKey);
            tplSportBookBlock.soccerAutoMasterPreset = Formater.FormatNumber4(dr["v1AutomPT_1"], langKey);
            tplSportBookBlock.soccerIsAuto = Formater.FormatNumber4(dr["v1isAutomPT_1"], langKey);

            tplSportBookBlock.BasketBallAgentPT = Formater.FormatNumber4(dr["v2apositiontaking_1"], langKey);
            tplSportBookBlock.BasketBallMasterPreset = Formater.FormatNumber4(dr["v2mpositiontaking_1"], langKey);
            tplSportBookBlock.BasketBallAutoMasterPreset = Formater.FormatNumber4(dr["v2AutomPT_1"], langKey);
            tplSportBookBlock.BasketBallIsAuto = Formater.FormatNumber4(dr["v2isAutomPT_1"], langKey);

            tplSportBookBlock.numberGameAgentPT = Formater.FormatNumber4(dr["v161apositiontaking_81"], langKey);
            tplSportBookBlock.numberGameMasterPreset = Formater.FormatNumber4(dr["v161mpositiontaking_81"], langKey);
            tplSportBookBlock.numberGameAutoMasterPreset = Formater.FormatNumber4(dr["v161AutomPT_81"], langKey);
            tplSportBookBlock.numberGameIsAuto = Formater.FormatNumber4(dr["v161isAutomPT_81"], langKey);

            tplSportBookBlock.soccerDeadBall = Formater.FormatNumber4(dr["v1MinPT_1"], langKey);
            tplSportBookBlock.soccerLiveBall = Formater.FormatNumber4(dr["v1MinLivePT_1"], langKey);
            tplSportBookBlock.basketBallDeadball = Formater.FormatNumber4(dr["v2MinPT_1"], langKey);
            tplSportBookBlock.numberGameDeadball = Formater.FormatNumber4(dr["v161MinPT_81"], langKey);
            tplSportBookBlock.numberGameLiveball = Formater.FormatNumber4(dr["v161MinLivePT_85"], langKey);

            this.tpl.ActiveTabHeader.showSetting = tplSportBookBlock;
        }

        private void RenderRacingBlock(DataTable dtb)
        {
            ReviewSettingRacing_Template tplRacingBlock = new ReviewSettingRacing_Template();
            DataRow dr = dtb.Rows[0];

            tplRacingBlock.CommissionAgent = Formater.FormatNumber4(dr["v151discount_41"], langKey);
            tplRacingBlock.minBet = Formater.FormatNumber4(dr["v151minbet_41"], langKey);
            tplRacingBlock.maxBet = Formater.FormatNumber4(dr["v151maxbet_41"], langKey);
            tplRacingBlock.maxPerMatch = Formater.FormatNumber4(dr["v151maxpermatch_41"], langKey);

            tplRacingBlock.agent = Formater.FormatNumber4(dr["v151apositiontaking_41"], langKey);
            tplRacingBlock.masterPreset = Formater.FormatNumber4(dr["v151mpositiontaking_41"], langKey);
            tplRacingBlock.masterAutoPreset = Formater.FormatNumber4(dr["v151AutomPT_41"], langKey);
            tplRacingBlock.isAuto = Formater.FormatNumber4(dr["v151isAutomPT_41"], langKey);
            tplRacingBlock.win = Formater.FormatNumber4(dr["v151MinPT_41"], langKey);

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

                        rowBlock.MasterPresetPT = Formater.FormatNumber4(gameKindDetail.MPositionTaking, langKey);
                        rowBlock.AgentMaxPT = Formater.FormatNumber4(gameKindDetail.MaxPositionTaking, langKey);
                        rowBlock.AgentMinPT = Formater.FormatNumber4(gameKindDetail.MinPositionTaking, langKey);
                        double comm = this.custCS.CasinoSync ? gameKindDetail.AgentRollingRate : gameKindDetail.MAgentRollingRate;
                        rowBlock.AgentCommission = Formater.FormatNumber4(comm, langKey);
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

            VirtualSportBlock.masterPT = Formater.FormatNumber4(dr["v180mpositiontaking_1201"], langKey);
            VirtualSportBlock.masterAutoPT = Formater.FormatNumber4(dr["v180AutomPT_1201"], langKey);
            VirtualSportBlock.isAuto = FormatIsAuto(dr["v180isAutomPT_1201"]);
            VirtualSportBlock.agentMaxPT = Formater.FormatNumber4(dr["v180apositiontaking_1201"], langKey);
            VirtualSportBlock.agentMinPT = Formater.FormatNumber4(dr["v180MinPT_41"], langKey);

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
                    this.tpl.ActiveTabHeader.virtualSports_class = "button";
                    this.tpl.ActiveTabHeader.liveCasino_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.casino_class = "buttonDisable";
                    break;
                case 4:
                    this.RenderVirtualSportBlock(dtb);
                    this.tpl.ActiveTabHeader.sportBook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.racebook_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.virtualSports_class = "button";
                    this.tpl.ActiveTabHeader.liveCasino_class = "buttonDisable";
                    this.tpl.ActiveTabHeader.casino_class = "buttonDisable";
                    break;
                default:
                    break;
            }
        }

        #endregion Utility
    }
}