using System;
using System.Web.Configuration;

/// <summary>
/// Summary description for Globals
/// </summary>
public static class Globals
{
    public readonly static ConfigSection.IBCAccounting IBCSetting = (ConfigSection.IBCAccounting)WebConfigurationManager.GetSection("IBCAgent");
    // Defind static value for Casino

    public static readonly string CASINO_NO = "01";

    public static readonly string PRESET_PT = "preset_pt";
    public static readonly string AUTO_PT = "auto_pt";
    public static readonly string MAX_PT = "max_pt";
    public static readonly string MIN_PT = "min_pt";
    public static readonly string COMM_PT = "comm";
    public static readonly string TAG_HEADER = "header";
    public static readonly string TAG_GROUP_SETTING = "group_setting";
    public static readonly string TAG_PRIVATE_SETTING = "private_setting";
    public static readonly string TAG_BETTING_LIMIT = "betting_limit";
    public static readonly string TAG_BET_SETTING = "cs-bet-setting";

    // Path to template
    public static readonly string CASINO_PT_TEMPLATE = "MemberInfo/CasinoPT.htm";
    public static readonly string CASINO_PT_HEADER_TEMPLATE = "MemberInfo/CasinoPTHeader.htm";

    public static readonly string CASINO_P2P_PT_TEMPLATE = "MemberInfo/CasinoP2PPT.htm";
    public static readonly string CASINO_OTHERS_PT_TEMPLATE = "MemberInfo/CasinoOthersPT.htm";

    ////////////////////////////////

    public static readonly string GAME_P2P = "P2P";
    public static readonly int GAME_P2P_ID = 10;
    public static readonly string GAME_LIVE = "Live";
    public static readonly int GAME_LIVE_ID = 8;
    public static readonly string GAME_RNG = "RnG";
    public static readonly int GAME_RNG_ID = 9;
    // Path to controls template (hidden, text, checkbox, button..)
    public static readonly string TPL_CTR_SELECTOPTION = "Controls/SelectOption.htm";
    public static readonly string TPL_CTR_CHECKBOX = "Controls/CheckBox.htm";
    public static readonly string TPL_CTR_HIDDEN = "Hidden.tpl";
    public static readonly string TPL_CTR_BUTTON = "Controls/Button.htm";
    public static readonly string TPL_CTR_TEXTBOX = "Controls/TextBox.htm";
    public static readonly string TPL_CTR_INPUT = "Controls/Input.htm";
    public static readonly string TPL_CTR_LABEL = "Controls/Label.htm";
    public static readonly string TPL_CTR_LINK = "Controls/Link.htm";
    // /////////////////////////

    public static readonly int CS_GAME_GROUP = 0;

    public static readonly string strMaxPT = "MaxPT";
    public static readonly string strMinPT = "MinPT";
    public static readonly string Comm = "Comm";
    public static readonly string strPT = "PT";
    public static readonly string strPresetPT = "PresetPT";
    public static readonly string strAutoPT = "AutoPT";

    public static readonly string Super = "S";
    public static readonly string Master = "M";
    public static readonly string Agent = "A";
    public static readonly string Member = "Mem";

    public static readonly int SuperLevel = 4;
    public static readonly int MasterLevel = 3;
    public static readonly int AgentLevel = 2;
    public static readonly int MemberLevel = 1;

    // Define max value of Min, Max, Auto, Preset PT, Comm
    public static readonly double MAX_MaxPTValue = 0.9;
    public static readonly double MAX_MinPTValue = 0.9;
    public static readonly double MAX_AutoPTValue = 0.9;
    public static readonly double MAX_PresetPTValue = 0.9;
    public static readonly double MAX_Comm = 0.01;
    public static readonly double CURRENT_MaxPTValue = 0.9;
    public static readonly double CURRENT_MinPTValue = 0.9;
    public static readonly double CURRENT_AutoPTValue = 0.9;
    public static readonly double CURRENT_PresetPTValue = 0.9;
    public static readonly double CURRENT_Comm = 0.01;

    public static readonly int INTERVAL_PT = 100;
    public static readonly int INTERVAL_COMM = 5;

    public static readonly string CHKGROUP = "ChkGroupSetting";
    public static readonly string strCHKAUTO = "ChkAuto";
    public static readonly string strCHK = "Chk";
    public static readonly string CHK_STAKE = "ChkStake";

    public static readonly string SEP_GAMEKIND_GAMETYPE = "_";

    // Hidden
    public static readonly string HIDDEN_GAMEDATA = "hidCSData";
    public static readonly string HIDDEN_RoldID = "hidRoleId";
    public static readonly string HIDDEN_IBCCustID = "hidCustID";
    public static readonly string HIDDEN_USERNAME = "hidUsername";
    public static readonly string HIDDEN_FIRSTNAME = "hidFirstname";
    public static readonly string HIDDEN_PHONE = "hidPhone";
    public static readonly string HIDDEN_FAX = "hidFax";
    public static readonly string HIDDEN_USERNAME2 = "hidUsername2";
    public static readonly string HIDDEN_LASTNAME = "hidLastname";
    public static readonly string HIDDEN_MOBILEPHONE = "hidMobilePhone";
    public static readonly string HIDDEN_SIBCCustID = "hidSCustID";
    public static readonly string HIDDEN_MIBCCustID = "hidMCustID";
    public static readonly string HIDDEN_AIBCCustID = "hidACustID";
    public static readonly string HIDDEN_PTByGameType = "hidPTByGameType";
    public static readonly string HIDDEN_ExchangeID = "hidExchangeID";
    public static readonly string HIDDEN_LangID = "hidLangID"; // hard code
    public static readonly string HIDDEN_CountryID = "hidCountryID";// hard code
    public static readonly string HIDDEN_CLOSED = "hidClosed";
    public static readonly string HIDDEN_SUSPEND = "hidSuspend";
    public static readonly string HIDDEN_PT = "hidPT";
    public static readonly string HIDDEN_UPDDOWNLLINE = "hidUpdDownline";

    public static readonly string OPEN_GAMETYPELIST_MARK = "[";
    public static readonly string CLOSE_GAMETYPELIST_MARK = "]";
    public static readonly string OPEN_STAKELIST_MARK = "(";
    public static readonly string CLOSE_STAKELIST_MARK = ")";
    public static readonly string SEP_STAKE = ", ";
    public static readonly string SEP_GAMETYPE = "-";
    public static readonly string SEP_GAMEKIND = ";";
    public static readonly string SEP_PT = "|";
    public static readonly string SEP_PT_VALUE = ":";
    // Class stylesheet
    public static readonly string CLASS_HIDDEN = "hidden";
    public static readonly string CLASS_ERROR_MAXVALUE = "errMaxValue";
    public static readonly string CLASS_SUBMIT = "b_add";
    public static readonly string CLASS_SELECT_COMMISSION = "commission";

    // Group increase, decrease PT button class, suffix and click function
    public static readonly string BTN_INC1_CLASS = "btn-inc1 btnpt_margin btn_hand";
    public static readonly string BTN_INC10_CLASS = "btn-inc10 btnpt_margin btn_hand";
    public static readonly string BTN_DEC1_CLASS = "btn-dec1 btnpt_margin btn_hand";
    public static readonly string BTN_DEC10_CLASS = "btn-dec10 btnpt_margin btn_hand";

    public static readonly string BTN_INC1_DISABLE_CLASS = "btn-inc1 btnpt_margin";
    public static readonly string BTN_INC10_DISABLE_CLASS = "btn-inc10 btnpt_margin";
    public static readonly string BTN_DEC1_DISABLE_CLASS = "btn-dec1 btnpt_margin";
    public static readonly string BTN_DEC10_DISABLE_CLASS = "btn-dec10 btnpt_margin";

    public static readonly string BTN_INC1_SUFFIX = "BtnInc_1";
    public static readonly string BTN_INC10_SUFFIX = "BtnInc_10";
    public static readonly string BTN_DEC1_SUFFIX = "BtnDec_1";
    public static readonly string BTN_DEC10_SUFFIX = "BtnDec_10";

    // SESSION
    public static readonly string SESSION_CUSTOMER = "customer";
    // CS PT setting id

    public static readonly string TXT_MAXWINID = "txtMaxWin";
    public static readonly string TXT_MAXTRANSTERCREDIT = "txtMaxTransferCredit";
    public static readonly string TXT_MAXLOSEID = "txtMaxLose";
    public static readonly string BTN_ResetID = "btnReset";

    // Nolimited value
    public static readonly int CS_NOLIMITED_WINLOSE = 0;

    // Class style
    public static readonly string CLASS_CHECKBOX_HIDDEN = "hidden";
    /////////

    // END

    public static readonly string CTR_INPUT_BUTTON = "button";
    public static readonly string CTR_INPUT_HIDDEN = "hidden";
    public static readonly string CTR_INPUT_TEXTBOX = "text";
    public static readonly string CTR_INPUT_TEXTBOX_PREFIX = "txt";
    public static readonly string CTR_INPUT_CHECKBOX = "checkbox";
    public static readonly string CTR_OPTIONS_PREFIX = "opt";

    // Define ID
    public static readonly string ID_CHECKBOX_DISABLEDCASINO = "chkDisabledCasino";
    public static readonly string ID_CHECKBOX_DISABLEDCASINO_DL = "chkDisabledCasinoDL";
    public static readonly string ID_SPAN_DOWNLINE_OPT = "spanDownlineOpt";
    public static readonly string ID_CHECKBOX_CLOSED = "chkClosed";
    public static readonly string ID_CHECKBOX_SUSPEND = "chkSuspend";

    // Define prefix of control
    public static readonly string CTR_LABEL_PREFIX = "error";
    public static readonly string PREFIX_CHECKBOX = "chk";

    // Hard code value
    public static readonly int CONST_LangID = 0; // hard code
    public static readonly int CONST_CountryID = 0;// hard code
    public static readonly double CONST_CHANGEPT_1 = 0.01;
    public static readonly double CONST_CHANGEPT_10 = 0.1;

    // PT string
    public static readonly string s4Max = "s4Max", s3Max = "s3Max", s1Max = "s1Max", m3Max = "m3Max", m2Max = "m2Max", m1Max = "m1Max", a2Max = "a2Max", a1Max = "a1Max";
    public static readonly string u3Max = "u3Max", n4Max = "n4Max", n3Max = "n3Max", n2Max = "n2Max", l4Max = "l4Max", l3Max = "l3Max", l2Max = "l2Max";
    public static readonly string m3MaxAuto = "m3MaxAuto", n3MaxAuto = "n3MaxAuto";

    // Casino Javascript Variables
    public static readonly string JS_arrLiveGameType = "arrLiveGameType";
    public static readonly string JS_arrRnGGameType = "arrRnGGameType";
    public static readonly string JS_arrP2PGameType = "arrP2PGameType";
    public static readonly string JS_arrGameKind = "arrGameKind";
    public static readonly string JS_arrGameKindID = "arrGameKindID";
    public static readonly string JS_VariableFormat = "{0} = {1};";
    public static readonly string JS_VariableDefinitionFormat = "var {0} = {1};";
    public static readonly string JS_arrayFormat = "[{0}]";

    // Define javascript functions

    // Button click
    public static readonly string BTN_CLICK_INC_DEC_FUNC = "";
    public static readonly string BTN_CLICK_RESET_CS_PT_FUNC = "";
    /// <summary>
    /// {0}: button control, "this"
    /// {1}: value
    /// </summary>
    public static readonly string BTN_CHANGEPT_CLICK = "ChangePT({0}, {1})"; //{0}

    // Select option change
    public static readonly string CS_SELECTOPTION_ONCHANGE_GROUP_FUNC = "OnGroupSettingChange({0})";
    public static readonly string CS_SELECTOPTION_ONCHANGE_GROUP_PARAM = "this";//"this" is the control use as group setting
    public static readonly string CS_SELECTOPTION_ONCHANGE_FUNC = "";
    public static readonly string CS_SELECTOPTION_ONCHANGE_PARAM = "";

    public static readonly string CS_SELECTOPTION_ONCHANGE_PRESET_FUNC = "ChangePresetPT({0})";
    public static readonly string CS_SELECTOPTION_ONCHANGE_PRESET_PARAM = "this";
    // Checkbox click
    public static readonly string CS_ChkAUTO_CLICK_FUNC = "CheckAuto({0})";
    public static readonly string CS_ChkGROUP_CLICK_FUNC = "CheckGroupSetting({0})"; //{0}: arguments of this funciton
    public static readonly string CS_ChkAutoGROUP_CLICK_FUNC = "CheckGroupAuto({0})"; //{0}: arguments of this funciton

    // Submit function
    public static readonly string CS_SUBMIT_FUNC = "DelayReloadPage(); $('btnSubmit').click();";
    public static readonly string CS_SUBMIT_HREF = "javascript:void();";

    // Define provider type
    public static readonly string PROVIDER_BINGO_API = "DAL.APIClient.APIBingoProvider";
    public static readonly string PROVIDER_CS_API = "DAL.APIClient.APICasinoProvider";
    public static readonly string PROVIDER_IBC_LIB = "DAL.IBCClient.IBCLibProvider";
    ///////

    // Account type

    public enum AccountType : int
    {
        Super = 4, Master = 3, Agent = 2, Member = 1
    }

    // Input control type
    public enum InputType
    {
        Button = 4, Text = 3, Hidden = 2, CheckBox = 1
    }

    // Casino Group
    public enum CasinoGroup
    {
        Group1 = 1, Group2 = 2
    }

    public static readonly string CASINO_GROUP1 = "01";
    public static readonly string CASINO_GROUP2 = "02";

    public static string GetCasinoGroup(CasinoGroup group)
    {
        try
        {
            switch (group)
            {
                case CasinoGroup.Group1:
                    return CASINO_GROUP1;
                case CasinoGroup.Group2:
                    return CASINO_GROUP2;
            }
            return string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string GetInputType(InputType input)
    {
        try
        {
            switch (input)
            {
                case InputType.Button:
                    return CTR_INPUT_BUTTON;
                case InputType.CheckBox:
                    return CTR_INPUT_CHECKBOX;
                case InputType.Hidden:
                    return CTR_INPUT_HIDDEN;
                case InputType.Text:
                    return CTR_INPUT_TEXTBOX;
            }
            return string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // Error
    public enum CasinoError : int
    {
        Not_Casino_Customer = 0
    }

    public static string GetCasinoError(CasinoError errorCode)
    {
        switch (errorCode)
        {
            case CasinoError.Not_Casino_Customer:
                {
                    return "Not casino's Customer";
                }
        }
        return string.Empty;
    }
}