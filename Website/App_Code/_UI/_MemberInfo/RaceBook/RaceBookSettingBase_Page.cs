// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.05.30">Improve performance, check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Text;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._Components.RacingStatus_Control;
    using Sunplus.Agent.UI._MemberInfo.RaceBook.MinMaxBet_Control;
    using Sunplus.Agent.UI.PositionTaking.RaceBook;
    using Sunplus.Agent.Utilities;

    public abstract class RaceBookSettingBase_Page : TplPage
    {
        #region Variable

        private const double COMM_INTERVAL = 0.0005;
        private MasterPage_Template _templateMain = new MasterPage_Template();
        private Hashtable hashTableVars = new Hashtable();
        private Hashtable hashTableRegs = new Hashtable();
        
        #endregion Variable

        #region Properties
        
        public RaceBookSettingBase_Page()
        {
            this.hashTableVars.Add("lblalerMaxbetCompare", Resources.Racing.lblalerMaxbetCompare);
            this.hashTableVars.Add("lblalertMaxBetNegative", Resources.Racing.lblalertMaxBetNegative);
            this.hashTableVars.Add("lblalertMaxPerMatchCompare", Resources.Racing.lblalertMaxPerMatchCompare);
            this.hashTableVars.Add("lblalertMaxPerMatchNegative", Resources.Racing.lblalertMaxPerMatchNegative);
            this.hashTableVars.Add("lblalertMinbetCompare", Resources.Racing.lblalertMinbetCompare);
            this.hashTableVars.Add("lblalertMinBetNegative", Resources.Racing.lblalertMinBetNegative);
            this.hashTableVars.Add("alertMaxBetLessMinBet", Resources.Racing.alertMaxBetLessMinBet);
            this.hashTableVars.Add("alertMaxPerMatchLessMaxBet", Resources.Racing.alertMaxPerMatchLessMaxBet);
            this.hashTableVars.Add("alertIncorrectMinBet", Resources.Racing.alertIncorrectMinBet);
            this.hashTableVars.Add("alertIncorrectMaxBet", Resources.Racing.alertIncorrectMaxBet);
            this.hashTableVars.Add("alertIncorrectMaxPerMatch", Resources.Racing.alertIncorrectMaxPerMatch);
        }

        public MasterPage_Template TemplateMain
        {
            get
            {
                return this._templateMain;
            }
        }

        public int CustId { get; set; }

        #endregion Properties

        public void HashTableRegister(object name, object value)
        {
            this.hashTableRegs.Add(name, value);
        }

        public void RegisterClientHiddenControl()
        {
            StringBuilder res = new StringBuilder();
            foreach (DictionaryEntry item in this.hashTableRegs)
            {
                res.Append("<input type=\"hidden\" id=\"" + "ID_" + item.Key + "\" name=\"" + item.Key + "\" value=\"" + item.Value + "\" />");
            }

            this.TemplateMain.hidden_PT = res.ToString();
        }

        public void JavaScriptRegister(object name, object value)
        {
            this.hashTableVars.Add(name, value);
        }

        public void RegisterClientVariables()
        {
            foreach (DictionaryEntry item in this.hashTableVars)
            {
                this.RegisterJavascriptObject(item.Key, item.Value);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int custId = 0;
            int.TryParse(this.Request["custid"], out custId);
            this.CustId = custId;
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (this.CustId == 0)
            {
                return new ErrorPage_Page("Invalid customer id!");
            }

            this.hashTableVars.Add("IsShowRacing_151_41", UserPermission.IsShowRacing_151_41);
            this.hashTableVars.Add("IsShowRacing_151_31", UserPermission.IsShowRacing_151_31);
            this.hashTableVars.Add("IsShowRacing_151_51", UserPermission.IsShowRacing_151_51);
            this.hashTableVars.Add("IsShowRacing_152_41", UserPermission.IsShowRacing_152_41);
            this.hashTableVars.Add("IsShowRacing_152_51", UserPermission.IsShowRacing_152_51);
            this.hashTableVars.Add("IsShowRacing_153_51", UserPermission.IsShowRacing_153_51);

            return null;
        }

        public void CheckedStatusRacing(bool isUpline, bool isDownline, bool isDisabled, RacingStatus_Control_Template tpl)
        {
            this.CheckedStatusRacing(isUpline, isDownline, false, isDisabled, tpl);
        }

        public void CheckedStatusRacing(bool isUpline, bool isDownline, bool isDisabled, bool disabledAll, RacingStatus_Control_Template tpl)
        {
            string disall = string.Empty;
            if (disabledAll)
            {
                disall = "disabled";
            }

            if (isUpline)
            {
                tpl.uplineChecked = "disabled";
                tpl.Downline.downlineChecked = "disabled";
            }
            else
            {
                ////disabled
                if (isDisabled && isDownline)
                {
                    tpl.uplineChecked = disall;
                    tpl.Downline.downlineChecked = "disabled";
                }
                else if (!isDisabled && !isDownline)
                {
                    tpl.uplineChecked = "checked='true'" + " " + disall;
                    tpl.Downline.downlineChecked = "checked='true' disabled";
                }
                else if (!isDisabled && isDownline)
                {
                    tpl.uplineChecked = "checked='true'" + " " + disall;
                    tpl.Downline.downlineChecked = "disabled";
                }
                else
                {
                    tpl.uplineChecked = disall;
                    tpl.Downline.downlineChecked = "disabled";
                }
            }
        }

        public void RenderCustomerInfo(DataRow dr, UserInfo_Template tpl)
        {
            this.RenderCustomerInfo(dr, tpl, Resources.Racing.edithorseracing);
        }

        public void RenderCustomerInfo(DataRow dr, UserInfo_Template tpl, string title)
        {
            tpl.username_text = Resources.Racing.username;
            tpl.firstname_text = Resources.Racing.firstname;
            tpl.lastname_text = Resources.Racing.lastname;
            tpl.phone_text = Resources.Racing.phone;
            tpl.mobile_text = Resources.Racing.mobilephone;
            tpl.fax_text = Resources.Racing.fax;
            this.TemplateMain.title = tpl.title = title + " " + dr["username"] as string;

            this.hashTableVars.Add("username", dr["username"]);

            tpl.username_val = dr["username"] as string;
            tpl.firstname_val = dr["firstname"] as string;
            tpl.lastname_val = dr["lastname"] as string;
            tpl.phone_val = dr["phone"] as string;
            tpl.mobile_val = dr["mobilephone"] as string;
            tpl.fax_val = dr["fax"] as string;
        }

        public void RenderStatus(
            DataRow dr,
            ref RacingStatus_Control_Template tplStatus_151_41,
            ref RacingStatus_Control_Template tplStatus_151_31,
            ref RacingStatus_Control_Template tplStatus_151_51,
            ref RacingStatus_Control_Template tplStatus_152_41,
            ref RacingStatus_Control_Template tplStatus_152_51,
            ref RacingStatus_Control_Template tplStatus_153_51)
        {
            this.RenderStatus(dr, ref tplStatus_151_41, ref tplStatus_151_31, ref tplStatus_151_51, ref tplStatus_152_41, ref tplStatus_152_51, ref tplStatus_153_51, false);
        }

        public void RenderStatus(
            DataRow dr,
            ref RacingStatus_Control_Template tplStatus_151_41,
            ref RacingStatus_Control_Template tplStatus_151_31,
            ref RacingStatus_Control_Template tplStatus_151_51,
            ref RacingStatus_Control_Template tplStatus_152_41,
            ref RacingStatus_Control_Template tplStatus_152_51,
            ref RacingStatus_Control_Template tplStatus_153_51,
            bool disabledAll)
        {
            bool disableUplines = false, disableDownlines = false, disable = false;

            DataColumnCollection cols = dr.Table.Columns;

            ////for HR UK
            tplStatus_151_41 = new RacingStatus_Control_Template();
            tplStatus_151_41.enable = Resources.Racing.enable;
            tplStatus_151_41.uplineID = "Disable151_41";
            tplStatus_151_41.downlineID = tplStatus_151_41.Downline.downlineID = "DisableDownlines151_41";

            if (cols.Contains("DisableUplines151_41"))
            {
                disableUplines = Convert.ToBoolean(dr["DisableUplines151_41"]);
            }

            if (cols.Contains("DisableDownlines151_41"))
            {
                disableDownlines = Convert.ToBoolean(dr["DisableDownlines151_41"]);
            }

            disable = Convert.ToBoolean(dr["Disable151_41"]);

            this.hashTableVars.Add("DisableUplines151_41", disableUplines);
            this.hashTableVars.Add("DisableDownlines151_41", disableDownlines);
            this.hashTableVars.Add("Disable151_41", disable);

            this.CheckedStatusRacing(disableUplines, disableDownlines, disable, disabledAll, tplStatus_151_41);

            ////for HR SA
            tplStatus_151_31 = new RacingStatus_Control_Template();
            tplStatus_151_31.enable = Resources.Racing.enable;
            tplStatus_151_31.uplineID = "Disable151_31";
            tplStatus_151_31.downlineID = tplStatus_151_31.Downline.downlineID = "DisableDownlines151_31";

            if (cols.Contains("DisableUplines151_31"))
            {
                disableUplines = Convert.ToBoolean(dr["DisableUplines151_31"]);
            }

            if (cols.Contains("DisableDownlines151_31"))
            {
                disableDownlines = Convert.ToBoolean(dr["DisableDownlines151_31"]);
            }

            disable = Convert.ToBoolean(dr["Disable151_31"]);

            this.hashTableVars.Add("DisableUplines151_31", disableUplines);
            this.hashTableVars.Add("DisableDownlines151_31", disableDownlines);
            this.hashTableVars.Add("Disable151_31", disable);

            this.CheckedStatusRacing(disableUplines, disableDownlines, disable, disabledAll, tplStatus_151_31);

            // for HR AU
            tplStatus_151_51 = new RacingStatus_Control_Template();
            tplStatus_151_51.enable = Resources.Racing.enable;
            tplStatus_151_51.uplineID = "Disable151_51";
            tplStatus_151_51.downlineID = tplStatus_151_51.Downline.downlineID = "DisableDownlines151_51";

            if (cols.Contains("DisableUplines151_51"))
            {
                disableUplines = Convert.ToBoolean(dr["DisableUplines151_51"]);
            }

            if (cols.Contains("DisableDownlines151_51"))
            {
                disableDownlines = Convert.ToBoolean(dr["DisableDownlines151_51"]);
            }

            disable = Convert.ToBoolean(dr["Disable151_51"]);

            this.hashTableVars.Add("DisableUplines151_51", disableUplines);
            this.hashTableVars.Add("DisableDownlines151_51", disableDownlines);
            this.hashTableVars.Add("Disable151_51", disable);

            this.CheckedStatusRacing(disableUplines, disableDownlines, disable, disabledAll, tplStatus_151_51);

            ////for GH UK
            tplStatus_152_41 = new RacingStatus_Control_Template();
            tplStatus_152_41.enable = Resources.Racing.enable;
            tplStatus_152_41.uplineID = "Disable152_41";
            tplStatus_152_41.downlineID = tplStatus_152_41.Downline.downlineID = "DisableDownlines152_41";

            if (cols.Contains("DisableUplines152_41"))
            {
                disableUplines = Convert.ToBoolean(dr["DisableUplines152_41"]);
            }

            if (cols.Contains("DisableDownlines152_41"))
            {
                disableDownlines = Convert.ToBoolean(dr["DisableDownlines152_41"]);
            }

            disable = Convert.ToBoolean(dr["Disable152_41"]);

            this.hashTableVars.Add("DisableUplines152_41", disableUplines);
            this.hashTableVars.Add("DisableDownlines152_41", disableDownlines);
            this.hashTableVars.Add("Disable152_41", disable);

            this.CheckedStatusRacing(disableUplines, disableDownlines, disable, disabledAll, tplStatus_152_41);

            ////for GH AU
            tplStatus_152_51 = new RacingStatus_Control_Template();
            tplStatus_152_51.enable = Resources.Racing.enable;
            tplStatus_152_51.uplineID = "Disable152_51";
            tplStatus_152_51.downlineID = tplStatus_152_51.Downline.downlineID = "DisableDownlines152_51";

            if (cols.Contains("DisableUplines152_51"))
            {
                disableUplines = Convert.ToBoolean(dr["DisableUplines152_51"]);
            }

            if (cols.Contains("DisableDownlines152_51"))
            {
                disableDownlines = Convert.ToBoolean(dr["DisableDownlines152_51"]);
            }

            disable = Convert.ToBoolean(dr["Disable152_51"]);

            this.hashTableVars.Add("DisableUplines152_51", disableUplines);
            this.hashTableVars.Add("DisableDownlines152_51", disableDownlines);
            this.hashTableVars.Add("Disable152_51", disable);

            this.CheckedStatusRacing(disableUplines, disableDownlines, disable, disabledAll, tplStatus_152_51);

            // for GH AU
            tplStatus_153_51 = new RacingStatus_Control_Template();
            tplStatus_153_51.enable = Resources.Racing.enable;
            tplStatus_153_51.uplineID = "Disable153_51";
            tplStatus_153_51.downlineID = tplStatus_153_51.Downline.downlineID = "DisableDownlines153_51";

            if (cols.Contains("DisableUplines153_51"))
            {
                disableUplines = Convert.ToBoolean(dr["DisableUplines153_51"]);
            }

            if (cols.Contains("DisableDownlines153_51"))
            {
                disableDownlines = Convert.ToBoolean(dr["DisableDownlines153_51"]);
            }

            disable = Convert.ToBoolean(dr["Disable153_51"]);

            this.hashTableVars.Add("DisableUplines153_51", disableUplines);
            this.hashTableVars.Add("DisableDownlines153_51", disableDownlines);
            this.hashTableVars.Add("Disable153_51", disable);

            this.CheckedStatusRacing(disableUplines, disableDownlines, disable, disabledAll, tplStatus_153_51);
        }

        public abstract void RenderStatus(DataRow dr, TplCore.Element.IElement e);

        public void RenderViewBetSetting(
           DataRow dr,
           ref MinMaxBet_Control_Template tplMinMax_151_41,
           ref MinMaxBet_Control_Template tplMinMax_151_31,
           ref MinMaxBet_Control_Template tplMinMax_151_51,
           ref MinMaxBet_Control_Template tplMinMax_152_41,
           ref MinMaxBet_Control_Template tplMinMax_152_51,
           ref MinMaxBet_Control_Template tplMinMax_153_51)
        {
            // For HR - UK/Irish
            tplMinMax_151_41 = new MinMaxBet_Control_Template();
            tplMinMax_151_41.MinMaxBet_Control = null;

            tplMinMax_151_41.MinMaxBet_View.MinBetVal = Formater.DecFormat(dr["v151minbet_41"], 0);
            tplMinMax_151_41.MinMaxBet_View.MaxBetVal = Formater.DecFormat(dr["v151maxbet_41"], 0);
            tplMinMax_151_41.MinMaxBet_View.MaxPerMatchVal = Formater.DecFormat(dr["v151maxpermatch_41"], 0);

            // For HR - SA
            tplMinMax_151_31 = new MinMaxBet_Control_Template();
            tplMinMax_151_31.MinMaxBet_Control = null;

            tplMinMax_151_31.MinMaxBet_View.MinBetVal = Formater.DecFormat(dr["v151minbet_31"], 0);
            tplMinMax_151_31.MinMaxBet_View.MaxBetVal = Formater.DecFormat(dr["v151maxbet_31"], 0);
            tplMinMax_151_31.MinMaxBet_View.MaxPerMatchVal = Formater.DecFormat(dr["v151maxpermatch_31"], 0);

            // For HR - AU
            tplMinMax_151_51 = new MinMaxBet_Control_Template();
            tplMinMax_151_51.MinMaxBet_Control = null;

            tplMinMax_151_51.MinMaxBet_View.MinBetVal = Formater.DecFormat(dr["v151minbet_51"], 0);
            tplMinMax_151_51.MinMaxBet_View.MaxBetVal = Formater.DecFormat(dr["v151maxbet_51"], 0);
            tplMinMax_151_51.MinMaxBet_View.MaxPerMatchVal = Formater.DecFormat(dr["v151maxpermatch_51"], 0);

            // For GreyHound - UK/Irish
            tplMinMax_152_41 = new MinMaxBet_Control_Template();
            tplMinMax_152_41.MinMaxBet_Control = null;

            tplMinMax_152_41.MinMaxBet_View.MinBetVal = Formater.DecFormat(dr["v152minbet_41"], 0);
            tplMinMax_152_41.MinMaxBet_View.MaxBetVal = Formater.DecFormat(dr["v152maxbet_41"], 0);
            tplMinMax_152_41.MinMaxBet_View.MaxPerMatchVal = Formater.DecFormat(dr["v152maxpermatch_41"], 0);

            // For GreyHound - AU
            tplMinMax_152_51 = new MinMaxBet_Control_Template();
            tplMinMax_152_51.MinMaxBet_Control = null;

            tplMinMax_152_51.MinMaxBet_View.MinBetVal = Formater.DecFormat(dr["v152minbet_51"], 0);
            tplMinMax_152_51.MinMaxBet_View.MaxBetVal = Formater.DecFormat(dr["v152maxbet_51"], 0);
            tplMinMax_152_51.MinMaxBet_View.MaxPerMatchVal = Formater.DecFormat(dr["v152maxpermatch_51"], 0);

            // For GreyHound - AU
            tplMinMax_153_51 = new MinMaxBet_Control_Template();
            tplMinMax_153_51.MinMaxBet_Control = null;

            tplMinMax_153_51.MinMaxBet_View.MinBetVal = Formater.DecFormat(dr["v153minbet_51"], 0);
            tplMinMax_153_51.MinMaxBet_View.MaxBetVal = Formater.DecFormat(dr["v153maxbet_51"], 0);
            tplMinMax_153_51.MinMaxBet_View.MaxPerMatchVal = Formater.DecFormat(dr["v153maxpermatch_51"], 0);
        }

        public void RenderBetSetting(
            DataRow dr,
            ref MinMaxBet_Control_Template tplMinMax_151_41,
            ref MinMaxBet_Control_Template tplMinMax_151_31,
            ref MinMaxBet_Control_Template tplMinMax_151_51,
            ref MinMaxBet_Control_Template tplMinMax_152_41,
            ref MinMaxBet_Control_Template tplMinMax_152_51,
            ref MinMaxBet_Control_Template tplMinMax_153_51)
        {
            this.RenderBetSetting(dr, ref tplMinMax_151_41, ref tplMinMax_151_31, ref tplMinMax_151_51, ref tplMinMax_152_41, ref tplMinMax_152_51, ref tplMinMax_153_51, false);
        }

        public void RenderBetSetting(
            DataRow dr,
            ref MinMaxBet_Control_Template tplMinMax_151_41,
            ref MinMaxBet_Control_Template tplMinMax_151_31,
            ref MinMaxBet_Control_Template tplMinMax_151_51,
            ref MinMaxBet_Control_Template tplMinMax_152_41,
            ref MinMaxBet_Control_Template tplMinMax_152_51,
            ref MinMaxBet_Control_Template tplMinMax_153_51,
            bool disabled)
        {
            // For HR - UK/Irish
            tplMinMax_151_41 = new MinMaxBet_Control_Template();
            tplMinMax_151_41.MinMaxBet_Control.disabled = disabled ? "Disabled" : string.Empty;
            tplMinMax_151_41.MinMaxBet_View = null;
            tplMinMax_151_41.MinMaxBet_Control.MinBetID = "MinBet_151_41";
            tplMinMax_151_41.MinMaxBet_Control.MaxBetID = "MaxBet_151_41";
            tplMinMax_151_41.MinMaxBet_Control.MaxPerMatchID = "MaxPerMatch_151_41";
            tplMinMax_151_41.MinMaxBet_Control.MinBetMin.MinBetMinID = "MinBet_151_41_Min";
            tplMinMax_151_41.MinMaxBet_Control.MaxBetMax.MaxBetMaxID = "MaxBet_151_41_Max";
            tplMinMax_151_41.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxID = "MaxPerMatch_151_41_Max";

            tplMinMax_151_41.MinMaxBet_Control.MinBetVal = Formater.DecFormat(dr["v151minbet_41"], 0);
            tplMinMax_151_41.MinMaxBet_Control.MaxBetVal = Formater.DecFormat(dr["v151maxbet_41"], 0);
            tplMinMax_151_41.MinMaxBet_Control.MaxPerMatchVal = Formater.DecFormat(dr["v151maxpermatch_41"], 0);

            if (Convert.ToDouble(dr["v151MinBet_41_Min"]) == 0)
            {
                tplMinMax_151_41.MinMaxBet_Control.MinBetMin = null;
            }
            else
            {
                tplMinMax_151_41.MinMaxBet_Control.MinBetMin.MinBetMinVal = Formater.DecFormat(dr["v151MinBet_41_Min"], 0);
            }

            if (Convert.ToDouble(dr["v151MaxBet_41_Max"]) == 0)
            {
                tplMinMax_151_41.MinMaxBet_Control.MaxBetMax = null;
            }
            else
            {
                tplMinMax_151_41.MinMaxBet_Control.MaxBetMax.MaxBetMaxVal = Formater.DecFormat(dr["v151MaxBet_41_Max"], 0);
            }

            if (Convert.ToDouble(dr["v151MaxperMatch_41_Max"]) == 0)
            {
                tplMinMax_151_41.MinMaxBet_Control.MaxPerMatchMax = null;
            }
            else
            {
                tplMinMax_151_41.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxVal = Formater.DecFormat(dr["v151MaxperMatch_41_Max"], 0);
            }

            // For HR - SA
            tplMinMax_151_31 = new MinMaxBet_Control_Template();
            tplMinMax_151_31.MinMaxBet_Control.disabled = disabled ? "Disabled" : string.Empty;
            tplMinMax_151_31.MinMaxBet_View = null;
            tplMinMax_151_31.MinMaxBet_Control.MinBetID = "MinBet_151_31";
            tplMinMax_151_31.MinMaxBet_Control.MaxBetID = "MaxBet_151_31";
            tplMinMax_151_31.MinMaxBet_Control.MaxPerMatchID = "MaxPerMatch_151_31";
            tplMinMax_151_31.MinMaxBet_Control.MinBetMin.MinBetMinID = "MinBet_151_31_Min";
            tplMinMax_151_31.MinMaxBet_Control.MaxBetMax.MaxBetMaxID = "MaxBet_151_31_Max";
            tplMinMax_151_31.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxID = "MaxPerMatch_151_31_Max";

            tplMinMax_151_31.MinMaxBet_Control.MinBetVal = Formater.DecFormat(dr["v151minbet_31"], 0);
            tplMinMax_151_31.MinMaxBet_Control.MaxBetVal = Formater.DecFormat(dr["v151maxbet_31"], 0);
            tplMinMax_151_31.MinMaxBet_Control.MaxPerMatchVal = Formater.DecFormat(dr["v151maxpermatch_31"], 0);

            if (Convert.ToDouble(dr["v151MinBet_31_Min"]) == 0)
            {
                tplMinMax_151_31.MinMaxBet_Control.MinBetMin = null;
            }
            else
            {
                tplMinMax_151_31.MinMaxBet_Control.MinBetMin.MinBetMinVal = Formater.DecFormat(dr["v151MinBet_31_Min"], 0);
            }

            if (Convert.ToDouble(dr["v151MaxBet_31_Max"]) == 0)
            {
                tplMinMax_151_31.MinMaxBet_Control.MaxBetMax = null;
            }
            else
            {
                tplMinMax_151_31.MinMaxBet_Control.MaxBetMax.MaxBetMaxVal = Formater.DecFormat(dr["v151MaxBet_31_Max"], 0);
            }

            if (Convert.ToDouble(dr["v151MaxperMatch_31_Max"]) == 0)
            {
                tplMinMax_151_31.MinMaxBet_Control.MaxPerMatchMax = null;
            }
            else
            {
                tplMinMax_151_31.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxVal = Formater.DecFormat(dr["v151MaxperMatch_31_Max"], 0);
            }

            // For HR - AU
            tplMinMax_151_51 = new MinMaxBet_Control_Template();
            tplMinMax_151_51.MinMaxBet_Control.disabled = disabled ? "Disabled" : string.Empty;
            tplMinMax_151_51.MinMaxBet_View = null;
            tplMinMax_151_51.MinMaxBet_Control.MinBetID = "MinBet_151_51";
            tplMinMax_151_51.MinMaxBet_Control.MaxBetID = "MaxBet_151_51";
            tplMinMax_151_51.MinMaxBet_Control.MaxPerMatchID = "MaxPerMatch_151_51";
            tplMinMax_151_51.MinMaxBet_Control.MinBetMin.MinBetMinID = "MinBet_151_51_Min";
            tplMinMax_151_51.MinMaxBet_Control.MaxBetMax.MaxBetMaxID = "MaxBet_151_51_Max";
            tplMinMax_151_51.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxID = "MaxPerMatch_151_51_Max";

            tplMinMax_151_51.MinMaxBet_Control.MinBetVal = Formater.DecFormat(dr["v151minbet_51"], 0);
            tplMinMax_151_51.MinMaxBet_Control.MaxBetVal = Formater.DecFormat(dr["v151maxbet_51"], 0);
            tplMinMax_151_51.MinMaxBet_Control.MaxPerMatchVal = Formater.DecFormat(dr["v151maxpermatch_51"], 0);

            if (Convert.ToDouble(dr["v151MinBet_51_Min"]) == 0)
            {
                tplMinMax_151_51.MinMaxBet_Control.MinBetMin = null;
            }
            else
            {
                tplMinMax_151_51.MinMaxBet_Control.MinBetMin.MinBetMinVal = Formater.DecFormat(dr["v151MinBet_51_Min"], 0);
            }

            if (Convert.ToDouble(dr["v151MaxBet_51_Max"]) == 0)
            {
                tplMinMax_151_51.MinMaxBet_Control.MaxBetMax = null;
            }
            else
            {
                tplMinMax_151_51.MinMaxBet_Control.MaxBetMax.MaxBetMaxVal = Formater.DecFormat(dr["v151MaxBet_51_Max"], 0);
            }

            if (Convert.ToDouble(dr["v151MaxperMatch_51_Max"]) == 0)
            {
                tplMinMax_151_51.MinMaxBet_Control.MaxPerMatchMax = null;
            }
            else
            {
                tplMinMax_151_51.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxVal = Formater.DecFormat(dr["v151MaxperMatch_51_Max"], 0);
            }

            // For GreyHound - UK/Irish
            tplMinMax_152_41 = new MinMaxBet_Control_Template();
            tplMinMax_152_41.MinMaxBet_Control.disabled = disabled ? "Disabled" : string.Empty;
            tplMinMax_152_41.MinMaxBet_View = null;
            tplMinMax_152_41.MinMaxBet_Control.MinBetID = "MinBet_152_41";
            tplMinMax_152_41.MinMaxBet_Control.MaxBetID = "MaxBet_152_41";
            tplMinMax_152_41.MinMaxBet_Control.MaxPerMatchID = "MaxPerMatch_152_41";
            tplMinMax_152_41.MinMaxBet_Control.MinBetMin.MinBetMinID = "MinBet_152_41_Min";
            tplMinMax_152_41.MinMaxBet_Control.MaxBetMax.MaxBetMaxID = "MaxBet_152_41_Max";
            tplMinMax_152_41.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxID = "MaxPerMatch_152_41_Max";

            tplMinMax_152_41.MinMaxBet_Control.MinBetVal = Formater.DecFormat(dr["v152minbet_41"], 0);
            tplMinMax_152_41.MinMaxBet_Control.MaxBetVal = Formater.DecFormat(dr["v152maxbet_41"], 0);
            tplMinMax_152_41.MinMaxBet_Control.MaxPerMatchVal = Formater.DecFormat(dr["v152maxpermatch_41"], 0);

            if (Convert.ToDouble(dr["v152MinBet_41_Min"]) == 0)
            {
                tplMinMax_152_41.MinMaxBet_Control.MinBetMin = null;
            }
            else
            {
                tplMinMax_152_41.MinMaxBet_Control.MinBetMin.MinBetMinVal = Formater.DecFormat(dr["v152MinBet_41_Min"], 0);
            }

            if (Convert.ToDouble(dr["v152MaxBet_41_Max"]) == 0)
            {
                tplMinMax_152_41.MinMaxBet_Control.MaxBetMax = null;
            }
            else
            {
                tplMinMax_152_41.MinMaxBet_Control.MaxBetMax.MaxBetMaxVal = Formater.DecFormat(dr["v152MaxBet_41_Max"], 0);
            }

            if (Convert.ToDouble(dr["v152MaxperMatch_41_Max"]) == 0)
            {
                tplMinMax_152_41.MinMaxBet_Control.MaxPerMatchMax = null;
            }
            else
            {
                tplMinMax_152_41.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxVal = Formater.DecFormat(dr["v152MaxperMatch_41_Max"], 0);
            }

            // For GreyHound - AU
            tplMinMax_152_51 = new MinMaxBet_Control_Template();
            tplMinMax_152_51.MinMaxBet_Control.disabled = disabled ? "Disabled" : string.Empty;
            tplMinMax_152_51.MinMaxBet_View = null;
            tplMinMax_152_51.MinMaxBet_Control.MinBetID = "MinBet_152_51";
            tplMinMax_152_51.MinMaxBet_Control.MaxBetID = "MaxBet_152_51";
            tplMinMax_152_51.MinMaxBet_Control.MaxPerMatchID = "MaxPerMatch_152_51";
            tplMinMax_152_51.MinMaxBet_Control.MinBetMin.MinBetMinID = "MinBet_152_51_Min";
            tplMinMax_152_51.MinMaxBet_Control.MaxBetMax.MaxBetMaxID = "MaxBet_152_51_Max";
            tplMinMax_152_51.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxID = "MaxPerMatch_152_51_Max";

            tplMinMax_152_51.MinMaxBet_Control.MinBetVal = Formater.DecFormat(dr["v152minbet_51"], 0);
            tplMinMax_152_51.MinMaxBet_Control.MaxBetVal = Formater.DecFormat(dr["v152maxbet_51"], 0);
            tplMinMax_152_51.MinMaxBet_Control.MaxPerMatchVal = Formater.DecFormat(dr["v152maxpermatch_51"], 0);

            if (Convert.ToDouble(dr["v152MinBet_51_Min"]) == 0)
            {
                tplMinMax_152_51.MinMaxBet_Control.MinBetMin = null;
            }
            else
            {
                tplMinMax_152_51.MinMaxBet_Control.MinBetMin.MinBetMinVal = Formater.DecFormat(dr["v152MinBet_51_Min"], 0);
            }

            if (Convert.ToDouble(dr["v152MaxBet_51_Max"]) == 0)
            {
                tplMinMax_152_51.MinMaxBet_Control.MaxBetMax = null;
            }
            else
            {
                tplMinMax_152_51.MinMaxBet_Control.MaxBetMax.MaxBetMaxVal = Formater.DecFormat(dr["v152MaxBet_51_Max"], 0);
            }

            if (Convert.ToDouble(dr["v152MaxperMatch_51_Max"]) == 0)
            {
                tplMinMax_152_51.MinMaxBet_Control.MaxPerMatchMax = null;
            }
            else
            {
                tplMinMax_152_51.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxVal = Formater.DecFormat(dr["v152MaxperMatch_51_Max"], 0);
            }

            // For GreyHound - AU
            tplMinMax_153_51 = new MinMaxBet_Control_Template();
            tplMinMax_153_51.MinMaxBet_Control.disabled = disabled ? "Disabled" : string.Empty;
            tplMinMax_153_51.MinMaxBet_View = null;
            tplMinMax_153_51.MinMaxBet_Control.MinBetID = "MinBet_153_51";
            tplMinMax_153_51.MinMaxBet_Control.MaxBetID = "MaxBet_153_51";
            tplMinMax_153_51.MinMaxBet_Control.MaxPerMatchID = "MaxPerMatch_153_51";
            tplMinMax_153_51.MinMaxBet_Control.MinBetMin.MinBetMinID = "MinBet_153_51_Min";
            tplMinMax_153_51.MinMaxBet_Control.MaxBetMax.MaxBetMaxID = "MaxBet_153_51_Max";
            tplMinMax_153_51.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxID = "MaxPerMatch_153_51_Max";

            tplMinMax_153_51.MinMaxBet_Control.MinBetVal = Formater.DecFormat(dr["v153minbet_51"], 0);
            tplMinMax_153_51.MinMaxBet_Control.MaxBetVal = Formater.DecFormat(dr["v153maxbet_51"], 0);
            tplMinMax_153_51.MinMaxBet_Control.MaxPerMatchVal = Formater.DecFormat(dr["v153maxpermatch_51"], 0);

            if (Convert.ToDouble(dr["v153MinBet_51_Min"]) == 0)
            {
                tplMinMax_153_51.MinMaxBet_Control.MinBetMin = null;
            }
            else
            {
                tplMinMax_153_51.MinMaxBet_Control.MinBetMin.MinBetMinVal = Formater.DecFormat(dr["v153MinBet_51_Min"], 0);
            }

            if (Convert.ToDouble(dr["v153MaxBet_51_Max"]) == 0)
            {
                tplMinMax_153_51.MinMaxBet_Control.MaxBetMax = null;
            }
            else
            {
                tplMinMax_153_51.MinMaxBet_Control.MaxBetMax.MaxBetMaxVal = Formater.DecFormat(dr["v153MaxBet_51_Max"], 0);
            }

            if (Convert.ToDouble(dr["v153MaxperMatch_51_Max"]) == 0)
            {
                tplMinMax_153_51.MinMaxBet_Control.MaxPerMatchMax = null;
            }
            else
            {
                tplMinMax_153_51.MinMaxBet_Control.MaxPerMatchMax.MaxPerMatch_MaxVal = Formater.DecFormat(dr["v153MaxperMatch_51_Max"], 0);
            }
        }

        public abstract void RenderBetSetting(DataRow dr, TplCore.Element.IElement e);

        public void RenderCommission(
            DataRow dr,
            TplCore.Element.IElement tpl,
            out double comm_151_41,
            out double comm_151_31,
            out double comm_151_51,
            out double comm_152_41,
            out double comm_152_51,
            out double comm_153_51)
        {
            double.TryParse(dr["v151discount_41"].ToString(), out comm_151_41);
            double.TryParse(dr["v151discount_31"].ToString(), out comm_151_31);
            double.TryParse(dr["v151discount_51"].ToString(), out comm_151_51);

            double.TryParse(dr["v152discount_41"].ToString(), out comm_152_41);
            double.TryParse(dr["v152discount_51"].ToString(), out comm_152_51);

            double.TryParse(dr["v153discount_51"].ToString(), out comm_153_51);
        }

        public void RenderCommission(
            DataRow dr,
            TplCore.Element.IElement tpl,
            out PT_Selection comm_151_41,
            out PT_Selection comm_151_31,
            out PT_Selection comm_151_51,
            out PT_Selection comm_152_41,
            out PT_Selection comm_152_51,
            out PT_Selection comm_153_51)
        {
            this.RenderCommission(dr, tpl, out comm_151_41, out comm_151_31, out comm_151_51, out comm_152_41, out comm_152_51, out comm_153_51, false);
        }

        public void RenderCommission(
            DataRow dr,
            TplCore.Element.IElement tpl,
            out PT_Selection comm_151_41,
            out PT_Selection comm_151_31,
            out PT_Selection comm_151_51,
            out PT_Selection comm_152_41,
            out PT_Selection comm_152_51,
            out PT_Selection comm_153_51,
            bool disabled)
        {
            double maxval = 0, currentval = 0;

            double.TryParse(dr["v151discount_41_Max"].ToString(), out maxval);
            double.TryParse(dr["v151discount_41"].ToString(), out currentval);
            comm_151_41 = new PT_Selection(this.TemplateMain, "v151discount_41", currentval, disabled, 0, maxval, COMM_INTERVAL);

            double.TryParse(dr["v151discount_31_Max"].ToString(), out maxval);
            double.TryParse(dr["v151discount_31"].ToString(), out currentval);
            comm_151_31 = new PT_Selection(this.TemplateMain, "v151discount_31", currentval, disabled, 0, maxval, COMM_INTERVAL);

            double.TryParse(dr["v151discount_51_Max"].ToString(), out maxval);
            double.TryParse(dr["v151discount_51"].ToString(), out currentval);
            comm_151_51 = new PT_Selection(this.TemplateMain, "v151discount_51", currentval, disabled, 0, maxval, COMM_INTERVAL);

            double.TryParse(dr["v152discount_41_Max"].ToString(), out maxval);
            double.TryParse(dr["v152discount_41"].ToString(), out currentval);
            comm_152_41 = new PT_Selection(this.TemplateMain, "v152discount_41", currentval, disabled, 0, maxval, COMM_INTERVAL);

            double.TryParse(dr["v152discount_51_Max"].ToString(), out maxval);
            double.TryParse(dr["v152discount_51"].ToString(), out currentval);
            comm_152_51 = new PT_Selection(this.TemplateMain, "v152discount_51", currentval, disabled, 0, maxval, COMM_INTERVAL);

            double.TryParse(dr["v153discount_51_Max"].ToString(), out maxval);
            double.TryParse(dr["v153discount_51"].ToString(), out currentval);
            comm_153_51 = new PT_Selection(this.TemplateMain, "v153discount_51", currentval, disabled, 0, maxval, COMM_INTERVAL);
        }

        public abstract void RenderCommission(DataRow dr, TplCore.Element.IElement e);
    }
}