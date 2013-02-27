/// <summary>
/// Summary description for SearchAndCopy_CheckConstraintData
/// </summary>
///

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using Fanex.Age.UI;
    using Fanex.PTEngine.BusinessObjects;
    using Fanex.PTVirtualSports.Services;
    using Sunplus.Agent.Model.Entities;

    public class CheckConstraintDataForRaceBook
    {
        Hashtable htbSporttype_Bettype = new Hashtable();

        public CheckConstraintDataForRaceBook()
        {
            htbSporttype_Bettype.Add("153", new string[] { "51", "52" });
            htbSporttype_Bettype.Add("152", new string[] { "41", "42", "51", "52" });
            htbSporttype_Bettype.Add("151", new string[] { "31", "32", "41", "42", "51", "52" });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public bool IsChangedData(DataRow dr)
        {
            foreach (string key in htbSporttype_Bettype.Keys)
            {
                string[] bettypes = (string[])htbSporttype_Bettype[key];
                string parameter = string.Format("v{0}apositiontaking_", key) + "{0}";
                string parameter_max = string.Format("v{0}pos_", key) + "{0}_max";

                foreach (string bettype in bettypes)
                {
                    string param = string.Format(parameter, bettype);
                    string param_max = string.Format(parameter_max, bettype);

                    if (Convert.ToDouble(dr[param]) > Convert.ToDouble(dr[param_max]))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="htbParameters"></param>
        public void GeneratePositionTaking(DataRow dr, ref Hashtable htbParameters)
        {
            string[] Bettypes_Exclude = new string[] { "32", "42", "52" };

            foreach (string key in htbSporttype_Bettype.Keys)
            {
                string[] bettypes = (string[])htbSporttype_Bettype[key];
                string parameter = string.Format("v{0}apositiontaking_", key) + "{0}";
                string parameter_max = string.Format("v{0}pos_", key) + "{0}_max";

                string parameter_betlimit_min = string.Format("v{0}minbet_", key) + "{0}";
                string parameter_betlimit_max = string.Format("v{0}maxbet_", key) + "{0}";
                string parameter_betlimit_MaxPerMatch = string.Format("v{0}maxpermatch_", key) + "{0}";

                string parameter_discount = string.Format("v{0}discount_", key) + "{0}";
                string parameter_disable = string.Format("disable{0}_", key) + "{0}";

                foreach (string bettype in bettypes)
                {
                    string param = string.Format(parameter, bettype);
                    string param_max = string.Format(parameter_max, bettype);

                    double MinValue = Math.Min(Convert.ToDouble(dr[param]), Convert.ToDouble(dr[param_max]));
                    htbParameters.Add(param, MinValue);

                    string key_param = string.Empty;

                    if (!Bettypes_Exclude.Contains<string>(bettype))
                    {
                        key_param = string.Format(parameter_betlimit_min, bettype);
                        htbParameters.Add(key_param, dr[key_param]);

                        key_param = string.Format(parameter_betlimit_max, bettype);
                        htbParameters.Add(key_param, dr[key_param]);

                        key_param = string.Format(parameter_betlimit_MaxPerMatch, bettype);
                        htbParameters.Add(key_param, dr[key_param]);

                        key_param = string.Format(parameter_discount, bettype);
                        htbParameters.Add(key_param, dr[key_param]);

                        key_param = string.Format(parameter_disable, bettype);
                        htbParameters.Add(key_param, dr[key_param]);
                    }
                }
            }
            htbParameters.Add("language", UserSession.Current.LangKey);
            htbParameters.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            htbParameters.Add("userid", UserSession.Current.CustId);
            htbParameters.Add("errcode", 0);
            htbParameters.Add("errmsg", "");
        }
    }

    public class CheckConstraintDataForSportBook
    {
        Hashtable htbSporttype_Bettype = new Hashtable();

        public CheckConstraintDataForSportBook()
        {
            htbSporttype_Bettype.Add("1", new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            htbSporttype_Bettype.Add("1_live", new string[] { "1", "3", "7", "8" });
            htbSporttype_Bettype.Add("2", new string[] { "1", "2", "3", "7", "8", "20" });
            htbSporttype_Bettype.Add("3", new string[] { "1", "2", "3", "20" });
            htbSporttype_Bettype.Add("5", new string[] { "1", "2", "3", "20" });
            htbSporttype_Bettype.Add("8", new string[] { "1", "3", "20" });
            htbSporttype_Bettype.Add("10", new string[] { "1", "2", "3", "20" });
            htbSporttype_Bettype.Add("11", new string[] { "1", "20" });
            htbSporttype_Bettype.Add("99", new string[] { "1", "2", "3", "9", "20" });
            htbSporttype_Bettype.Add("161", new string[] { "81", "83", "86", "88" });
            htbSporttype_Bettype.Add("161_live", new string[] { "85", "86", "87", "89", "90" });
        }

        public bool IsChangedData(DataRow dr)
        {
            string parameter = string.Empty;
            string parameter_max = string.Empty;
            foreach (string key in htbSporttype_Bettype.Keys)
            {
                string[] bettypes = (string[])htbSporttype_Bettype[key];
                if (key == "1")
                {
                    parameter = "apositiontaking_{0}";
                    parameter_max = "maxpos_{0}";
                }
                else if (key == "1_live")
                {
                    parameter = "alivepositiontaking_{0}";
                    parameter_max = "maxlpos_{0}";
                }
                else if (key == "161_live")
                {
                    parameter = "v161alivepositiontaking_{0}";
                    parameter_max = "maxv161lpos_{0}";
                }
                else
                {
                    parameter = string.Format("v{0}apositiontaking_", key) + "{0}";
                    parameter_max = string.Format("maxv{0}pos_", key) + "{0}";
                }
                foreach (string bettype in bettypes)
                {
                    string param = string.Format(parameter, bettype);
                    string param_max = string.Format(parameter_max, bettype);
                    if (Convert.ToDouble(dr[param]) > Convert.ToDouble(dr[param_max]))
                        return true;
                }
            }
            return false;
        }

        public void GenerateInfoAccount(DataRow dr, ref Hashtable htbParameters)
        {
            htbParameters.Add("custid", 0);
            htbParameters.Add("playerdiscount", dr["playerdiscount"]);
            htbParameters.Add("playerdiscountcs", dr["playerdiscountcs"]);
            htbParameters.Add("playerdiscount1x2", dr["playerdiscount1x2"]);
            htbParameters.Add("playerdiscountnumber", dr["playerdiscountnumber"]);
            htbParameters.Add("suspended", dr["suspended"]);
            htbParameters.Add("recommend", UserSession.Current.CustId);
            htbParameters.Add("ugroup", dr["ugroup"]);
            htbParameters.Add("closed", dr["closed"]);
            htbParameters.Add("issearchandcopy", 1);
            htbParameters.Add("disablenumber", dr["disablenumber"]);
            htbParameters.Add("language", UserSession.Current.LangKey);
            htbParameters.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            htbParameters.Add("userid", UserSession.Current.CustId);
            htbParameters.Add("creator", UserSession.Current.UserId);
            htbParameters.Add("subaccid", UserSession.Current.SubAccId);
            htbParameters.Add("errcode", 0);
            htbParameters.Add("errmsg", "");
        }

        public void GeneratePositionTaking(DataRow dr, ref Hashtable htbParameters)
        {
            string parameter = string.Empty;
            string parameter_max = string.Empty;
            foreach (string key in htbSporttype_Bettype.Keys)
            {
                string[] bettypes = (string[])htbSporttype_Bettype[key];
                if (key == "1")
                {
                    parameter = "apositiontaking_{0}";
                    parameter_max = "maxpos_{0}";
                }
                else if (key == "1_live")
                {
                    parameter = "alivepositiontaking_{0}";
                    parameter_max = "maxlpos_{0}";
                }
                else if (key == "161_live")
                {
                    parameter = "v161alivepositiontaking_{0}";
                    parameter_max = "maxv161lpos_{0}";
                }
                else
                {
                    parameter = string.Format("v{0}apositiontaking_", key) + "{0}";
                    parameter_max = string.Format("maxv{0}pos_", key) + "{0}";
                }
                foreach (string bettype in bettypes)
                {
                    string param = string.Format(parameter, bettype);
                    string param_max = string.Format(parameter_max, bettype);

                    double MinValue = Math.Min(Convert.ToDouble(dr[param]), Convert.ToDouble(dr[param_max]));
                    htbParameters.Add(param, MinValue);
                }
            }
        }

        public void GenerateBettingLimit(DataRow dr, ref Hashtable htbParameters)
        {
            string[] Sporttypes = new string[] { "1", "2", "3", "5", "8", "10", "11", "99", "99mp", "161" };
            string[] Parameters = new string[] {
                                                    "minbetv{0}",
                                                    "maxbetv{0}",
                                                    "maxpermatchv{0}"
                                                };

            foreach (string item in Sporttypes)
            {
                if (item == "1")
                {
                    htbParameters.Add("minbet", dr["minbet"]);
                    htbParameters.Add("maxbet", dr["maxbet"]);
                    htbParameters.Add("maxpermatch", dr["maxpermatch"]);
                }
                else
                {
                    if (item == "161")
                    {
                        htbParameters.Add("maxperballv161", dr["maxperballv161"]);
                    }
                    foreach (string param in Parameters)
                    {
                        string FormatParam = string.Format(param, item);
                        if (!htbParameters.ContainsKey(FormatParam))
                            htbParameters.Add(FormatParam, dr[FormatParam]);
                    }
                }
            }
        }
    }

    public class CheckConstraintDataForLiveCasino
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public bool IsChangedData(int custIdOld)
        {
            var ptService = new Fanex.LiveCasinoSetting.Business.SettingService();
            var model = ptService.GetMemberSetting(custIdOld, Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
            var settingPT = model.PTSetting as Fanex.LiveCasinoSetting.Model.PTSetting;

            if (Convert.ToDouble(settingPT.Upline.PresetPT) > Convert.ToDouble(settingPT.Upline.MaxPT))
            {
                return true;
            }

            return false;
        }
    }

    public class CheckConstraintDataForVirtualSports
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public bool IsChangedData(int custIdOld)
        {
            IRequestContext oldCustomerContext = new RequestContext
            {
                CustId = UserSession.Current.CustId,
                TargetCustId = custIdOld,
                TargetRoleId = 1,
                FormId = FormType.EditPT,
                SubAccId = UserSession.Current.SubAccId,
                AdminId = UserSession.Current.AdminId,
                Language = UserSession.Current.LangKey,
                LangKey = UserSession.Current.LangKey,
                RoleId = 2
            };

            CustomerService customerService = new CustomerService();
            var oldCustomer = customerService.GetCustomer(oldCustomerContext);

            foreach (SportType sport in oldCustomer.PtSetting.SportTypes)
            {
                foreach (BetTypeGroupSet bettypeGroupSet in sport.BetTypeGroupSets)
                {
                    foreach (BetTypeGroup bettypeGroup in bettypeGroupSet.BetTypeGroups)
                    {
                        foreach (BetType bettype in bettypeGroup.BetTypes)
                        {
                            if (bettype.Pt.A1 > bettype.Pt.A2)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}