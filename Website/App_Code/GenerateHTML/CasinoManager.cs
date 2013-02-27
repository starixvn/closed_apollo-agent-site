/// <summary>
/// Manage class for Casino Business
/// </summary>
namespace BLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using DAL;
    using DNAide.Core;
    using DNAide.Utils;
    using DNAide.Web;
    using Resources;
    using Sunplus.Agent.Model.Entities;

    public class CasinoManager
    {
        /// <summary>
        /// Max Super PT
        /// </summary>
        private static double s4 = 0.9, s3 = 0.9, s1 = 0.9, m4 = 0.9, m3 = 0.9, m2 = 0.9, m1 = 0.9, a4 = 0.9, a2 = 0.9, a1 = 0.9;

        private static double u1 = 0.9, u3 = 0.9, n4 = 0, n3 = 0, n2 = 0, l4 = 0.9, l3 = 0.9, l2 = 0.9;
        private static double s4Max = 0.9, s3Max = 0.9, s1Max = 0.9, m3Max = 0.9, m2Max = 0.9, m1Max = 0.9, a2Max = 0.9, a1Max = 0.9;
        private static double u1Max = 0.9, u3Max = 0.9, n4Max = 0.9, n3Max = 0.9, n2Max = 0.9, l4Max = 0.9, l3Max = 0.9, l2Max = 0.9;
        private static double m3MaxAuto = 0.9, n3MaxAuto = 0.9, maxAutoPT = 0.9;
        private static bool i3 = false, i1 = false;

        private static double s4Min = 0, s3Min = 0, s1Min = 0, m3Min = 0, m2Min = 0, m1Min = 0, a2Min = 0, a1Min = 0;
        private static double u3Min = 0, n4Min = 0, n3Min = 0, n2Min = 0, l4Min = 0, l3Min = 0, l2Min = 0;
        private static double m3MinAuto = 0, n3MinAuto = 0;
        private static double maxPT = 0.9, minPT = 0;
        private static List<string> lsJSData = null;
        private static List<GameKindDetail> listGameKind = null;
        private static Hashtable htGameType = null;

        public static CSCustomerDetail GetCustomerSettingCS(int ibcCustId)
        {
            return IBCProvider.Casino.GetCustSettingCS(ibcCustId);
        }

        public static void GetCustSettingCS(ref CSCustomerDetail customer)
        {
            IBCProvider.Casino.GetCustSettingCS(ref customer);
        }

        public static CSCustomerDetail GetCustInfoIBC(int ibcCustId)
        {
            CSCustomerDetail customer = IBCProvider.IBCCustomer.GetCustInfoIBC(ibcCustId);
            return customer;
        }

        public static List<GameKindDetail> GetGameKinds()
        {
            return IBCProvider.Casino.GetGameKinds();
        }

        /// <summary>
        /// Get Casino game kind list
        /// </summary>
        /// <param name="casinoNo"></param>
        /// gameGroup = "01", "02"
        /// <returns></returns>
        public static List<GameKindDetail> GetGameKinds(string casinoNo)
        {
            return IBCProvider.Casino.GetGameKinds(casinoNo);
        }

        public static List<GameKindDetail> GetGameKinds(int ibcCustId)
        {
            return IBCProvider.Casino.GetGameKinds(ibcCustId);
        }

        /// <summary>
        /// Edit Super, Master on member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static List<GameKindDetail> GetGameKinds_EditMember(int memberId)
        {
            return IBCProvider.Casino.GetGameKinds_EditMember(memberId);
        }

        /// <summary>
        /// Edit Super, Master on member
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="gameKindId"></param>
        /// <returns></returns>
        public static List<GameTypeDetail> GetGameTypes_EditMember(int memberId, int gameKindId)
        {
            return IBCProvider.Casino.GetGameTypes_EditMember(memberId, gameKindId);
        }

        public static List<GameKindDetail> GetGameKinds_MasterEditMember(int memberId)
        {
            return IBCProvider.Casino.GetGameKinds_MasterEditMember(memberId);
        }

        public static List<GameTypeDetail> GetGameTypes_MasterEditMember(int memberId, int gameKindId)
        {
            return IBCProvider.Casino.GetGameTypes_MasterEditMember(memberId, gameKindId);
        }

        public static List<StakeLevelDetail> GetCustStakeLevels(int ibcCustId, int gameTypeId)
        {
            return IBCProvider.Casino.GetCustStakeLevels(ibcCustId, gameTypeId);
        }

        /// <summary>
        /// Get stake list by game type
        /// </summary>
        /// <param name="gameTypeId"></param>
        /// <returns></returns>
        public static List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeId, int currencyId)
        {
            return IBCProvider.Casino.GetGameTypeStakeLevels(gameTypeId, currencyId);
        }

        public static List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeId, string currencyName)
        {
            return IBCProvider.Casino.GetGameTypeStakeLevels(gameTypeId, currencyName);
        }

        /// <summary>
        /// Get game type list of a certain game kind
        /// </summary>
        /// <param name="gameKindId">ID of game kind</param>
        /// <returns>List GameTypeDetail</returns>
        public static List<GameTypeDetail> GetGameType(int gameKindId)
        {
            return IBCProvider.Casino.GetGameType(gameKindId);
        }

        public static List<GameTypeDetail> GetGameType(int ibcCustId, int gameKindId)
        {
            return IBCProvider.Casino.GetGameType(ibcCustId, gameKindId);
        }

        public static string GenCasinoPTHTML(CSCustomerDetail customer, Globals.CasinoGroup group)
        {
            string groupNo = Globals.GetCasinoGroup(group);
            return GenCasinoPTHTML(customer, groupNo);
        }

        public static string GenCasinoPTHTML(CSCustomerDetail customer, string group)
        {
            string returnHtml = string.Empty;
            string ptTemplate = TemplateHandler.LoadTemplate(Globals.CASINO_PT_TEMPLATE);

            if (customer.IsMultipleEdit)
            {
                TemplateHandler.RemoveBlock(ref ptTemplate, "single-edit");
            }

            lsJSData = new List<string>();
            htGameType = new Hashtable();
            listGameKind = new List<GameKindDetail>();

            TemplateHandler.Assign(ref ptTemplate, "op", Resources.Lang.updateCS);

            if (UserSession.Current.RoleId == 4 && customer.RoleId == 2)
            {
                TemplateHandler.Assign(ref ptTemplate, "submit", Resources.Lang.close);
                TemplateHandler.RemoveBlock(ref ptTemplate, "editting");
                TemplateHandler.Assign(ref ptTemplate, "func", "parent.ageWnd.close();");
            }
            else
            {
                TemplateHandler.Assign(ref ptTemplate, "submit", Resources.Lang.submit);
                TemplateHandler.Assign(ref ptTemplate, "func", string.Empty);
            }
            TemplateHandler.Assign(ref ptTemplate, "cancel", Resources.Lang.cancel);

            if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["nobackground"]))
            {
                TemplateHandler.RemoveBlock(ref ptTemplate, "editting");
            }

            Globals.AccountType accountType = (Globals.AccountType)(customer.RoleId);
            AssignCommonHeader(ref ptTemplate);
            try
            {
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        returnHtml = GenCSHTMLSuper(ptTemplate, customer, group);
                        break;

                    case Globals.AccountType.Master:
                        returnHtml = GenCSHTMLMaster(ptTemplate, customer, group);
                        break;

                    case Globals.AccountType.Agent:
                        returnHtml = GenCSHTMLAgent(ptTemplate, customer, group);
                        break;

                    case Globals.AccountType.Member:
                        returnHtml = GenCSHTMLMember(ptTemplate, customer, group);
                        break;
                }
                GenGameList(ref lsJSData);
                AssignJavaScript(ref returnHtml, lsJSData);
                return returnHtml;
            }
            catch (Exception ex)
            {
                return new Fanex.Age.UI.Components.ErrorPage_Page("Not casino's Customer");
            }
        }

        private static string GenCSHTMLSuper(string tplPT, CSCustomerDetail customer, string group)
        {
            int custId = customer.IBCCustID;
            string gameKindName = "Live";

            // Others game kind
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);

            string tplData = TemplateHandler.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);

            string tplPrivate;
            double maxSMaxPT = Globals.MAX_MaxPTValue, maxSMinPT = Globals.MAX_MinPTValue, maxSComm = Globals.MAX_Comm;
            double maxGroupSMaxPT = Globals.MAX_MaxPTValue, maxGroupSMinPT = Globals.MAX_MinPTValue, maxGroupSComm = Globals.MAX_Comm;
            double SMaxPTGroup = Globals.CURRENT_MaxPTValue, SMinPTGroup = Globals.CURRENT_MinPTValue, SCommGroup = Globals.CURRENT_Comm; //current GROUP value
            double SMaxPT = Globals.CURRENT_MaxPTValue, SMinPT = Globals.CURRENT_MinPTValue, SComm = Globals.CURRENT_Comm; // current value
            string strSMaxPT = Globals.Super + Globals.strMaxPT, strSMinPT = Globals.Super + Globals.strMinPT, strSComm = Globals.Super + Globals.Comm;
            int intervalPT = Globals.INTERVAL_PT; // interval of select options
            int intervalComm = Globals.INTERVAL_COMM;
            string sep = Globals.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 3;
            // Build header
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.PRESET_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.AUTO_PT);
            TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT, Resources.Lang.lblMaxPTSuper);
            TemplateHandler.Assign(ref tplHeader, Globals.MIN_PT, Resources.Lang.MinPTSuper);
            TemplateHandler.Assign(ref tplHeader, Globals.COMM_PT, Resources.Lang.lblCommPTSuper);
            // Build content
            TemplateHandler.RemoveBlock(ref tplData, Globals.PRESET_PT);
            TemplateHandler.RemoveBlock(ref tplData, Globals.AUTO_PT);
            TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);

            // Get games kind detail from the casino system if this customer
            // Have already been synchronized
            List<GameKindDetail> lsGameKind = (customer.CasinoSync) ? GetGameKinds(customer.IBCCustID) : GetGameKinds(group);
            bool useGroupPT = (customer.NoCSData) ? false : !customer.PTByGameType;
            string postGameData = string.Empty;
            string ctrHidden = string.Empty;
            int totalGameKind = 0;
            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;
                gameKindName = gameKindDetail.CodeID.ToString();
                SMaxPTGroup = gameKindDetail.MaxPositionTaking;
                SMinPTGroup = (gameKindDetail.MinPositionTaking == 0)
                                  ? maxGroupSMinPT
                                  : gameKindDetail.MinPositionTaking;
                SCommGroup = gameKindDetail.SMAgentRollingRate;

                List<GameTypeDetail> lsGameType = (customer.CasinoSync) ? GetGameType(customer.IBCCustID, gameKindDetail.CodeID) : GetGameType(gameKindDetail.CodeID);
                StringBuilder gameTypeData = new StringBuilder();
                if (!IsP2PGame(gameKindDetail.CodeName))
                {
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);
                        string SMaxPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSMaxPT;
                        string strSMinPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSMinPT;
                        string strSCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSComm;
                        string funcOnChangeGroupSetting = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM + ", " + (int)Globals.AccountType.Super);
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        string funcClickGroup = string.Format(Globals.CS_ChkGROUP_CLICK_FUNC, "this");
                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                            if (totalGameKind == 0)
                                TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, funcClickGroup));
                            else
                                TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", string.Empty);
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                                                   Helper.GenSelectOption(maxGroupSMaxPT, SMaxPTGroup, intervalPT,
                                                                          !useGroupPT, SMaxPTGroupID, funcOnChangeGroupSetting));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                                                   Helper.GenSelectOption(maxGroupSMinPT, SMinPTGroup, intervalPT,
                                                                          !useGroupPT, strSMinPTGroupID, funcOnChangeGroupSetting));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   Helper.GenSelectOption(maxGroupSComm, SCommGroup, intervalComm, !useGroupPT,
                                                                          strSCommGroupID, funcOnChangeGroupSetting));
                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", Helper.GenChangePTButtons(SMaxPTGroupID, !useGroupPT));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", Helper.GenChangePTButtons(strSMinPTGroupID, !useGroupPT));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", string.Empty);

                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt_txt", Helper.GenTextBox(SMaxPTGroupID + sep + Globals.CTR_INPUT_TEXTBOX_PREFIX, SMaxPTGroupID, SMaxPTGroup.ToString(), Globals.CLASS_HIDDEN, useGroupPT, string.Empty));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt_txt", Helper.GenTextBox(strSMinPTGroupID + sep + Globals.CTR_INPUT_TEXTBOX_PREFIX, strSMinPTGroupID, SMinPTGroup.ToString(), Globals.CLASS_HIDDEN, useGroupPT, string.Empty));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_txt", Helper.GenTextBox(strSCommGroupID + sep + Globals.CTR_INPUT_TEXTBOX_PREFIX, strSCommGroupID, SCommGroup.ToString(), Globals.CLASS_HIDDEN, useGroupPT, string.Empty));
                        }
                        // game type value
                        string tmpTplPrivate = tplPrivate;
                        string strSMaxPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSMaxPT;
                        string strSMinPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSMinPT;
                        string strSCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSComm;
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.OPEN_STAKELIST_MARK;

                        SMaxPT = (gameTypeDetail.MaxPositionTaking == 0)
                                    ? maxSMaxPT
                                    : gameTypeDetail.MaxPositionTaking;
                        SMinPT = (gameTypeDetail.MinPositionTaking == 0)
                                     ? maxSMinPT
                                     : gameTypeDetail.MinPositionTaking;
                        SComm = (gameTypeDetail.SMAgentRollingRate == 0)
                                    ? maxSComm
                                    : gameTypeDetail.SMAgentRollingRate;

                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                            Helper.GenSelectOption(maxSMaxPT, SMaxPT, intervalPT, useGroupPT,
                                                                      strSMaxPTID, funcOnChange));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                                               Helper.GenSelectOption(maxSMinPT, SMinPT, intervalPT, useGroupPT,
                                                                      strSMinPTID, funcOnChange));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               Helper.GenSelectOption(maxSComm, SComm, intervalComm, useGroupPT,
                                                                      strSCommID, funcOnChange));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                        TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                        // label error

                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error", Helper.GenLabel(strSMaxPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                               Helper.GenLabel(strSMinPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               Helper.GenLabel(strSCommID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        // assign gamekind name
                        // get betting limit list

                        TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                        List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();
                        lsStakeLevel = (customer.CasinoSync) ? GetCustStakeLevels(customer.IBCCustID, gameTypeDetail.GameTypeID) : GetGameTypeStakeLevels(gameTypeDetail.GameTypeID, customer.Currency);
                        string stakeLevels = string.Empty;
                        string funcChkStakeClick = string.Empty;
                        foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                        {
                            string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                             Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                            stakeLevels += Helper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                              funcChkStakeClick) + stakeDetail.StakeLevelName;
                            postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                        }

                        // Assign Stake levels
                        TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;

                        int subLength = postGameData.Length - ((lsStakeLevel.Count == 0) ? 0 : 2);
                        postGameData = postGameData.Substring(0, subLength) +
                                       Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),
                        if (totalGameType == lsGameType.Count)
                        {
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", (totalGameType * 2) + 1);
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
                else // p2p game
                {
                    tplData = TemplateHandler.LoadTemplate(Globals.CASINO_P2P_PT_TEMPLATE);
                    tmpTplData = tplData;
                    TemplateHandler.Assign(ref tmpTplData, "col-span-pt", 2);
                    TemplateHandler.Assign(ref tmpTplData, Globals.COMM_PT, Resources.Lang.lblCommPTSuper);
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);
                        string strSCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSComm;
                        string funcOnChangeGroupSetting = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM + ", " + (int)Globals.AccountType.Super);
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        string funcClickGroup = string.Format(Globals.CS_ChkGROUP_CLICK_FUNC, "this");
                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, funcClickGroup));
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   Helper.GenSelectOption(maxGroupSComm, SCommGroup, intervalComm, !useGroupPT,
                                                                          strSCommGroupID, funcOnChangeGroupSetting, Globals.CLASS_SELECT_COMMISSION));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", string.Empty);
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_txt", Helper.GenTextBox(strSCommGroupID + sep + Globals.CTR_INPUT_TEXTBOX_PREFIX, strSCommGroupID, SCommGroup.ToString(), Globals.CLASS_HIDDEN, useGroupPT, string.Empty));
                        }

                        // game type value
                        string tmpTplPrivate = tplPrivate;
                        string strSMaxPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSMaxPT;
                        string strSMinPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSMinPT;
                        string strSCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSComm;
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.SEP_GAMETYPE;

                        SComm = gameTypeDetail.SMAgentRollingRate;

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               Helper.GenSelectOption(maxSComm, SComm, intervalComm, useGroupPT,
                                                                      strSCommID, funcOnChange, Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);

                        // label error

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               Helper.GenLabel(strSCommID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;
                        // assign gamekind name

                        if (totalGameType == lsGameType.Count)
                        {
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", totalGameType + 1);
                            TemplateHandler.Assign(ref tmpTplData, "row-span-pt", totalGameType);
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);
                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
                totalGameKind++;
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());

            if (customer.CasinoSync)
            {
                GetCustSettingCS(ref customer);
            }

            AssignCSCustInfo(ref tplPT, customer, false);

            // assign AGE customer info
            AssignIBCCustInfo(ref tplPT, customer);

            // assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, customer));
            return tplPT;
        }

        private static string GenCSHTMLMaster(string tplPT, CSCustomerDetail customer, string group)
        {
            int custId = customer.IBCCustID;
            string gameKindName = "Live";
            // Others game kind
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
            string tplDataP2P = string.Empty;
            string tplPrivate;
            double maxMPT = Globals.MAX_MaxPTValue, maxMMinPT = Globals.MAX_MinPTValue, maxMComm = Globals.MAX_Comm, maxSPresetPT = Globals.MAX_PresetPTValue, maxSAutoPT = Globals.MAX_AutoPTValue; //max value
            double maxGroupMPT = Globals.MAX_MaxPTValue, maxGroupMMinPT = Globals.MAX_MinPTValue, maxGroupMComm = Globals.MAX_Comm, maxGroupSPresetPT = Globals.MAX_PresetPTValue, maxGroupSAutoPT = Globals.MAX_AutoPTValue; //max group value

            double MPTGroup = Globals.CURRENT_MaxPTValue, MMinPTGroup = Globals.CURRENT_MinPTValue, MCommGroup = Globals.CURRENT_Comm, SPresetPTGroup = Globals.MAX_PresetPTValue, SAutoPTGroup = Globals.MAX_AutoPTValue; //current GROUP value
            double MPT = Globals.CURRENT_MaxPTValue, MMinPT = Globals.CURRENT_MinPTValue, MComm = Globals.CURRENT_Comm, SPresetPT = Globals.MAX_PresetPTValue, SAutoPT = Globals.MAX_AutoPTValue; // current value
            string strMPT = Globals.Master + Globals.strPresetPT, strMMinPT = Globals.Master + Globals.strMinPT, strMComm = Globals.Master + Globals.Comm, strSPresetPT = Globals.Super + Globals.strPresetPT, strSAutoPT = Globals.Super + Globals.strAutoPT;
            int intervalPT = Globals.INTERVAL_PT; // interval of select options
            int intervalComm = Globals.INTERVAL_COMM;
            string sep = Globals.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 6;
            // build header
            TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT, Resources.Lang.lblPresetPTSuper); // super preset
            TemplateHandler.Assign(ref tplHeader, Globals.AUTO_PT, Resources.Lang.lblAutoPTSuper); // super auto

            TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT, Resources.Lang.lblMaxPTMaster);
            TemplateHandler.Assign(ref tplHeader, Globals.MIN_PT, Resources.Lang.MinPTMaster);
            TemplateHandler.Assign(ref tplHeader, Globals.COMM_PT, Resources.Lang.lblCommPTMaster);

            // build content
            TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);
            List<GameKindDetail> lsGameKind = (customer.CasinoSync) ? GetGameKinds(customer.IBCCustID) : GetGameKinds(UserSession.Current.CustId);
            listGameKind = lsGameKind;
            bool useGroupPT = (customer.NoCSData) ? (!customer.PTByGameTypeUpline) : !customer.PTByGameType;
            string postGameData = string.Empty;
            string hidPTData = string.Empty;
            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;
                gameKindName = gameKindDetail.CodeID.ToString();
                string MPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMPT;
                string strMMinPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMMinPT;
                string strMCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMComm;
                string strSPresetPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSPresetPT;
                string strSAutoPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSAutoPT;
                string strChkSAutoPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + Globals.strCHK + strSAutoPT;
                string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                string funcClickChkAutoGroup = string.Format(Globals.CS_ChkAutoGROUP_CLICK_FUNC, "this," + useGroupPT.ToString().ToLower());
                List<GameTypeDetail> lsGameType = (customer.CasinoSync) ? GetGameType(customer.IBCCustID, gameKindDetail.CodeID) : GetGameType(UserSession.Current.CustId, gameKindDetail.CodeID);
                htGameType.Add(gameKindDetail.CodeID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupSPresetPT = 0;
                maxGroupSAutoPT = 0;
                maxGroupMPT = 0;
                maxGroupMMinPT = 0;
                maxGroupMComm = 0;
                if (!IsP2PGame(gameKindDetail.CodeName))
                {
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                        }

                        // game type value
                        string tmpTplPrivate = tplPrivate;
                        string strMPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMPT;
                        string strMMinPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMMinPT;
                        string strMCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMComm;
                        string strSPresetPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSPresetPT;
                        string strSAutoPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSAutoPT;
                        string strChkSAutoPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + Globals.strCHK + strSAutoPT;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        string funcOnChangePreset = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_PRESET_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PRESET_PARAM);
                        string funcClickChkAuto = string.Format(Globals.CS_ChkAUTO_CLICK_FUNC, "this");
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.OPEN_STAKELIST_MARK;

                        // super pt from company S4
                        s4 = (customer.CasinoSync) ? gameTypeDetail.SPositionTakingUpline : gameTypeDetail.SPositionTaking;

                        // group max S Preset PT
                        maxGroupSPresetPT = Math.Max(s4, maxGroupSPresetPT);

                        // super pt s3
                        s3 = gameTypeDetail.SPositionTaking;
                        // super auto pt
                        u3Max = s4;

                        // max group s auto
                        maxGroupSAutoPT = Math.Max(u3Max, maxGroupSAutoPT);

                        u3 = (customer.CasinoSync) ? gameTypeDetail.AutoPositionTaking : u3Max;
                        /////////END Super///////////////

                        // max master max pt
                        m4 = (gameTypeDetail.IsAutoPositionTaking) ? s4 : (s4 - s3);
                        m3Max = m4;
                        // group max M PT
                        maxGroupMPT = Math.Max(m3Max, maxGroupMPT);
                        // master max pt
                        m3 = (customer.CasinoSync) ? (Math.Min(gameTypeDetail.MPositionTaking, m3Max)) : m3Max; //current master preset pt
                        // master min pt
                        n3Max = m3;

                        maxGroupMMinPT = Math.Max(maxGroupMMinPT, n3Max);

                        // max master max/min pt when auto pt is used
                        m3MaxAuto = s4;
                        n3MaxAuto = s4;
                        /////////END Master//////////
                        s3Max = s4;
                        n3 = (customer.CasinoSync) ? (Math.Min(gameTypeDetail.MinPositionTaking, n3Max)) : Math.Min(s3Min, n3Max);

                        maxSPresetPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : s3Max;
                        maxSAutoPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : u3Max;
                        maxMPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : m3Max;
                        maxMMinPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : n3Max;

                        // Commission
                        maxMComm = (customer.CasinoSync) ? gameTypeDetail.SMAgentRollingRateUpline : gameTypeDetail.SMAgentRollingRate;
                        maxMComm = (!customer.PTByGameType) ? Globals.MAX_Comm : Math.Min(maxMComm, Globals.MAX_Comm);

                        // max group comm
                        maxGroupMComm = Math.Max(maxGroupMComm, maxMComm);

                        MComm = (customer.CasinoSync) ? gameTypeDetail.MAgentRollingRate : gameTypeDetail.SMAgentRollingRate;
                        MComm = Math.Min(MComm, Globals.MAX_Comm);
                        //////////END Commission///////
                        SPresetPT = s3;
                        SAutoPT = u3;
                        MPT = m3;
                        MMinPT = n3;

                        hidPTData += strMPTID + sep + Globals.m3MaxAuto + Globals.SEP_PT_VALUE + m3MaxAuto.ToString() + Globals.SEP_PT;

                        // super preset
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                           Helper.GenSelectOption(maxSPresetPT, SPresetPT, intervalPT, (useGroupPT || gameTypeDetail.IsAutoPositionTaking),
                                                                     strSPresetPTID, funcOnChangePreset));

                        TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt",
                           Helper.GenSelectOption(maxSAutoPT, SAutoPT, intervalPT, (useGroupPT || !gameTypeDetail.IsAutoPositionTaking),
                                                                     strSAutoPTID, funcOnChange));

                        TemplateHandler.Assign(ref tmpTplPrivate, "chkAuto", Helper.GenCheckBox(strChkSAutoPTID, useGroupPT, gameTypeDetail.IsAutoPositionTaking, funcClickChkAuto));

                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                            Helper.GenSelectOption(maxMPT, MPT, intervalPT, useGroupPT,
                                                                      strMPTID, funcOnChange));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                                               Helper.GenSelectOption(maxMMinPT, MMinPT, intervalPT, useGroupPT,
                                                                      strMMinPTID, funcOnChangePreset));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               Helper.GenSelectOption(maxMComm, MComm, intervalComm, useGroupPT,
                                                                      strMCommID, funcOnChange, Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                        TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                        // label error
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                              Helper.GenLabel(strSPresetPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt_error",
                                               Helper.GenLabel(strSAutoPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error", Helper.GenLabel(strMPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                               Helper.GenLabel(strMMinPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               Helper.GenLabel(strMCommID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                        List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();
                        lsStakeLevel = (customer.CasinoSync) ? GetCustStakeLevels(customer.IBCCustID, gameTypeDetail.GameTypeID) : GetCustStakeLevels(UserSession.Current.CustId, gameTypeDetail.GameTypeID);
                        string stakeLevels = string.Empty;
                        string funcChkStakeClick = string.Empty;
                        int totalStake = 0;
                        foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                        {
                            if (!stakeDetail.IsChecked && !customer.CasinoSync)
                                continue;
                            string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                             Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                            stakeLevels += Helper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                              funcChkStakeClick) +
                                                              (totalStake == 0 ? Helper.GenLabel(string.Empty, stakeDetail.StakeLevelName, (customer.CasinoSync) ? "chkDisabled" : string.Empty) : stakeDetail.StakeLevelName);
                            postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                            totalStake++;
                        }

                        // Assign Stake levels
                        TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;

                        int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                        postGameData = postGameData.Substring(0, subLength) +
                                       Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                        if (totalGameType == lsGameType.Count)
                        {
                            // Group
                            if (customer.CasinoSync)
                            {
                                s4 = (!customer.PTByGameType) ? gameKindDetail.SPositionTakingUpline : maxGroupSPresetPT;
                                maxGroupMComm = (!customer.PTByGameType) ? gameKindDetail.SMAgentRollingRateUpline : maxGroupMComm;
                            }
                            else
                            {
                                s4 = gameKindDetail.SPositionTaking;
                                maxGroupMComm = gameKindDetail.SMAgentRollingRate;
                            }

                            m3MaxAuto = s4;
                            hidPTData += MPTGroupID + sep + Globals.m3MaxAuto + Globals.SEP_PT_VALUE + m3MaxAuto.ToString() + Globals.SEP_PT;
                            // super pt s3
                            s3 = gameKindDetail.SPositionTaking;
                            // super auto pt
                            u3Max = s4;
                            u3 = (customer.CasinoSync) ? gameKindDetail.AutoPositionTaking : u3Max;
                            /////////END Super///////////////

                            // max master max pt
                            m4 = (gameKindDetail.IsAutoPositionTaking) ? s4 : (s4 - s3);
                            m3Max = m4;
                            m3Max = Math.Max(maxGroupMPT, m3Max);

                            // master max pt
                            m3 = (customer.CasinoSync) ? (Math.Min(gameKindDetail.MPositionTaking, m3Max)) : m3Max; //current master preset pt
                            // master min pt
                            n3Max = m3;

                            n3 = (customer.CasinoSync) ? gameKindDetail.MinPositionTaking : 0;

                            // max master max/min pt when auto pt is used
                            m3MaxAuto = s4;
                            n3MaxAuto = s4;
                            /////////END Master//////////
                            s3Max = s4 - s3Min;
                            maxGroupSPresetPT = s3Max;
                            maxGroupSAutoPT = u3Max;
                            maxGroupMPT = m3Max;
                            maxGroupMMinPT = n3Max;

                            // Commission
                            MCommGroup = (customer.CasinoSync) ? Math.Min(gameKindDetail.MAgentRollingRate, maxGroupMComm) : Math.Min(gameKindDetail.SMAgentRollingRate, maxGroupMComm);
                            //////////END Commission///////
                            SPresetPTGroup = s3;
                            SAutoPTGroup = u3;
                            MPTGroup = m3;
                            MMinPTGroup = n3;
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, string.Empty)); //remove group setting check box
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                                                   Helper.GenSelectOption(maxGroupMPT, MPTGroup, intervalPT,
                                                                          false, MPTGroupID, funcOnChangeGroup));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                                                   Helper.GenSelectOption(maxGroupMMinPT, MMinPTGroup, intervalPT,
                                                                          false, strMMinPTGroupID, funcOnChangeGroup));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   Helper.GenSelectOption(maxGroupMComm, MCommGroup, intervalComm, false,
                                                                          strMCommGroupID, funcOnChangeGroup, Globals.CLASS_SELECT_COMMISSION));

                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                                                   Helper.GenSelectOption(maxGroupSPresetPT, SPresetPTGroup, intervalPT, gameKindDetail.IsAutoPositionTaking,
                                                                          strSPresetPTGroupID, funcOnChangeGroup));

                            TemplateHandler.Assign(ref tmpTplData, "group_auto_pt",
                                                   Helper.GenSelectOption(maxGroupSAutoPT, SAutoPTGroup, intervalPT, !gameKindDetail.IsAutoPositionTaking,
                                                                          strSAutoPTGroupID, funcOnChangeGroup));

                            TemplateHandler.Assign(ref tmpTplData, "chkGroupAuto", Helper.GenCheckBox(strChkSAutoPTGroupID, false, gameKindDetail.IsAutoPositionTaking, funcClickChkAutoGroup));

                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", Helper.GenChangePTButtons(MPTGroupID, false));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", Helper.GenChangePTButtons(strMMinPTGroupID, false));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", string.Empty);
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strSPresetPTGroupID, gameKindDetail.IsAutoPositionTaking));
                            TemplateHandler.Assign(ref tmpTplData, "group_auto_pt_btn", Helper.GenChangePTButtons(strSAutoPTGroupID, !gameKindDetail.IsAutoPositionTaking));

                            // END
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }

                #region P2P game

                else
                {
                    tplDataP2P = TemplateHandler.LoadTemplate(Globals.CASINO_P2P_PT_TEMPLATE);
                    TemplateHandler.Assign(ref tplDataP2P, Globals.COMM_PT, Resources.Lang.lblCommPTMaster);
                    tmpTplData = tplDataP2P;
                    TemplateHandler.Assign(ref tmpTplData, "col-span-pt", 5);
                    TemplateHandler.Assign(ref tmpTplData, Globals.COMM_PT, Resources.Lang.lblCommPTSuper);
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                        strMCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMComm;
                        string funcOnChangeGroupSetting = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                        string funcClickGroup = string.Format(Globals.CS_ChkGROUP_CLICK_FUNC, "this");
                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                        }

                        // game type value
                        string tmpTplPrivate = tplPrivate;
                        string strMCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMComm;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.SEP_GAMETYPE;

                        maxMComm = (customer.CasinoSync) ? gameTypeDetail.SMAgentRollingRateUpline : gameTypeDetail.SMAgentRollingRate;
                        MComm = (customer.CasinoSync) ? gameTypeDetail.MAgentRollingRate : gameKindDetail.SMAgentRollingRate;

                        maxMComm = Math.Min(maxMComm, Globals.MAX_Comm);
                        maxGroupMComm = Math.Max(maxGroupMComm, maxMComm);
                        MComm = Math.Min(MComm, Globals.MAX_Comm);

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               Helper.GenSelectOption(maxMComm, MComm, intervalComm, useGroupPT,
                                                                      strMCommID, funcOnChange, Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);

                        // label error

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               Helper.GenLabel(strMCommID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;
                        // assign gamekind name
                        if (totalGameType == lsGameType.Count)
                        {
                            // Group
                            if (customer.CasinoSync)
                            {
                                maxGroupMComm = (!customer.PTByGameType) ? gameKindDetail.SMAgentRollingRateUpline : maxGroupMComm;
                                MCommGroup = gameKindDetail.MAgentRollingRate;
                            }
                            else
                            {
                                maxGroupMComm = (!customer.PTByGameTypeUpline) ? gameKindDetail.SMAgentRollingRate : maxGroupMComm;
                                MCommGroup = gameKindDetail.SMAgentRollingRate;
                            }

                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, string.Empty));
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   Helper.GenSelectOption(maxGroupMComm, MCommGroup, intervalComm, !useGroupPT,
                                                                          strMCommGroupID, funcOnChangeGroupSetting, Globals.CLASS_SELECT_COMMISSION));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", string.Empty);
                            // END
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "row-span-pt", totalGameType);
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);
                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }

                #endregion P2P game
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : string.Empty);
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? string.Empty : "none");
            // assign Casino customer info
            AssignCSCustInfo(ref tplPT, customer, false);
            // assign AGE customer info
            AssignIBCCustInfo(ref tplPT, customer);
            // assign post data
            Hashtable hHiddenData = new Hashtable();
            hHiddenData.Add(Globals.HIDDEN_GAMEDATA, postGameData);
            hHiddenData.Add(Globals.HIDDEN_PT, hidPTData);
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(hHiddenData, customer));
            return tplPT.Replace(Environment.NewLine, string.Empty).Replace("  ", " ");
        }

        private static string GenCSHTMLAgent(string tplPT, CSCustomerDetail _cust, string csGroup)
        {
            Globals.AccountType accType = (Globals.AccountType)UserSession.Current.RoleId;
            switch (accType)
            {
                case Globals.AccountType.Super:
                    return GenCSHTMLSuperOnAgent(tplPT, _cust, csGroup);
                case Globals.AccountType.Master:
                    return GenCSHTMLMasterOnAgent(tplPT, _cust, csGroup);
            }
            return string.Empty;
        }

        private static string GenCSHTMLMasterOnAgent(string tplPT, CSCustomerDetail customer, string csGroup)
        {
            int custId = customer.IBCCustID;
            string gameKindName = "Live";
            // Others game kind
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
            string tplDataP2P = string.Empty;
            string tplPrivate;
            double maxAPT = Globals.MAX_MaxPTValue, maxAMinPT = Globals.MAX_MinPTValue, maxAComm = Globals.MAX_Comm, maxMPresetPT = Globals.MAX_PresetPTValue; //max value
            double maxGroupAPT = Globals.MAX_MaxPTValue, maxGroupAMinPT = Globals.MAX_MinPTValue, maxGroupAComm = Globals.MAX_Comm, maxGroupMPresetPT = Globals.MAX_PresetPTValue; //max group value

            double APTGroup = Globals.CURRENT_MaxPTValue, AMinPTGroup = Globals.CURRENT_MinPTValue, ACommGroup = Globals.CURRENT_Comm, MPresetPTGroup = Globals.MAX_PresetPTValue; //current GROUP value
            double APT = Globals.CURRENT_MaxPTValue, AMinPT = Globals.CURRENT_MinPTValue, AComm = Globals.CURRENT_Comm, MPresetPT = Globals.MAX_PresetPTValue; // current value
            string strAPT = Globals.Agent + Globals.strPresetPT, strAMinPT = Globals.Agent + Globals.strMinPT, strAComm = Globals.Agent + Globals.Comm, strMPresetPT = Globals.Master + Globals.strPresetPT;
            int intervalPT = Globals.INTERVAL_PT; // interval of select options
            int intervalComm = Globals.INTERVAL_COMM;
            string sep = Globals.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 4;
            // build header
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.AUTO_PT); // there are no auto master on agent function

            TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT, Resources.Lang.lblPresetPTMaster); // master preset
            TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT, Resources.Lang.lblMaxPTAgent);

            TemplateHandler.Assign(ref tplHeader, Globals.MIN_PT, Resources.Lang.MinPTAgent);

            TemplateHandler.Assign(ref tplHeader, Globals.COMM_PT, Resources.Lang.lblCommPTAgent);

            // build content
            TemplateHandler.RemoveBlock(ref tplData, Globals.AUTO_PT);
            TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);
            List<GameKindDetail> lsGameKind = (customer.CasinoSync) ? GetGameKinds(customer.IBCCustID) : GetGameKinds(UserSession.Current.CustId);
            listGameKind = lsGameKind;
            bool useGroupPT = (customer.NoCSData) ? !customer.PTByGameTypeUpline : !customer.PTByGameType;
            string postGameData = string.Empty;
            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;
                gameKindName = gameKindDetail.CodeID.ToString();
                string APTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAPT;
                string strAMinPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAMinPT;
                string strACommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAComm;
                string strMPresetPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMPresetPT;
                string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);

                List<GameTypeDetail> lsGameType = (customer.CasinoSync) ? GetGameType(customer.IBCCustID, gameKindDetail.CodeID) : GetGameType(UserSession.Current.CustId, gameKindDetail.CodeID);
                htGameType.Add(gameKindDetail.CodeID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupAPT = 0;
                maxGroupMPresetPT = 0;
                maxGroupAComm = 0;

                if (!IsP2PGame(gameKindDetail.CodeName))
                {
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                        }

                        // game type value
                        string tmpTplPrivate = tplPrivate;
                        string strAPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAPT;
                        string strAMinPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAMinPT;
                        string strACommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAComm;
                        string strMPresetPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMPresetPT;
                        string funcOnChangePreset = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_PRESET_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PRESET_PARAM);
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.OPEN_STAKELIST_MARK;

                        if (customer.CasinoSync)
                        {
                            m3 = gameTypeDetail.MPositionTakingUpline;
                            m2Max = m3;
                            m2 = Math.Min(gameTypeDetail.MPositionTaking, m2Max);
                            a2Max = m3 - m2;
                            a2 = Math.Min(a2Max, gameTypeDetail.APositionTaking);
                            n2Max = a2;
                            n2 = Math.Min(gameTypeDetail.MinPositionTaking, n2Max);

                            // commission
                            maxAComm = gameTypeDetail.MAgentRollingRateUpline;
                            AComm = gameTypeDetail.AgentRollingRate;
                        }
                        else // copy from master
                        {
                            m3 = gameTypeDetail.MPositionTaking;
                            m2Max = m3;
                            m2 = m2Max;
                            a2Max = m3 - m2;
                            a2 = a2Max;
                            n2Max = a2;
                            n2 = Math.Min(gameTypeDetail.MinPositionTaking, n2Max);
                            // commission
                            maxAComm = gameTypeDetail.MAgentRollingRate;
                            AComm = gameTypeDetail.MAgentRollingRate;
                        }
                        // maxPT = Math.Min(m3, maxPT); //max group value when use pt by game type
                        maxAComm = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : Math.Min(maxAComm, Globals.MAX_Comm);
                        AComm = Math.Min(AComm, Globals.MAX_Comm);
                        maxMPresetPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : m2Max;
                        maxAPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : a2Max;
                        maxAMinPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : n2Max;

                        maxGroupAPT = Math.Max(maxGroupAPT, maxAPT);
                        maxGroupMPresetPT = Math.Max(maxGroupMPresetPT, maxMPresetPT);
                        maxGroupAComm = Math.Max(maxGroupAComm, maxAComm);

                        APT = a2;
                        AMinPT = n2;
                        MPresetPT = m2;
                        // master preset
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                           Helper.GenSelectOption(maxMPresetPT, MPresetPT, intervalPT, useGroupPT,
                                                                     strMPresetPTID, funcOnChangePreset));

                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                            Helper.GenSelectOption(maxAPT, APT, intervalPT, useGroupPT,
                                                                      strAPTID, funcOnChangePreset));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                                               Helper.GenSelectOption(maxAMinPT, AMinPT, intervalPT, useGroupPT,
                                                                      strAMinPTID, string.Empty));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               Helper.GenSelectOption(maxAComm, AComm, intervalComm, useGroupPT,
                                                                      strACommID, string.Empty, Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                        TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                        // label error
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                              Helper.GenLabel(strMPresetPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error", Helper.GenLabel(strAPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                               Helper.GenLabel(strAMinPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               Helper.GenLabel(strACommID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                        List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();
                        lsStakeLevel = (customer.CasinoSync) ? GetCustStakeLevels(customer.IBCCustID, gameTypeDetail.GameTypeID) : GetCustStakeLevels(UserSession.Current.CustId, gameTypeDetail.GameTypeID);
                        string stakeLevels = string.Empty;
                        string funcChkStakeClick = string.Empty;
                        int totalStake = 0;
                        foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                        {
                            if (!stakeDetail.IsChecked && !customer.CasinoSync)
                                continue;
                            string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                             Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                            stakeLevels += Helper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                              funcChkStakeClick) +
                                                              (totalStake == 0 ? Helper.GenLabel(string.Empty, stakeDetail.StakeLevelName, (customer.CasinoSync) ? "chkDisabled" : string.Empty) : stakeDetail.StakeLevelName);
                            postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                            totalStake++;
                        }

                        // Assign Stake levels
                        TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;

                        int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                        postGameData = postGameData.Substring(0, subLength) +
                                       Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                        if (totalGameType == lsGameType.Count)
                        {
                            // Group
                            if (customer.CasinoSync)
                            {
                                m3 = (!customer.PTByGameType) ? gameKindDetail.MPositionTakingUpline : maxGroupMPresetPT;
                                m2Max = m3;
                                m2 = Math.Min(gameKindDetail.MPositionTaking, m2Max);
                                a2Max = m3 - m2;
                                a2 = Math.Min(a2Max, gameKindDetail.APositionTaking);
                                n2Max = a2;
                                n2 = Math.Min(gameKindDetail.MinPositionTaking, n2Max);

                                // commission
                                maxGroupAComm = (!customer.PTByGameType) ? gameKindDetail.MAgentRollingRateUpline : maxGroupAComm;
                                ACommGroup = gameKindDetail.AgentRollingRate;
                            }
                            else // copy from master
                            {
                                m3 = gameKindDetail.MPositionTaking;
                                m2Max = m3;
                                m2 = m2Max;
                                a2Max = m3 - m2;
                                a2 = a2Max;
                                n3 = gameKindDetail.MinPositionTaking;
                                n2Max = a2;
                                n2 = Math.Min(gameKindDetail.MinPositionTaking, n2Max);
                                // commission
                                maxGroupAComm = (!customer.PTByGameTypeUpline) ? gameKindDetail.MAgentRollingRate : maxGroupAComm;
                                ACommGroup = gameKindDetail.MAgentRollingRate;
                            }

                            maxGroupAComm = Math.Min(maxGroupAComm, Globals.MAX_Comm);
                            ACommGroup = (!customer.PTByGameType) ? Math.Min(ACommGroup, Globals.MAX_Comm) : Math.Min(ACommGroup, maxGroupAComm);
                            maxGroupMPresetPT = (!customer.PTByGameType) ? m2Max : Math.Max(m2Max, maxGroupMPresetPT);
                            maxGroupAPT = (!customer.PTByGameType) ? a2Max : Math.Max(a2Max, maxGroupAPT);
                            maxGroupAMinPT = n2Max;

                            APTGroup = a2;
                            AMinPTGroup = n2;
                            MPresetPTGroup = m2;
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, string.Empty)); //remove group setting check box
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                                                   Helper.GenSelectOption(maxGroupAPT, APTGroup, intervalPT,
                                                                          false, APTGroupID, funcOnChangeGroup));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                                                   Helper.GenSelectOption(maxGroupAMinPT, AMinPTGroup, intervalPT,
                                                                          false, strAMinPTGroupID, funcOnChangeGroup));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   Helper.GenSelectOption(maxGroupAComm, ACommGroup, intervalComm, false,
                                                                          strACommGroupID, funcOnChangeGroup, Globals.CLASS_SELECT_COMMISSION));

                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                                                   Helper.GenSelectOption(maxGroupMPresetPT, MPresetPTGroup, intervalPT, false,
                                                                          strMPresetPTGroupID, funcOnChangeGroup));

                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", Helper.GenChangePTButtons(APTGroupID, false));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", Helper.GenChangePTButtons(strAMinPTGroupID, false));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", string.Empty);
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strMPresetPTGroupID, false));
                            // END
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }

                #region P2P game

                else
                {
                    tplDataP2P = TemplateHandler.LoadTemplate(Globals.CASINO_P2P_PT_TEMPLATE);
                    TemplateHandler.Assign(ref tplDataP2P, Globals.COMM_PT, Resources.Lang.lblCommPTAgent);
                    tmpTplData = tplDataP2P;
                    TemplateHandler.Assign(ref tmpTplData, "col-span-pt", 3);
                    TemplateHandler.Assign(ref tmpTplData, Globals.COMM_PT, Resources.Lang.lblCommPTSuper);
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);
                        strACommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAComm;
                        string funcOnChangeGroupSetting = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                        string funcClickGroup = string.Format(Globals.CS_ChkGROUP_CLICK_FUNC, "this");
                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, string.Empty));
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   Helper.GenSelectOption(maxGroupAComm, ACommGroup, intervalComm, !useGroupPT,
                                                                          strACommGroupID, funcOnChangeGroupSetting, Globals.CLASS_SELECT_COMMISSION));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", string.Empty);
                        }

                        // game type value
                        string tmpTplPrivate = tplPrivate;
                        string strACommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAComm;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.SEP_GAMETYPE;

                        if (customer.CasinoSync)
                        {
                            // commission
                            maxAComm = gameTypeDetail.MAgentRollingRate;
                            AComm = gameTypeDetail.AgentRollingRate;
                        }
                        else // copy from master
                        {
                            // commission
                            maxAComm = gameKindDetail.MAgentRollingRate;
                            AComm = gameKindDetail.MAgentRollingRate;
                        }

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               Helper.GenSelectOption(maxAComm, AComm, intervalComm, useGroupPT,
                                                                      strACommID, funcOnChange, Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);

                        // label error

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               Helper.GenLabel(strACommID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;
                        // assign gamekind name
                        if (totalGameType == lsGameType.Count)
                        {
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "row-span-pt", totalGameType);
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);
                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }

                #endregion P2P game
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : string.Empty);
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? string.Empty : "none");
            AssignCSCustInfo(ref tplPT, customer, false);
            // assign AGE customer info
            AssignIBCCustInfo(ref tplPT, customer);
            // assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, customer));
            return tplPT;
        }

        private static string GenCSHTMLSuperOnAgent(string tplPT, CSCustomerDetail customer, string group)
        {
            if (!customer.CasinoSync) return Lang.noInfo;
            int custId = customer.IBCCustID;
            string gameKindName = "Live";
            // Others game kind
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
            string tplDataP2P = string.Empty;
            string tplPrivate;
            double maxAMaxPT = Globals.MAX_MaxPTValue, maxAMinPT = Globals.MAX_MinPTValue, maxAComm = Globals.MAX_Comm, maxMPresetPT = Globals.MAX_PresetPTValue; //max value
            double maxGroupAMaxPT = Globals.MAX_MaxPTValue, maxGroupAMinPT = Globals.MAX_MinPTValue, maxGroupAComm = Globals.MAX_Comm, maxGroupMPresetPT = Globals.MAX_PresetPTValue; //max group value

            double AMaxPTGroup = Globals.CURRENT_MaxPTValue, AMinPTGroup = Globals.CURRENT_MinPTValue, ACommGroup = Globals.CURRENT_Comm, MPresetPTGroup = Globals.MAX_PresetPTValue; //current GROUP value
            double AMaxPT = Globals.CURRENT_MaxPTValue, AMinPT = Globals.CURRENT_MinPTValue, AComm = Globals.CURRENT_Comm, MPresetPT = Globals.MAX_PresetPTValue; // current value
            string strAMaxPT = Globals.Agent + Globals.strMaxPT, strAMinPT = Globals.Agent + Globals.strMinPT, strAComm = Globals.Agent + Globals.Comm, strMPresetPT = Globals.Master + Globals.strPresetPT;
            int intervalPT = Globals.INTERVAL_PT; //interval of select options
            int intervalComm = Globals.INTERVAL_COMM;
            string sep = Globals.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 2;
            //build header
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.AUTO_PT); //there are no auto master on agent function
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.MAX_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.COMM_PT);
            TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT, Resources.Lang.lblPositionTakingMaster); //master preset
            TemplateHandler.Assign(ref tplHeader, Globals.MIN_PT, Resources.Lang.lblPositionTakingAgent);

            //build content
            TemplateHandler.RemoveBlock(ref tplData, Globals.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplData, Globals.MAX_PT);
            TemplateHandler.RemoveBlock(ref tplData, Globals.COMM_PT);
            TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);
            List<GameKindDetail> lsGameKind = (customer.CasinoSync) ? GetGameKinds(customer.IBCCustID) : GetGameKinds(group);
            listGameKind = lsGameKind;
            bool useGroupPT = (customer.NoCSData) ? !customer.PTByGameTypeUpline : !customer.PTByGameType;
            string postGameData = string.Empty;
            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;
                gameKindName = gameKindDetail.CodeID.ToString();

                AMinPTGroup = gameKindDetail.APositionTaking;
                MPresetPTGroup = gameKindDetail.MPositionTaking;

                List<GameTypeDetail> lsGameType = (customer.CasinoSync) ? GetGameType(customer.IBCCustID, gameKindDetail.CodeID) : GetGameType(gameKindDetail.CodeID);
                htGameType.Add(gameKindDetail.CodeID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();

                if (!IsP2PGame(gameKindDetail.CodeName))
                {
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);
                        string AMaxPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAMaxPT;
                        string strAMinPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAMinPT;
                        string strACommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAComm;
                        string strMPresetPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMPresetPT;
                        string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, string.Empty)); //remove group setting check box
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt", Helper.GenLabel(strAMinPTGroupID, CastAide.AsString(AMinPTGroup), string.Empty));
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt", Helper.GenLabel(strMPresetPTGroupID, CastAide.AsString(MPresetPTGroup), string.Empty));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", string.Empty);
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", string.Empty);
                        }

                        // game type value
                        string tmpTplPrivate = tplPrivate;
                        string strAMaxPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAMaxPT;
                        string strAMinPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAMinPT;
                        string strACommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAComm;
                        string strMPresetPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMPresetPT;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.OPEN_STAKELIST_MARK;

                        AMinPT = gameTypeDetail.APositionTaking;
                        MPresetPT = gameTypeDetail.MPositionTaking;

                        // master preset
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt", Helper.GenLabel(strMPresetPTID, CastAide.AsString(MPresetPT), string.Empty));

                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt", Helper.GenLabel(strAMinPTID, CastAide.AsString(AMinPT), string.Empty));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                        TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                        // label error
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                              Helper.GenLabel(strMPresetPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                               Helper.GenLabel(strAMinPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                        List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();
                        lsStakeLevel = (customer.CasinoSync) ? GetCustStakeLevels(customer.IBCCustID, gameTypeDetail.GameTypeID) : GetCustStakeLevels(UserSession.Current.CustId, customer.Currency);
                        string stakeLevels = string.Empty;
                        string funcChkStakeClick = string.Empty;
                        int totalStake = 0;
                        foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                        {
                            if (!stakeDetail.IsChecked && !customer.CasinoSync)
                                continue;
                            string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                             Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                            stakeLevels += Helper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                              funcChkStakeClick) +
                                                              (totalStake == 0 ? Helper.GenLabel(string.Empty, stakeDetail.StakeLevelName, (customer.CasinoSync) ? "chkDisabled" : string.Empty) : stakeDetail.StakeLevelName);
                            postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                            totalStake++;
                        }

                        // Assign Stake levels
                        TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;

                        int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                        postGameData = postGameData.Substring(0, subLength) +
                                       Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                        if (totalGameType == lsGameType.Count)
                        {
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
                else
                {
                    tplDataP2P = TemplateHandler.LoadTemplate(Globals.CASINO_P2P_PT_TEMPLATE);
                    TemplateHandler.Assign(ref tplDataP2P, Globals.COMM_PT, Resources.Lang.lblCommPTAgent);
                    tmpTplData = tplDataP2P;
                    TemplateHandler.Assign(ref tmpTplData, "col-span-pt", 3);
                    TemplateHandler.Assign(ref tmpTplData, Globals.COMM_PT, Resources.Lang.lblCommPTSuper);
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);
                        string strACommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAComm;
                        string funcOnChangeGroupSetting = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                        string funcClickGroup = string.Format(Globals.CS_ChkGROUP_CLICK_FUNC, "this");
                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, string.Empty));
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   Helper.GenSelectOption(maxGroupAComm, ACommGroup, intervalComm, !useGroupPT,
                                                                          strACommGroupID, funcOnChangeGroupSetting, Globals.CLASS_SELECT_COMMISSION));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", string.Empty);
                        }

                        // game type value
                        string tmpTplPrivate = tplPrivate;
                        string strACommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAComm;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.SEP_GAMETYPE;

                        AComm = (gameTypeDetail.AgentRollingRate == 0)
                                    ? maxAComm
                                    : gameTypeDetail.AgentRollingRate;
                        AComm = Math.Min(AComm, Globals.MAX_Comm);
                        maxAComm = Math.Min(maxAComm, Globals.MAX_Comm);
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               Helper.GenSelectOption(maxAComm, AComm, intervalComm, useGroupPT,
                                                                      strACommID, funcOnChange, Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);

                        // label error

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               Helper.GenLabel(strACommID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;
                        // assign gamekind name
                        if (totalGameType == lsGameType.Count)
                        {
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "row-span-pt", totalGameType);
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);
                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : string.Empty);
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? string.Empty : "none");
            AssignCSCustInfo(ref tplPT, customer, true);
            // assign AGE customer info
            AssignIBCCustInfo(ref tplPT, customer);
            // assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, customer));
            return tplPT;
        }

        ////////END AGENT //////////////

        private static string GenCSHTMLMember(string tplPT, CSCustomerDetail customer, string group)
        {
            Globals.AccountType accType = (Globals.AccountType)UserSession.Current.RoleId;
            switch (accType)
            {
                case Globals.AccountType.Super:
                    return GenCSHTMLSuperOnMember(tplPT, customer, group);
                case Globals.AccountType.Master:
                    return GenCSHTMLMasterOnMember(tplPT, customer, group);
                case Globals.AccountType.Agent:
                    return GenCSHTMLAgentOnMember(tplPT, customer, group);
            }
            return string.Empty;
        }

        /// <summary>
        /// Edit super PT on Member
        /// </summary>
        /// <param name="tplPT"></param>
        /// <param name="customer"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        private static string GenCSHTMLSuperOnMember(string tplPT, CSCustomerDetail customer, string csGroup)
        {
            if (!Globals.IBCSetting.Casino.ShowBetSetting || customer.IsMultipleEdit)
            {
                TemplateHandler.RemoveBlock(ref tplPT, Globals.TAG_BET_SETTING);
            }
            if (customer.IsMultipleEdit)
            {
                TemplateHandler.RemoveBlock(ref tplPT, "age-info");
            }
            if (!customer.CasinoSync)
            {
                return new Fanex.Age.UI.Components.ErrorPage_Page(Resources.Lang.noInfo); // this member have not been synchronized to CS yet.
            }
            int custId = customer.IBCCustID;
            string gameKindName = "Live";
            // Others game kind
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
            string tplPrivate;
            // Master
            double maxAPT = Globals.MAX_MaxPTValue; // max value
            double APT = Globals.CURRENT_MaxPTValue;
            double maxMaPT = Globals.MAX_MaxPTValue, maxSPresetPT = Globals.MAX_PresetPTValue, maxSAutoPT = Globals.MAX_AutoPTValue; //max value
            double maxGroupMaPT = Globals.MAX_MaxPTValue, maxGropuAPT = Globals.MAX_MaxPTValue, maxGroupSPresetPT = Globals.MAX_PresetPTValue, maxGroupSAutoPT = Globals.MAX_AutoPTValue; //max group value
            double MaPTGroup = Globals.CURRENT_MaxPTValue, APTGroup = Globals.CURRENT_MaxPTValue, SPresetPTGroup = Globals.MAX_PresetPTValue, SAutoPTGroup = Globals.MAX_AutoPTValue; //current GROUP value
            double MaPT = Globals.CURRENT_MaxPTValue, MMinPT = Globals.CURRENT_MinPTValue, MComm = Globals.CURRENT_Comm, SPresetPT = Globals.MAX_PresetPTValue, SAutoPT = Globals.MAX_AutoPTValue; // current value
            string strMaPT = Globals.Master + Globals.strPT, strAPT = Globals.Agent + Globals.strPT, strSPresetPT = Globals.Super + Globals.strPresetPT, strSAutoPT = Globals.Super + Globals.strAutoPT;
            int intervalPT = Globals.INTERVAL_PT; // interval of select options
            int intervalComm = Globals.INTERVAL_COMM;
            string sep = Globals.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 5;
            // Build header
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.COMM_PT);
            TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT, Resources.Lang.lblPositionTakingSuper); // super preset
            TemplateHandler.Assign(ref tplHeader, Globals.AUTO_PT, Resources.Lang.lblAutoPTSuper); // super auto

            TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT, Resources.Lang.lblPositionTakingMaster);
            TemplateHandler.Assign(ref tplHeader, Globals.MIN_PT, Resources.Lang.lblPositionTakingAgent);

            // Build content
            TemplateHandler.RemoveBlock(ref tplData, Globals.COMM_PT);
            TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);

            List<GameKindDetail> lsGameKind = GetGameKinds_EditMember(customer.IBCCustID);
            listGameKind = lsGameKind;
            bool useGroupPT = (customer.NoCSData) ? !customer.PTByGameTypeUpline : !customer.PTByGameType;
            string postGameData = string.Empty;
            maxPT = 0.9;
            minPT = 0;
            maxAutoPT = 0.9;
            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;

                gameKindName = gameKindDetail.CodeID.ToString();
                string MaPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMaPT;
                string APTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAPT;
                string strSPresetPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSPresetPT;
                string strSAutoPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSAutoPT;
                string strChkSAutoPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + Globals.strCHK + strSAutoPT;
                string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                string funcClickChkAutoGroup = string.Format(Globals.CS_ChkAutoGROUP_CLICK_FUNC, "this," + useGroupPT.ToString().ToLower());

                List<GameTypeDetail> lsGameType = GetGameTypes_EditMember(customer.IBCCustID, gameKindDetail.CodeID);
                htGameType.Add(gameKindDetail.CodeID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupSPresetPT = 0;
                maxGroupSAutoPT = 0;
                if (!IsP2PGame(gameKindDetail.CodeName))
                {
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                        }

                        // Game type value
                        string tmpTplPrivate = tplPrivate;
                        string strMaPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMaPT;
                        string strAPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAPT;
                        string strSPresetPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSPresetPT;
                        string strSAutoPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSAutoPT;
                        string strChkSAutoPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + Globals.strCHK + strSAutoPT;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        string funcClickChkAuto = string.Format(Globals.CS_ChkAUTO_CLICK_FUNC, "this");
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.OPEN_STAKELIST_MARK;

                        s4 = gameTypeDetail.SuperMaxPositionTaking;
                        m1 = gameTypeDetail.MPositionTaking;
                        a1 = gameTypeDetail.APositionTaking;
                        s1Max = s4 - (m1 + a1);
                        s1Min = gameTypeDetail.SuperMinPositionTaking;
                        s1Min = ((m1 + a1) >= s1Min) ? minPT : (s1Min - (m1 + a1));
                        u1Max = s4;
                        u1 = gameTypeDetail.AutoPositionTaking;
                        s1 = gameTypeDetail.SPositionTaking;

                        maxSPresetPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : s1Max;
                        maxSAutoPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : u1Max;

                        // Group value
                        maxGroupSPresetPT = Math.Max(s1Max, maxGroupSPresetPT);
                        maxGroupSAutoPT = Math.Max(u1Max, maxGroupSAutoPT);

                        MaPT = m1;
                        APT = a1;

                        SPresetPT = Math.Min(s1, s1Max);
                        SAutoPT = Math.Min(u1, u1Max);
                        // Super preset
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                           Helper.GenSelectOption(maxSPresetPT, s1Min, SPresetPT, intervalPT, (useGroupPT || gameTypeDetail.IsAutoPositionTaking),
                                                                     strSPresetPTID, funcOnChange));

                        TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt",
                           Helper.GenSelectOption(maxSAutoPT, SAutoPT, intervalPT, (useGroupPT || !gameTypeDetail.IsAutoPositionTaking),
                                                                     strSAutoPTID, funcOnChange));

                        TemplateHandler.Assign(ref tmpTplPrivate, "chkAuto", Helper.GenCheckBox(strChkSAutoPTID, useGroupPT, gameTypeDetail.IsAutoPositionTaking, funcClickChkAuto));

                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                            Helper.GenLabel(strMaPTID, MaPT.ToString(), string.Empty));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                                               Helper.GenLabel(strAPTID, APT.ToString(), string.Empty));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                        TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                        // Label error
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                              Helper.GenLabel(strSPresetPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt_error",
                                               Helper.GenLabel(strSAutoPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error", Helper.GenLabel(strMaPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                               Helper.GenLabel(strAPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                        List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();
                        lsStakeLevel = (customer.CasinoSync) ? GetCustStakeLevels(customer.IBCCustID, gameTypeDetail.GameTypeID) : GetCustStakeLevels(customer.AIBCCustID, customer.Currency);
                        string stakeLevels = string.Empty;
                        string funcChkStakeClick = string.Empty;
                        int totalStake = 0;
                        foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                        {
                            if (!stakeDetail.IsChecked && !customer.CasinoSync)
                                continue;
                            string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                             Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                            stakeLevels += Helper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                              funcChkStakeClick) +
                                                              (totalStake == 0 ? Helper.GenLabel(string.Empty, stakeDetail.StakeLevelName, (customer.CasinoSync) ? "chkDisabled" : string.Empty) : stakeDetail.StakeLevelName);
                            postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                            totalStake++;
                        }

                        // Assign Stake levels
                        TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;

                        int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                        postGameData = postGameData.Substring(0, subLength) +
                                       Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                        if (totalGameType == lsGameType.Count)
                        {
                            // Group
                            s4 = gameKindDetail.SuperMaxPositionTaking;
                            m1 = gameKindDetail.MPositionTaking;
                            a1 = gameKindDetail.APositionTaking;
                            s1Max = (!customer.PTByGameType) ? (s4 - (m1 + a1)) : maxGroupSPresetPT;
                            s1Min = gameKindDetail.SuperMinPositionTaking;
                            s1Min = ((m1 + a1) >= s1Min) ? minPT : (s1Min - (m1 + a1));
                            u1Max = (!customer.PTByGameType) ? s4 : maxGroupSAutoPT;
                            u1 = gameKindDetail.AutoPositionTaking;
                            s1 = gameKindDetail.SPositionTaking;

                            maxGroupSPresetPT = s1Max;
                            maxGroupSAutoPT = u1Max;

                            MaPTGroup = m1;
                            APTGroup = a1;

                            SPresetPTGroup = Math.Min(s1, s1Max);
                            SAutoPTGroup = Math.Min(u1, u1Max);

                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, string.Empty)); //remove group setting check box
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            // Master pt
                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                                                   Helper.GenLabel(MaPTGroupID, MaPTGroup.ToString(), string.Empty));
                            // Agent pt
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                                                   Helper.GenLabel(APTGroupID, APTGroup.ToString(), string.Empty));

                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                                                   Helper.GenSelectOption(maxGroupSPresetPT, s1Min, SPresetPTGroup, intervalPT, gameKindDetail.IsAutoPositionTaking,
                                                                          strSPresetPTGroupID, funcOnChangeGroup));

                            TemplateHandler.Assign(ref tmpTplData, "group_auto_pt",
                                                   Helper.GenSelectOption(maxGroupSAutoPT, SAutoPTGroup, intervalPT, !gameKindDetail.IsAutoPositionTaking,
                                                                          strSAutoPTGroupID, funcOnChangeGroup));

                            TemplateHandler.Assign(ref tmpTplData, "chkGroupAuto", Helper.GenCheckBox(strChkSAutoPTGroupID, false, gameKindDetail.IsAutoPositionTaking, funcClickChkAutoGroup));

                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", string.Empty);
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", string.Empty);
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strSPresetPTGroupID, gameKindDetail.IsAutoPositionTaking));
                            TemplateHandler.Assign(ref tmpTplData, "group_auto_pt_btn", Helper.GenChangePTButtons(strSAutoPTGroupID, !gameKindDetail.IsAutoPositionTaking));
                            // END
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : string.Empty);
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? string.Empty : "none");
            // Assign Casino customer info
            AssignCSCustInfo(ref tplPT, customer, true);
            // Assign AGE customer info
            AssignIBCCustInfo(ref tplPT, customer);
            // Assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, customer));
            return tplPT;
        }

        /// <summary>
        /// Master PT on member
        /// </summary>
        /// <param name="tplPT"></param>
        /// <param name="customer"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GenCSHTMLMasterOnMember(string tplPT, CSCustomerDetail customer, string group)
        {
            if (!Globals.IBCSetting.Casino.ShowBetSetting || customer.IsMultipleEdit)
            {
                TemplateHandler.RemoveBlock(ref tplPT, Globals.TAG_BET_SETTING);
            }
            if (customer.IsMultipleEdit)
            {
                TemplateHandler.RemoveBlock(ref tplPT, "age-info");
            }

            if (!customer.CasinoSync)
            {
                return new Fanex.Age.UI.Components.ErrorPage_Page(Resources.Lang.noInfo);
            }
            int custId = customer.IBCCustID;
            string gameKindName = "Live";
            // Others game kind
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
            string tplPrivate;
            double maxMPT = Globals.MAX_PresetPTValue, maxMPresetPT = Globals.MAX_PresetPTValue; // max value
            double maxGroupMPT = Globals.MAX_PresetPTValue, maxGroupAPT = Globals.MAX_PresetPTValue; // max group value

            double MPTGroup = Globals.CURRENT_PresetPTValue, APTGroup = Globals.MAX_PresetPTValue; // current GROUP value
            double MPT = Globals.CURRENT_MaxPTValue, APT = Globals.MAX_PresetPTValue; // current value
            string strMPT = Globals.Master + Globals.strPresetPT, strAPT = Globals.Agent + Globals.strPT;
            int intervalPT = Globals.INTERVAL_PT; // interval of select options
            int intervalComm = Globals.INTERVAL_COMM;
            string sep = Globals.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 2;
            // Build header
            TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT, Resources.Lang.lblPositionTakingMaster); //master pt

            TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT, Resources.Lang.lblPositionTakingAgent); //agent pt

            // Remove min, max, auto pt
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.COMM_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.MIN_PT);

            // Build content
            TemplateHandler.RemoveBlock(ref tplData, Globals.COMM_PT);
            TemplateHandler.RemoveBlock(ref tplData, Globals.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplData, Globals.MIN_PT);
            TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);

            List<GameKindDetail> lsGameKind = GetGameKinds_MasterEditMember(customer.IBCCustID);
            listGameKind = lsGameKind;
            bool useGroupPT = (customer.NoCSData) ? !customer.PTByGameTypeUpline : !customer.PTByGameType;
            string postGameData = string.Empty;

            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;
                gameKindName = gameKindDetail.CodeID.ToString();
                string strMPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMPT;
                string strAPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAPT;
                string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                List<GameTypeDetail> lsGameType = GetGameTypes_MasterEditMember(customer.IBCCustID, gameKindDetail.CodeID);
                htGameType.Add(gameKindDetail.CodeID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupMPT = 0;

                if (!IsP2PGame(gameKindDetail.CodeName))
                {
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                        }

                        // Game type value
                        string tmpTplPrivate = tplPrivate;
                        string strMPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMPT;
                        string strAPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAPT;
                        string funcOnChange = string.Empty;
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.OPEN_STAKELIST_MARK;

                        m3 = gameTypeDetail.MasterMaxPositionTaking;
                        m1 = gameTypeDetail.MPositionTaking;
                        a1 = gameTypeDetail.APositionTaking;
                        s1 = gameTypeDetail.SPositionTaking;
                        s4 = gameTypeDetail.SuperSPositionTaking;// 0.9
                        i1 = gameTypeDetail.IsAutoPositionTaking;
                        i3 = gameTypeDetail.IsAutoPositionTakingUpline;

                        m1Min = gameTypeDetail.MasterMinPositionTaking;

                        m1Min = (a1 >= m1Min) ? minPT : (m1Min - a1);

                        m1Max = m3 - a1;// 0.2
                        m1Max = m1Max < 0 ? 0 : m1Max;
                        s4Max = s4 - (s1 + a1);// 0.4

                        m1Max = (i1 == true || i3 == true) ? m1Max : m1Max > s4Max ? s4Max : m1Max;

                        maxMPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : m1Max;
                        // Max group mpt
                        maxGroupMPT = Math.Max(maxGroupMPT, m1Max);
                        MPT = m1;
                        APT = a1;

                        // Super preset
                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt", Helper.GenLabel(strAPTID, APT.ToString(), string.Empty));

                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt", Helper.GenSelectOption(maxMPT, m1Min, MPT, intervalPT, useGroupPT, strMPTID, funcOnChange));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                        TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                        // Label error
                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error", Helper.GenLabel(strAPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                               Helper.GenLabel(strMPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                        List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();

                        lsStakeLevel = (customer.CasinoSync) ? GetCustStakeLevels(customer.IBCCustID, gameTypeDetail.GameTypeID) : GetCustStakeLevels(UserSession.Current.CustId, customer.Currency);
                        string stakeLevels = string.Empty;
                        string funcChkStakeClick = string.Empty;
                        int totalStake = 0;
                        foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                        {
                            if (!stakeDetail.IsChecked && !customer.CasinoSync)
                                continue;
                            string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                             Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                            stakeLevels += Helper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                              funcChkStakeClick) +
                                                              (totalStake == 0 ? Helper.GenLabel(string.Empty, stakeDetail.StakeLevelName, (customer.CasinoSync) ? "chkDisabled" : string.Empty) : stakeDetail.StakeLevelName);
                            postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                            totalStake++;
                        }

                        // Assign Stake levels
                        TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;

                        int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                        postGameData = postGameData.Substring(0, subLength) +
                                       Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                        if (totalGameType == lsGameType.Count)
                        {
                            // Group value
                            m3 = gameKindDetail.MasterMaxPositionTaking;
                            m1 = gameKindDetail.MPositionTaking;
                            a1 = gameKindDetail.APositionTaking;
                            s1 = gameKindDetail.SPositionTaking;
                            s4 = gameKindDetail.SuperSPositionTaking;
                            m1Min = gameKindDetail.MasterMinPositionTaking;
                            i1 = gameKindDetail.IsAutoPositionTaking;
                            i3 = gameKindDetail.IsAutoPositionTakingUpline;

                            m1Min = (a1 >= m1Min) ? minPT : (m1Min - a1);
                            m1Max = (!customer.PTByGameType) ? (m3 - a1) : maxGroupMPT;
                            m1Max = m1Max < 0 ? 0 : m1Max;

                            s4Max = s4 - (s1 + a1);
                            m1Max = (i1 == true || i3 == true) ? m1Max : m1Max > s4Max ? s4Max : m1Max;

                            maxGroupMPT = m1Max;
                            MPTGroup = m1;
                            APTGroup = a1;
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, string.Empty)); //remove group setting check box
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            // Member commission
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt", Helper.GenSelectOption(maxGroupMPT, m1Min, MPTGroup, intervalPT, false, strMPTGroupID, funcOnChangeGroup));
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strMPTGroupID, false));
                            // Agent pt
                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt", Helper.GenLabel(strAPTGroupID, APTGroup.ToString(), string.Empty));
                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", string.Empty);
                            // END
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : string.Empty);
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? string.Empty : "none");
            AssignCSCustInfo(ref tplPT, customer, true);
            // Assign AGE customer info
            AssignIBCCustInfo(ref tplPT, customer);
            // Assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, customer));
            return tplPT;
        }

        /// <summary>
        /// Edit Agent PT on Member
        /// </summary>
        /// <param name="tplPT"></param>
        /// <param name="customer"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        private static string GenCSHTMLAgentOnMember(string tplPT, CSCustomerDetail customer, string group)
        {
            if (!Globals.IBCSetting.Casino.ShowBetSetting) TemplateHandler.RemoveBlock(ref tplPT, Globals.TAG_BET_SETTING);
            int custId = customer.IBCCustID;
            string gameKindName = "Live";
            // Others game kind
            StringBuilder data = new StringBuilder();
            string tplHeader = TemplateHandler.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
            string tplData = TemplateHandler.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
            string tplDataP2P = TemplateHandler.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
            string tplPrivate;
            double maxMComm = Globals.MAX_Comm, maxAPresetPT = Globals.MAX_PresetPTValue; // max value
            double maxGroupMComm = Globals.MAX_Comm, maxGroupAPresetPT = Globals.MAX_PresetPTValue; // max group value

            double MCommGroup = Globals.CURRENT_Comm, APresetPTGroup = Globals.MAX_PresetPTValue; // current GROUP value
            double MComm = Globals.CURRENT_Comm, APresetPT = Globals.MAX_PresetPTValue; // current value
            string strMComm = Globals.Member + Globals.Comm, strAPresetPT = Globals.Agent + Globals.strPresetPT;
            int intervalPT = Globals.INTERVAL_PT; // interval of select options
            int intervalComm = Globals.INTERVAL_COMM;
            string sep = Globals.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 2;
            // Build header
            TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT, Resources.Lang.lblPresetPTAgent); //agent pt

            TemplateHandler.Assign(ref tplHeader, Globals.COMM_PT, Resources.Lang.lblMemberCommission);

            // Remove min, max, auto pt
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.MAX_PT);
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.MIN_PT);

            // Build content
            TemplateHandler.RemoveBlock(ref tplData, Globals.AUTO_PT);
            TemplateHandler.RemoveBlock(ref tplData, Globals.MAX_PT);
            TemplateHandler.RemoveBlock(ref tplData, Globals.MIN_PT);
            TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);
            List<GameKindDetail> lsGameKind = (customer.CasinoSync) ? GetGameKinds(customer.IBCCustID) : GetGameKinds(UserSession.Current.CustId);
            listGameKind = lsGameKind;
            bool useGroupPT = (customer.NoCSData) ? !customer.PTByGameTypeUpline : !customer.PTByGameType;
            string postGameData = string.Empty;
            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;
                gameKindName = gameKindDetail.CodeID.ToString();
                string strMCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMComm;
                string strAPresetPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAPresetPT;
                string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);

                List<GameTypeDetail> lsGameType = (customer.CasinoSync) ? GetGameType(customer.IBCCustID, gameKindDetail.CodeID) : GetGameType(UserSession.Current.CustId, gameKindDetail.CodeID);
                htGameType.Add(gameKindDetail.CodeID, lsGameType);
                StringBuilder gameTypeData = new StringBuilder();
                maxGroupAPresetPT = 0;
                maxGroupMComm = 0;
                if (!IsP2PGame(gameKindDetail.CodeName))
                {
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                        }

                        // Game type value
                        string tmpTplPrivate = tplPrivate;
                        string strMCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMComm;
                        string strAPresetPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAPresetPT;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC, Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        postGameData += gameTypeDetail.GameTypeID.ToString() + Globals.OPEN_STAKELIST_MARK;

                        if (customer.CasinoSync)
                        {
                            a2 = gameTypeDetail.APositionTakingUpline;
                            a1Max = a2;
                            a1 = (gameTypeDetail.APositionTaking > a1Max) ? a1Max : gameTypeDetail.APositionTaking;
                            a1Min = gameTypeDetail.MinPositionTakingUpline;
                            // Commission
                            maxMComm = gameTypeDetail.AgentRollingRateUpline;
                            MComm = gameTypeDetail.MemberRollingRate;
                        }
                        else // Copy from master
                        {
                            a2 = gameTypeDetail.APositionTaking;
                            a1Max = a2;
                            a1 = gameTypeDetail.APositionTaking;
                            a1Min = gameTypeDetail.MinPositionTaking;
                            // Commission
                            maxMComm = gameTypeDetail.AgentRollingRate;
                            MComm = gameTypeDetail.AgentRollingRate;
                        }

                        maxMComm = (!customer.PTByGameType) ? Globals.MAX_Comm : Math.Min(maxMComm, Globals.MAX_Comm);

                        MComm = Math.Min(MComm, Globals.MAX_Comm);

                        maxAPresetPT = (!customer.PTByGameType) ? Globals.MAX_MaxPTValue : a1Max;
                        // Group value
                        maxGroupMComm = Math.Max(maxMComm, maxGroupMComm);
                        maxGroupAPresetPT = Math.Max(maxGroupAPresetPT, maxAPresetPT);
                        APresetPT = a1;
                        // Super preset
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                           Helper.GenSelectOption(maxAPresetPT, a1Min, APresetPT, intervalPT, useGroupPT,
                                                                     strAPresetPTID, funcOnChange));

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               Helper.GenSelectOption(maxMComm, MComm, intervalComm, useGroupPT,
                                                                      strMCommID, funcOnChange, Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                        TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                        // Label error
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                              Helper.GenLabel(strAPresetPTID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               Helper.GenLabel(strMCommID + sep + Globals.CTR_LABEL_PREFIX, string.Empty, Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", Lang.lblBettingLimit);

                        List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();
                        lsStakeLevel = (customer.CasinoSync) ? GetCustStakeLevels(customer.IBCCustID, gameTypeDetail.GameTypeID) : GetCustStakeLevels(UserSession.Current.CustId, gameTypeDetail.GameTypeID);
                        string stakeLevels = string.Empty;
                        string funcChkStakeClick = string.Empty;
                        int totalStake = 0;
                        foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                        {
                            if (!stakeDetail.IsChecked && !customer.CasinoSync)
                                continue;
                            string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                             Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                            stakeLevels += Helper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                              funcChkStakeClick) +
                                                              (totalStake == 0 ? Helper.GenLabel(string.Empty, stakeDetail.StakeLevelName, (customer.CasinoSync) ? "chkDisabled" : string.Empty) : stakeDetail.StakeLevelName);
                            postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                            totalStake++;
                        }

                        // Assign Stake levels
                        TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;

                        int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                        postGameData = postGameData.Substring(0, subLength) +
                                       Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                        if (totalGameType == lsGameType.Count)
                        {
                            // Group
                            if (customer.CasinoSync)
                            {
                                a2 = (!customer.PTByGameType) ? Math.Min(gameKindDetail.APositionTakingUpline, gameKindDetail.MaxPositionTakingUpline) : maxGroupAPresetPT;
                                maxGroupMComm = (!customer.PTByGameType) ? gameKindDetail.AgentRollingRateUpline : maxGroupMComm;
                                MCommGroup = gameKindDetail.MemberRollingRate;
                                a1Min = (!customer.PTByGameType) ? gameKindDetail.MinPositionTakingUpline : minPT;
                            }
                            else
                            {
                                a2 = (!customer.PTByGameTypeUpline) ? gameKindDetail.APositionTaking : maxGroupAPresetPT;
                                maxGroupMComm = (!customer.PTByGameTypeUpline) ? gameKindDetail.AgentRollingRate : maxGroupMComm;
                                MCommGroup = gameKindDetail.AgentRollingRate;
                                a1Min = (!customer.PTByGameTypeUpline) ? gameKindDetail.MinPositionTaking : minPT;
                            }
                            maxGroupMComm = Math.Min(maxGroupMComm, Globals.MAX_Comm);
                            MCommGroup = Math.Min(MCommGroup, Globals.MAX_Comm);
                            maxGroupAPresetPT = a2;
                            a1 = gameKindDetail.APositionTaking;
                            a1Min = Math.Min(a1, a1Min);
                            APresetPTGroup = a1;
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", Helper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false, useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, string.Empty)); //remove group setting check box
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", Lang.lblGroupSetting);
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   Helper.GenSelectOption(maxGroupMComm, MCommGroup, intervalComm, false,
                                                                          strMCommGroupID, funcOnChangeGroup, Globals.CLASS_SELECT_COMMISSION));

                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                                                   Helper.GenSelectOption(maxGroupAPresetPT, a1Min, APresetPTGroup, intervalPT, false,
                                                                          strAPresetPTGroupID, funcOnChangeGroup));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", string.Empty);
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", Helper.GenChangePTButtons(strAPresetPTGroupID, false));
                            // END
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind", GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) + Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : string.Empty);
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? string.Empty : "none");
            AssignCSCustInfo(ref tplPT, customer, false);
            // Assign AGE customer info
            AssignIBCCustInfo(ref tplPT, customer);
            // Assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, customer));
            return tplPT;
        }

        //End Member
        private static void GenGameList(ref List<string> lsJS)
        {
            string gameKinds = string.Empty;
            string gameKindIds = string.Empty;
            string gameTypeValues = string.Empty;
            string gameTypeNames = "arrGameType";
            int totalGameType = 0;
            List<GameKindDetail> lsGameKind = listGameKind;
            if (lsGameKind.Count > 0)
            {
                foreach (GameKindDetail gameKindDetail in lsGameKind)
                {
                    gameTypeValues = string.Empty;
                    gameTypeNames = "arrGameType";
                    totalGameType = 0;

                    List<GameTypeDetail> lsGameType = (htGameType[gameKindDetail.CodeID] != null) ? (List<GameTypeDetail>)htGameType[gameKindDetail.CodeID] : null;
                    if (lsGameType != null && lsGameType.Count > 0)
                    {
                        foreach (GameTypeDetail gameTypeDetail in lsGameType)
                        {
                            if (0 == totalGameType)
                                gameKindIds += gameKindDetail.CodeID + ",";
                            gameTypeValues += gameTypeDetail.GameTypeID + ",";
                            totalGameType++;
                            if (totalGameType == lsGameType.Count)
                            {
                                gameTypeValues = gameTypeValues.Substring(0, gameTypeValues.Length - 1);
                                gameTypeNames = gameTypeNames + Globals.SEP_GAMEKIND_GAMETYPE + gameKindDetail.CodeID;
                                lsJS.Add(string.Format(Globals.JS_VariableDefinitionFormat, gameTypeNames, string.Format(Globals.JS_arrayFormat, gameTypeValues)));
                                gameKinds += gameTypeNames + ",";
                            }
                        }
                    }
                }
            }
            gameKinds = (!string.IsNullOrEmpty(gameKinds)) ? string.Format(Globals.JS_VariableFormat, Globals.JS_arrGameKind, string.Format(Globals.JS_arrayFormat, gameKinds.Substring(0, gameKinds.Length - 1))) : "[]";
            gameKindIds = (!string.IsNullOrEmpty(gameKindIds)) ? string.Format(Globals.JS_VariableFormat, Globals.JS_arrGameKindID, string.Format(Globals.JS_arrayFormat, gameKindIds.Substring(0, gameKindIds.Length - 1))) : "[]";
            lsJS.Add(gameKindIds);
            lsJS.Add(gameKinds);
        }

        private static bool IsP2PGame(string gameKindName)
        {
            if (gameKindName.ToLower().IndexOf(Globals.GAME_P2P.ToLower()) == -1)
            {
                return false;
            }
            return true;
        }

        private static bool IsLiveGame(string gameKindName)
        {
            if (gameKindName.ToLower().IndexOf(Globals.GAME_LIVE.ToLower()) == -1)
            {
                return false;
            }
            return true;
        }

        private static bool IsRnGGame(string gameKindName)
        {
            if (gameKindName.ToLower().IndexOf(Globals.GAME_RNG.ToLower()) == -1)
            {
                return false;
            }
            return true;
        }

        private static void AssignCSCustInfo(ref string tplCSPT, CSCustomerDetail customer, bool readOnly)
        {
            if (Globals.IBCSetting.Casino.ShowBetSetting)
            {
                TemplateHandler.RemoveBlock(ref tplCSPT, "max-win-lose");
            }

            TemplateHandler.Assign(ref tplCSPT, "lblAccumulated", Lang.lblAccumulatedWinLoss);
            TemplateHandler.Assign(ref tplCSPT, "lblMaxTransferCredit", Lang.lblMaxTransferCredit);
            TemplateHandler.Assign(ref tplCSPT, "lblMaxWin", Lang.lblMaxWinAllowed);
            TemplateHandler.Assign(ref tplCSPT, "lblMaxLose", Lang.lblMaxLoseAllowed);
            TemplateHandler.Assign(ref tplCSPT, "lblMin", Lang.lblMin);

            TemplateHandler.Assign(ref tplCSPT, "Reset", (readOnly || !customer.CasinoSync) ? string.Empty : Helper.GenLink(Lang.reset, "javascript:;", string.Empty, "ResetMaxTransfer('" + customer.IBCCustID + "')"));

            TemplateHandler.Assign(ref tplCSPT, "accumulatedValue", customer.TotalWinLoss.ToString());
            TemplateHandler.Assign(ref tplCSPT, "maxTransferCredit",
                (readOnly)
                ?
                Helper.GenLabel(Globals.TXT_MAXTRANSTERCREDIT, customer.MaxTransferCredit.ToString(), string.Empty)
                :
                Helper.GenTextBox(Globals.TXT_MAXTRANSTERCREDIT,
                    Globals.TXT_MAXTRANSTERCREDIT,
                    !customer.CasinoSync ? customer.DefaultTransferCredit.ToString() : customer.MaxTransferCredit.ToString(),
                    string.Empty,
                    false,
                    string.Empty,
                    "return numberOnly(event)"
                    )
            );

            TemplateHandler.Assign(ref tplCSPT, "cbxAutoResetchecked", (customer.IsAutoReset || !customer.CasinoSync) ? "checked" : string.Empty);

            TemplateHandler.Assign(ref tplCSPT, "autoReset", Lang.autoReset);

            TemplateHandler.Assign(ref tplCSPT, "isSync", (!customer.CasinoSync && !customer.IsMultipleEdit).ToString().ToLower());
            TemplateHandler.Assign(ref tplCSPT, "MaxTransferCreditValue", customer.MaxTransferCredit);

            TemplateHandler.Assign(ref tplCSPT, "disabled", readOnly ? "disabled" : string.Empty);
            TemplateHandler.Assign(ref tplCSPT, "transferCreditId", Globals.TXT_MAXTRANSTERCREDIT);

            TemplateHandler.Assign(ref tplCSPT, "maxWinValue", (readOnly) ? Helper.GenLabel(Globals.TXT_MAXWINID, CastAide.AsString(customer.MaxWin, string.Empty), string.Empty) : Helper.GenTextBox(Globals.TXT_MAXWINID, CastAide.AsString(customer.MaxWin, string.Empty), string.Empty, false, string.Empty));
            TemplateHandler.Assign(ref tplCSPT, "maxLoseValue", (readOnly) ? Helper.GenLabel(Globals.TXT_MAXLOSEID, CastAide.AsString(customer.MaxLose, string.Empty), string.Empty) : Helper.GenTextBox(Globals.TXT_MAXLOSEID, CastAide.AsString(customer.MaxLose, string.Empty), string.Empty, false, string.Empty));
            TemplateHandler.Assign(ref tplCSPT, "minValue", CastAide.AsString(customer.MinCreditBase, string.Empty));

            TemplateHandler.Assign(ref tplCSPT, "noLimitedValue", FormatHandler.FormatNumber2(customer.MaxTransferCreditLimit, UserSession.Current.LangKey));
            TemplateHandler.Assign(ref tplCSPT, "minLimitedValue", FormatHandler.FormatNumber2(customer.MinTransferCreditLimit, UserSession.Current.LangKey));

            TemplateHandler.Assign(ref tplCSPT, "submit", (readOnly) ? string.Empty : Helper.GenLink(Lang.submit, Globals.CS_SUBMIT_HREF, Globals.CLASS_SUBMIT, Globals.CS_SUBMIT_FUNC));
        }

        private static void AssignIBCCustInfo(ref string tplCSPT, CSCustomerDetail customer)
        {
            if (!customer.IsMultipleEdit)
            {
                TemplateHandler.Assign(ref tplCSPT, "lblUsername", Lang.username);
                TemplateHandler.Assign(ref tplCSPT, "lblFirstname", Lang.firstname);
                TemplateHandler.Assign(ref tplCSPT, "lblPhone", Lang.lblPhone);
                TemplateHandler.Assign(ref tplCSPT, "lblFax", Lang.lblFax);
                TemplateHandler.Assign(ref tplCSPT, "lblUsername2", Lang.nickname);
                TemplateHandler.Assign(ref tplCSPT, "lblLastname", Lang.lastname);
                TemplateHandler.Assign(ref tplCSPT, "lblMobile", Lang.lblMobilePhone);
                TemplateHandler.Assign(ref tplCSPT, "lblClosed", Lang.lblStatusClosed);
                TemplateHandler.Assign(ref tplCSPT, "lblSuspend", Lang.suspend);
                TemplateHandler.Assign(ref tplCSPT, "username", customer.UserName);
                TemplateHandler.Assign(ref tplCSPT, "fax", customer.Fax);
                TemplateHandler.Assign(ref tplCSPT, "firstname", customer.FirstName);
                TemplateHandler.Assign(ref tplCSPT, "phone", customer.Phone);
                TemplateHandler.Assign(ref tplCSPT, "username2", customer.UserName2);
                TemplateHandler.Assign(ref tplCSPT, "lastname", customer.LastName);
                TemplateHandler.Assign(ref tplCSPT, "mobile", customer.MobilePhone);
            }

            string labels1 = string.Empty;
            string labels2 = string.Empty;

            if (customer.CasinoSync && (customer.RoleId == 2 || customer.RoleId == 3))
            {
                labels1 = (customer.RoleId == 2) ? Resources.Lang.agent : Resources.Lang.master;
                labels2 = Resources.Lang.casinodownlines;
            }

            labels1 = "<label for='" + Globals.ID_CHECKBOX_DISABLEDCASINO + "'>" + labels1 + "</label>";
            labels2 = "<label for='" + Globals.ID_CHECKBOX_DISABLEDCASINO_DL + "'>" + labels2 + "</label>";

            string checkBoxes = "&nbsp;" + Helper.GenCheckBox(Globals.ID_CHECKBOX_DISABLEDCASINO, (UserSession.Current.RoleId - customer.RoleId != 1) || AppSession.IsCasinoInDBDisabled, !customer.CasinoDisabled && (customer.CasinoSync || customer.IsMultipleEdit), "MarkChangeStatus()") + labels1;

            if (customer.RoleId != 1 && customer.CasinoSync && UserSession.Current.IsCasinoSync)
            {
                if (customer.CasinoDisabled)
                {
                    checkBoxes += "&nbsp;<span id=\"" + Globals.ID_SPAN_DOWNLINE_OPT + "\" style=\"visibility: hidden; font-weight: normal\">" + Helper.GenCheckBox(Globals.ID_CHECKBOX_DISABLEDCASINO_DL, false, false, string.Empty) + labels2 + Helper.GenHidden(Globals.HIDDEN_UPDDOWNLLINE, string.Empty) + "</span>";
                }
                else
                {
                    checkBoxes += "&nbsp;&nbsp;&nbsp;&nbsp;<span id=\"" + Globals.ID_SPAN_DOWNLINE_OPT + "\" style=\"visibility: hidden; font-weight: normal; color: red\">(" + Resources.Lang.casinodownlinesdisabled + ")</span>";
                }
            }

            TemplateHandler.Assign(ref tplCSPT, "disableCS", checkBoxes);
            TemplateHandler.Assign(ref tplCSPT, "lblEnableCS", Resources.Lang.lblEnableCS);
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
            jsData += "var ID_CHECKBOX_DISABLEDCASINO = '" + Globals.ID_CHECKBOX_DISABLEDCASINO + "';\r\n";
            jsData += "var ID_CHECKBOX_DISABLEDCASINO_DL = '" + Globals.ID_CHECKBOX_DISABLEDCASINO_DL + "';\r\n";
            jsData += "var ID_SPAN_DOWNLINE_OPT = '" + Globals.ID_SPAN_DOWNLINE_OPT + "';\r\n";
            jsData += "var strHidUpdDownline = '" + Globals.HIDDEN_UPDDOWNLLINE + "';\r\n";
            jsData += "</script>";
            TemplateHandler.Assign(ref tplPT, "jsdata", jsData);
        }

        private static void GenSubmitButton(ref string tplCSPT, bool hidden)
        {
            TemplateHandler.Assign(ref tplCSPT, "submit", (hidden) ? string.Empty : Helper.GenLink(Lang.submit, Globals.CS_SUBMIT_HREF, Globals.CLASS_SUBMIT, Globals.CS_SUBMIT_FUNC));
        }

        private static string GenHiddenDatas(string gameData, CSCustomerDetail cust)
        {
            string hiddens = string.Empty;
            hiddens += Helper.GenHidden(Globals.HIDDEN_GAMEDATA, gameData);
            hiddens += Helper.GenHidden(Globals.HIDDEN_RoldID, cust.RoleId.ToString()); //master role id = 3
            hiddens += Helper.GenHidden("nobackground", HttpContext.Current.Request.QueryString["nobackground"]); //master role id = 3
            hiddens += Helper.GenHidden("isupdate", HttpContext.Current.Request.QueryString["isupdate"] == null ? string.Empty : HttpContext.Current.Request.QueryString["isupdate"]);

            hiddens += Helper.GenHidden(Globals.HIDDEN_LangID, Globals.CONST_LangID.ToString()); // Hard code
            hiddens += Helper.GenHidden(Globals.HIDDEN_CountryID, Globals.CONST_CountryID.ToString()); // Hard code

            return hiddens;
        }

        private static string GenHiddenDatas(Hashtable datas, CSCustomerDetail cust)
        {
            string hiddens = string.Empty;
            hiddens += Helper.GenHidden(Globals.HIDDEN_RoldID, cust.RoleId.ToString()); //master role id = 3
            hiddens += Helper.GenHidden("nobackground", HttpContext.Current.Request.QueryString["nobackground"]);
            hiddens += Helper.GenHidden("isupdate", HttpContext.Current.Request.QueryString["isupdate"] == null ? string.Empty : HttpContext.Current.Request.QueryString["isupdate"]);
            hiddens += Helper.GenHidden(Globals.HIDDEN_LangID, Globals.CONST_LangID.ToString()); // Hard code
            hiddens += Helper.GenHidden(Globals.HIDDEN_CountryID, Globals.CONST_CountryID.ToString()); // Hard code
            IDictionaryEnumerator enumerator = datas.GetEnumerator();
            while (enumerator.MoveNext())
            {
                hiddens += Helper.GenHidden(enumerator.Key.ToString(), enumerator.Value.ToString());
            }

            return hiddens;
        }

        private static void AssignCommonHeader(ref string tplCSPT)
        {
            TemplateHandler.Assign(ref tplCSPT, "lblCustomerInfomation", Lang.lblCustomerInfomation);
            TemplateHandler.Assign(ref tplCSPT, "lblBetSetting", Lang.lblBetSetting);
            TemplateHandler.Assign(ref tplCSPT, "lblPositionTaking", Lang.positiontaking);
        }

        private static int GetGameKindRowSpan(int totalGameType, bool useGroupSetting, string gameKindName)
        {
            int rowSpan = 0;
            string browser = HttpContext.Current.Request.Browser.Browser;
            if (IsP2PGame(gameKindName))
            {
                if (browser == "IE")
                {
                    rowSpan = totalGameType + 1;
                }
                else
                {
                    rowSpan = useGroupSetting ? totalGameType : totalGameType + 1;
                }
            }
            else
            {
                if (browser == "IE")
                {
                    rowSpan = (totalGameType * 2) + 1;
                }
                else
                {
                    rowSpan = useGroupSetting ? (totalGameType + 1) : ((totalGameType * 2) + 1);
                }
            }
            return rowSpan;
        }
    }
}