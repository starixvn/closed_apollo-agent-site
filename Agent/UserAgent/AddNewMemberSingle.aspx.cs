using System;
using System.Text;
using System.Collections;
using System.Data;

using DNAide.Web;
using DNAide.Core;
using DNAide.Utils;
using Fanex.Data.DNAide;

using Sunplus.Agent.Model.Entities.Permission;

public partial class UserAgent_AddNewMember_Single : TplPage
{
    string strGroupA = "", strGroupB = "", strGroupC = "", strGroupD = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //check the difference of position taking, min bet, maxbet, maxpermatch.(If different i strue => redirect to full mode form) Acc_CustomerInsMember_Ag_IsSimple        
        if (!AccountInfo.IsSimple(Sunplus.Agent.Model.Entities.UserSession.Current.CustId))
        {
            Response.Write("<script type='text/javascript'>location.href='AddNewMember.aspx?simple=0';</script>");
            return;
        }
        this.CssGroup = "main";

        string strContent = TemplateHandler.LoadTemplate("MemberInfo/AddNewMemberSingle.html");

        if (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal)
            TemplateHandler.Assign(ref strContent, "clickable", "return false");
        else
            TemplateHandler.Assign(ref strContent, "clickable", String.Empty);

        AssignText(ref strContent);
        string strBtnGroup = TemplateHandler.LoadTemplate("MemberInfo/BtnGroup.html");
        try
        {
            Hashtable ht = new Hashtable();
            ht.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
            DbResult rs = AccountInfo.Acc_CustomerSelAgentInfo(ht);
            DataRow dr = rs.ResultSet.Tables[0].Rows[0];
            //string strComhdp = FormatHandler.FormatNumber2(CastAide.AsDecimal(dr["groupa"]) * 100, this.Culture);
            strGroupA = FormatHandler.FormatNumber4(dr["groupa"], this.Culture);
            strGroupB = FormatHandler.FormatNumber4(dr["groupb"], this.Culture);
            strGroupC = FormatHandler.FormatNumber4(dr["groupc"], this.Culture);
            strGroupD = FormatHandler.FormatNumber4(dr["groupd"], this.Culture);
            string strUserNameNo = dr["newusername"].ToString();
            string[] strArrNum = GetListNumber(strUserNameNo);

            Hashtable htbDis = new Hashtable();
            htbDis.Add("Suspend", true);
            string myTemplate = TemplateHandler.LoadTemplate("Memberinfo/DisableStatus.html");
            string mycolspan = "<td colspan='2'><td>";
            TemplateHandler.ReplaceBlock(ref strContent, "DisableStatus", StatusSetting.StatusDisable(mycolspan, htbDis, myTemplate));

            TemplateHandler.Assign(ref strContent, "suspend-text", Resources.Lang.suspended);

            TemplateHandler.RemoveBlock(ref strContent, "downline");

            TemplateHandler.Assign(ref strContent, "hdAllowedCurr-value", CastAide.AsInt16(dr["AllowedCurr"]));
            TemplateHandler.Assign(ref strContent, "number1-val", strArrNum[0]);
            TemplateHandler.Assign(ref strContent, "number2-val", strArrNum[1]);
            TemplateHandler.Assign(ref strContent, "number3-val", strArrNum[2]);
            TemplateHandler.Assign(ref strContent, "slgroup", AssignData4Group());
            TemplateHandler.Assign(ref strContent, "agusername", Sunplus.Agent.Model.Entities.UserSession.Current.UserName);

            double dcMinVal = Math.Round(CastAide.AsDouble(dr["apositiontaking_1_min"]), 4);
            double dcMinValOther = Math.Round(CastAide.AsDouble(dr["apositiontaking_5_min"]), 4);
            double dcMinValOutright = Math.Round(CastAide.AsDouble(dr["apositiontaking_10_min"]), 4);
            double dcMinValNumberGame = Math.Round(CastAide.AsDouble(dr["v161apositiontaking_81_min"]), 4);

            double dcLiveMinValHdp = Math.Round(CastAide.AsDouble(dr["alivepositiontaking_1_min"]), 4);


            double dcMaxVal = Math.Round(CastAide.AsDouble(dr["apositiontaking_1"]), 4);
            double dcMaxValOther = Math.Round(CastAide.AsDouble(dr["apositiontaking_5"]), 4);
            double dcMaxValOutright = Math.Round(CastAide.AsDouble(dr["apositiontaking_10"]), 4);
            double dcMaxValNumberGame = Math.Round(CastAide.AsDouble(dr["v161apositiontaking_81"]), 4);

            double dcLiveMaxValHdp = Math.Round(CastAide.AsDouble(dr["alivepositiontaking_1"]), 4);


            double dcMinBet = CastAide.AsDouble(dr["minbet_max"]);
            double dcMaxBet = CastAide.AsDouble(dr["maxbet_min"]);
            double dcMaxPerMatch = CastAide.AsDouble(dr["maxpermatch_max"]);
            double dcMaxPerBall = CastAide.AsDouble(dr["maxperballv161_max"]);
            //Commission and positiontaking
            TemplateHandler.Assign(ref strContent, "slmempt", PTDropdownlist("slPT", dcMinVal, dcMaxVal, dcMaxVal));
            string strBtnHdp = strBtnGroup;
            TemplateHandler.Assign(ref strBtnHdp, "ddl-Id", "slPT");
            TemplateHandler.Assign(ref strContent, "hdp-btn", strBtnHdp);
            TemplateHandler.Assign(ref strContent, "slotherpt", PTDropdownlist("slOtherPT", dcMinValOther, dcMaxValOther, dcMaxValOther));
            TemplateHandler.Assign(ref strContent, "slmemoutrightpt", PTDropdownlist("slOutrightPT", dcMinValOutright, dcMaxValOutright, dcMaxValOutright));
            TemplateHandler.Assign(ref strContent, "sllivehandicap", PTDropdownlist("slLivePT", dcLiveMinValHdp, dcLiveMaxValHdp, dcLiveMaxValHdp));
            TemplateHandler.Assign(ref strContent, "numbergamept", PTDropdownlist("slNumberGamePT", dcMinValNumberGame, dcMaxValNumberGame, dcMaxValNumberGame));

            strBtnHdp = strBtnGroup;
            TemplateHandler.Assign(ref strBtnHdp, "ddl-Id", "slLivePT");
            TemplateHandler.Assign(ref strContent, "live-hdp-btn", strBtnHdp);

            //min max
            TemplateHandler.Assign(ref strContent, "maxcredit-val", FormatHandler.FormatNumber2(dr["credit_max"], this.Culture));
            TemplateHandler.Assign(ref strContent, "maxpermatch-val", FormatHandler.FormatNumber2(dr["maxpermatch_max"], this.Culture));

            TemplateHandler.Assign(ref strContent, "MaxPerBall-max", FormatHandler.FormatNumber2(dcMaxPerBall, this.Culture));

            if (dcMinBet <= 0)
            {
                TemplateHandler.Assign(ref strContent, "hideMinbet", "hidden");
            }
            else
            {
                TemplateHandler.Assign(ref strContent, "hideMinbet", string.Empty);
            }

            if (dcMaxBet <= 0)
            {
                TemplateHandler.Assign(ref strContent, "hideMaxbet", "hidden");
            }
            else
            {
                TemplateHandler.Assign(ref strContent, "hideMaxbet", string.Empty);
            }

            if (dcMaxPerMatch <= 0)
            {
                TemplateHandler.Assign(ref strContent, "hideMaxpermatch", "hidden");
            }
            else
            {
                TemplateHandler.Assign(ref strContent, "hideMaxpermatch", string.Empty);
            }

            if (dcMaxPerBall <= 0)
            {
                TemplateHandler.Assign(ref strContent, "maxperball_clss", "hidden");
            }
            else
            {
                TemplateHandler.Assign(ref strContent, "maxperball_clss", string.Empty);
            }

            TemplateHandler.Assign(ref strContent, "maxpermatchmin-val", FormatHandler.FormatNumber2(dr["maxpermatch_min"], this.Culture));
            TemplateHandler.Assign(ref strContent, "minbet-val", FormatHandler.FormatNumber2(dr["minbet_max"], this.Culture));
            TemplateHandler.Assign(ref strContent, "maxbet-val", FormatHandler.FormatNumber2(dr["maxbet_min"], this.Culture));

            TemplateHandler.Assign(ref strContent, "MaxPerBall-val", FormatHandler.FormatNumber2(dcMaxPerBall, this.Culture));


            decimal dcDiscountOther = Math.Round(CastAide.AsDecimal(dr["discountcs"]), 4);
            decimal dcDiscount1x2 = Math.Round(CastAide.AsDecimal(dr["discount1x2"]), 4);


            TemplateHandler.Assign(ref strContent, "comother-value", FormatHandler.FormatNumber4(dcDiscountOther, this.Culture));
            TemplateHandler.Assign(ref strContent, "CS_TG_option", AssignDataForComm(CastAide.AsDecimal(dcDiscountOther), 5));

            TemplateHandler.Assign(ref strContent, "1x2option", AssignDataForComm(CastAide.AsDecimal(dcDiscount1x2), 5));
            TemplateHandler.Assign(ref strContent, "discount1x2-val", FormatHandler.FormatNumber4(dcDiscount1x2, this.Culture));
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission || !UserPermission.IsShowNumberGame)
            {
                TemplateHandler.RemoveBlock(ref strContent, "none-numbergame");
            }
            else
            {
                decimal dcDiscountnumber = Math.Round(CastAide.AsDecimal(dr["discountnumber"]), 4);
                TemplateHandler.Assign(ref strContent, "numbergameoption", AssignDataForComm(CastAide.AsDecimal(dcDiscountnumber), 5));
                TemplateHandler.Assign(ref strContent, "discountnumbergame-val", FormatHandler.FormatNumber4(dcDiscountnumber, this.Culture));
            }



            TemplateHandler.Assign(ref strContent, "suspend-check", CastAide.AsBoolean(dr["suspended"]) ? "checked disabled" : "");
            TemplateHandler.Assign(ref strContent, "suspended-upline", CastAide.AsBoolean(dr["suspended"]) ? "1" : "");

            TemplateHandler.Assign(ref strContent, "hdAllowedCurr-value", CastAide.AsInt16(dr["AllowedCurr"]));


            VariableScript();
        }
        catch (Exception ex)
        {
            DNAideLog.Write(ex.Message);
            this.TplSetContent(ex.Message);
            return;
            //throw ex;
        }
        this.TplSetContent(strContent);
    }
    string AssignDataForComm(decimal value, int vstep)
    {
        decimal csm = 0;
        StringBuilder _option = new StringBuilder();
        csm = value * 10000;
        int index = 0;
        while (csm > 0)
        {
            decimal _value = csm / 10000;
            if (index == 0)
            {
                _option.Append("<option value=\"" + FormatHandler.FormatNumber4(_value, this.Culture) + "\"" + "selected='true' >" + FormatHandler.FormatNumber4(_value, this.Culture) + "</option>");
            }
            else
            {
                _option.Append("<option value=\"" + FormatHandler.FormatNumber4(_value, this.Culture) + "\"" + " >" + FormatHandler.FormatNumber4(_value, this.Culture) + "</option>");
            }
            csm = csm - vstep;
            index = index + 1;
        }
        _option.Append("<option value='0'>0</option>");
        return _option.ToString();
    }
    string PTDropdownlist(string strId, double minVal, double maxVal, double currentVal)
    {
        StringBuilder strBody = new StringBuilder();
        try
        {
            //Boolean isInRange = false;
            int intToValue = CastAide.AsInt32(maxVal * 100);
            int intFromValue = CastAide.AsInt32(minVal * 100);
            while (intToValue >= intFromValue)
            {
                Double ptValue = CastAide.AsDouble(intToValue) / 100;
                if (ptValue == currentVal)
                {
                    //isInRange = true;
                    strBody.Append("<option selected='true' value='" + NumberFormat.DecFormat(ptValue, 2) + "'>" + NumberFormat.DecFormat(ptValue, 2) + "</option>");
                }
                else
                    strBody.Append("<option value='" + NumberFormat.DecFormat(ptValue, 2) + "'>" + NumberFormat.DecFormat(ptValue, 2) + "</option>");
                intToValue = intToValue - 1;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return "<select id ='" + strId + "' name='" + strId + "' class='selectcomm'>" + strBody.ToString() + "</select>";
    }

    string AssignData4Group()
    {
        string str = "<select id = 'slGroup' name ='slGroup'onchange='ChangeGroup();' class='selectcomm'>";
        Hashtable ht2 = new Hashtable();
        ht2.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
        DbResult rs = AccountInfo.Acc_CustomerSelCustomerInfo(ht2);


        if (rs.ResultSet.Tables[0].Rows.Count > 0)
            if (rs.ResultSet.Tables[0].Rows[0]["currency"].ToString().ToLower() == "won")
            {
                str += "<option value='" + strGroupA + "' selected>dea</option>";
                str += "<option value='" + strGroupB + "'>deb</option>";
                str += "<option value='" + strGroupC + "'>dec</option>";
                str += "<option value='" + strGroupD + "'>ded</option>";
            }
            else
            {
                str += "<option value='" + strGroupA + "' selected>a</option>";
                str += "<option value='" + strGroupB + "'>b</option>";
                str += "<option value='" + strGroupC + "'>c</option>";
                str += "<option value='" + strGroupD + "'>d</option>";
                if (rs.ResultSet.Tables[0].Rows[0]["site"].ToString() == "IBSBET")
                {
                    str += "<option value='" + strGroupA + "'>indoa</option>";
                    str += "<option value='" + strGroupB + "'>indob</option>";
                    str += "<option value='" + strGroupC + "'>indoc</option>";
                    str += "<option value='" + strGroupD + "'>indod</option>";
                }
                else
                {
                    str += "<option value='" + strGroupA + "'>hka</option>";
                    str += "<option value='" + strGroupB + "'>hkb</option>";
                    str += "<option value='" + strGroupC + "'>hkc</option>";
                    str += "<option value='" + strGroupD + "'>hkd</option>";
                }
            }
        str += "</select>";
        return str;
    }

    string[] GetListNumber(string currentVal)
    {
        string[] result = { "", "", "" };
        try
        {
            StringBuilder htmlNo1 = new StringBuilder();
            StringBuilder htmlNo2 = new StringBuilder();
            StringBuilder htmlNo3 = new StringBuilder();

            string no1 = currentVal.Substring(0, 1).ToUpper();
            string no2 = currentVal.Substring(1, 1).ToUpper();
            string no3 = currentVal.Substring(2, 1).ToUpper();
            string strNextNo1 = no1;
            string strNextNo2 = no2;
            string strNextNo3 = "";

            string[] arr = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int index = 0;
            if (no3 == arr[arr.Length - 1])
            {
                strNextNo3 = arr[0];
                if (no2 == arr[arr.Length - 1])
                {
                    strNextNo2 = arr[0];
                    index = Array.IndexOf(arr, no1);
                    if (index >= arr.Length - 1)
                        strNextNo1 = arr[0];
                    else
                        strNextNo1 = arr[index + 1];
                }
                else
                {
                    index = Array.IndexOf(arr, no2);
                    if (index >= arr.Length - 1)
                        strNextNo2 = arr[0];
                    else
                        strNextNo2 = arr[index + 1];
                }
            }
            else
            {
                index = Array.IndexOf(arr, no3);
                if (index >= arr.Length - 1)
                    strNextNo3 = arr[0];
                else
                    strNextNo3 = arr[index + 1];
            }

            for (int i = 0; i < arr.Length; i++)
            {
                if (strNextNo2 == arr[i])
                    htmlNo2.Append("<option selected='true' value='" + arr[i] + "'>" + arr[i] + "</option>");
                else
                    htmlNo2.Append("<option value='" + arr[i] + "'>" + arr[i] + "</option>");

                if (strNextNo1 == arr[i])
                    htmlNo1.Append("<option selected='true' value='" + arr[i] + "'>" + arr[i] + "</option>");
                else
                    htmlNo1.Append("<option value='" + arr[i] + "'>" + arr[i] + "</option>");

                if (strNextNo3 == arr[i])
                    htmlNo3.Append("<option selected='true' value='" + arr[i] + "'>" + arr[i] + "</option>");
                else
                    htmlNo3.Append("<option value='" + arr[i] + "'>" + arr[i] + "</option>");
            }

            result[0] = ListSelNum("Number1", htmlNo1.ToString());
            result[1] = ListSelNum("Number2", htmlNo2.ToString());
            result[2] = ListSelNum("Number3", htmlNo3.ToString());
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }

    string ListSelNum(string strId, string strVal)
    {
        string strDDTpl = TemplateHandler.LoadTemplate("MemberInfo/DdlMemPT.html");
        TemplateHandler.Assign(ref strDDTpl, "ddl-Id", strId);
        TemplateHandler.Assign(ref strDDTpl, "ddl-val", strVal);
        return strDDTpl;
    }
    void VariableScript()
    {
        string script = string.Empty;

        script += "<script language='javascript'>";
        script += " var alertcredit='" + Resources.Lang.alertIncorrectAmaxcredit + "';";
        script += " var alertMinbet='" + Resources.Lang.alertIncorrectMinBet + "';";
        script += " var alertMaxbet='" + Resources.Lang.alertIncorrectMaxBet + "';";
        script += " var alertMaxpermatch='" + Resources.Lang.alertIncorrectMaxPerMatch + "';";

        script += " var alertCompare3='" + Resources.Lang.alertMaxLessMin + "'; ";
        script += " var alertCompare='" + Resources.Lang.alertMaxLessMaxPer + "'; ";
        script += " var alertminbetC='" + Resources.Lang.lblalertMinbetCompare + "';";
        script += " var alertMaxbetC='" + Resources.Lang.lblalerMaxbetCompare + "';";
        script += " var alertMaxpermatchC='" + Resources.Lang.lblalertMaxPerMatchCompare + "';";
        script += " var alertMaxpermatchCMin='" + Resources.Lang.lblalertMaxPerMatchCompareMin + "';";
        script += " var alertMaxpermatchCMax='" + Resources.Lang.lblalertMaxPerMatchCompareMax + "';";

        script += " var alertUserName='" + Resources.Lang.alertUserName + "';";
        script += " var lblPasswordRequire='" + Resources.Lang.lblPasswordRequire + "';";
        script += " var alertPasswordEmpty='" + Resources.Lang.alertPasswordEmpty + "';";
        script += " var alertIncorrectCredit='" + Resources.Lang.alertMemberCredit + "';";

        script += " var alertMaxperball='" + Resources.Lang.alertIncorrectMaxPerBall + "';";
        script += " var alertMaxperballCMax='" + Resources.Lang.lblalertMaxPerBallCompareMax + "';";
        script += " var alertMaxperballC='" + Resources.Lang.lblalertMaxPerBallCompare + "';";
        script += " var alertMaxPerMatchLessMaxPerBall='" + Resources.Lang.alertMaxPerMatchLessMaxPerBall + "';";
        script += " var alertMaxPerBallLessMaxBet='" + Resources.Lang.alertMaxPerBallLessMaxBet + "';";

        string lblCreditInvalid = string.Empty;

        lblCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMasterMaxCredit);
        script += " var alertCreditInvalid='" + lblCreditInvalid + "';";
        string strAlertMinPT = Resources.Lang.alertLessthanMinPT;
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Lang.lblMasterPresetPosition);
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Lang.lblSuperPresetPosition);
        script += " var alertLessthanMinPT='" + strAlertMinPT + "';";
        script += "</script>";
        Response.Write(script);

    }
    void AssignText(ref string strContent)
    {
        TemplateHandler.Assign(ref strContent, "add", Resources.Lang.btnAdd);
        TemplateHandler.Assign(ref strContent, "editmode", Resources.Lang.fullmode);
        TemplateHandler.Assign(ref strContent, "processing", Resources.Lang.processing);
        TemplateHandler.Assign(ref strContent, "newmem", Resources.Lang.lblAddNewMember);
        TemplateHandler.Assign(ref strContent, "username", Resources.Lang.username);
        TemplateHandler.Assign(ref strContent, "password", Resources.Lang.password);
        TemplateHandler.Assign(ref strContent, "fname", Resources.Lang.firstname);
        TemplateHandler.Assign(ref strContent, "lname", Resources.Lang.lastname);
        TemplateHandler.Assign(ref strContent, "phone", Resources.Lang.lblPhone);
        TemplateHandler.Assign(ref strContent, "mobilephone", Resources.Lang.lblMobilePhone);
        TemplateHandler.Assign(ref strContent, "maxcredit", Resources.Lang.lblMemberMaxCredit);
        TemplateHandler.Assign(ref strContent, "fax", Resources.Lang.lblFax);
        TemplateHandler.Assign(ref strContent, "group", Resources.Lang.group);
        TemplateHandler.Assign(ref strContent, "closed", Resources.Lang.lblStatusClosed);
        TemplateHandler.Assign(ref strContent, "suspended", Resources.Lang.suspended);
        TemplateHandler.Assign(ref strContent, "commission", Resources.Lang.commission);
        TemplateHandler.Assign(ref strContent, "lblCommissionForAgent", Resources.Lang.lblCommissionForAgent);
        TemplateHandler.Assign(ref strContent, "lblAsianHDC_OU_OE", Resources.Lang.lblAsianHDC_OU_OE);
        TemplateHandler.Assign(ref strContent, "lbl1x2_CS_TG", Resources.Lang.lblOtherTypes);
        TemplateHandler.Assign(ref strContent, "lblCommissionForMember", Resources.Lang.lblCommissionForMember);
        TemplateHandler.Assign(ref strContent, "betsetting", Resources.Lang.lblBetSetting);
        TemplateHandler.Assign(ref strContent, "minbet", Resources.Lang.lblMinBet);
        TemplateHandler.Assign(ref strContent, "maxbet", Resources.Lang.lblMaxBet);
        TemplateHandler.Assign(ref strContent, "maxpermatch", Resources.Lang.lblMaxPerMatch);
        TemplateHandler.Assign(ref strContent, "Commission", Resources.Lang.commission);
        TemplateHandler.Assign(ref strContent, "comforagt", Resources.Lang.lblCommissionForAgent);
        TemplateHandler.Assign(ref strContent, "asianhdp", Resources.Lang.lblAsianHDC_OU_OE);
        TemplateHandler.Assign(ref strContent, "otherbettype", Resources.Lang.lblOtherTypes);
        TemplateHandler.Assign(ref strContent, "comformem", Resources.Lang.lblCommissionForMember);
        TemplateHandler.Assign(ref strContent, "handicap", Resources.Lang.lblHandicapFC);
        TemplateHandler.Assign(ref strContent, "overunder", Resources.Lang.lblOverUnder);
        TemplateHandler.Assign(ref strContent, "oddeven", Resources.Lang.lblOddEven);
        TemplateHandler.Assign(ref strContent, "1x2", Resources.Lang.lbl1x2);
        TemplateHandler.Assign(ref strContent, "correctscore", Resources.Lang.correctscore);
        TemplateHandler.Assign(ref strContent, "totalgoal", Resources.Lang.totalgoal);
        TemplateHandler.Assign(ref strContent, "parlay", Resources.Lang.parlay);
        TemplateHandler.Assign(ref strContent, "pt", Resources.Lang.positiontaking.Replace("%", ""));
        TemplateHandler.Assign(ref strContent, "outright", Resources.Lang.lblOutRight);
        TemplateHandler.Assign(ref strContent, "deadball", Resources.Lang.lblDeadball);
        TemplateHandler.Assign(ref strContent, "live", "<span style=\"color:#FF6600;\">" + Resources.Lang.lblLive + "</span> ");
        TemplateHandler.Assign(ref strContent, "explanationtxt", Resources.Lang.txtexplainSingleMode);
        TemplateHandler.Assign(ref strContent, "fullversion", Resources.Lang.fullversion);
        TemplateHandler.Assign(ref strContent, "copyfromhdp", Resources.Lang.lblCopyFromHandicap);
        TemplateHandler.Assign(ref strContent, "othersetting", Resources.Lang.othersetting);
        TemplateHandler.Assign(ref strContent, "lblMaxPerBall", Resources.Lang.NumberGameMaxperball);
        if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission || !UserPermission.IsShowNumberGame)
        {
            TemplateHandler.Assign(ref strContent, "lblCommissionForAgent_number", string.Empty);
            TemplateHandler.Assign(ref strContent, "lblCommissionForMember_number", string.Empty);
            TemplateHandler.Assign(ref strContent, "numbergame", string.Empty);
        }
        else
        {
            TemplateHandler.Assign(ref strContent, "lblCommissionForAgent_number", Resources.Lang.lblCommissionForAgent);
            TemplateHandler.Assign(ref strContent, "lblCommissionForMember_number", Resources.Lang.lblCommissionForMember);
            TemplateHandler.Assign(ref strContent, "numbergame", Resources.Lang.numbergame);
        }
    }
}
