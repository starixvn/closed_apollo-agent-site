using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using DNAide.Core;
using System.Resources;
using System.Collections;
using DNAide.Web;
/// <summary>
/// Summary description for Common
/// </summary>
/// 

    public static partial class CommonGenerateHTML
    {
        public static void GenerateErrorMsg(string errMsg, ref string outputErrMsg)
        {
            if (!string.IsNullOrEmpty(errMsg))
            {
                errMsg = errMsg.Replace("\a", "^");
                if (errMsg.IndexOf("^") != -1)
                    errMsg = errMsg.Substring(0, errMsg.Length - 1);
                string strmsg = TemplateHandler.LoadTemplate("MemberInfo/EnrichMssg.html");
                string strTR = TemplateHandler.CopyBlock(strmsg, "limit-msg");
                string strTemp = "";
                string StrFinal = "";
                string[] ArrErrMsg = errMsg.Split('^');
                for (int i = 0; i < ArrErrMsg.Length; i++)
                {
                    strTemp = strTR;
                    TemplateHandler.Assign(ref strTemp, "myclass", "EnrichErrmsg");
                    TemplateHandler.Assign(ref strTemp, "mydata", ArrErrMsg[i]);
                    StrFinal += strTemp;
                }
                TemplateHandler.ReplaceBlock(ref strmsg, "limit-msg", StrFinal);
                outputErrMsg = strmsg;
            }
            else
            {
                outputErrMsg = string.Empty;
            }
        }
        public static void GenerateSuccMsg(string errCode, ref string outputErrMsg)
        {
            if (!string.IsNullOrEmpty(errCode) && errCode != "0")
            {
                errCode = errCode.Replace("\a", "^").Substring(0, errCode.Length - 1);
                string strmsg = TemplateHandler.LoadTemplate("MemberInfo/EnrichMssg.html");
                string strTR = TemplateHandler.CopyBlock(strmsg, "limit-msg");
                string strTemp = string.Empty;
                string StrFinal = string.Empty;
                string[] ArrErrMsg = errCode.Split('^');
                for (int i = 0; i < ArrErrMsg.Length; i++)
                {
                    if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency && (CastAide.AsInt32(ArrErrMsg[i]) == -109 || CastAide.AsInt32(ArrErrMsg[i]) == -112)) continue;
                    strTemp = strTR;
                    TemplateHandler.Assign(ref strTemp, "myclass", "EnrichSuccmsg");
                    TemplateHandler.Assign(ref strTemp, "mydata", ReadResourceValue(ArrErrMsg[i]));
                    StrFinal += strTemp;
                }
                TemplateHandler.ReplaceBlock(ref strmsg, "limit-msg", StrFinal);
                outputErrMsg = strmsg;
            }
            else
            {
                outputErrMsg = string.Empty;
            }
        }
        public static int GenerateErrorMsg(string errCode, string errMsg, ref string outputErrMsg,ref bool callAPI)
        {
            if (!string.IsNullOrEmpty(errCode) && !string.IsNullOrEmpty(errMsg) && errCode != "0")
            {
                if (errCode.Length > 1)
                {
                    errCode = errCode.Replace("\a", "^").Substring(0, errCode.Length - 1);
                    errMsg = errMsg.Replace("\a", "^").Substring(0, errMsg.Length - 1);
                }
                string strmsg = TemplateHandler.LoadTemplate("MemberInfo/EnrichMssg.html");
                string strTR = TemplateHandler.CopyBlock(strmsg, "limit-msg");
                string strTemp = "";
                string StrFinal = "";
                string[] ArrErrCode = errCode.Split('^');
                string[] ArrErrMsg = errMsg.Split('^');
                string strNumber = "";
                bool isErr = false;
                callAPI = false;
                for (int i = 0; i < ArrErrCode.Length; i++)
                {
                    if(!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency && (CastAide.AsInt32(ArrErrCode[i]) == -109 || CastAide.AsInt32(ArrErrCode[i]) == -112)) continue;
                    if (CastAide.AsInt32(ArrErrCode[i]) <= 0)
                    {
                        strNumber += ArrErrCode[i] + "^";
                        strTemp = strTR;
                        TemplateHandler.Assign(ref strTemp, "myclass", "EnrichSuccmsg");
                        TemplateHandler.Assign(ref strTemp, "mydata", ReadResourceValue(ArrErrCode[i]));
                        StrFinal += strTemp;
                        if (CastAide.AsInt32(ArrErrCode[i]) >= -103)
                            callAPI = true;
                    }
                    else if (CastAide.AsInt32(ArrErrCode[i]) > 0)
                    {
                        strTemp = strTR;
                        TemplateHandler.Assign(ref strTemp, "myclass", "EnrichErrmsg");
                        TemplateHandler.Assign(ref strTemp, "mydata", ArrErrMsg[i]);
                        StrFinal += strTemp;
                        isErr = true;
                    }
                }
                TemplateHandler.ReplaceBlock(ref strmsg, "limit-msg", StrFinal);
                outputErrMsg = strmsg;
                if (isErr) return 1;
                else return 0;
            }
            return 0;
        }
        public static int GenerateErrorMsg(string errCode, string errMsg, ref string outputErrMsg, string userName, ref bool callAPI)
        {
            if (!string.IsNullOrEmpty(errCode) && !string.IsNullOrEmpty(errMsg) && errCode != "0")
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    errCode = errCode.Replace("\a", "^").Substring(0, errCode.Replace("\a", "^").Length - 1);
                    errMsg = errMsg.Replace("\a", "^").Substring(0, errMsg.Replace("\a", "^").Length - 1);
                    userName = userName.Substring(0, userName.Length - 1);
                    string strmsg = TemplateHandler.LoadTemplate("MemberInfo/EnrichMssg.html");
                    string strTR = TemplateHandler.CopyBlock(strmsg, "limit-msg");
                    string strTemp = "";
                    string StrFinal = "";

                    string[] ArrErrCode = errCode.Split('$');
                    string[] ArrErrMsg = errMsg.Split('$');
                    string[] ArrUsername = userName.Split('^'); 
                    bool isErr = false;
                    callAPI = false;
                    for (int i = 0; i < ArrErrCode.Length; i++)
                    {
                        string[] Arrcode = ArrErrCode[i].Split('^');
                        string[] Arrmsg = ArrErrMsg[i].Split('^');
                        string _username = ArrUsername[i];
                        for (int j = 0; j < Arrcode.Length; j++)
                        {
                            if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency && (CastAide.AsInt32(Arrcode[j]) == -109 || CastAide.AsInt32(Arrcode[j]) == -112)) continue;
                            if (CastAide.AsInt32(Arrcode[j]) > 0)
                            {
                                if (j == 0)
                                {
                                    strTemp = strTR;
                                    TemplateHandler.Assign(ref strTemp, "myclass", "EnrichUserName");
                                    TemplateHandler.Assign(ref strTemp, "mydata", _username);
                                    StrFinal += strTemp;
                                }
                                strTemp = strTR;
                                TemplateHandler.Assign(ref strTemp, "myclass", "EnrichErrmsg");
                                TemplateHandler.Assign(ref strTemp, "mydata", Arrmsg[j]);
                                StrFinal += strTemp;
                                isErr = true;
                            }
                            else
                            {
                                if (j == 0)
                                {
                                    strTemp = strTR;
                                    TemplateHandler.Assign(ref strTemp, "myclass", "EnrichUserName");
                                    TemplateHandler.Assign(ref strTemp, "mydata", _username);
                                    StrFinal += strTemp;
                                }
                                strTemp = strTR;
                                TemplateHandler.Assign(ref strTemp, "myclass", "EnrichSuccmsg");
                                TemplateHandler.Assign(ref strTemp, "mydata", ReadResourceValue(Arrcode[j]));
                                StrFinal += strTemp;
                                if (CastAide.AsInt32(ArrErrCode[i]) >= -103)
                                    callAPI = true;
                            }
                        }
                    }
                    TemplateHandler.ReplaceBlock(ref strmsg, "limit-msg", StrFinal);
                    outputErrMsg = strmsg;
                    if (isErr) return 1;
                    else return 0;
                }
                else
                {
                    return GenerateErrorMsg(errCode, errMsg, ref outputErrMsg, ref callAPI); 
                }
            }
            return 0;
        }
        private static string ReadResourceValue(string key)
        {
            
            string resourceValue = string.Empty;
            string file = string.Empty;
            switch (Sunplus.Agent.Model.Entities.UserSession.Current.Language)
            {
                case "en-US": file = "LangMsg.resx"; break;
                case "ja-JP": file = "LangMsg.ja-JP.resx"; break;
                case "zh-TW": file = "LangMsg.zh-TW.resx"; break;
                case "zh-CN": file = "LangMsg.zh-CN.resx"; break;
                case "th-TH": file = "LangMsg.th-TH.resx"; break;
                case "ko-KR": file = "LangMsg.ko-KR.resx"; break;
                default: file = "LangMsg.resx"; break;
            }
            try
            {           
                        
                string myfile = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\App_GlobalResources\\" + file;
                if (File.Exists(myfile))
                {
                    ResXResourceReader resReader = new ResXResourceReader(myfile);
                    foreach (DictionaryEntry die in resReader)
                    {
                        if (die.Key.ToString().ToLower() == "succ_" + (CastAide.AsInt32(key) * -1).ToString())
                        {
                            resourceValue = die.Value.ToString();
                            break;
                        }
                    }
                }
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                resourceValue = string.Empty;
            }
            return resourceValue;
        }       
    }

