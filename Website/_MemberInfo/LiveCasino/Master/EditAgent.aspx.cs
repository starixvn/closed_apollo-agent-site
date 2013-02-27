// --------------------------------------------------------------------------
// <copyright file="EditAgent.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.4.13">Format styles</change>
// </history>

namespace Fanex.Age.UI.LiveCasino.Master
{
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.LiveCasinoSetting.Model;
    using Resources;

    /// <summary>
    /// Master edit agent pt-comm
    /// </summary>
    public partial class EditAgent_Page : LiveCasinoBase
    {
        /// <summary>
        /// Render UI
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            base.RenderContent(writer);

            var pt = Data[PtCommSetting] as PTSetting;
            var uplinePT = pt.Upline as PTSetting;
            var uiPT = CasinoUI.MasterEditAgentPT;

            uiPT.MasterPresetPT = Lang.lblPresetPTMaster;
            uiPT.MasterAutoPresetPT = Lang.lblAutoMasterResestPosition;
            uiPT.AgentMaxPT = Lang.lblMaxPTAgent;
            uiPT.AgentMinPT = Lang.MinPTAgent;
            uiPT.AgentCommission = Lang.lblCommPTAgent;

            if (uplinePT.IsDisabledAutoPresetPT)
            {
                uiPT.hiden = "Hiden";
            }
            else
            {
                uiPT.hiden = string.Empty;
            }

            uiPT.MasterPresetPT_vl = new PT_Selection(SettingPage, "pt_1", ToDouble(uplinePT.PresetPT), uplinePT.IsAutoPresetPT, ToDouble(uplinePT.MinPT), ToDouble(uplinePT.MaxPT), ToDouble(IntervalPT), true, true, "PTChanging(this)");
            uiPT.MasterAutoPresetPT_vl = new PT_Selection(SettingPage, "pt_2", ToDouble(uplinePT.AutoPresetPT), !uplinePT.IsAutoPresetPT, 0, ToDouble(uplinePT.MaxPT), ToDouble(IntervalPT), true, true, string.Empty);
            uiPT.MasterIsAutoPresetPT_vl = new PT_AutoCheckBox_Control(SettingPage, "chkAutoPT", uplinePT.IsAutoPresetPT, false, "checkPT(this)");
            
            uiPT.AgentMaxPT_vl = new PT_Selection(SettingPage, "pt_3", ToDouble(pt.MaxPT), false, 0, ToDouble(uplinePT.MaxPT - uplinePT.PresetPT), ToDouble(IntervalPT), true, true, string.Empty);
            uiPT.AgentMinPT_vl = new PT_Selection(SettingPage, "pt_4", ToDouble(pt.MinPT), false, 0, ToDouble(pt.MaxPT), ToDouble(IntervalPT), true, true, string.Empty);
            uiPT.AgentCommission_vl = new PT_Selection(SettingPage, "comm_5", ToDouble(pt.Comm), false, 0, ToDouble(uplinePT.Comm), ToDouble(IntervalComm), true, true, string.Empty);

            SettingPage.PTCommDetail = uiPT;
            SettingPage.mainDiv = "mainDiv";

            writer.Write(this.SettingPage);
        }

        /// <summary>
        /// Registers variables of upline and downline
        /// </summary>
        /// <param name="pt">PT of upline and downline</param>
        protected override void RegisterJsPT(PTSetting pt)
        {
            var uplinePT = pt.Upline;
            decimal maxpt = uplinePT.MaxPT - pt.PresetPT;

            this.RegisterJavascriptObject("PresetMaxPT", this.ToDecimal(uplinePT.MaxPT));
            this.RegisterJavascriptObject("PresetMinPT", this.ToDecimal(uplinePT.MinPT));

            this.RegisterJavascriptObject("MaxPT_Max", this.ToDecimal(maxpt));
            this.RegisterJavascriptObject("MaxPT_Min", 0);

            this.RegisterJavascriptObject("MinPT_Max", this.ToDecimal(uplinePT.MaxPT));
            this.RegisterJavascriptObject("MinPT_Min", 0);

            this.RegisterJavascriptObject("oldAuto", uplinePT.IsAutoPresetPT);
        }
    }
}