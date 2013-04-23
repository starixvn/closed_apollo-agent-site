using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Fanex.Data.DNAide;
using DNAide.Web;
using DNAide.Core;
using System.Text;
using System.Collections;

/// <summary>
/// Summary description for PositionTaking
/// </summary>
public class PositionTaking
{   
    public static string GetListPositionTaking(double minVal, double maxVal, double CurrVal)
    {
        string content = string.Empty;
        StringBuilder strOption = new StringBuilder();
        try
        {
            bool isInRange = false;
            int intToValue = CastAide.AsInt32(maxVal * 100, 0);
            int intFromValue = CastAide.AsInt32(minVal * 100, 0);
            while (intToValue >= intFromValue)
            {
                double ptValue = CastAide.AsDouble(intToValue, 0) / 100;
                if (ptValue == CurrVal)
                {
                    isInRange = true;
                    strOption.Append("<option selected='true' value='" + ptValue.ToString() + "'>" + ptValue.ToString() + "</option>");
                }
                else
                    strOption.Append("<option value='" + ptValue.ToString() + "'>" + ptValue.ToString() + "</option>");
                intToValue--;
            }
            if (!isInRange)
            {
                content = "<option selected='true' value='" + CurrVal.ToString() + "'>" + CurrVal.ToString() + "</option>";
                //strOption.Append("<option selected='true' value='" + CurrVal.ToString() + "'>" + CurrVal.ToString() + "</option>");
            }
            content += strOption.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return content;
    }
    public static string GetListPositionTaking(double minVal, double maxVal, double CurrVal,int interval)
    {
        string content = string.Empty;
        StringBuilder strOption = new StringBuilder();
        try
        {
            bool isInRange = false;
            int intToValue = CastAide.AsInt32(maxVal * interval, 0);
            int intFromValue = CastAide.AsInt32(minVal * interval, 0);
            while (intToValue >= intFromValue)
            {
                double ptValue = CastAide.AsDouble(intToValue, 0) / interval;
                if (ptValue == CurrVal)
                {
                    isInRange = true;
                    strOption.Append("<option selected='true' value='" + ptValue.ToString() + "'>" + ptValue.ToString() + "</option>");
                }
                else
                    strOption.Append("<option value='" + ptValue.ToString() + "'>" + ptValue.ToString() + "</option>");
                intToValue--;
            }
            if (!isInRange)
            {
                content = "<option selected='true' value='" + CurrVal.ToString() + "'>" + CurrVal.ToString() + "</option>";
                //strOption.Append("<option selected='true' value='" + CurrVal.ToString() + "'>" + CurrVal.ToString() + "</option>");
            }
            content += strOption.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return content;
    }   
    public static string GetListGroup(double maxVal, double CurrVal)
    {
        StringBuilder strOption = new StringBuilder();
        try
        {
            int groupValue = CastAide.AsInt32(maxVal * 10000, 0);
            if (CurrVal > maxVal)
                strOption.Append("<option>" + CurrVal.ToString() + "</option>");
            while (groupValue >= 0)
            {
                double setValue = CastAide.AsDouble(groupValue, 0) / 10000;
                if (setValue == CurrVal)
                    strOption.Append("<option selected='true'>" + setValue.ToString() + "</option>");
                else
                    strOption.Append("<option>" + setValue.ToString() + "</option>");
                groupValue = groupValue - 5;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return strOption.ToString();
    }

    public static string GetListGroup(double maxVal, double CurrVal,int interval)
    {
        StringBuilder strOption = new StringBuilder();
        try
        {
            int groupValue = CastAide.AsInt32(maxVal * 10000, 0);
            if (CurrVal > maxVal)
                strOption.Append("<option>" + CurrVal.ToString() + "</option>");
            while (groupValue >= 0)
            {
                double setValue = CastAide.AsDouble(groupValue, 0) / 10000;
                if (setValue == CurrVal)
                    strOption.Append("<option selected='true'>" + setValue.ToString() + "</option>");
                else
                    strOption.Append("<option>" + setValue.ToString() + "</option>");
                groupValue = groupValue - interval;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return strOption.ToString();
    }
   
    public static string AssignMulipleHidden(Hashtable htb)
    {
        StringBuilder StrHTML = new StringBuilder();
        try
        {
            string hiddenTpl = TemplateHandler.LoadTemplate("MemberInfo/HiddenTpl.html");            
            IDictionaryEnumerator enumerator = htb.GetEnumerator();
            string tpl = "";
            while (enumerator.MoveNext())
            {
                tpl = hiddenTpl;
                TemplateHandler.Assign(ref tpl, "hid-id", enumerator.Key);
                TemplateHandler.Assign(ref tpl, "hid-val", enumerator.Value);
                StrHTML.Append(tpl); 
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return StrHTML.ToString();
    }

    public static string ReplaceData(string str)
    {
        str = HttpUtility.HtmlDecode(str);  
        return str.Replace("\"", "&quot;").Replace("'", "&#39;").Replace("\n", "");
    }
    
    public static string GenerateTemplPT(string SportTypeId, string[] ArrLevel, string[] ArrTitleLevel, string[] ArrBettype, string[] ArrTitleBettype,string[] ArrWidth)
    {
        string templMain = TemplateHandler.LoadTemplate("MemberInfo/CommonPT.html");
        int j = 0;        

        StringBuilder tdResult = new StringBuilder();
        StringBuilder trResult = new StringBuilder();
        foreach (string lv in ArrLevel)
        {
            int l = 0;
            int i = 0;            
            string tr = TemplateHandler.CopyBlock(templMain, "Mytr");
            if (lv == "")
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                    else
                        TemplateHandler.Assign(ref td, "value", ArrTitleBettype[i]);
                    TemplateHandler.Assign(ref td, "clss-text", string.Empty);
                    TemplateHandler.Assign(ref td, "width", ArrWidth[i]);
                    //
                    tdResult.Append(td);
                    i++;
                }
                string trEmpty = tr;
                TemplateHandler.Assign(ref trEmpty, "td-text", tdResult.ToString());
                trResult.Append(trEmpty);
            }
            else if (lv == CommonPT.CopyFromOU || lv == CommonPT.CopyFromHdp || lv == CommonPT.CopyFromWin || lv == CommonPT.CopyPreset )
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                    {
                        TemplateHandler.Assign(ref td, "value", string.Empty );
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        tdResult.Append(td);
                    }
                    else
                    {
                        td = TemplateHandler.CopyBlock(templMain, "Mytd-colspan");
                        string strCopy = TemplateHandler.CopyBlock(templMain, "Myinput-copy");
                        TemplateHandler.Assign(ref td, "colnum", "3");
                        if (lv == CommonPT.CopyPreset)
                        {                            
                            TemplateHandler.Assign(ref td, "value", "{ChkCopyPreset}" + " " + ArrTitleLevel[j + 2]);
                            TemplateHandler.Assign(ref td, "clss-text", "CopyPreset");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopy}" + " " + ArrTitleLevel[j + 1]);
                            TemplateHandler.Assign(ref td, "clss-text", "l");
                        }
                        
                        tdResult.Append(td);
                        break;
                    }

                }

                string trCopy = tr;
                TemplateHandler.Assign(ref trCopy, "td-text", tdResult.ToString());
                trResult.Append(trCopy);
            }
            else
            {
                tdResult = new StringBuilder();
                int w = 0;
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            td = TemplateHandler.CopyBlock(templMain, "Mytd-rowspan");
                            TemplateHandler.Assign(ref td, "rownum", "2");
                        }
                        TemplateHandler.Assign(ref td, "value", ArrTitleLevel[j + 1]);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else if (bettype == null)
                    {
                        TemplateHandler.Assign(ref td, "value", "&nbsp;");
                        TemplateHandler.Assign(ref td, "clss-text", "");
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            TemplateHandler.Assign(ref td, "value", "{Chk-Auto-" + SportTypeId.ToString() + bettype + "}");
                        }
                        else
                        {
                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-S-Control}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-M-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-A-Control}");
                                }

                            }
                            else
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }

                        }
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    tdResult.Append(td);
                    w++;
                }
                string trNormal = tr;
                TemplateHandler.Assign(ref trNormal, "td-text", tdResult.ToString());
                trResult.Append(trNormal);
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {

                    tdResult = new StringBuilder();
                    foreach (string bettype in ArrBettype)
                    {
                        if (bettype != "")
                        {
                            string td = TemplateHandler.CopyBlock(templMain, "Mytd");

                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_AutoSuper )
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                            }
                            else if (bettype == null)
                            {
                                TemplateHandler.Assign(ref td, "value", "&nbsp;");
                                TemplateHandler.Assign(ref td, "clss-text", "");
                            }
                            else
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }                            
                            

                            tdResult.Append(td);                            
                            l++;
                        }

                    }
                    string trAuto = tr;
                    TemplateHandler.Assign(ref trAuto, "td-text", tdResult.ToString());
                    trResult.Append(trAuto);
                }
                j++;
            }
        }
        string temp = TemplateHandler.CopyBlock(templMain, "MyTemplMain");
        TemplateHandler.ReplaceBlock(ref temp, "text", trResult.ToString());
        TemplateHandler.Assign(ref temp, "title-col", ArrLevel.Length.ToString());        
        return temp;
    }
    public static string GenerateTemplPT(string SportTypeId, string[] ArrLevel, string[] ArrTitleLevel, string[] ArrBettype, string[] ArrTitleBettype,string[] ArrWidth, bool hideAuto)
    {
        string templMain = TemplateHandler.LoadTemplate("MemberInfo/CommonPT.html");
        int j = 0;

        StringBuilder tdResult = new StringBuilder();
        StringBuilder trResult = new StringBuilder();
        foreach (string lv in ArrLevel)
        {
            int l = 0;
            int i = 0;
            string tr = TemplateHandler.CopyBlock(templMain, "Mytr");
            if (lv == "")
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                    else
                        TemplateHandler.Assign(ref td, "value", ArrTitleBettype[i]);
                    TemplateHandler.Assign(ref td, "clss-text", string.Empty);                    
                    TemplateHandler.Assign(ref td, "width", ArrWidth[i]);
                    //
                    tdResult.Append(td);
                    i++;
                }
                string trEmpty = tr;
                TemplateHandler.Assign(ref trEmpty, "td-text", tdResult.ToString());
                trResult.Append(trEmpty);
            }
            else if (lv == CommonPT.CopyFromOU || lv == CommonPT.CopyFromHdp || lv == CommonPT.CopyFromWin || lv == CommonPT.CopyPreset)
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                    {
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        tdResult.Append(td);
                    }
                    else
                    {
                        td = TemplateHandler.CopyBlock(templMain, "Mytd-colspan");
                        string strCopy = TemplateHandler.CopyBlock(templMain, "Myinput-copy");
                        TemplateHandler.Assign(ref td, "colnum", "3");
                        if (lv == CommonPT.CopyPreset)
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopyPreset}" + " " + ArrTitleLevel[j + 2]);
                            TemplateHandler.Assign(ref td, "clss-text", "CopyPreset");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopy}" + " " + ArrTitleLevel[j + 1]);
                            TemplateHandler.Assign(ref td, "clss-text", "l");
                        }
                        
                        tdResult.Append(td);
                        break;
                    }

                }

                string trCopy = tr;
                TemplateHandler.Assign(ref trCopy, "td-text", tdResult.ToString());
                trResult.Append(trCopy);
            }
            else
            {
                tdResult = new StringBuilder();
                int w = 0;
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            td = TemplateHandler.CopyBlock(templMain, "Mytd-rowspan");
                            TemplateHandler.Assign(ref td, "rownum", "2");
                        }
                        TemplateHandler.Assign(ref td, "value", ArrTitleLevel[j + 1]);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else if (bettype == null)
                    {
                        TemplateHandler.Assign(ref td, "value", "&nbsp;");
                        TemplateHandler.Assign(ref td, "clss-text", "");
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            TemplateHandler.Assign(ref td, "value", "{Chk-Auto-" + SportTypeId.ToString() + bettype + "}");
                        }
                        else
                        {
                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-S-Control}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-M-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-A-Control}");
                                }

                            }
                            else
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }

                        }
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }

                    tdResult.Append(td);
                    l++;
                }
                string trNormal = tr;
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {
                    if (hideAuto)
                        TemplateHandler.Assign(ref trNormal, "clssTr-text", "hidden");
                }
                TemplateHandler.Assign(ref trNormal, "td-text", tdResult.ToString());
                trResult.Append(trNormal);
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {

                    tdResult = new StringBuilder();
                    foreach (string bettype in ArrBettype)
                    {
                        if (bettype != "")
                        {
                            string td = TemplateHandler.CopyBlock(templMain, "Mytd");

                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                            }
                            else if (bettype == null)
                            {
                                TemplateHandler.Assign(ref td, "value", "&nbsp;");
                                TemplateHandler.Assign(ref td, "clss-text", "");
                            }
                            else
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }


                            tdResult.Append(td);
                            l++;
                        }

                    }
                    string trAuto = tr;
                    if(hideAuto)
                        TemplateHandler.Assign(ref trAuto, "clssTr-text", "hidden");
                    TemplateHandler.Assign(ref trAuto, "td-text", tdResult.ToString());
                    trResult.Append(trAuto);
                }
                j++;
            }
        }
        string temp = TemplateHandler.CopyBlock(templMain, "MyTemplMain");
        TemplateHandler.ReplaceBlock(ref temp, "text", trResult.ToString());
        TemplateHandler.Assign(ref temp, "title-col", ArrLevel.Length.ToString());
        //TemplateHandler.Assign(ref temp, "pt-text", Resources.Lang.positiontaking);
        return temp;
    }
    public static string GenerateTemplPT_HR(string SportTypeId, string[] ArrLevel, string[] ArrTitleLevel, string[] ArrBettype, string[] ArrTitleBettype, string[] ArrWidth, string TitleSportType,string suffix)
    {
        string templMain = TemplateHandler.LoadTemplate("MemberInfo/CommonPT.html");
        
        string minmaxbet = TemplateHandler.CopyBlock(templMain, "MinMaxBet");

        TemplateHandler.Assign(ref minmaxbet, "sporttype", suffix); 
        int j = 0;

        StringBuilder Result = new StringBuilder();
        StringBuilder tdResult = new StringBuilder();
        StringBuilder trResult = new StringBuilder();
        foreach (string lv in ArrLevel)
        {
            int l = 0;
            int i = 0;
            string tr = TemplateHandler.CopyBlock(templMain, "Mytr");
            if (lv == "")
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                    else
                        TemplateHandler.Assign(ref td, "value", ArrTitleBettype[i]);
                    TemplateHandler.Assign(ref td, "clss-text", string.Empty);
                    TemplateHandler.Assign(ref td, "width", ArrWidth[i]);
                    //
                    tdResult.Append(td);
                    i++;
                }
                string trEmpty = tr;
                TemplateHandler.Assign(ref trEmpty, "td-text", tdResult.ToString());
                trResult.Append(trEmpty);
            }
            else if (lv == CommonPT.CopyFromOU || lv == CommonPT.CopyFromHdp || lv == CommonPT.CopyFromWin || lv == CommonPT.CopyPreset)
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                    {
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        tdResult.Append(td);
                    }
                    else
                    {
                        td = TemplateHandler.CopyBlock(templMain, "Mytd-colspan");
                        string strCopy = TemplateHandler.CopyBlock(templMain, "Myinput-copy");
                        TemplateHandler.Assign(ref td, "colnum", "3");
                        if (lv == CommonPT.CopyPreset)
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopyPreset}" + " " + ArrTitleLevel[j + 2]);
                            TemplateHandler.Assign(ref td, "clss-text", "CopyPreset");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopy}" + " " + ArrTitleLevel[j + 1]);
                            TemplateHandler.Assign(ref td, "clss-text", "l");
                        }

                        tdResult.Append(td);
                        break;
                    }

                }

                string trCopy = tr;
                TemplateHandler.Assign(ref trCopy, "td-text", tdResult.ToString());
                trResult.Append(trCopy);
            }
            else
            {
                tdResult = new StringBuilder();
                int w = 0;
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    
                    if (bettype == "")
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            td = TemplateHandler.CopyBlock(templMain, "Mytd-rowspan");
                            TemplateHandler.Assign(ref td, "rownum", "2");
                        }
                        TemplateHandler.Assign(ref td, "value", ArrTitleLevel[j + 1]);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else if (bettype == null)
                    {
                        TemplateHandler.Assign(ref td, "value", "&nbsp;");
                        TemplateHandler.Assign(ref td, "clss-text", "");
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            TemplateHandler.Assign(ref td, "value", "{Chk-Auto-" + SportTypeId.ToString() + bettype + "}");
                        }
                        else
                        {
                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-S-Control}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-M-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-A-Control}");
                                }

                            }
                            else
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }

                        }
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    tdResult.Append(td);
                    w++;
                }
                string trNormal = tr;
                TemplateHandler.Assign(ref trNormal, "td-text", tdResult.ToString());
                trResult.Append(trNormal);
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {

                    tdResult = new StringBuilder();
                    foreach (string bettype in ArrBettype)
                    {
                        if (bettype != "")
                        {
                            string td = TemplateHandler.CopyBlock(templMain, "Mytd");

                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                            }
                            else if (bettype == null)
                            {
                                TemplateHandler.Assign(ref td, "value", "&nbsp;");
                                TemplateHandler.Assign(ref td, "clss-text", "");
                            }
                            else
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }


                            tdResult.Append(td);
                            l++;
                        }

                    }
                    string trAuto = tr;
                    TemplateHandler.Assign(ref trAuto, "td-text", tdResult.ToString());
                    trResult.Append(trAuto);
                }
                j++;
            }
        }               

        string div = TemplateHandler.CopyBlock(templMain, "DivTitle");
        TemplateHandler.Assign(ref div, "sporttype", TitleSportType);        

        string divInfo = TemplateHandler.CopyBlock(templMain, "MyDiv");
        TemplateHandler.Assign(ref divInfo, "pt-text", minmaxbet);
        TemplateHandler.Assign(ref divInfo, "divId", "divUpdInfo" + suffix);
        TemplateHandler.Assign(ref divInfo, "divclss", string.Empty);

        string temp = TemplateHandler.CopyBlock(templMain, "MyTemplMain");
        TemplateHandler.ReplaceBlock(ref temp, "text", trResult.ToString());
        TemplateHandler.Assign(ref temp, "title-col", ArrLevel.Length.ToString());

        string divPT = TemplateHandler.CopyBlock(templMain, "MyDiv");
        TemplateHandler.Assign(ref divPT, "pt-text", temp);
        TemplateHandler.Assign(ref divPT, "divId", "divUpdPT" + suffix);
        TemplateHandler.Assign(ref divPT, "divclss", string.Empty);
        
        string chkdisable = TemplateHandler.CopyBlock(templMain, "CheckboxDisableHR");
        TemplateHandler.Assign(ref chkdisable, "sporttype", suffix); 
        string divDisable = TemplateHandler.CopyBlock(templMain, "MyDiv");
        TemplateHandler.Assign(ref divDisable, "pt-text", chkdisable);
        TemplateHandler.Assign(ref divDisable, "divId", "divUpdDisable" + suffix);
        TemplateHandler.Assign(ref divDisable, "divclss", string.Empty);        

        string tableMain = TemplateHandler.CopyBlock(templMain, "TablePT");
        TemplateHandler.Assign(ref tableMain, "MinMaxBet", divInfo);
        TemplateHandler.Assign(ref tableMain, "Pt", divPT);
        TemplateHandler.Assign(ref tableMain, "Status", divDisable);
       

        Result.Append(div);
        Result.Append("<br>");    
        Result.Append(tableMain);
        Result.Append("<br>");
        return Result.ToString();
    }
    public static string GenerateTemplPT_HR(string SportTypeId, string[] ArrLevel, string[] ArrTitleLevel, string[] ArrBettype, string[] ArrTitleBettype, string[] ArrWidth, string TitleSportType, string suffix, bool hideAuto)
    {
        string templMain = TemplateHandler.LoadTemplate("MemberInfo/CommonPT.html");

        string minmaxbet = TemplateHandler.CopyBlock(templMain, "MinMaxBet");

        TemplateHandler.Assign(ref minmaxbet, "sporttype", suffix);
        int j = 0;

        StringBuilder Result = new StringBuilder();
        StringBuilder tdResult = new StringBuilder();
        StringBuilder trResult = new StringBuilder();
        foreach (string lv in ArrLevel)
        {
            int l = 0;
            int i = 0;
            string tr = TemplateHandler.CopyBlock(templMain, "Mytr");
            if (lv == "")
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                    else
                        TemplateHandler.Assign(ref td, "value", ArrTitleBettype[i]);
                    TemplateHandler.Assign(ref td, "clss-text", string.Empty);
                    TemplateHandler.Assign(ref td, "width", ArrWidth[i]);
                    //
                    tdResult.Append(td);
                    i++;
                }
                string trEmpty = tr;
                TemplateHandler.Assign(ref trEmpty, "td-text", tdResult.ToString());
                trResult.Append(trEmpty);
            }
            else if (lv == CommonPT.CopyFromOU || lv == CommonPT.CopyFromHdp || lv == CommonPT.CopyFromWin || lv == CommonPT.CopyPreset)
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                    {
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        tdResult.Append(td);
                    }
                    else
                    {
                        td = TemplateHandler.CopyBlock(templMain, "Mytd-colspan");
                        string strCopy = TemplateHandler.CopyBlock(templMain, "Myinput-copy");
                        TemplateHandler.Assign(ref td, "colnum", "3");
                        if (lv == CommonPT.CopyPreset)
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopyPreset}" + " " + ArrTitleLevel[j + 2]);
                            TemplateHandler.Assign(ref td, "clss-text", "CopyPreset");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopy}" + " " + ArrTitleLevel[j + 1]);
                            TemplateHandler.Assign(ref td, "clss-text", "l");
                        }

                        tdResult.Append(td);
                        break;
                    }

                }

                string trCopy = tr;
                TemplateHandler.Assign(ref trCopy, "td-text", tdResult.ToString());
                trResult.Append(trCopy);
            }
            else
            {
                tdResult = new StringBuilder();
                int w = 0;
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");

                    if (bettype == "")
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            td = TemplateHandler.CopyBlock(templMain, "Mytd-rowspan");
                            TemplateHandler.Assign(ref td, "rownum", "2");
                        }
                        TemplateHandler.Assign(ref td, "value", ArrTitleLevel[j + 1]);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else if (bettype == null)
                    {
                        TemplateHandler.Assign(ref td, "value", "&nbsp;");
                        TemplateHandler.Assign(ref td, "clss-text", "");
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            TemplateHandler.Assign(ref td, "value", "{Chk-Auto-" + SportTypeId.ToString() + bettype + "}");
                        }
                        else
                        {
                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-S-Control}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-M-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-A-Control}");
                                }

                            }
                            else
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }

                        }
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    tdResult.Append(td);
                    w++;
                }
                string trNormal = tr;
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {
                    if (hideAuto)
                        TemplateHandler.Assign(ref trNormal, "clssTr-text", "hidden");
                }
                TemplateHandler.Assign(ref trNormal, "td-text", tdResult.ToString());
                trResult.Append(trNormal);
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {

                    tdResult = new StringBuilder();
                    foreach (string bettype in ArrBettype)
                    {
                        if (bettype != "")
                        {
                            string td = TemplateHandler.CopyBlock(templMain, "Mytd");

                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                            }
                            else if (bettype == null)
                            {
                                TemplateHandler.Assign(ref td, "value", "&nbsp;");
                                TemplateHandler.Assign(ref td, "clss-text", "");
                            }
                            else
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }


                            tdResult.Append(td);
                            l++;
                        }

                    }
                    string trAuto = tr;
                    if (hideAuto)
                        TemplateHandler.Assign(ref trAuto, "clssTr-text", "hidden");
                    TemplateHandler.Assign(ref trAuto, "td-text", tdResult.ToString());
                    trResult.Append(trAuto);
                }
                j++;
            }
        }

        string div = TemplateHandler.CopyBlock(templMain, "DivTitle");
        TemplateHandler.Assign(ref div, "sporttype", TitleSportType);

        string divInfo = TemplateHandler.CopyBlock(templMain, "MyDiv");
        TemplateHandler.Assign(ref divInfo, "pt-text", minmaxbet);
        TemplateHandler.Assign(ref divInfo, "divId", "divUpdInfo" + suffix);
        TemplateHandler.Assign(ref divInfo, "divclss", string.Empty);

        string temp = TemplateHandler.CopyBlock(templMain, "MyTemplMain");
        TemplateHandler.ReplaceBlock(ref temp, "text", trResult.ToString());
        TemplateHandler.Assign(ref temp, "title-col", ArrLevel.Length.ToString());

        string divPT = TemplateHandler.CopyBlock(templMain, "MyDiv");
        TemplateHandler.Assign(ref divPT, "pt-text", temp);
        TemplateHandler.Assign(ref divPT, "divId", "divUpdPT" + suffix);
        TemplateHandler.Assign(ref divPT, "divclss", string.Empty);

        string chkdisable = TemplateHandler.CopyBlock(templMain, "CheckboxDisableHR");
        TemplateHandler.Assign(ref chkdisable, "sporttype", suffix);
        string divDisable = TemplateHandler.CopyBlock(templMain, "MyDiv");
        TemplateHandler.Assign(ref divDisable, "pt-text", chkdisable);
        TemplateHandler.Assign(ref divDisable, "divId", "divUpdDisable" + suffix);
        TemplateHandler.Assign(ref divDisable, "divclss", string.Empty);

     
        string tableMain = TemplateHandler.CopyBlock(templMain, "TablePT");
        TemplateHandler.Assign(ref tableMain, "MinMaxBet", divInfo);
        TemplateHandler.Assign(ref tableMain, "Pt", divPT);
        TemplateHandler.Assign(ref tableMain, "Status", divDisable);


        Result.Append(div);
        Result.Append("<br>");
        Result.Append(tableMain);
        Result.Append("<br>");
        return Result.ToString();
    }
    public static string GenerateTemplPT_HR(string SportTypeId, string[] ArrLevel, string[] ArrTitleLevel, string[] ArrBettype, string[] ArrTitleBettype, string[] ArrWidth, string TitleSportType, string suffix, bool hideAuto, bool ViewBetSetting)
    {
        string templMain = TemplateHandler.LoadTemplate("MemberInfo/CommonPT.html");

        string minmaxbet = (ViewBetSetting) ? TemplateHandler.CopyBlock(templMain, "ViewMinMaxBet") : TemplateHandler.CopyBlock(templMain, "MinMaxBet");

        TemplateHandler.Assign(ref minmaxbet, "sporttype", suffix);
        int j = 0;

        StringBuilder Result = new StringBuilder();
        StringBuilder tdResult = new StringBuilder();
        StringBuilder trResult = new StringBuilder();
        foreach (string lv in ArrLevel)
        {
            int l = 0;
            int i = 0;
            string tr = TemplateHandler.CopyBlock(templMain, "Mytr");
            if (lv == "")
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                    else
                        TemplateHandler.Assign(ref td, "value", ArrTitleBettype[i]);
                    TemplateHandler.Assign(ref td, "clss-text", string.Empty);
                    TemplateHandler.Assign(ref td, "width", ArrWidth[i]);
                    //
                    tdResult.Append(td);
                    i++;
                }
                string trEmpty = tr;
                TemplateHandler.Assign(ref trEmpty, "td-text", tdResult.ToString());
                trResult.Append(trEmpty);
            }
            else if (lv == CommonPT.CopyFromOU || lv == CommonPT.CopyFromHdp || lv == CommonPT.CopyFromWin || lv == CommonPT.CopyPreset)
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                    {
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        tdResult.Append(td);
                    }
                    else
                    {
                        td = TemplateHandler.CopyBlock(templMain, "Mytd-colspan");
                        string strCopy = TemplateHandler.CopyBlock(templMain, "Myinput-copy");
                        TemplateHandler.Assign(ref td, "colnum", "3");
                        if (lv == CommonPT.CopyPreset)
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopyPreset}" + " " + ArrTitleLevel[j + 2]);
                            TemplateHandler.Assign(ref td, "clss-text", "CopyPreset");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopy}" + " " + ArrTitleLevel[j + 1]);
                            TemplateHandler.Assign(ref td, "clss-text", "l");
                        }

                        tdResult.Append(td);
                        break;
                    }

                }

                string trCopy = tr;
                TemplateHandler.Assign(ref trCopy, "td-text", tdResult.ToString());
                trResult.Append(trCopy);
            }
            else
            {
                tdResult = new StringBuilder();
                int w = 0;
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");

                    if (bettype == "")
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            td = TemplateHandler.CopyBlock(templMain, "Mytd-rowspan");
                            TemplateHandler.Assign(ref td, "rownum", "2");
                        }
                        TemplateHandler.Assign(ref td, "value", ArrTitleLevel[j + 1]);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else if (bettype == null)
                    {
                        TemplateHandler.Assign(ref td, "value", "&nbsp;");
                        TemplateHandler.Assign(ref td, "clss-text", "");
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            TemplateHandler.Assign(ref td, "value", "{Chk-Auto-" + SportTypeId.ToString() + bettype + "}");
                        }
                        else
                        {
                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-S-Control}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-M-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-A-Control}");
                                }

                            }
                            else
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }

                        }
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    tdResult.Append(td);
                    w++;
                }
                string trNormal = tr;
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {
                    if (hideAuto)
                        TemplateHandler.Assign(ref trNormal, "clssTr-text", "hidden");
                }
                TemplateHandler.Assign(ref trNormal, "td-text", tdResult.ToString());
                trResult.Append(trNormal);
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {

                    tdResult = new StringBuilder();
                    foreach (string bettype in ArrBettype)
                    {
                        if (bettype != "")
                        {
                            string td = TemplateHandler.CopyBlock(templMain, "Mytd");

                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                            }
                            else if (bettype == null)
                            {
                                TemplateHandler.Assign(ref td, "value", "&nbsp;");
                                TemplateHandler.Assign(ref td, "clss-text", "");
                            }
                            else
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }


                            tdResult.Append(td);
                            l++;
                        }

                    }
                    string trAuto = tr;
                    if (hideAuto)
                        TemplateHandler.Assign(ref trAuto, "clssTr-text", "hidden");
                    TemplateHandler.Assign(ref trAuto, "td-text", tdResult.ToString());
                    trResult.Append(trAuto);
                }
                j++;
            }
        }

        string div = TemplateHandler.CopyBlock(templMain, "DivTitle");
        TemplateHandler.Assign(ref div, "sporttype", TitleSportType);

        string divInfo = (ViewBetSetting) ? TemplateHandler.CopyBlock(templMain, "MyDiv-2") : TemplateHandler.CopyBlock(templMain, "MyDiv-2");
        TemplateHandler.Assign(ref divInfo, "pt-text", minmaxbet);
        TemplateHandler.Assign(ref divInfo, "divId", "divUpdInfo" + suffix);
        TemplateHandler.Assign(ref divInfo, "divclss", string.Empty);

        string temp = TemplateHandler.CopyBlock(templMain, "MyTemplMain");
        TemplateHandler.ReplaceBlock(ref temp, "text", trResult.ToString());
        TemplateHandler.Assign(ref temp, "title-col", ArrLevel.Length.ToString());

        string divPT = (ViewBetSetting) ? TemplateHandler.CopyBlock(templMain, "MyDiv-2") : TemplateHandler.CopyBlock(templMain, "MyDiv-2");
        TemplateHandler.Assign(ref divPT, "pt-text", temp);
        TemplateHandler.Assign(ref divPT, "divId", "divUpdPT" + suffix);
        TemplateHandler.Assign(ref divPT, "divclss", string.Empty);

        string chkdisable = TemplateHandler.CopyBlock(templMain, "CheckboxDisableHR-NoneEdit");
        TemplateHandler.Assign(ref chkdisable, "sporttype", suffix);
        string divDisable = TemplateHandler.CopyBlock(templMain, "MyDiv-2");
        TemplateHandler.Assign(ref divDisable, "pt-text", chkdisable);
        TemplateHandler.Assign(ref divDisable, "divId", "divUpdDisable" + suffix);
        TemplateHandler.Assign(ref divDisable, "divclss", string.Empty); 

        string tableMain = TemplateHandler.CopyBlock(templMain, "TablePT");
        TemplateHandler.Assign(ref tableMain, "MinMaxBet", divInfo);
        TemplateHandler.Assign(ref tableMain, "Pt", divPT);
        TemplateHandler.Assign(ref tableMain, "Status", divDisable);
                
        Result.Append(div);
        Result.Append("<br>");
        Result.Append(tableMain);
        Result.Append("<br>");
        return Result.ToString();
    }
    public static string GenerateTemplPT_ViewHR(string SportTypeId, string[] ArrLevel, string[] ArrTitleLevel, string[] ArrBettype, string[] ArrTitleBettype, string[] ArrWidth, string TitleSportType, string suffix, bool hideAuto)
    {
        string templMain = TemplateHandler.LoadTemplate("MemberInfo/CommonPT.html");

        string minmaxbet = TemplateHandler.CopyBlock(templMain, "ViewMinMaxBet");

        TemplateHandler.Assign(ref minmaxbet, "sporttype", suffix);
        int j = 0;

        StringBuilder Result = new StringBuilder();
        StringBuilder tdResult = new StringBuilder();
        StringBuilder trResult = new StringBuilder();
        foreach (string lv in ArrLevel)
        {
            int l = 0;
            int i = 0;
            string tr = TemplateHandler.CopyBlock(templMain, "Mytr");
            if (lv == "")
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                    else
                        TemplateHandler.Assign(ref td, "value", ArrTitleBettype[i]);
                    TemplateHandler.Assign(ref td, "clss-text", string.Empty);
                    TemplateHandler.Assign(ref td, "width", ArrWidth[i]);
                    //
                    tdResult.Append(td);
                    i++;
                }
                string trEmpty = tr;
                TemplateHandler.Assign(ref trEmpty, "td-text", tdResult.ToString());
                trResult.Append(trEmpty);
            }
            else if (lv == CommonPT.CopyFromOU || lv == CommonPT.CopyFromHdp || lv == CommonPT.CopyFromWin || lv == CommonPT.CopyPreset)
            {
                tdResult = new StringBuilder();
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");
                    if (bettype == "")
                    {
                        TemplateHandler.Assign(ref td, "value", string.Empty);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        tdResult.Append(td);
                    }
                    else
                    {
                        td = TemplateHandler.CopyBlock(templMain, "Mytd-colspan");
                        string strCopy = TemplateHandler.CopyBlock(templMain, "Myinput-copy");
                        TemplateHandler.Assign(ref td, "colnum", "3");
                        if (lv == CommonPT.CopyPreset)
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopyPreset}" + " " + ArrTitleLevel[j + 2]);
                            TemplateHandler.Assign(ref td, "clss-text", "CopyPreset");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref td, "value", "{ChkCopy}" + " " + ArrTitleLevel[j + 1]);
                            TemplateHandler.Assign(ref td, "clss-text", "l");
                        }

                        tdResult.Append(td);
                        break;
                    }

                }

                string trCopy = tr;
                TemplateHandler.Assign(ref trCopy, "td-text", tdResult.ToString());
                trResult.Append(trCopy);
            }
            else
            {
                tdResult = new StringBuilder();
                int w = 0;
                foreach (string bettype in ArrBettype)
                {
                    string td = TemplateHandler.CopyBlock(templMain, "Mytd");

                    if (bettype == "")
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            td = TemplateHandler.CopyBlock(templMain, "Mytd-rowspan");
                            TemplateHandler.Assign(ref td, "rownum", "2");
                        }
                        TemplateHandler.Assign(ref td, "value", ArrTitleLevel[j + 1]);
                        TemplateHandler.Assign(ref td, "clss-text", "r");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else if (bettype == null)
                    {
                        TemplateHandler.Assign(ref td, "value", "&nbsp;");
                        TemplateHandler.Assign(ref td, "clss-text", "");
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    else
                    {
                        if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                        {
                            TemplateHandler.Assign(ref td, "value", "{Chk-Auto-" + SportTypeId.ToString() + bettype + "}");
                        }
                        else
                        {
                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-S-Control}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-M-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-A-Control}");
                                }

                            }
                            else
                            {
                                if (lv == CommonPT.PT_SuperPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_MasterPreset)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }

                        }
                        TemplateHandler.Assign(ref td, "clss-text", "c");
                        TemplateHandler.Assign(ref td, "width", ArrWidth[w]);
                    }
                    tdResult.Append(td);
                    w++;
                }
                string trNormal = tr;
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {
                    if (hideAuto)
                        TemplateHandler.Assign(ref trNormal, "clssTr-text", "hidden");
                }
                TemplateHandler.Assign(ref trNormal, "td-text", tdResult.ToString());
                trResult.Append(trNormal);
                if (lv == CommonPT.PT_AutoSuper || lv == CommonPT.PT_AutoMaster || lv == CommonPT.PT_AutoAgent)
                {

                    tdResult = new StringBuilder();
                    foreach (string bettype in ArrBettype)
                    {
                        if (bettype != "")
                        {
                            string td = TemplateHandler.CopyBlock(templMain, "Mytd");

                            if (bettype == CommonPT.OverUnder || bettype == CommonPT.Handicap || bettype == CommonPT.Win)
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}" + " {Btn-Auto-Control}");
                                }
                            }
                            else if (bettype == null)
                            {
                                TemplateHandler.Assign(ref td, "value", "&nbsp;");
                                TemplateHandler.Assign(ref td, "clss-text", "");
                            }
                            else
                            {
                                if (lv == CommonPT.PT_AutoSuper)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-S-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else if (lv == CommonPT.PT_AutoMaster)
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-M-" + SportTypeId.ToString() + bettype + "}");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref td, "value", "{Sel-Auto-A-" + SportTypeId.ToString() + bettype + "}");
                                }
                            }


                            tdResult.Append(td);
                            l++;
                        }

                    }
                    string trAuto = tr;
                    if (hideAuto)
                        TemplateHandler.Assign(ref trAuto, "clssTr-text", "hidden");
                    TemplateHandler.Assign(ref trAuto, "td-text", tdResult.ToString());
                    trResult.Append(trAuto);
                }
                j++;
            }
        }

        string div = TemplateHandler.CopyBlock(templMain, "DivTitle");
        TemplateHandler.Assign(ref div, "sporttype", TitleSportType);

        string divInfo = TemplateHandler.CopyBlock(templMain, "MyDiv-2");
        TemplateHandler.Assign(ref divInfo, "pt-text", minmaxbet);
        TemplateHandler.Assign(ref divInfo, "divId", "divUpdInfo" + suffix);
        TemplateHandler.Assign(ref divInfo, "divclss", string.Empty);

        string temp = TemplateHandler.CopyBlock(templMain, "MyTemplMain");
        TemplateHandler.ReplaceBlock(ref temp, "text", trResult.ToString());
        TemplateHandler.Assign(ref temp, "title-col", ArrLevel.Length.ToString());

        string divPT = TemplateHandler.CopyBlock(templMain, "MyDiv-2");
        TemplateHandler.Assign(ref divPT, "pt-text", temp);
        TemplateHandler.Assign(ref divPT, "divId", "divUpdPT" + suffix);
        TemplateHandler.Assign(ref divPT, "divclss", string.Empty);

        string chkdisable = TemplateHandler.CopyBlock(templMain, "CheckboxDisableHR");
        TemplateHandler.Assign(ref chkdisable, "sporttype", suffix);
        string divDisable = TemplateHandler.CopyBlock(templMain, "MyDiv-2");
        TemplateHandler.Assign(ref divDisable, "pt-text", chkdisable);
        TemplateHandler.Assign(ref divDisable, "divId", "divUpdDisable" + suffix);
        TemplateHandler.Assign(ref divDisable, "divclss", string.Empty); 

        string tableMain = TemplateHandler.CopyBlock(templMain, "TablePT");
        TemplateHandler.Assign(ref tableMain, "MinMaxBet", divInfo);
        TemplateHandler.Assign(ref tableMain, "Pt", divPT);
        TemplateHandler.Assign(ref tableMain, "Status", divDisable);


        Result.Append(div);
        Result.Append("<br>");
        Result.Append(tableMain);
        Result.Append("<br>");
        return Result.ToString();
    }
}
