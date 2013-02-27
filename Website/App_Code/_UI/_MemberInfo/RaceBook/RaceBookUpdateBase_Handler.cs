using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Fanex.Age.UI;
using Fanex.Age.UI.Components;
using Fanex.Age.UI.Components.PT_Control;
using Sunplus.Agent.Model.BL.Racing;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.UI._Components.RacingStatus_Control;
using Sunplus.Agent.UI._MemberInfo.RaceBook.MinMaxBet_Control;
using Sunplus.Agent.UI.PositionTaking.RaceBook;
using Sunplus.Agent.Utilities;

namespace Sunplus.Agent.UI._MemberInfo.RaceBook
{
    public abstract class RaceBookUpdateBase_Handler : TplHandler
    {
        protected string custId;
        protected bool isEdit = true;

        /// <summary>
        /// All parameter types in store procedures
        /// </summary>
        string[] parameters = new string[] {
            "v{0}discount_{1}",
            "v{0}spositiontaking_{1}",
            "v{0}mpositiontaking_{1}",
            "v{0}apositiontaking_{1}",
            "v{0}positiontaking_{1}_min",

            "auto_v{0}spositiontaking_{1}",
            "auto_v{0}mpositiontaking_{1}",
            "auto_v{0}mpositiontaking_mem_{1}",

            "isauto_v{0}spositiontaking_{1}",
            "isauto_v{0}smpositiontaking_{1}",
            "isauto_v{0}mpositiontaking_{1}",
            "isauto_v{0}mpositiontaking_mem_{1}",

            "iscopypreset{0}_{1}",
        };

        public RaceBookUpdateBase_Handler()
        {
        }

        protected Result _result = new Result(ErrorType.Success, null);

        public override void RenderResult(ref JSONData data)
        {
            data.AsResult(_result);
        }

        public override object ValidateRequest(Hashtable p)
        {
            bool.TryParse(Request["isedit"], out isEdit);

            if (CurrentUserSession.IsInternal)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            if (isEdit) // edit
            {
                if (!CurrentUserSession.SubPageAccess.UpdateAccount)
                {
                    return new Result(ErrorType.AccessDenied, Resources.Lang.lblconfirmclosesubacc);
                }
            }
            else
            {
                if (!CurrentUserSession.SubPageAccess.CreateAccount)
                {
                    return new Result(ErrorType.AccessDenied, Resources.Lang.lblconfirmclosesubacc);
                }
            }

            if (string.IsNullOrEmpty(custId))
            {
                return new Result(ErrorType.Unknown, "Invalid customer id!");
            }

            CheckAndCopyWhenDisabled(ref p);

            CheckAndCopyFromUK_Irish_Tote(ref p);

            CheckAndResetDefaultForAutoPT(ref p);

            CheckAndCopyFromWin(ref p);

            RemoveEmptyValues(ref p);

            return null;
        }

        private string RemoveMessage(string errMsg)
        {
            StringBuilder NewMsg = new StringBuilder();
            string[] detailMsg = Regex.Split(errMsg, "<br>");
            NewMsg.Append(detailMsg[0]);
            NewMsg.Append("<br>");
            for (int i = 1; i < detailMsg.Length; i++)
            {
                if (UserPermission.IsShowRacing_151_31 && (detailMsg[i].IndexOf("Horse Racing - Win") != -1 || detailMsg[i].IndexOf("Horse Racing - Place") != -1))
                {
                    NewMsg.Append(detailMsg[i]);
                    NewMsg.Append("<br>");
                }
                else if (UserPermission.IsShowRacing_151_41 && (detailMsg[i].IndexOf("Horse Racing - Win.UK Tote") != -1 || detailMsg[i].IndexOf("Horse Racing - Place.UK Tote") != -1))
                {
                    NewMsg.Append(detailMsg[i]);
                    NewMsg.Append("<br>");
                }
                else if (UserPermission.IsShowRacing_151_51 && (detailMsg[i].IndexOf("Horse Racing - Win.Aust Tote") != -1 || detailMsg[i].IndexOf("Horse Racing - Place.Aust Tote") != -1))
                {
                    NewMsg.Append(detailMsg[i]);
                    NewMsg.Append("<br>");
                }
                else if (UserPermission.IsShowRacing_152_41 && (detailMsg[i].IndexOf("Greyhound - Win.UK Tote") != -1 || detailMsg[i].IndexOf("Greyhound - Place.UK Tote") != -1))
                {
                    NewMsg.Append(detailMsg[i]);
                    NewMsg.Append("<br>");
                }
                else if (UserPermission.IsShowRacing_152_51 && (detailMsg[i].IndexOf("Greyhound - Win.Aust Tote") != -1 || detailMsg[i].IndexOf("Greyhound - Place.Aust Tote") != -1))
                {
                    NewMsg.Append(detailMsg[i]);
                    NewMsg.Append("<br>");
                }
                else if (UserPermission.IsShowRacing_153_51 && (detailMsg[i].IndexOf("Harness - Win.Aust Tote") != -1 || detailMsg[i].IndexOf("Harness - Place.Aust Tote") != -1))
                {
                    NewMsg.Append(detailMsg[i]);
                    NewMsg.Append("<br>");
                }
            }
            return NewMsg.ToString();
        }

        private bool IsRemoveMessage(int errCode)
        {
            if (errCode < 0)
            {
                switch (errCode)
                {
                    //HR SA Tote
                    case -109:
                    case -112:
                        {
                            if (!UserPermission.IsShowRacing_151_31)
                                return true;
                            else
                                return false;
                        }
                    //HR UK/Irish Tote
                    case -110:
                    case -111:
                        {
                            if (!UserPermission.IsShowRacing_151_41)
                                return true;
                            else
                                return false;
                        }
                    //HR AUST Tote
                    case -113:
                    case -117:
                        {
                            if (!UserPermission.IsShowRacing_151_51)
                                return true;
                            else
                                return false;
                        }
                    //GH UK/Irish Tote
                    case -114:
                    case -118:
                        {
                            if (!UserPermission.IsShowRacing_152_41)
                                return true;
                            else
                                return false;
                        }
                    //GH AUST Tote
                    case -115:
                    case -119:
                        {
                            if (!UserPermission.IsShowRacing_152_51)
                                return true;
                            else
                                return false;
                        }
                    //GH AUST Tote
                    case -116:
                    case -120:
                        {
                            if (!UserPermission.IsShowRacing_153_51)
                                return true;
                            else
                                return false;
                        }
                }
            }
            return false;
        }

        public void GetResourceMsg(ref string[] ArrCodes, ref string[] ArrMsgs)
        {
            List<string> Msgs = new List<string>();
            List<string> Codes = new List<string>();

            for (int i = 0; i < ArrCodes.Length; i++)
            {
                if (!string.IsNullOrEmpty(ArrCodes[i]))
                {
                    if (Convert.ToInt32(ArrCodes[i]) < 0)
                    {
                        if (!IsRemoveMessage(Convert.ToInt32(ArrCodes[i])))
                        {
                            Msgs.Add(ReadResourceValue(ArrCodes[i]));
                            Codes.Add(ArrCodes[i]);
                        }
                    }
                    else
                    {
                        Msgs.Add(RemoveMessage(ArrMsgs[i]));
                        Codes.Add(ArrCodes[i]);
                    }
                }
            }

            ArrCodes = Codes.ToArray();
            ArrMsgs = Msgs.ToArray();
        }

        public string ReadResourceValue(string key)
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
                        if (die.Key.ToString().ToLower() == "succ_" + (Convert.ToInt32(key) * -1).ToString())
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
                resourceValue = "";
            }
            return resourceValue;
        }

        public override void ProcessRequest(ref Hashtable p)
        {
            BetTypeEnables = new bool[] {
                UserPermission.IsShowRacing_151_41,
                UserPermission.IsShowRacing_151_31,
                UserPermission.IsShowRacing_151_51,
                UserPermission.IsShowRacing_152_41,
                UserPermission.IsShowRacing_152_51,
                UserPermission.IsShowRacing_153_51
            };

            custId = Request["custid"];

            p.Add("updatemultiple", Request["ismult"]);
            p.Add("custid", custId);
            p.Add("userid", CurrentUserSession.UserId);
            p.Add("subaccid", CurrentUserSession.SubAccId);
            p.Add("language", CurrentUserSession.LangKey);
            p.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());

            if (Request["ismult"] == "true")
            {
                bool isCopyAll = Convert.ToBoolean(Request["chkCopyAll"]);
                // Allow Update
                GetRequestParams(ref p, "ChkUpdateStatus_{0}_{1}", "updatedisable{0}_{1}", typeof(bool));
                GetRequestParams(ref p, "ChkUpdateCommission_{0}_{1}", "updatediscountv{0}_{1}", typeof(bool), isCopyAll);
                GetRequestParams(ref p, "chkupdatebetsetting_{0}_{1}", "updateminmaxbet{0}_{1}", typeof(bool));
                GetRequestParams(ref p, "ChkUpdate_{0}_{1}", "updatepositiontakingv{0}_{1}", typeof(bool), isCopyAll);
                GetOneRequestParam_Combination(ref p, "ChkUpdateCommission_{0}_{1}", "updatecommission", typeof(bool));
                GetOneRequestParam_Combination(ref p, "ChkUpdate_{0}_{1}", "updatepositiontaking", typeof(bool));
            }

            // Min Bet, Max Bet, Max Per Match
            GetRequestParams(ref p, "MinBet_{0}_{1}", "v{0}minbet_{1}", typeof(double));
            GetRequestParams(ref p, "MaxBet_{0}_{1}", "v{0}maxbet_{1}", typeof(double));
            GetRequestParams(ref p, "MaxPerMatch_{0}_{1}", "v{0}maxpermatch_{1}", typeof(double));

            // Disable status
            GetRequestBooleanWithRevert(ref p, "Disable{0}_{1}", "disable{0}_{1}");

            // Disable downline status
            GetRequestBooleanWithRevert(ref p, "DisableDownlines{0}_{1}", "disabledownlines{0}_{1}");

            // Commission
            GetRequestParams(ref p, "v{0}discount_{1}", "v{0}discount_{1}", typeof(decimal));

            #region Win

            GetRequestParams(ref p, "listSuper_{0}_{1}", "v{0}spositiontaking_{1}", typeof(double));
            GetRequestParams(ref p, "listAutoSuper_{0}_{1}", "auto_v{0}spositiontaking_{1}", typeof(double));
            GetRequestParams(ref p, "chkAutoSuper_{0}_{1}", "isauto_v{0}spositiontaking_{1}", typeof(bool));

            // Auto super on member
            GetRequestParams(ref p, "chkAutoSuper_Master_{0}_{1}", "isauto_v{0}smpositiontaking_{1}", typeof(bool));

            GetRequestParams(ref p, "listMaster_{0}_{1}", "v{0}mpositiontaking_{1}", typeof(double));
            GetRequestParams(ref p, "listAutoMaster_{0}_{1}", "auto_v{0}mpositiontaking_{1}", typeof(double));

            GetRequestParams(ref p, "chkAutoMaster_Agent_{0}_{1}", "isauto_v{0}mpositiontaking_{1}", typeof(bool));
            GetRequestParams(ref p, "chkAutoMaster_{0}_{1}", "isauto_v{0}mpositiontaking_{1}", typeof(bool));

            // Auto master on member
            GetRequestParams(ref p, "listAutoMaster_{0}_{1}", "auto_v{0}mpositiontaking_mem_{1}", typeof(double));

            GetRequestParams(ref p, "chkAutoMaster_{0}_{1}", "isauto_v{0}mpositiontaking_mem_{1}", typeof(bool));

            GetRequestParams(ref p, "chkApplyAllMember_{0}_{1}", "iscopypreset{0}_{1}", typeof(bool));
            GetRequestParams(ref p, "listAgent_{0}_{1}", "v{0}apositiontaking_{1}", typeof(double));
            GetRequestParams(ref p, "listMember_{0}_{1}", "v{0}apositiontaking_{1}", typeof(double));

            // Edit Min PT
            GetRequestParams(ref p, "listMinPT_{0}_{1}", "v{0}positiontaking_{1}_min", typeof(double));

            #endregion Win

            #region Place

            GetRequestParams2(ref p, "listSuper_{0}_{1}", "v{0}spositiontaking_{1}", typeof(double));
            GetRequestParams2(ref p, "listAutoSuper_{0}_{1}", "auto_v{0}spositiontaking_{1}", typeof(double));
            GetRequestParams2(ref p, "chkAutoSuper_{0}_{1}", "isauto_v{0}spositiontaking_{1}", typeof(bool));

            // Auto super on member

            GetRequestParams2(ref p, "chkAutoSuper_Master_{0}_{1}", "isauto_v{0}smpositiontaking_{1}", typeof(bool));

            GetRequestParams2(ref p, "listMaster_{0}_{1}", "v{0}mpositiontaking_{1}", typeof(double));
            GetRequestParams2(ref p, "listAutoMaster_{0}_{1}", "auto_v{0}mpositiontaking_{1}", typeof(double));

            GetRequestParams2(ref p, "chkAutoMaster_Agent_{0}_{1}", "isauto_v{0}mpositiontaking_{1}", typeof(bool));
            // Auto master on member
            GetRequestParams2(ref p, "listAutoMaster_{0}_{1}", "auto_v{0}mpositiontaking_mem_{1}", typeof(double));
            GetRequestParams2(ref p, "chkAutoMaster_{0}_{1}", "isauto_v{0}mpositiontaking_mem_{1}", typeof(bool));

            GetRequestParams2(ref p, "chkAutoMaster_{0}_{1}", "isauto_v{0}mpositiontaking_{1}", typeof(bool));
            GetRequestParams2(ref p, "chkApplyAllMember_{0}_{1}", "iscopypreset{0}_{1}", typeof(bool));
            GetRequestParams2(ref p, "listAgent_{0}_{1}", "v{0}apositiontaking_{1}", typeof(double));
            GetRequestParams2(ref p, "listMember_{0}_{1}", "v{0}apositiontaking_{1}", typeof(double));

            // Edit Min PT
            GetRequestParams2(ref p, "listMinPT_{0}_{1}", "v{0}positiontaking_{1}_min", typeof(double));

            #endregion Place

            // New PT for Member
            GetOneRequestParam(ref p, "memberPT", "newspositiontaking", typeof(double));
            GetOneRequestParam(ref p, "isUpdateMember", "updateallmembers", typeof(bool));
        }

        protected void RemoveEmptyValues(ref Hashtable p)
        {
            string[] keys = new string[p.Keys.Count];
            p.Keys.CopyTo(keys, 0);
            // Remove null values
            foreach (object key in keys)
            {
                if (p[key] == null)
                {
                    p.Remove(key);
                }
            }
        }

        #region CheckAndCopy

        protected void CheckAndCopyFromWin(ref Hashtable p)
        {
            //chkCopyFromWin_151_41
            string chkCopyFromWin = "chkCopyFromWin_{0}_{1}";
            int c = SportIds.Length;

            for (int i = 0; i < c; i++)
            {
                string name = string.Format(chkCopyFromWin, SportIds[i], BetTypeIds[i]);
                string value = Request[name];

                // Copy from Win to Place

                if (value == "true")
                {
                    int d = parameters.Length;
                    for (int j = 0; j < d; j++)
                    {
                        p[string.Format(parameters[j], SportIds[i], BetTypeIds2[i])] =
                            p[string.Format(parameters[j], SportIds[i], BetTypeIds[i])];
                    }
                }
            }
        }

        /// <summary>
        /// Copy all button was checked
        /// </summary>
        /// <param name="p"></param>
        protected void CheckAndCopyFromUK_Irish_Tote(ref Hashtable p)
        {
            // Copy all Commission and Position Taking from UK/Irish Tote - Win
            if (Request["chkCopyAll"] == "true")
            {
                int c = SportIds.Length;

                for (int i = 1; i < c; i++)
                {
                    int d = parameters.Length;
                    for (int j = 0; j < d; j++)
                    {
                        p[string.Format(parameters[j], SportIds[i], BetTypeIds[i])] =
                            p[string.Format(parameters[j], SportIds[i], BetTypeIds2[i])] =
                            p[string.Format(parameters[j], SportIds[0], BetTypeIds2[0])] =
                            p[string.Format(parameters[j], SportIds[0], BetTypeIds[0])];
                    }
                }
            }
        }

        protected void CheckAndCopyWhenDisabled(ref Hashtable p)
        {
            int c = SportIds.Length;

            for (int i = 1; i < c; i++)
            {
                if (BetTypeIds[i] == 31 || BetTypeIds2[i] == 32)
                {
                    continue;
                }

                if (!BetTypeEnables[i])
                {
                    p[string.Format("updateminmaxbet{0}_{1}", SportIds[i], BetTypeIds[i])] =
                        p[string.Format("updateminmaxbet{0}_{1}", SportIds[i], BetTypeIds2[i])] =
                        true;
                }
                if (BetTypeEnables[i]) continue;

                //// Status
                p[string.Format("disable{0}_{1}", SportIds[i], BetTypeIds[i])] =
                    p[string.Format("disable{0}_{1}", SportIds[0], BetTypeIds[0])];

                p[string.Format("disabledownlines{0}_{1}", SportIds[i], BetTypeIds[i])] =
                    p[string.Format("disabledownlines{0}_{1}", SportIds[0], BetTypeIds[0])];

                p[string.Format("updatedisable{0}_{1}", SportIds[i], BetTypeIds[i])] =
                    p[string.Format("updatedisable{0}_{1}", SportIds[0], BetTypeIds[0])];

                int d = parameters.Length;
                for (int j = 0; j < d; j++)
                {
                    p[string.Format(parameters[j], SportIds[i], BetTypeIds[i])] =
                        p[string.Format(parameters[j], SportIds[0], BetTypeIds[0])];

                    p[string.Format(parameters[j], SportIds[i], BetTypeIds2[i])] =
                        p[string.Format(parameters[j], SportIds[0], BetTypeIds2[0])];
                }
            }
        }

        protected void CheckAndResetDefaultForAutoPT(ref Hashtable p)
        {
            int c = SportIds.Length;
            for (int i = 0; i < c; i++)
            {
                // Super Auto Win
                if (false.Equals(p[string.Format("isauto_v{0}spositiontaking_{1}", SportIds[i], BetTypeIds[i])]))
                {
                    p[string.Format("auto_v{0}spositiontaking_{1}", SportIds[i], BetTypeIds[i])] = 0;
                }
                else if (true.Equals(p[string.Format("isauto_v{0}spositiontaking_{1}", SportIds[i], BetTypeIds[i])]))
                {
                    p[string.Format("v{0}spositiontaking_{1}", SportIds[i], BetTypeIds[i])] = 0;
                }

                // Super Auto Place
                if (false.Equals(p[string.Format("isauto_v{0}spositiontaking_{1}", SportIds[i], BetTypeIds2[i])]))
                {
                    p[string.Format("auto_v{0}spositiontaking_{1}", SportIds[i], BetTypeIds2[i])] = 0;
                }
                else if (true.Equals(p[string.Format("isauto_v{0}spositiontaking_{1}", SportIds[i], BetTypeIds2[i])]))
                {
                    p[string.Format("v{0}spositiontaking_{1}", SportIds[i], BetTypeIds2[i])] = 0;
                }

                // Master Auto Win
                if (false.Equals(p[string.Format("isauto_v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds[i])]))
                {
                    p[string.Format("auto_v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds[i])] = 0;
                }
                else if (true.Equals(p[string.Format("isauto_v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds[i])]))
                {
                    p[string.Format("v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds[i])] = 0;
                }

                // Master Auto Place
                if (false.Equals(p[string.Format("isauto_v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds2[i])]))
                {
                    p[string.Format("auto_v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds2[i])] = 0;
                }
                else if (true.Equals(p[string.Format("isauto_v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds2[i])]))
                {
                    p[string.Format("v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds2[i])] = 0;
                }

                // Master Auto Mem Win
                if (false.Equals(p[string.Format("isauto_v{0}mpositiontaking_mem_{1}", SportIds[i], BetTypeIds[i])]))
                {
                    p[string.Format("auto_v{0}mpositiontaking_mem_{1}", SportIds[i], BetTypeIds[i])] = 0;
                }
                else if (true.Equals(p[string.Format("isauto_v{0}mpositiontaking_mem_{1}", SportIds[i], BetTypeIds[i])]))
                {
                    p[string.Format("v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds[i])] = 0;
                }

                // Master Auto Mem Place
                if (false.Equals(p[string.Format("isauto_v{0}mpositiontaking_mem_{1}", SportIds[i], BetTypeIds2[i])]))
                {
                    p[string.Format("auto_v{0}mpositiontaking_mem_{1}", SportIds[i], BetTypeIds2[i])] = 0;
                }
                else if (true.Equals(p[string.Format("isauto_v{0}mpositiontaking_mem_{1}", SportIds[i], BetTypeIds2[i])]))
                {
                    p[string.Format("v{0}mpositiontaking_{1}", SportIds[i], BetTypeIds2[i])] = 0;
                }
            }
        }

        #endregion CheckAndCopy

        protected int[] SportIds = new int[] { 151, 151, 151, 152, 152, 153 };
        protected int[] BetTypeIds = new int[] { 41, 31, 51, 41, 51, 51 };
        protected int[] BetTypeIds2 = new int[] { 42, 32, 52, 42, 52, 52 }; // For Position Taking : Win, Place

        protected bool[] BetTypeEnables = null;

        #region GetRequestParams

        // Get one Param
        protected void GetOneRequestParam(ref Hashtable p, string getNameFormat, string saveNameFormat, Type type)
        {
            string name = saveNameFormat;
            string value = Request[getNameFormat];

            if (value != null)
            {
                try
                {
                    if (!p.ContainsKey(name))
                        p.Add(name, Convert.ChangeType(value, type));
                }
                catch (FormatException e)
                {
                    _result = new Result(ErrorType.Unknown, e.Message);
                }
            }
        }

        protected void GetOneRequestParam_Combination(ref Hashtable p, string checkingNameFormat, string saveNameFormat, Type type)
        {
            int c = SportIds.Length;
            bool isAdd = false;
            for (int i = 0; i < c; i++)
            {
                bool value = Convert.ToBoolean(Request[string.Format(checkingNameFormat, SportIds[i], BetTypeIds[i])]);

                if (value)
                {
                    isAdd = true;
                    break;
                }
            }
            if (isAdd)
            {
                if (!p.ContainsKey(saveNameFormat))
                    p.Add(saveNameFormat, true);
            }
            else
            {
                if (!p.ContainsKey(saveNameFormat))
                    p.Add(saveNameFormat, false);
            }
        }

        protected void GetRequestParams(ref Hashtable p, string getNameFormat, string saveNameFormat, Type type, bool isCopyAll)
        {
            int c = SportIds.Length;

            for (int i = 0; i < c; i++)
            {
                string name = string.Format(saveNameFormat, SportIds[i], BetTypeIds[i]);
                string value = (isCopyAll) ? Request[string.Format(getNameFormat, SportIds[0], BetTypeIds[0])] : Request[string.Format(getNameFormat, SportIds[i], BetTypeIds[i])];

                if (value != null)
                {
                    try
                    {
                        if (!p.ContainsKey(name))
                            p.Add(name, Convert.ChangeType(value, type));
                    }
                    catch (FormatException e)
                    {
                        _result = new Result(ErrorType.Unknown, e.Message);
                    }
                }
            }
        }

        //Win
        protected void GetRequestParams(ref Hashtable p, string getNameFormat, string saveNameFormat, Type type)
        {
            //int c = SportIds.Length;

            //for (int i = 0; i < c; i++)
            //{
            //    string name = string.Format(saveNameFormat, SportIds[i], BetTypeIds[i]);
            //    string value = Request[string.Format(getNameFormat, SportIds[i], BetTypeIds[i])];

            //    if (value != null)
            //    {
            //        try
            //        {
            //            if (!p.ContainsKey(name))
            //                p.Add(name, Convert.ChangeType(value, type));
            //        }
            //        catch (FormatException e)
            //        {
            //            _result = new Result(ErrorType.Unknown, e.Message);
            //        }
            //    }
            //}
            GetRequestParams(ref p, getNameFormat, saveNameFormat, type, false);
        }

        // Resolve enable/disable issue (check box show enable but db key is disable)
        protected void GetRequestBooleanWithRevert(ref Hashtable p, string getNameFormat, string saveNameFormat)
        {
            int c = SportIds.Length;

            for (int i = 0; i < c; i++)
            {
                string name = string.Format(saveNameFormat, SportIds[i], BetTypeIds[i]);
                string value = Request[string.Format(getNameFormat, SportIds[i], BetTypeIds[i])];

                if (value != null)
                {
                    try
                    {
                        p.Add(name, !(bool)(Convert.ChangeType(value, typeof(bool))));
                    }
                    catch (FormatException e)
                    {
                        _result = new Result(ErrorType.Unknown, e.Message);
                    }
                }
            }
        }

        //Place
        protected void GetRequestParams2(ref Hashtable p, string getNameFormat, string saveNameFormat, Type type)
        {
            int c = SportIds.Length;

            for (int i = 0; i < c; i++)
            {
                string name = string.Format(saveNameFormat, SportIds[i], BetTypeIds2[i]);
                string value = Request[string.Format(getNameFormat, SportIds[i], BetTypeIds2[i])];

                if (value != null)
                {
                    try
                    {
                        if (!p.ContainsKey(name))
                            p.Add(name, Convert.ChangeType(value, type));
                    }
                    catch (FormatException e)
                    {
                        _result = new Result(ErrorType.Unknown, e.Message);
                    }
                }
            }
        }

        #endregion GetRequestParams
    }
}