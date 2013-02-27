/// <summary>
/// Manage class for Bingo Business
/// </summary>
namespace BLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using DNAide.Core;
    using DNAide.Utils;
    using DNAide.Web;
    using Resources;
    using Sunplus.Bingo.Entities;

    public class BingoManager
    {
        private static double m3MaxAuto = 0.9;
        private static List<string> lsJSData = null;
        private static List<GameKind> listGameKind = null;
        private static Hashtable htGameType = null;

        public static string GenBingoPTHTML(ICustomer _cust, BingoDef.BingoGroup BingoGroup)
        {
            string groupNo = BingoDef.GetBingoGroup(BingoGroup);
            return GenBingoPTHTML(_cust, groupNo);
        }

        public static string GenBingoPTHTML(ICustomer _cust, string BingoGroup)
        {
            string retHTML = "";
            string tplPT = TemplateHandler.LoadTemplate(BingoDef.BINGO_PT_TEMPLATE);

            bool IsMultiple = (HttpContext.Current.Request["isMult"] == "1");
            if (IsMultiple)
            {
                TemplateHandler.RemoveBlock(ref tplPT, "single-edit");
            }

            lsJSData = new List<string>();
            htGameType = new Hashtable();
            listGameKind = new List<GameKind>();
            if (_cust.SyncBingo || IsMultiple)
            {
                TemplateHandler.Assign(ref tplPT, "op", Resources.Bingo.updateBingo);
                TemplateHandler.RemoveBlock(ref tplPT, "sync-enable");
            }
            else
            {
                TemplateHandler.Assign(ref tplPT, "op", Resources.Bingo.insertBingo);
                TemplateHandler.RemoveBlock(ref tplPT, "inserting");
            }

            if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4 && _cust.RoleId == 2)
            {
                TemplateHandler.Assign(ref tplPT, "submit", Resources.Lang.close);
                TemplateHandler.RemoveBlock(ref tplPT, "editting");
                TemplateHandler.Assign(ref tplPT, "func", "parent.ageWnd.close();");
            }
            else
            {
                TemplateHandler.Assign(ref tplPT, "submit", Resources.Lang.submit);
                TemplateHandler.Assign(ref tplPT, "func", "CheckConfirmUpdDownline();");
            }
            TemplateHandler.Assign(ref tplPT, "cancel", Resources.Lang.cancel);

            if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["nobackground"]))
            {
                TemplateHandler.RemoveBlock(ref tplPT, "editting");
            }

            BingoDef.AccountType accountType = (BingoDef.AccountType)(_cust.RoleId);
            AssignCommonHeader(ref tplPT);

            switch (accountType)
            {
                //case BingoDef.AccountType.Super:
                //    retHTML = GenBingoHTMLSuper(tplPT, _cust, BingoGroup);
                //    break;

                case BingoDef.AccountType.Master:
                    retHTML = GenBingoHTMLMaster(tplPT, _cust, BingoGroup);
                    break;

                case BingoDef.AccountType.Agent:
                    retHTML = GenBingoHTMLAgent(tplPT, _cust, BingoGroup);
                    break;

                case BingoDef.AccountType.Member:
                    retHTML = GenBingoHTMLMember(tplPT, _cust, BingoGroup);
                    break;
            }

            GenGameList(ref lsJSData);
            AssignJavaScript(ref retHTML, lsJSData);
            return retHTML;
        }

        /* FOR MASTER */

        private static string GenBingoHTMLMaster(string tplPT, ICustomer _cust, string BingoGroup)
        {
            #region local define

            int custId = _cust.ID;
            string gameKindName = "Live";
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(BingoDef.BINGO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(BingoDef.BINGO_OTHERS_PT_TEMPLATE);
            // Check remove Commision
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
            {
                TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.COMM_PT);
                TemplateHandler.RemoveBlock(ref tplData, BingoDef.COMM_PT);
            }
            string tplPrivate;
            double maxMMaxPT = BingoDef.MAX_MaxPTValue, maxMMinPT = BingoDef.MAX_MinPTValue, maxMComm = BingoDef.MAX_Comm, maxSPresetPT = BingoDef.MAX_PresetPTValue, maxSAutoPT = BingoDef.MAX_AutoPTValue; //max value
            double maxGroupMPT = BingoDef.MAX_MaxPTValue, maxGroupMMinPT = BingoDef.MAX_MinPTValue, maxGroupMComm = BingoDef.MAX_Comm, maxGroupSPresetPT = BingoDef.MAX_PresetPTValue, maxGroupSAutoPT = BingoDef.MAX_AutoPTValue; //max group value

            double MPTGroup = BingoDef.CURRENT_MaxPTValue, MMinPTGroup = BingoDef.CURRENT_MinPTValue, MCommGroup = BingoDef.CURRENT_Comm, SPresetPTGroup = BingoDef.MAX_PresetPTValue, SAutoPTGroup = BingoDef.MAX_AutoPTValue; //current GROUP value
            double MMaxPT = BingoDef.CURRENT_MaxPTValue, MMinPT = BingoDef.CURRENT_MinPTValue, MComm = BingoDef.CURRENT_Comm, SPresetPT = BingoDef.MAX_PresetPTValue, SAutoPT = BingoDef.MAX_AutoPTValue; // current value
            string strMPT = BingoDef.Master + BingoDef.strPresetPT, strMMinPT = BingoDef.Master + BingoDef.strMinPT, strMComm = BingoDef.Comm, strSPresetPT = BingoDef.Super + BingoDef.strPresetPT, strSAutoPT = BingoDef.Super + BingoDef.strAutoPT;

            int intervalPT = BingoDef.INTERVAL_PT; //interval of select options
            int intervalComm = BingoDef.INTERVAL_COMM;
            string sep = BingoDef.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 6;

            #endregion local define

            #region build header

            TemplateHandler.Assign(ref tplHeader, BingoDef.PRESET_PT, Resources.Lang.lblPresetPTSuper); //super preset
            TemplateHandler.Assign(ref tplHeader, BingoDef.AUTO_PT, Resources.Lang.lblAutoPTSuper); //super auto
            TemplateHandler.Assign(ref tplHeader, BingoDef.MAX_PT, Resources.Lang.lblMaxPTMaster);
            TemplateHandler.Assign(ref tplHeader, BingoDef.MIN_PT, Resources.Lang.MinPTMaster);
            TemplateHandler.Assign(ref tplHeader, BingoDef.COMM_PT, Resources.Lang.lblCommPTMaster);

            #endregion build header

            #region build content

            TemplateHandler.ReplaceBlock(ref tplData, BingoDef.TAG_HEADER, tplHeader);
            List<GameKind> lsGameKind = _cust.GameKindList;
            listGameKind = lsGameKind;
            bool useGroupPT = _cust.IsUsedGameKind;
            string postGameData = "";
            string hidPTData = "";

            #endregion build content

            foreach (GameKind gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;
                gameKindName = gameKindDetail.ID.ToString();
                string MPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strMPT;
                string strMMinPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strMMinPT;
                string strMCommGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strMComm;
                string strSPresetPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strSPresetPT;
                string strSAutoPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strSAutoPT;
                string strChkSAutoPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + BingoDef.strCHK + strSAutoPT;
                string funcOnChangeGroup = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                string funcClickChkAutoGroup = string.Format(BingoDef.BINGO_ChkAutoGROUP_CLICK_FUNC, "this," + useGroupPT.ToString().ToLower());
                List<GameType> lsGameType = gameKindDetail.GameTypeList;
                htGameType[gameKindDetail.ID] =  lsGameType;
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupSPresetPT = 0;
                maxGroupSAutoPT = 0;
                maxGroupMPT = 0;
                maxGroupMMinPT = 0;
                maxGroupMComm = 0;

                foreach (GameType gameTypeDetail in lsGameType)
                {
                    tplPrivate = TemplateHandler.CopyBlock(tmpTplData, BingoDef.TAG_PRIVATE_SETTING);
                    if (0 == totalGameType)
                    {
                        postGameData += gameKindDetail.ID + BingoDef.OPEN_GAMETYPELIST_MARK;
                    }
                    string tmpTplPrivate = tplPrivate;
                    string strMPTID = gameKindName + sep + gameTypeDetail.ID + sep + strMPT;
                    string strMMinPTID = gameKindName + sep + gameTypeDetail.ID + sep + strMMinPT;
                    string strMCommID = gameKindName + sep + gameTypeDetail.ID + sep + strMComm;
                    string strSPresetPTID = gameKindName + sep + gameTypeDetail.ID + sep + strSPresetPT;
                    string strSAutoPTID = gameKindName + sep + gameTypeDetail.ID + sep + strSAutoPT;
                    string strChkSAutoPTID = gameKindName + sep + gameTypeDetail.ID + sep + BingoDef.strCHK + strSAutoPT;
                    string funcOnChange = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_PARAM);
                    string funcOnChangePreset = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_PRESET_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_PRESET_PARAM);
                    string funcClickChkAuto = string.Format(BingoDef.BINGO_ChkAUTO_CLICK_FUNC, "this");
                    postGameData += gameTypeDetail.ID.ToString() + BingoDef.OPEN_STAKELIST_MARK;

                    // max master max/min pt when auto pt is used
                    //m3MaxAuto = (_cust.SyncBingo) ? gameTypeDetail.PositionTakingUpline : gameTypeDetail.PositionTaking;
                    m3MaxAuto = (_cust.SyncBingo) ? gameTypeDetail.PositionTakingUplineMax : gameTypeDetail.PositionTaking;

                    #region Gen gameType Node

                    hidPTData += strMPTID + sep + BingoDef.m3MaxAuto + BingoDef.SEP_PT_VALUE + m3MaxAuto.ToString() + BingoDef.SEP_PT;

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                       Helper.GenSelectOption(gameTypeDetail.PositionTakingUplineMax, gameTypeDetail.PositionTakingUpline, intervalPT, (useGroupPT || gameTypeDetail.IsAutoPositionTaking), strSPresetPTID, funcOnChangePreset));

                    TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt",
                       Helper.GenSelectOption(gameTypeDetail.PositionTakingUplineMax, gameTypeDetail.AutoPositionTaking, intervalPT, (useGroupPT || !gameTypeDetail.IsAutoPositionTaking), strSAutoPTID, funcOnChange));

                    TemplateHandler.Assign(ref tmpTplPrivate, "chkAuto", GenCheckBox(strChkSAutoPTID, useGroupPT, gameTypeDetail.IsAutoPositionTaking, funcClickChkAuto));

                    TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                        Helper.GenSelectOption(gameTypeDetail.PositionTakingMax, gameTypeDetail.PositionTaking, intervalPT, useGroupPT, strMPTID, funcOnChangePreset));

                    TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                        Helper.GenSelectOption(gameTypeDetail.PositionTaking, gameTypeDetail.MinPositionTaking, intervalPT, useGroupPT, strMMinPTID, funcOnChangePreset));

                    TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                        Helper.GenSelectOption(gameTypeDetail.CommissionUpline, gameTypeDetail.Commission, intervalComm, useGroupPT, strMCommID, funcOnChange, BingoDef.CLASS_SELECT_COMMISSION));

                    TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.Name);
                    TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);
                    TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                    #endregion Gen gameType Node

                    #region label error

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                          Helper.GenLabel(strSPresetPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt_error",
                                           Helper.GenLabel(strSAutoPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error", Helper.GenLabel(strMPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                           Helper.GenLabel(strMMinPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                           Helper.GenLabel(strMCommID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));

                    #endregion label error

                    #region Assign Stake levels

                    List<BettingRange> lsStakeLevel = gameTypeDetail.BettingRangeList;
                    string stakeLevels = "";
                    string funcChkStakeClick = "";
                    int totalStake = 0;
                    foreach (BettingRange stakeDetail in lsStakeLevel)
                    {
                        if (!stakeDetail.IsChecked && !_cust.SyncBingo)
                            continue;
                        string chkStakeID = gameKindDetail.ID + sep + gameTypeDetail.ID + sep +
                                         BingoDef.CHK_STAKE + sep + stakeDetail.ID;
                        stakeLevels += GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                          funcChkStakeClick) +
                                                          (totalStake == 0 ? Helper.GenLabel("", stakeDetail.Name, (_cust.SyncBingo) ? "chkDisabled" : "") : stakeDetail.Name);
                        postGameData += stakeDetail.ID + BingoDef.SEP_STAKE;
                        totalStake++;
                    }
                    TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                    gameTypeData.Append(tmpTplPrivate);
                    totalGameType++;

                    int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                    postGameData = postGameData.Substring(0, subLength) +
                                   BingoDef.CLOSE_STAKELIST_MARK + BingoDef.SEP_GAMETYPE; // ),

                    #endregion Assign Stake levels

                    #region Group Setting

                    if (totalGameType == lsGameType.Count)
                    {
                        m3MaxAuto = (_cust.SyncBingo) ? gameKindDetail.PositionTakingUplineMax : gameKindDetail.PositionTaking;

                        hidPTData += MPTGroupID + sep + BingoDef.m3MaxAuto + BingoDef.SEP_PT_VALUE + m3MaxAuto.ToString() + BingoDef.SEP_PT;

                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTakingUplineMax, gameKindDetail.PositionTakingUpline, intervalPT, gameKindDetail.IsAutoPositionTaking, strSPresetPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_auto_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTakingUplineMax, gameKindDetail.AutoPositionTaking, intervalPT, !gameKindDetail.IsAutoPositionTaking, strSAutoPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTakingMax, gameKindDetail.PositionTaking, intervalPT, false, MPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTaking, gameKindDetail.MinPositionTaking, intervalPT, false, strMMinPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                            Helper.GenSelectOption(gameKindDetail.CommissionUpline, gameKindDetail.Commission, intervalComm, false, strMCommGroupID, funcOnChangeGroup, BingoDef.CLASS_SELECT_COMMISSION));

                        TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", GenCheckBox(gameKindName + sep + BingoDef.CHKGROUP, false, useGroupPT, BingoDef.CLASS_CHECKBOX_HIDDEN, "")); //remove group setting check box
                        TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                        TemplateHandler.Assign(ref tmpTplData, "chkGroupAuto", GenCheckBox(strChkSAutoPTGroupID, false, gameKindDetail.IsAutoPositionTaking, funcClickChkAutoGroup));
                        TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.Name));
                        TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.Name);

                        TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", Helper.GenChangePTButtons(MPTGroupID, false));
                        TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", Helper.GenChangePTButtons(strMMinPTGroupID, false));
                        TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");
                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strSPresetPTGroupID, gameKindDetail.IsAutoPositionTaking));
                        TemplateHandler.Assign(ref tmpTplData, "group_auto_pt_btn", Helper.GenChangePTButtons(strSAutoPTGroupID, !gameKindDetail.IsAutoPositionTaking));

                        TemplateHandler.ReplaceBlock(ref tmpTplData, BingoDef.TAG_PRIVATE_SETTING, gameTypeData.ToString());

                        data.Append(tmpTplData);
                        totalGameType = 0;
                        gameTypeData = new StringBuilder();
                        postGameData = postGameData.Substring(0, postGameData.Length - 1) + BingoDef.CLOSE_GAMETYPELIST_MARK + BingoDef.SEP_GAMEKIND;
                    }

                    #endregion Group Setting
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");

            bool IsMultiple = (HttpContext.Current.Request["isMult"] == "1");
            TemplateHandler.Assign(ref tplPT, "isSync", (!_cust.SyncBingo && !IsMultiple).ToString().ToLower());
            TemplateHandler.Assign(ref tplPT, "exitConfirmMsg", (!_cust.SyncBingo && !IsMultiple) ? Resources.Bingo.exitBingoConfirmMsg : "");
            TemplateHandler.Assign(ref tplPT, "submit", Helper.GenLink(Lang.submit, BingoDef.BINGO_SUBMIT_HREF, BingoDef.CLASS_SUBMIT, BingoDef.BINGO_SUBMIT_FUNC));

            //assign Bet Setting
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoMaxTransfer)
                TemplateHandler.RemoveBlock(ref tplPT, "Bingo-bet-setting");
            else
                AssignBingoCustInfo(ref tplPT, _cust, false);

            //assign AGE customer info
            AssignIBCCustInfo(ref tplPT, _cust);

            //assign post data
            Hashtable hHiddenData = new Hashtable();
            hHiddenData.Add(BingoDef.HIDDEN_GAMEDATA, postGameData);
            hHiddenData.Add(BingoDef.HIDDEN_PT, hidPTData);
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(hHiddenData, _cust));
            //HttpContext.Current.Session[BingoDef.SESSION_CUSTOMER] = _cust;
            return tplPT.Replace(Environment.NewLine, string.Empty).Replace("  ", " ");
        }

        /* END MASTER */

        /* START AGENT */

        private static string GenBingoHTMLAgent(string tplPT, ICustomer _cust, string BingoGroup)
        {
            BingoDef.AccountType accType = (BingoDef.AccountType)Sunplus.Agent.Model.Entities.UserSession.Current.RoleId;
            switch (accType)
            {
                case BingoDef.AccountType.Super:
                    return GenBingoHTMLSuperOnAgent(tplPT, _cust, BingoGroup);
                case BingoDef.AccountType.Master:
                    return GenBingoHTMLMasterOnAgent(tplPT, _cust, BingoGroup);
            }
            return "";
        }

        private static string GenBingoHTMLMasterOnAgent(string tplPT, ICustomer _cust, string BingoGroup)
        {
            #region local define

            int custId = _cust.ID;
            string gameKindName = "Live";
            StringBuilder data = new StringBuilder();
            // Check remove Commision
            string tplHeader = TemplateHandler.LoadTemplate(BingoDef.BINGO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(BingoDef.BINGO_OTHERS_PT_TEMPLATE);
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
            {
                TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.COMM_PT);
                TemplateHandler.RemoveBlock(ref tplData, BingoDef.COMM_PT);
            }
            string tplPrivate;
            double maxAPT = BingoDef.MAX_MaxPTValue, maxAMinPT = BingoDef.MAX_MinPTValue, maxAComm = BingoDef.MAX_Comm, maxMPresetPT = BingoDef.MAX_PresetPTValue; //max value
            double maxGroupAPT = BingoDef.MAX_MaxPTValue, maxGroupAMinPT = BingoDef.MAX_MinPTValue, maxGroupAComm = BingoDef.MAX_Comm, maxGroupMPresetPT = BingoDef.MAX_PresetPTValue; //max group value

            double APTGroup = BingoDef.CURRENT_MaxPTValue, AMinPTGroup = BingoDef.CURRENT_MinPTValue, ACommGroup = BingoDef.CURRENT_Comm, MPresetPTGroup = BingoDef.MAX_PresetPTValue; //current GROUP value
            double APT = BingoDef.CURRENT_MaxPTValue, AMinPT = BingoDef.CURRENT_MinPTValue, AComm = BingoDef.CURRENT_Comm, MPresetPT = BingoDef.MAX_PresetPTValue; // current value
            string strAPT = BingoDef.Agent + BingoDef.strPresetPT, strAMinPT = BingoDef.Agent + BingoDef.strMinPT, strAComm = BingoDef.Comm, strMPresetPT = BingoDef.Master + BingoDef.strPresetPT;

            int intervalPT = BingoDef.INTERVAL_PT; //interval of select options
            int intervalComm = BingoDef.INTERVAL_COMM;
            string sep = BingoDef.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 4;

            #endregion local define

            #region build header

            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.AUTO_PT); //there are no auto master on agent function
            TemplateHandler.Assign(ref tplHeader, BingoDef.PRESET_PT, Resources.Lang.lblPresetPTMaster); //master preset
            TemplateHandler.Assign(ref tplHeader, BingoDef.MAX_PT, Resources.Lang.lblMaxPTAgent);
            TemplateHandler.Assign(ref tplHeader, BingoDef.MIN_PT, Resources.Lang.MinPTAgent);
            TemplateHandler.Assign(ref tplHeader, BingoDef.COMM_PT, Resources.Lang.lblCommPTAgent);

            #endregion build header

            #region build content

            TemplateHandler.RemoveBlock(ref tplData, BingoDef.AUTO_PT);
            TemplateHandler.ReplaceBlock(ref tplData, BingoDef.TAG_HEADER, tplHeader);
            List<GameKind> lsGameKind = _cust.GameKindList;
            listGameKind = lsGameKind;
            bool useGroupPT = _cust.IsUsedGameKind;
            string postGameData = "";

            #endregion build content

            foreach (GameKind gameKindDetail in lsGameKind)
            {
                #region local define

                string tmpTplData = tplData;
                gameKindName = gameKindDetail.ID.ToString();
                string APTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strAPT;
                string strAMinPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strAMinPT;
                string strACommGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strAComm;
                string strMPresetPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strMPresetPT;
                string funcOnChangeGroup = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_PARAM);

                List<GameType> lsGameType = gameKindDetail.GameTypeList;
                htGameType.Add(gameKindDetail.ID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupAPT = 0;
                maxGroupMPresetPT = 0;
                maxGroupAComm = 0;

                #endregion local define

                foreach (GameType gameTypeDetail in lsGameType)
                {
                    #region game type value

                    tplPrivate = TemplateHandler.CopyBlock(tmpTplData, BingoDef.TAG_PRIVATE_SETTING);
                    if (0 == totalGameType)
                    {
                        postGameData += gameKindDetail.ID + BingoDef.OPEN_GAMETYPELIST_MARK;
                    }
                    string tmpTplPrivate = tplPrivate;
                    string strAPTID = gameKindName + sep + gameTypeDetail.ID + sep + strAPT;
                    string strAMinPTID = gameKindName + sep + gameTypeDetail.ID + sep + strAMinPT;
                    string strACommID = gameKindName + sep + gameTypeDetail.ID + sep + strAComm;
                    string strMPresetPTID = gameKindName + sep + gameTypeDetail.ID + sep + strMPresetPT;
                    string funcOnChangePreset = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_PRESET_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_PRESET_PARAM);
                    postGameData += gameTypeDetail.ID.ToString() + BingoDef.OPEN_STAKELIST_MARK;

                    #endregion game type value

                    #region gen gameType Node

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                        Helper.GenSelectOption(gameTypeDetail.PositionTakingUplineMax, gameTypeDetail.PositionTakingUplineMin, gameTypeDetail.PositionTakingUpline, intervalPT, useGroupPT, strMPresetPTID, funcOnChangePreset));

                    TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                        Helper.GenSelectOption(gameTypeDetail.PositionTakingMax, gameTypeDetail.PositionTaking, intervalPT, useGroupPT, strAPTID, funcOnChangePreset));

                    TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                        Helper.GenSelectOption(gameTypeDetail.PositionTaking, gameTypeDetail.MinPositionTaking, intervalPT, useGroupPT, strAMinPTID, ""));

                    TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                        Helper.GenSelectOption(gameTypeDetail.CommissionUpline, gameTypeDetail.Commission, intervalComm, useGroupPT, strACommID, "", BingoDef.CLASS_SELECT_COMMISSION));

                    TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.Name);
                    TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                    #endregion gen gameType Node

                    #region label error

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                        Helper.GenLabel(strMPresetPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error", Helper.GenLabel(strAPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                        Helper.GenLabel(strAMinPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                        Helper.GenLabel(strACommID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                    #endregion label error

                    #region Betting Range

                    List<BettingRange> lsStakeLevel = new List<BettingRange>();
                    lsStakeLevel = gameTypeDetail.BettingRangeList;
                    string stakeLevels = "";
                    string funcChkStakeClick = "";
                    int totalStake = 0;
                    foreach (BettingRange stakeDetail in lsStakeLevel)
                    {
                        if (!stakeDetail.IsChecked && !_cust.SyncBingo)
                            continue;
                        string chkStakeID = gameKindDetail.ID + sep + gameTypeDetail.ID + sep +
                                         BingoDef.CHK_STAKE + sep + stakeDetail.ID;
                        stakeLevels += GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                          funcChkStakeClick) +
                                                          (totalStake == 0 ? Helper.GenLabel("", stakeDetail.Name, (_cust.SyncBingo) ? "chkDisabled" : "") : stakeDetail.Name);
                        postGameData += stakeDetail.ID + BingoDef.SEP_STAKE;
                        totalStake++;
                    }

                    //Assign Stake levels
                    TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                    gameTypeData.Append(tmpTplPrivate);
                    totalGameType++;

                    int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                    postGameData = postGameData.Substring(0, subLength) +
                                   BingoDef.CLOSE_STAKELIST_MARK + BingoDef.SEP_GAMETYPE; // ),

                    #endregion Betting Range

                    #region Group Setting

                    if (totalGameType == lsGameType.Count)
                    {
                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTakingUplineMax, gameKindDetail.PositionTakingUplineMin, gameKindDetail.PositionTakingUpline, intervalPT, false, strMPresetPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTakingMax, gameKindDetail.PositionTaking, intervalPT, false, APTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTaking, gameKindDetail.MinPositionTaking, intervalPT, false, strAMinPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                            Helper.GenSelectOption(gameKindDetail.CommissionUpline, gameKindDetail.Commission, intervalComm, false, strACommGroupID, funcOnChangeGroup, BingoDef.CLASS_SELECT_COMMISSION));

                        TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", GenCheckBox(gameKindName + sep + BingoDef.CHKGROUP, false, useGroupPT, BingoDef.CLASS_CHECKBOX_HIDDEN, "")); //remove group setting check box
                        TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                        TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.Name));
                        TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.Name);

                        TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", Helper.GenChangePTButtons(APTGroupID, false));
                        TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", Helper.GenChangePTButtons(strAMinPTGroupID, false));
                        TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");
                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strMPresetPTGroupID, false));

                        TemplateHandler.ReplaceBlock(ref tmpTplData, BingoDef.TAG_PRIVATE_SETTING, gameTypeData.ToString());
                        data.Append(tmpTplData);
                        totalGameType = 0;
                        gameTypeData = new StringBuilder();
                        postGameData = postGameData.Substring(0, postGameData.Length - 1) + BingoDef.CLOSE_GAMETYPELIST_MARK + BingoDef.SEP_GAMEKIND;

                    #endregion Group Setting
                    }
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");

            bool IsMultiple = (HttpContext.Current.Request["isMult"] == "1");
            TemplateHandler.Assign(ref tplPT, "isSync", (!_cust.SyncBingo && !IsMultiple).ToString().ToLower());
            TemplateHandler.Assign(ref tplPT, "exitConfirmMsg", (!_cust.SyncBingo && !IsMultiple) ? Resources.Bingo.exitBingoConfirmMsg : "");
            TemplateHandler.Assign(ref tplPT, "submit", Helper.GenLink(Lang.submit, BingoDef.BINGO_SUBMIT_HREF, BingoDef.CLASS_SUBMIT, BingoDef.BINGO_SUBMIT_FUNC));

            //assign Bet Setting
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoMaxTransfer)
                TemplateHandler.RemoveBlock(ref tplPT, "Bingo-bet-setting");
            else
                AssignBingoCustInfo(ref tplPT, _cust, false);

            //assign AGE customer info
            AssignIBCCustInfo(ref tplPT, _cust);

            //assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
            //HttpContext.Current.Session[BingoDef.SESSION_CUSTOMER] = _cust;
            return tplPT;
        }

        private static string GenBingoHTMLSuperOnAgent(string tplPT, ICustomer _cust, string BingoGroup)
        {
            #region local define

            if (!_cust.SyncBingo)
                return Lang.noInfo;
            int custId = _cust.ID;
            string gameKindName = "Live";
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(BingoDef.BINGO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(BingoDef.BINGO_OTHERS_PT_TEMPLATE);
            // Check remove Commision
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
            {
                TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.COMM_PT);
                TemplateHandler.RemoveBlock(ref tplData, BingoDef.COMM_PT);
            }
            string tplPrivate;
            double maxAMaxPT = BingoDef.MAX_MaxPTValue, maxAMinPT = BingoDef.MAX_MinPTValue, maxAComm = BingoDef.MAX_Comm, maxMPresetPT = BingoDef.MAX_PresetPTValue; //max value
            double maxGroupAMaxPT = BingoDef.MAX_MaxPTValue, maxGroupAMinPT = BingoDef.MAX_MinPTValue, maxGroupAComm = BingoDef.MAX_Comm, maxGroupMPresetPT = BingoDef.MAX_PresetPTValue; //max group value

            double AMaxPTGroup = BingoDef.CURRENT_MaxPTValue, AMinPTGroup = BingoDef.CURRENT_MinPTValue, ACommGroup = BingoDef.CURRENT_Comm, MPresetPTGroup = BingoDef.MAX_PresetPTValue; //current GROUP value
            double AMaxPT = BingoDef.CURRENT_MaxPTValue, AMinPT = BingoDef.CURRENT_MinPTValue, AComm = BingoDef.CURRENT_Comm, MPresetPT = BingoDef.MAX_PresetPTValue; // current value
            string strAMaxPT = BingoDef.Agent + BingoDef.strMaxPT, strAMinPT = BingoDef.Agent + BingoDef.strMinPT, strAComm = BingoDef.Comm, strMPresetPT = BingoDef.Master + BingoDef.strPresetPT;

            int intervalPT = BingoDef.INTERVAL_PT; //interval of select options
            int intervalComm = BingoDef.INTERVAL_COMM;
            string sep = BingoDef.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 2;

            #endregion local define

            #region build header

            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.AUTO_PT); //there are no auto master on agent function
            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.MAX_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.COMM_PT);
            TemplateHandler.Assign(ref tplHeader, BingoDef.PRESET_PT, Resources.Lang.lblPositionTakingMaster); //master preset
            TemplateHandler.Assign(ref tplHeader, BingoDef.MIN_PT, Resources.Lang.lblPositionTakingAgent);

            #endregion build header

            #region build content

            TemplateHandler.RemoveBlock(ref tplData, BingoDef.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplData, BingoDef.MAX_PT);
            TemplateHandler.RemoveBlock(ref tplData, BingoDef.COMM_PT);
            TemplateHandler.ReplaceBlock(ref tplData, BingoDef.TAG_HEADER, tplHeader);
            List<GameKind> lsGameKind = _cust.GameKindList;
            listGameKind = lsGameKind;
            bool useGroupPT = _cust.IsUsedGameKind;
            string postGameData = "";

            #endregion build content

            foreach (GameKind gameKindDetail in lsGameKind)
            {
                #region local define

                string tmpTplData = tplData;
                gameKindName = gameKindDetail.ID.ToString();
                List<GameType> lsGameType = gameKindDetail.GameTypeList;
                htGameType.Add(gameKindDetail.ID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();

                #endregion local define

                foreach (GameType gameTypeDetail in lsGameType)
                {
                    #region Gen GameType Node

                    tplPrivate = TemplateHandler.CopyBlock(tmpTplData, BingoDef.TAG_PRIVATE_SETTING);
                    string AMaxPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strAMaxPT;
                    string strAMinPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strAMinPT;
                    string strACommGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strAComm;
                    string strMPresetPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strMPresetPT;
                    string funcOnChangeGroup = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_PARAM);

                    #region Group Setting

                    if (0 == totalGameType)
                    {
                        postGameData += gameKindDetail.ID + BingoDef.OPEN_GAMETYPELIST_MARK;
                        TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", GenCheckBox(gameKindName + sep + BingoDef.CHKGROUP, false, useGroupPT, BingoDef.CLASS_CHECKBOX_HIDDEN, "")); //remove group setting check box
                        TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                        TemplateHandler.Assign(ref tmpTplData, "group_min_pt", Helper.GenLabel(strAMinPTGroupID, CastAide.AsString(gameKindDetail.PositionTaking), ""));
                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt", Helper.GenLabel(strMPresetPTGroupID, CastAide.AsString(gameKindDetail.PositionTakingUpline), ""));
                        TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", "");
                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", "");
                    }

                    #endregion Group Setting

                    //game type value
                    string tmpTplPrivate = tplPrivate;
                    string strAMaxPTID = gameKindName + sep + gameTypeDetail.ID + sep + strAMaxPT;
                    string strAMinPTID = gameKindName + sep + gameTypeDetail.ID + sep + strAMinPT;
                    string strACommID = gameKindName + sep + gameTypeDetail.ID + sep + strAComm;
                    string strMPresetPTID = gameKindName + sep + gameTypeDetail.ID + sep + strMPresetPT;
                    string funcOnChange = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_PARAM);
                    postGameData += gameTypeDetail.ID.ToString() + BingoDef.OPEN_STAKELIST_MARK;

                    //master preset
                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt", Helper.GenLabel(strMPresetPTID, CastAide.AsString(gameTypeDetail.PositionTakingUpline), ""));
                    TemplateHandler.Assign(ref tmpTplPrivate, "min_pt", Helper.GenLabel(strAMinPTID, CastAide.AsString(gameTypeDetail.PositionTaking), ""));
                    TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.Name);
                    TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                    #endregion Gen GameType Node

                    #region label error

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                        Helper.GenLabel(strMPresetPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                        Helper.GenLabel(strAMinPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                    #endregion label error

                    #region Betting Range

                    List<BettingRange> lsStakeLevel = new List<BettingRange>();
                    lsStakeLevel = gameTypeDetail.BettingRangeList;
                    string stakeLevels = "";
                    string funcChkStakeClick = "";
                    int totalStake = 0;
                    foreach (BettingRange stakeDetail in lsStakeLevel)
                    {
                        if (!stakeDetail.IsChecked && !_cust.SyncBingo)
                            continue;
                        string chkStakeID = gameKindDetail.ID + sep + gameTypeDetail.ID + sep +
                                         BingoDef.CHK_STAKE + sep + stakeDetail.ID;
                        stakeLevels += GenCheckBox(chkStakeID, true, stakeDetail.IsChecked,
                                                          funcChkStakeClick) +
                                                          (totalStake == 0 ? Helper.GenLabel("", stakeDetail.Name, (_cust.SyncBingo) ? "chkDisabled" : "") : stakeDetail.Name);
                        postGameData += stakeDetail.ID + BingoDef.SEP_STAKE;
                        totalStake++;
                    }

                    //Assign Stake levels
                    TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                    gameTypeData.Append(tmpTplPrivate);
                    totalGameType++;

                    int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                    postGameData = postGameData.Substring(0, subLength) +
                                   BingoDef.CLOSE_STAKELIST_MARK + BingoDef.SEP_GAMETYPE; // ),

                    #endregion Betting Range

                    #region Group Setting

                    if (totalGameType == lsGameType.Count)
                    {
                        TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.Name));
                        TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.Name);
                        TemplateHandler.ReplaceBlock(ref tmpTplData, BingoDef.TAG_PRIVATE_SETTING, gameTypeData.ToString());

                        data.Append(tmpTplData);
                        totalGameType = 0;
                        gameTypeData = new StringBuilder();
                        postGameData = postGameData.Substring(0, postGameData.Length - 1) + BingoDef.CLOSE_GAMETYPELIST_MARK + BingoDef.SEP_GAMEKIND;
                    }

                    #endregion Group Setting
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");

            TemplateHandler.Assign(ref tplPT, "isSync", "false");
            //assign Bet Setting
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoMaxTransfer)
                TemplateHandler.RemoveBlock(ref tplPT, "Bingo-bet-setting");
            else
                AssignBingoCustInfo(ref tplPT, _cust, true);

            //assign AGE customer info
            AssignIBCCustInfo(ref tplPT, _cust);

            //assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
            //HttpContext.Current.Session[BingoDef.SESSION_CUSTOMER] = _cust;
            return tplPT;
        }

        /* END AGENT */

        /* START MEMBER */

        private static string GenBingoHTMLMember(string tplPT, ICustomer _cust, string BingoGroup)
        {
            BingoDef.AccountType accType = (BingoDef.AccountType)Sunplus.Agent.Model.Entities.UserSession.Current.RoleId;
            switch (accType)
            {
                case BingoDef.AccountType.Super:
                    return GenBingoHTMLSuperOnMember(tplPT, _cust, BingoGroup);
                case BingoDef.AccountType.Master:
                    return GenBingoHTMLMasterOnMember(tplPT, _cust, BingoGroup);
                case BingoDef.AccountType.Agent:
                    return GenBingoHTMLAgentOnMember(tplPT, _cust, BingoGroup);
            }
            return "";
        }

        private static string GenBingoHTMLSuperOnMember(string tplPT, ICustomer _cust, string BingoGroup)
        {
            #region local define

            bool IsMultiple = (HttpContext.Current.Request["isMult"] == "1");
            if (!BingoDef.IBCSetting.Bingo.ShowBetSetting || IsMultiple)
            {
                TemplateHandler.RemoveBlock(ref tplPT, BingoDef.TAG_BET_SETTING);
            }
            if (IsMultiple)
            {
                TemplateHandler.RemoveBlock(ref tplPT, "age-info");
            }
            if (!_cust.SyncBingo)
            {
                return Lang.noInfo; //this member have not been synchronized to Bingo yet.
            }
            int custId = _cust.ID;
            string gameKindName = "Live";
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(BingoDef.BINGO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(BingoDef.BINGO_OTHERS_PT_TEMPLATE);
            // Check remove Commision
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
            {
                TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.COMM_PT);
                TemplateHandler.RemoveBlock(ref tplData, BingoDef.COMM_PT);
            }
            string tplPrivate;
            //master
            double maxAPT = BingoDef.MAX_MaxPTValue; //max value
            double APT = BingoDef.CURRENT_MaxPTValue;
            double maxMaPT = BingoDef.MAX_MaxPTValue, maxSPresetPT = BingoDef.MAX_PresetPTValue, maxSAutoPT = BingoDef.MAX_AutoPTValue; //max value
            double maxGroupMaPT = BingoDef.MAX_MaxPTValue, maxGropuAPT = BingoDef.MAX_MaxPTValue, maxGroupSPresetPT = BingoDef.MAX_PresetPTValue, maxGroupSAutoPT = BingoDef.MAX_AutoPTValue; //max group value
            double MaPTGroup = BingoDef.CURRENT_MaxPTValue, APTGroup = BingoDef.CURRENT_MaxPTValue, SPresetPTGroup = BingoDef.MAX_PresetPTValue, SAutoPTGroup = BingoDef.MAX_AutoPTValue; //current GROUP value
            double MaPT = BingoDef.CURRENT_MaxPTValue, MMinPT = BingoDef.CURRENT_MinPTValue, MComm = BingoDef.CURRENT_Comm, SPresetPT = BingoDef.MAX_PresetPTValue, SAutoPT = BingoDef.MAX_AutoPTValue; // current value
            string strMaPT = BingoDef.Master + BingoDef.strPT, strAPT = BingoDef.Agent + BingoDef.strPT, strSPresetPT = BingoDef.Super + BingoDef.strPresetPT, strSAutoPT = BingoDef.Super + BingoDef.strAutoPT;

            int intervalPT = BingoDef.INTERVAL_PT; //interval of select options
            int intervalComm = BingoDef.INTERVAL_COMM;
            string sep = BingoDef.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 5;

            #endregion local define

            #region build header

            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.COMM_PT);
            TemplateHandler.Assign(ref tplHeader, BingoDef.PRESET_PT, Resources.Lang.lblPositionTakingSuper); //super preset
            TemplateHandler.Assign(ref tplHeader, BingoDef.AUTO_PT, Resources.Lang.lblAutoPTSuper); //super auto

            TemplateHandler.Assign(ref tplHeader, BingoDef.MAX_PT, Resources.Lang.lblPositionTakingMaster);
            TemplateHandler.Assign(ref tplHeader, BingoDef.MIN_PT, Resources.Lang.lblPositionTakingAgent);

            #endregion build header

            #region build content

            TemplateHandler.RemoveBlock(ref tplData, BingoDef.COMM_PT);
            TemplateHandler.ReplaceBlock(ref tplData, BingoDef.TAG_HEADER, tplHeader);

            List<GameKind> lsGameKind = _cust.GameKindList;
            listGameKind = lsGameKind;
            bool useGroupPT = _cust.IsUsedGameKind;
            string postGameData = "";

            #endregion build content

            foreach (GameKind gameKindDetail in lsGameKind)
            {
                #region local define

                string tmpTplData = tplData;
                gameKindName = gameKindDetail.ID.ToString();
                string MaPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strMaPT;
                string APTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strAPT;
                string strSPresetPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strSPresetPT;
                string strSAutoPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strSAutoPT;
                string strChkSAutoPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + BingoDef.strCHK + strSAutoPT;
                string funcOnChangeGroup = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                string funcClickChkAutoGroup = string.Format(BingoDef.BINGO_ChkAutoGROUP_CLICK_FUNC, "this," + useGroupPT.ToString().ToLower());

                List<GameType> lsGameType = gameKindDetail.GameTypeList;
                htGameType.Add(gameKindDetail.ID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupSPresetPT = 0;
                maxGroupSAutoPT = 0;

                #endregion local define

                foreach (GameType gameTypeDetail in lsGameType)
                {
                    #region game type value

                    tplPrivate = TemplateHandler.CopyBlock(tmpTplData, BingoDef.TAG_PRIVATE_SETTING);
                    if (0 == totalGameType)
                    {
                        postGameData += gameKindDetail.ID + BingoDef.OPEN_GAMETYPELIST_MARK;
                    }

                    string tmpTplPrivate = tplPrivate;
                    string strMaPTID = gameKindName + sep + gameTypeDetail.ID + sep + strMaPT;
                    string strAPTID = gameKindName + sep + gameTypeDetail.ID + sep + strAPT;
                    string strSPresetPTID = gameKindName + sep + gameTypeDetail.ID + sep + strSPresetPT;
                    string strSAutoPTID = gameKindName + sep + gameTypeDetail.ID + sep + strSAutoPT;
                    string strChkSAutoPTID = gameKindName + sep + gameTypeDetail.ID + sep + BingoDef.strCHK + strSAutoPT;
                    string funcOnChange = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_PARAM);
                    string funcClickChkAuto = string.Format(BingoDef.BINGO_ChkAUTO_CLICK_FUNC, "this");
                    postGameData += gameTypeDetail.ID.ToString() + BingoDef.OPEN_STAKELIST_MARK;

                    #endregion game type value

                    #region Gen GameType Node

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                        Helper.GenSelectOption(gameTypeDetail.PositionTakingMax, gameTypeDetail.MinPositionTaking, gameTypeDetail.PositionTaking, intervalPT, (useGroupPT || gameTypeDetail.IsAutoPositionTaking), strSPresetPTID, funcOnChange));

                    TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt",
                        Helper.GenSelectOption(gameTypeDetail.PositionTakingUplineMax, gameTypeDetail.AutoPositionTaking, intervalPT, (useGroupPT || !gameTypeDetail.IsAutoPositionTaking), strSAutoPTID, funcOnChange));

                    TemplateHandler.Assign(ref tmpTplPrivate, "chkAuto", GenCheckBox(strChkSAutoPTID, useGroupPT, gameTypeDetail.IsAutoPositionTaking, funcClickChkAuto));

                    TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                        Helper.GenLabel(strMaPTID, gameTypeDetail.PositionTakingMaster.ToString(), ""));

                    TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                        Helper.GenLabel(strAPTID, gameTypeDetail.PositionTakingAgent.ToString(), ""));

                    TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.Name);
                    TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                    #endregion Gen GameType Node

                    #region label error

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                        Helper.GenLabel(strSPresetPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt_error",
                        Helper.GenLabel(strSAutoPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error",
                        Helper.GenLabel(strMaPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                        Helper.GenLabel(strAPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                    #endregion label error

                    #region Betting Range

                    List<BettingRange> lsStakeLevel = new List<BettingRange>();
                    lsStakeLevel = gameTypeDetail.BettingRangeList;
                    string stakeLevels = "";
                    string funcChkStakeClick = "";
                    int totalStake = 0;
                    foreach (BettingRange stakeDetail in lsStakeLevel)
                    {
                        if (!stakeDetail.IsChecked && !_cust.SyncBingo)
                            continue;
                        string chkStakeID = gameKindDetail.ID + sep + gameTypeDetail.ID + sep +
                                         BingoDef.CHK_STAKE + sep + stakeDetail.ID;
                        stakeLevels += GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                          funcChkStakeClick) +
                                                          (totalStake == 0 ? Helper.GenLabel("", stakeDetail.Name, (_cust.SyncBingo) ? "chkDisabled" : "") : stakeDetail.Name);
                        postGameData += stakeDetail.ID + BingoDef.SEP_STAKE;
                        totalStake++;
                    }

                    //Assign Stake levels
                    TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                    gameTypeData.Append(tmpTplPrivate);
                    totalGameType++;

                    int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                    postGameData = postGameData.Substring(0, subLength) +
                                   BingoDef.CLOSE_STAKELIST_MARK + BingoDef.SEP_GAMETYPE; // ),

                    #endregion Betting Range

                    if (totalGameType == lsGameType.Count)
                    {
                        #region Group Setting

                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTakingMax, gameKindDetail.MinPositionTaking, gameKindDetail.PositionTaking, intervalPT, gameKindDetail.IsAutoPositionTaking, strSPresetPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_auto_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTakingUplineMax, gameKindDetail.AutoPositionTaking, intervalPT, !gameKindDetail.IsAutoPositionTaking, strSAutoPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                            Helper.GenLabel(MaPTGroupID, gameKindDetail.PositionTakingMaster.ToString(), ""));

                        TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                            Helper.GenLabel(APTGroupID, gameKindDetail.PositionTakingAgent.ToString(), ""));

                        TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", GenCheckBox(gameKindName + sep + BingoDef.CHKGROUP, false, useGroupPT, BingoDef.CLASS_CHECKBOX_HIDDEN, "")); //remove group setting check box
                        TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                        TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.Name));
                        TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.Name);

                        TemplateHandler.Assign(ref tmpTplData, "chkGroupAuto", GenCheckBox(strChkSAutoPTGroupID, false, gameKindDetail.IsAutoPositionTaking, funcClickChkAutoGroup));
                        TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", "");
                        TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", "");
                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strSPresetPTGroupID, gameKindDetail.IsAutoPositionTaking));
                        TemplateHandler.Assign(ref tmpTplData, "group_auto_pt_btn", Helper.GenChangePTButtons(strSAutoPTGroupID, !gameKindDetail.IsAutoPositionTaking));

                        TemplateHandler.ReplaceBlock(ref tmpTplData, BingoDef.TAG_PRIVATE_SETTING, gameTypeData.ToString());
                        data.Append(tmpTplData);
                        totalGameType = 0;
                        gameTypeData = new StringBuilder();
                        postGameData = postGameData.Substring(0, postGameData.Length - 1) + BingoDef.CLOSE_GAMETYPELIST_MARK + BingoDef.SEP_GAMEKIND;

                        #endregion Group Setting
                    }
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");

            TemplateHandler.Assign(ref tplPT, "isSync", (!_cust.SyncBingo && !IsMultiple).ToString().ToLower());
            TemplateHandler.Assign(ref tplPT, "exitConfirmMsg", (!_cust.SyncBingo && !IsMultiple) ? Resources.Bingo.exitBingoConfirmMsg : "");
            TemplateHandler.Assign(ref tplPT, "submit", Helper.GenLink(Lang.submit, BingoDef.BINGO_SUBMIT_HREF, BingoDef.CLASS_SUBMIT, BingoDef.BINGO_SUBMIT_FUNC));

            //assign Bet Setting
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoMaxTransfer)
                TemplateHandler.RemoveBlock(ref tplPT, "Bingo-bet-setting");
            else
                AssignBingoCustInfo(ref tplPT, _cust, true);

            //assign AGE customer info
            AssignIBCCustInfo(ref tplPT, _cust);

            //assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
            //HttpContext.Current.Session[BingoDef.SESSION_CUSTOMER] = _cust;
            return tplPT;
        }

        public static string GenBingoHTMLMasterOnMember(string tplPT, ICustomer _cust, string BingoGroup)
        {
            #region local define

            bool IsMultiple = (HttpContext.Current.Request["isMult"] == "1");
            if (!BingoDef.IBCSetting.Bingo.ShowBetSetting || IsMultiple)
            {
                TemplateHandler.RemoveBlock(ref tplPT, BingoDef.TAG_BET_SETTING);
            }
            if (IsMultiple)
            {
                TemplateHandler.RemoveBlock(ref tplPT, "age-info");
            }

            if (!_cust.SyncBingo)
                return Lang.noInfo;
            int custId = _cust.ID;
            string gameKindName = "Live";
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(BingoDef.BINGO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(BingoDef.BINGO_OTHERS_PT_TEMPLATE);
            // Check remove Commision
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
            {
                TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.COMM_PT);
                TemplateHandler.RemoveBlock(ref tplData, BingoDef.COMM_PT);
            }
            string tplPrivate;
            double maxMMaxPT = BingoDef.MAX_PresetPTValue, maxMPresetPT = BingoDef.MAX_PresetPTValue; //max value
            double maxGroupMPT = BingoDef.MAX_PresetPTValue, maxGroupAPT = BingoDef.MAX_PresetPTValue; //max group value

            double MPTGroup = BingoDef.CURRENT_PresetPTValue, APTGroup = BingoDef.MAX_PresetPTValue; //current GROUP value
            double MPT = BingoDef.CURRENT_MaxPTValue, APT = BingoDef.MAX_PresetPTValue; //current value
            string strMPT = BingoDef.Master + BingoDef.strPresetPT, strAPT = BingoDef.Agent + BingoDef.strPT;

            int intervalPT = BingoDef.INTERVAL_PT; //interval of select options
            int intervalComm = BingoDef.INTERVAL_COMM;
            string sep = BingoDef.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 2;

            #endregion local define

            #region build header

            TemplateHandler.Assign(ref tplHeader, BingoDef.PRESET_PT, Resources.Lang.lblPositionTakingMaster); //master pt
            TemplateHandler.Assign(ref tplHeader, BingoDef.MAX_PT, Resources.Lang.lblPositionTakingAgent); //agent pt

            //remove min, max, auto pt
            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.COMM_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.MIN_PT);

            #endregion build header

            #region build content

            TemplateHandler.RemoveBlock(ref tplData, BingoDef.COMM_PT);
            TemplateHandler.RemoveBlock(ref tplData, BingoDef.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplData, BingoDef.MIN_PT);
            TemplateHandler.ReplaceBlock(ref tplData, BingoDef.TAG_HEADER, tplHeader);

            List<GameKind> lsGameKind = _cust.GameKindList;
            listGameKind = lsGameKind;
            bool useGroupPT = _cust.IsUsedGameKind;
            string postGameData = "";

            #endregion build content

            foreach (GameKind gameKindDetail in lsGameKind)
            {
                #region local define

                string tmpTplData = tplData;
                gameKindName = gameKindDetail.ID.ToString();
                string strMPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strMPT;
                string strAPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strAPT;
                string funcOnChangeGroup = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                List<GameType> lsGameType = gameKindDetail.GameTypeList;
                htGameType.Add(gameKindDetail.ID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupMPT = 0;

                #endregion local define

                foreach (GameType gameTypeDetail in lsGameType)
                {
                    #region game type value

                    tplPrivate = TemplateHandler.CopyBlock(tmpTplData, BingoDef.TAG_PRIVATE_SETTING);
                    if (0 == totalGameType)
                        postGameData += gameKindDetail.ID + BingoDef.OPEN_GAMETYPELIST_MARK;

                    string tmpTplPrivate = tplPrivate;
                    string strMPTID = gameKindName + sep + gameTypeDetail.ID + sep + strMPT;
                    string strAPTID = gameKindName + sep + gameTypeDetail.ID + sep + strAPT;
                    string funcOnChange = "";//string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_PARAM);
                    postGameData += gameTypeDetail.ID.ToString() + BingoDef.OPEN_STAKELIST_MARK;

                    #endregion game type value

                    #region Gen GameType Node

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                        Helper.GenSelectOption(gameTypeDetail.PositionTakingMax, gameTypeDetail.MinPositionTaking, gameTypeDetail.PositionTaking, intervalPT, useGroupPT, strMPTID, funcOnChange));
                    TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                        Helper.GenLabel(strAPTID, gameTypeDetail.PositionTakingAgent.ToString(), ""));
                    TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.Name);
                    TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                    #endregion Gen GameType Node

                    #region label error

                    TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error",
                        Helper.GenLabel(strAPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                        Helper.GenLabel(strMPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                    #endregion label error

                    #region Betting Range

                    List<BettingRange> lsStakeLevel = new List<BettingRange>();
                    //lsStakeLevel = (_cust.SyncBingo) ? GetStakeLevels(_cust.ID, gameTypeDetail.ID) : GetStakeLevels(gameTypeDetail.ID);
                    lsStakeLevel = gameTypeDetail.BettingRangeList;
                    string stakeLevels = "";
                    string funcChkStakeClick = "";
                    int totalStake = 0;
                    foreach (BettingRange stakeDetail in lsStakeLevel)
                    {
                        if (!stakeDetail.IsChecked && !_cust.SyncBingo)
                            continue;
                        string chkStakeID = gameKindDetail.ID + sep + gameTypeDetail.ID + sep +
                                         BingoDef.CHK_STAKE + sep + stakeDetail.ID;
                        stakeLevels += GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                          funcChkStakeClick) +
                                                          (totalStake == 0 ? Helper.GenLabel("", stakeDetail.Name, (_cust.SyncBingo) ? "chkDisabled" : "") : stakeDetail.Name);
                        postGameData += stakeDetail.ID + BingoDef.SEP_STAKE;
                        totalStake++;
                    }

                    //Assign Stake levels
                    TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                    gameTypeData.Append(tmpTplPrivate);
                    totalGameType++;

                    int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                    postGameData = postGameData.Substring(0, subLength) +
                                   BingoDef.CLOSE_STAKELIST_MARK + BingoDef.SEP_GAMETYPE; // ),

                    #endregion Betting Range

                    if (totalGameType == lsGameType.Count)
                    {
                        #region group setting

                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTakingMax, gameKindDetail.MinPositionTaking, gameKindDetail.PositionTaking, intervalPT, false, strMPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                            Helper.GenLabel(strAPTGroupID, gameKindDetail.PositionTakingAgent.ToString(), ""));

                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strMPTGroupID, false));
                        TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", "");

                        TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", GenCheckBox(gameKindName + sep + BingoDef.CHKGROUP, false, useGroupPT, BingoDef.CLASS_CHECKBOX_HIDDEN, "")); //remove group setting check box
                        TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                        TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.Name));
                        TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.Name);

                        TemplateHandler.ReplaceBlock(ref tmpTplData, BingoDef.TAG_PRIVATE_SETTING, gameTypeData.ToString());
                        data.Append(tmpTplData);
                        totalGameType = 0;
                        gameTypeData = new StringBuilder();
                        postGameData = postGameData.Substring(0, postGameData.Length - 1) + BingoDef.CLOSE_GAMETYPELIST_MARK + BingoDef.SEP_GAMEKIND;

                        #endregion group setting
                    }
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");

            TemplateHandler.Assign(ref tplPT, "isSync", (!_cust.SyncBingo && !IsMultiple).ToString().ToLower());
            TemplateHandler.Assign(ref tplPT, "exitConfirmMsg", (!_cust.SyncBingo && !IsMultiple) ? Resources.Bingo.exitBingoConfirmMsg : "");
            TemplateHandler.Assign(ref tplPT, "submit", Helper.GenLink(Lang.submit, BingoDef.BINGO_SUBMIT_HREF, BingoDef.CLASS_SUBMIT, BingoDef.BINGO_SUBMIT_FUNC));

            //assign Bet Setting
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoMaxTransfer)
                TemplateHandler.RemoveBlock(ref tplPT, "Bingo-bet-setting");
            else
                AssignBingoCustInfo(ref tplPT, _cust, true);

            //assign AGE customer info
            AssignIBCCustInfo(ref tplPT, _cust);

            //assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
            //HttpContext.Current.Session[BingoDef.SESSION_CUSTOMER] = _cust;
            return tplPT;
        }

        private static string GenBingoHTMLAgentOnMember(string tplPT, ICustomer _cust, string BingoGroup)
        {
            #region local define

            if (!BingoDef.IBCSetting.Bingo.ShowBetSetting)
                TemplateHandler.RemoveBlock(ref tplPT, BingoDef.TAG_BET_SETTING);
            int custId = _cust.ID;
            string gameKindName = "Live";
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(BingoDef.BINGO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(BingoDef.BINGO_OTHERS_PT_TEMPLATE);
            // Check remove Commision
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
            {
                TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.COMM_PT);
                TemplateHandler.RemoveBlock(ref tplData, BingoDef.COMM_PT);
            }
            string tplGroup;
            string tplPrivate;
            double maxMComm = BingoDef.MAX_Comm, maxAPresetPT = BingoDef.MAX_PresetPTValue; //max value
            double maxGroupMComm = BingoDef.MAX_Comm, maxGroupAPresetPT = BingoDef.MAX_PresetPTValue; //max group value

            double MCommGroup = BingoDef.CURRENT_Comm, APresetPTGroup = BingoDef.MAX_PresetPTValue; //current GROUP value
            double MComm = BingoDef.CURRENT_Comm, APresetPT = BingoDef.MAX_PresetPTValue; // current value
            string strMComm = BingoDef.Comm, strAPresetPT = BingoDef.Agent + BingoDef.strPresetPT;

            int intervalPT = BingoDef.INTERVAL_PT; //interval of select options
            int intervalComm = BingoDef.INTERVAL_COMM;
            string sep = BingoDef.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 2;

            #endregion local define

            #region build header

            TemplateHandler.Assign(ref tplHeader, BingoDef.PRESET_PT, Resources.Lang.lblPresetPTAgent); //agent pt
            TemplateHandler.Assign(ref tplHeader, BingoDef.COMM_PT, Resources.Lang.lblMemberCommission);

            //remove min, max, auto pt
            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.MAX_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, BingoDef.MIN_PT);

            #endregion build header

            #region build content

            TemplateHandler.RemoveBlock(ref tplData, BingoDef.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplData, BingoDef.MAX_PT);
            TemplateHandler.RemoveBlock(ref tplData, BingoDef.MIN_PT);
            TemplateHandler.ReplaceBlock(ref tplData, BingoDef.TAG_HEADER, tplHeader);
            List<GameKind> lsGameKind = _cust.GameKindList;
            listGameKind = lsGameKind;
            bool useGroupPT = _cust.IsUsedGameKind;
            string postGameData = "";

            #endregion build content

            foreach (GameKind gameKindDetail in lsGameKind)
            {
                #region local define

                string tmpTplData = tplData;
                gameKindName = gameKindDetail.ID.ToString();
                string strMCommGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strMComm;
                string strAPresetPTGroupID = gameKindName + sep + BingoDef.BINGO_GAME_GROUP + sep + strAPresetPT;
                string funcOnChangeGroup = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_GROUP_PARAM);

                List<GameType> lsGameType = gameKindDetail.GameTypeList;
                htGameType.Add(gameKindDetail.ID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupAPresetPT = 0;
                maxGroupMComm = 0;

                #endregion local define

                foreach (GameType gameTypeDetail in lsGameType)
                {
                    #region game type value

                    tplPrivate = TemplateHandler.CopyBlock(tmpTplData, BingoDef.TAG_PRIVATE_SETTING);
                    if (0 == totalGameType)
                        postGameData += gameKindDetail.ID + BingoDef.OPEN_GAMETYPELIST_MARK;

                    string tmpTplPrivate = tplPrivate;
                    string strMCommID = gameKindName + sep + gameTypeDetail.ID + sep + strMComm;
                    string strAPresetPTID = gameKindName + sep + gameTypeDetail.ID + sep + strAPresetPT;
                    string funcOnChange = string.Format(BingoDef.BINGO_SELECTOPTION_ONCHANGE_FUNC, BingoDef.BINGO_SELECTOPTION_ONCHANGE_PARAM);
                    postGameData += gameTypeDetail.ID.ToString() + BingoDef.OPEN_STAKELIST_MARK;

                    #endregion game type value

                    #region Gen GameType Node

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                        Helper.GenSelectOption(gameTypeDetail.PositionTakingMax, gameTypeDetail.MinPositionTaking, (_cust.SyncBingo) ? gameTypeDetail.PositionTaking : gameTypeDetail.PositionTakingMax, intervalPT, useGroupPT, strAPresetPTID, funcOnChange));

                    TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                        Helper.GenSelectOption(gameTypeDetail.CommissionUpline, gameTypeDetail.Commission, intervalComm, useGroupPT, strMCommID, funcOnChange, BingoDef.CLASS_SELECT_COMMISSION));

                    TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.Name);
                    TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                    #endregion Gen GameType Node

                    #region label error

                    TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                          Helper.GenLabel(strAPresetPTID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                           Helper.GenLabel(strMCommID + sep + BingoDef.CTR_LABEL_PREFIX, "", BingoDef.CLASS_ERROR_MAXVALUE));
                    TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                    #endregion label error

                    #region Betting Range

                    List<BettingRange> lsStakeLevel = new List<BettingRange>();
                    lsStakeLevel = gameTypeDetail.BettingRangeList;
                    string stakeLevels = "";
                    string funcChkStakeClick = "";
                    int totalStake = 0;
                    foreach (BettingRange stakeDetail in lsStakeLevel)
                    {
                        if (!stakeDetail.IsChecked && !_cust.SyncBingo)
                            continue;
                        string chkStakeID = gameKindDetail.ID + sep + gameTypeDetail.ID + sep +
                                         BingoDef.CHK_STAKE + sep + stakeDetail.ID;
                        stakeLevels += GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                          funcChkStakeClick) +
                                                          (totalStake == 0 ? Helper.GenLabel("", stakeDetail.Name, (_cust.SyncBingo) ? "chkDisabled" : "") : stakeDetail.Name);
                        postGameData += stakeDetail.ID + BingoDef.SEP_STAKE;
                        totalStake++;
                    }

                    //Assign Stake levels
                    TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                    gameTypeData.Append(tmpTplPrivate);
                    totalGameType++;

                    int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                    postGameData = postGameData.Substring(0, subLength) +
                                   BingoDef.CLOSE_STAKELIST_MARK + BingoDef.SEP_GAMETYPE; // ),

                    #endregion Betting Range

                    if (totalGameType == lsGameType.Count)
                    {
                        #region Group Setting

                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                            Helper.GenSelectOption(gameKindDetail.PositionTakingMax, gameKindDetail.MinPositionTaking, (_cust.SyncBingo) ? gameKindDetail.PositionTaking : gameKindDetail.PositionTakingMax, intervalPT, false, strAPresetPTGroupID, funcOnChangeGroup));

                        TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                            Helper.GenSelectOption(gameKindDetail.CommissionUpline, gameKindDetail.Commission, intervalComm, false, strMCommGroupID, funcOnChangeGroup, BingoDef.CLASS_SELECT_COMMISSION));

                        TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strAPresetPTGroupID, false));
                        TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");

                        TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", GenCheckBox(gameKindName + sep + BingoDef.CHKGROUP, false, useGroupPT, BingoDef.CLASS_CHECKBOX_HIDDEN, "")); //remove group setting check box
                        TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                        TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.Name));
                        TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.Name);

                        TemplateHandler.ReplaceBlock(ref tmpTplData, BingoDef.TAG_PRIVATE_SETTING, gameTypeData.ToString());
                        data.Append(tmpTplData);
                        totalGameType = 0;
                        gameTypeData = new StringBuilder();
                        postGameData = postGameData.Substring(0, postGameData.Length - 1) + BingoDef.CLOSE_GAMETYPELIST_MARK + BingoDef.SEP_GAMEKIND;

                        #endregion Group Setting
                    }
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");

            bool IsMultiple = (HttpContext.Current.Request["isMult"] == "1");
            TemplateHandler.Assign(ref tplPT, "isSync", (!_cust.SyncBingo && !IsMultiple).ToString().ToLower());
            TemplateHandler.Assign(ref tplPT, "exitConfirmMsg", (!_cust.SyncBingo && !IsMultiple) ? Resources.Bingo.exitBingoConfirmMsg : "");
            TemplateHandler.Assign(ref tplPT, "submit", Helper.GenLink(Lang.submit, BingoDef.BINGO_SUBMIT_HREF, BingoDef.CLASS_SUBMIT, BingoDef.BINGO_SUBMIT_FUNC));

            //assign Bet Setting
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoMaxTransfer)
                TemplateHandler.RemoveBlock(ref tplPT, "Bingo-bet-setting");
            else
                AssignBingoCustInfo(ref tplPT, _cust, false);

            //assign AGE customer info
            AssignIBCCustInfo(ref tplPT, _cust);

            //assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
            //HttpContext.Current.Session[BingoDef.SESSION_CUSTOMER] = _cust;
            return tplPT;
        }

        /* END MEMBER */

        private static void GenGameList(ref List<string> lsJS)
        {
            string arrGameKind = "";
            string arrGameKindID = "";
            string arrGameTypeValue = "";
            string arrGameTypeName = "arrGameType";
            int totalGameType = 0;
            List<GameKind> lsGameKind = listGameKind;
            if (lsGameKind.Count > 0)
            {
                foreach (GameKind gameKindDetail in lsGameKind)
                {
                    arrGameTypeValue = "";
                    arrGameTypeName = "arrGameType";
                    totalGameType = 0;

                    List<GameType> lsGameType = (htGameType[gameKindDetail.ID] != null) ? (List<GameType>)htGameType[gameKindDetail.ID] : null;
                    if (lsGameType != null && lsGameType.Count > 0)
                    {
                        foreach (GameType gameTypeDetail in lsGameType)
                        {
                            if (0 == totalGameType)
                                arrGameKindID += gameKindDetail.ID + ",";
                            arrGameTypeValue += gameTypeDetail.ID + ",";
                            totalGameType++;
                            if (totalGameType == lsGameType.Count)
                            {
                                arrGameTypeValue = arrGameTypeValue.Substring(0, arrGameTypeValue.Length - 1);
                                arrGameTypeName = arrGameTypeName + BingoDef.SEP_GAMEKIND_GAMETYPE + gameKindDetail.ID;
                                lsJS.Add(string.Format(BingoDef.JS_VariableDefinitionFormat, arrGameTypeName, string.Format(BingoDef.JS_arrayFormat, arrGameTypeValue)));
                                arrGameKind += arrGameTypeName + ",";
                            }
                        }
                    }
                }
            }
            arrGameKind = (!string.IsNullOrEmpty(arrGameKind)) ? string.Format(BingoDef.JS_VariableFormat, BingoDef.JS_arrGameKind, string.Format(BingoDef.JS_arrayFormat, arrGameKind.Substring(0, arrGameKind.Length - 1))) : "[]";
            arrGameKindID = (!string.IsNullOrEmpty(arrGameKindID)) ? string.Format(BingoDef.JS_VariableFormat, BingoDef.JS_arrGameKindID, string.Format(BingoDef.JS_arrayFormat, arrGameKindID.Substring(0, arrGameKindID.Length - 1))) : "[]";
            lsJS.Add(arrGameKindID);
            lsJS.Add(arrGameKind);
        }

        private static string GenHiddenDatas(string gameData, ICustomer cust)
        {
            string strHiddens = "";
            strHiddens += Helper.GenHidden(BingoDef.HIDDEN_GAMEDATA, gameData);
            strHiddens += Helper.GenHidden(BingoDef.HIDDEN_RoldID, cust.RoleId.ToString()); //master role id = 3
            strHiddens += Helper.GenHidden("nobackground", HttpContext.Current.Request.QueryString["nobackground"]); //master role id = 3
            strHiddens += Helper.GenHidden("isupdate", HttpContext.Current.Request.QueryString["isupdate"] == null ? "" : HttpContext.Current.Request.QueryString["isupdate"]);
            strHiddens += Helper.GenHidden(BingoDef.HIDDEN_LangID, BingoDef.CONST_LangID.ToString()); // Hard code
            strHiddens += Helper.GenHidden(BingoDef.HIDDEN_CountryID, BingoDef.CONST_CountryID.ToString()); // Hard code
            return strHiddens;
        }

        private static string GenHiddenDatas(Hashtable datas, ICustomer cust)
        {
            string strHiddens = "";
            strHiddens += Helper.GenHidden(BingoDef.HIDDEN_RoldID, cust.RoleId.ToString()); //master role id = 3
            strHiddens += Helper.GenHidden("nobackground", HttpContext.Current.Request.QueryString["nobackground"]);
            strHiddens += Helper.GenHidden("isupdate", HttpContext.Current.Request.QueryString["isupdate"] == null ? "" : HttpContext.Current.Request.QueryString["isupdate"]);
            strHiddens += Helper.GenHidden(BingoDef.HIDDEN_LangID, BingoDef.CONST_LangID.ToString()); // Hard code
            strHiddens += Helper.GenHidden(BingoDef.HIDDEN_CountryID, BingoDef.CONST_CountryID.ToString()); // Hard code
            IDictionaryEnumerator _enumerator = datas.GetEnumerator();
            while (_enumerator.MoveNext())
            {
                strHiddens += Helper.GenHidden(_enumerator.Key.ToString(), _enumerator.Value.ToString());
            }

            return strHiddens;
        }

        private static void AssignBingoCustInfo(ref string tplBingoPT, ICustomer cust, bool readOnly)
        {
            /* START : Current block is hidden. We'll use them later */
            TemplateHandler.RemoveBlock(ref tplBingoPT, "max-win-lose");
            /* if (BingoDef.IBCSetting.Bingo.ShowBetSetting)
                TemplateHandler.RemoveBlock(ref tplBingoPT, "max-win-lose");
            /* END */

            TemplateHandler.Assign(ref tplBingoPT, "lblAccumulated", Lang.lblAccumulatedWinLoss);
            TemplateHandler.Assign(ref tplBingoPT, "lblMaxTransferCredit", Lang.lblMaxTransferCredit);
            TemplateHandler.Assign(ref tplBingoPT, "lblMaxWin", Lang.lblMaxWinAllowed);
            TemplateHandler.Assign(ref tplBingoPT, "lblMaxLose", Lang.lblMaxLoseAllowed);
            TemplateHandler.Assign(ref tplBingoPT, "lblMin", Lang.lblMin);

            TemplateHandler.Assign(ref tplBingoPT, "Reset", (readOnly || !cust.SyncBingo) ? "" : Helper.GenLink(Lang.reset, "javascript:;", "", "ResetMaxTransfer('" + cust.ID + "')"));

            TemplateHandler.Assign(ref tplBingoPT, "accumulatedValue", FormatHandler.FormatNumber2(cust.ActualMaxTransfer, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
            TemplateHandler.Assign(ref tplBingoPT, "maxTransferCredit",
                (readOnly)
                ?
                Helper.GenLabel(BingoDef.TXT_MAXTRANSTERCREDIT, FormatHandler.FormatNumber2(cust.MaxTransferCredit, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey), "")
                :
                Helper.GenTextBox(BingoDef.TXT_MAXTRANSTERCREDIT,
                    BingoDef.TXT_MAXTRANSTERCREDIT,
                    !cust.SyncBingo ? cust.DefaultTransferCredit.ToString() : FormatHandler.FormatNumber2(cust.MaxTransferCredit, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey),
                    "",
                    false,
                    "",
                    "return numberOnly(event)"
                    )
            );

            TemplateHandler.Assign(ref tplBingoPT, "cbxAutoResetchecked", (cust.IsAutoReset || !cust.SyncBingo) ? "checked" : "");
            TemplateHandler.Assign(ref tplBingoPT, "autoReset", Lang.autoReset);
            TemplateHandler.Assign(ref tplBingoPT, "disabled", readOnly ? "disabled" : "");
            TemplateHandler.Assign(ref tplBingoPT, "transferCreditId", BingoDef.TXT_MAXTRANSTERCREDIT);

            //Simon: TemplateHandler.Assign(ref tplBingoPT, "maxWinValue", (readOnly) ? Helper.GenLabel(BingoDef.TXT_MAXWINID, CastAide.AsString(cust.MaxWin, ""), "") : Helper.GenTextBox(BingoDef.TXT_MAXWINID, CastAide.AsString(cust.MaxWin, ""), "", false, ""));
            //Simon: TemplateHandler.Assign(ref tplBingoPT, "maxLoseValue", (readOnly) ? Helper.GenLabel(BingoDef.TXT_MAXLOSEID, CastAide.AsString(cust.MaxLose, ""), "") : Helper.GenTextBox(BingoDef.TXT_MAXLOSEID, CastAide.AsString(cust.MaxLose, ""), "", false, ""));
            TemplateHandler.Assign(ref tplBingoPT, "minValue", CastAide.AsString(cust.MaxTransferCredit, ""));

            string strMaxLimited = FormatHandler.FormatNumber2(cust.MaxLimit, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
            TemplateHandler.Assign(ref tplBingoPT, "noLimitedValue", (strMaxLimited == "0") ? Lang.noLimited : strMaxLimited);
            TemplateHandler.Assign(ref tplBingoPT, "minLimitedValue", FormatHandler.FormatNumber2(cust.MinLimit, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey));
        }

        private static void AssignIBCCustInfo(ref string tplBingoPT, ICustomer cust)
        {
            bool IsMultiple = (HttpContext.Current.Request["isMult"] == "1");
            if (!IsMultiple)
            {
                TemplateHandler.Assign(ref tplBingoPT, "lblUsername", Lang.username);
                TemplateHandler.Assign(ref tplBingoPT, "lblFirstname", Lang.firstname);
                TemplateHandler.Assign(ref tplBingoPT, "lblPhone", Lang.lblPhone);
                TemplateHandler.Assign(ref tplBingoPT, "lblFax", Lang.lblFax);
                TemplateHandler.Assign(ref tplBingoPT, "lblUsername2", Lang.nickname);
                TemplateHandler.Assign(ref tplBingoPT, "lblLastname", Lang.lastname);
                TemplateHandler.Assign(ref tplBingoPT, "lblMobile", Lang.lblMobilePhone);
                TemplateHandler.Assign(ref tplBingoPT, "lblClosed", Lang.lblStatusClosed);
                TemplateHandler.Assign(ref tplBingoPT, "lblSuspend", Lang.suspend);
                TemplateHandler.Assign(ref tplBingoPT, "username", cust.UserName);
                TemplateHandler.Assign(ref tplBingoPT, "fax", cust.Fax);
                TemplateHandler.Assign(ref tplBingoPT, "firstname", cust.FirstName);
                TemplateHandler.Assign(ref tplBingoPT, "phone", cust.MobilePhone);
                TemplateHandler.Assign(ref tplBingoPT, "username2", cust.NickName);
                TemplateHandler.Assign(ref tplBingoPT, "lastname", cust.LastName);
                TemplateHandler.Assign(ref tplBingoPT, "mobile", cust.MobilePhone);
            }

            string strCbxLB1 = "";
            string strCbxLB2 = "";

            if (cust.SyncBingo && (cust.RoleId == 2 || cust.RoleId == 3))
            {
                strCbxLB1 = (cust.RoleId == 2) ? Resources.Lang.agent : Resources.Lang.master;
                strCbxLB2 = Resources.Bingo.Bingodownlines;
            }

            strCbxLB1 = "<label for='" + BingoDef.ID_CHECKBOX_DISABLEDBINGO + "'>" + strCbxLB1 + "</label>";
            strCbxLB2 = "<label for='" + BingoDef.ID_CHECKBOX_DISABLEDBINGO_DL + "'>" + strCbxLB2 + "</label>";

            //string strCbkBox = "&nbsp;" + GenCheckBox(BingoDef.ID_CHECKBOX_DISABLEDBINGO, (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - cust.RoleId != 1) || AppSession.IsBingoInDBDisabled, cust.IsEnabledBingo && (cust.SyncBingo || IsMultiple), "MarkChangeStatus()") + strCbxLB1;
            string strCbkBox = "&nbsp;" + GenCheckBox(BingoDef.ID_CHECKBOX_DISABLEDBINGO, false, true/*cust.IsEnabledBingo && (cust.SyncBingo || IsMultiple)*/, "MarkChangeStatus()") + strCbxLB1;

            if (cust.RoleId != 1 && cust.SyncBingo)
            {
                if (!cust.IsEnabledBingo)
                {
                    strCbkBox += "&nbsp;<span id=\"" + BingoDef.ID_SPAN_DOWNLINE_OPT + "\" style=\"visibility: hidden; font-weight: normal\">" + GenCheckBox(BingoDef.ID_CHECKBOX_DISABLEDBINGO_DL, false, false, "") + strCbxLB2 + Helper.GenHidden(BingoDef.HIDDEN_UPDDOWNLLINE, "") + "</span>";
                }
                else
                {
                    strCbkBox += "&nbsp;&nbsp;&nbsp;&nbsp;<span id=\"" + BingoDef.ID_SPAN_DOWNLINE_OPT + "\" style=\"visibility: hidden; font-weight: normal; color: red\">(" + Resources.Bingo.Bingodownlinesdisabled + ")</span>";
                }
            }

            TemplateHandler.Assign(ref tplBingoPT, "disableBingo", strCbkBox);
            TemplateHandler.Assign(ref tplBingoPT, "lblEnableBingo", Resources.Bingo.lblEnableBingo);
        }

        private static void AssignJavaScript(ref string tplPT, List<string> lsData)
        {
            string jsData = "<script type='text/javascript'>";
            if (lsData != null)
            {
                foreach (string sData in lsData)
                {
                    jsData += sData;
                }
            }
            jsData += "var ID_CHECKBOX_DISABLEDBingo = '" + BingoDef.ID_CHECKBOX_DISABLEDBINGO + "';\r\n";
            jsData += "var ID_CHECKBOX_DISABLEDBingo_DL = '" + BingoDef.ID_CHECKBOX_DISABLEDBINGO_DL + "';\r\n";
            jsData += "var ID_SPAN_DOWNLINE_OPT = '" + BingoDef.ID_SPAN_DOWNLINE_OPT + "';\r\n";
            jsData += "var strHidUpdDownline = '" + BingoDef.HIDDEN_UPDDOWNLLINE + "';\r\n";
            jsData += "</script>";
            TemplateHandler.Assign(ref tplPT, "jsdata", jsData);
        }

        private static void AssignCommonHeader(ref string tplBingoPT)
        {
            TemplateHandler.Assign(ref tplBingoPT, "lblCustomerInfomation", Lang.lblCustomerInfomation);
            TemplateHandler.Assign(ref tplBingoPT, "lblBetSetting", Lang.lblBetSetting);
            TemplateHandler.Assign(ref tplBingoPT, "lblPositionTaking", Lang.positiontaking);
        }

        private static int GetGameKindRowSpan(int totalGameType, bool useGroupSetting, string gameKindName)
        {
            int rowSpan = 0;
            string browser = HttpContext.Current.Request.Browser.Browser;
            if (browser == "IE")
            {
                rowSpan = (totalGameType * 2) + 1;
            }
            else
                rowSpan = useGroupSetting ? (totalGameType + 1) : ((totalGameType * 2) + 1);
            return rowSpan;
        }

        public static string GenCheckBox(string id, bool disabled, bool check, string funcClick)
        {
            return GenCheckBox(id, disabled, check, "", funcClick);
        }

        public static string GenCheckBox(string id, bool disabled, bool check, string classStyle, string funcClick)
        {
            string tplInput = TemplateHandler.LoadTemplate(Globals.TPL_CTR_INPUT);
            TemplateHandler.Assign(ref tplInput, "type", "checkbox");
            TemplateHandler.Assign(ref tplInput, "inputId", id);
            TemplateHandler.Assign(ref tplInput, "inputName", id);
            TemplateHandler.Assign(ref tplInput, "value", "1");
            TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");

            TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

            TemplateHandler.Assign(ref tplInput, "class", classStyle);
            TemplateHandler.Assign(ref tplInput, "checked", (check) ? "checked='checked'" : "");
            return tplInput;
        }
    }
}