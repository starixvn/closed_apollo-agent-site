// --------------------------------------------------------------------------
// <copyright file="EditMaster.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.4.13">Format styles</change>
// </history>

namespace Fanex.Age.UI.LiveCasino.Super
{
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.LiveCasinoSetting.Model;
    using Resources;

    /// <summary>
    /// Super edit master PT - game setting
    /// </summary>
    public partial class EditMaster_Page : LiveCasinoBase
    {
        /// <summary>
        /// Render UI
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            base.RenderContent(writer);

            // Only set PT/Comm in this method
            var pt = Data[PtCommSetting] as PTSetting;
            var uplinePT = pt.Upline as PTSetting;
            var uiPT = CasinoUI.SuperEditMasterPT;

            uiPT.SuperPresetPT = Lang.lblPresetPTSuper;
            uiPT.SuperAutoPresetPT = Lang.lblAutoPTSuper;

            uiPT.MasterMaxPT = Lang.lblMaxPTMaster;
            uiPT.MasterMinPT = Lang.MinPTMaster;
            uiPT.MasterCommission = Lang.lblCommPTMaster;

            uiPT.SuperPresetPT_vl = new PT_Selection(SettingPage, "pt_1", ToDouble(uplinePT.PresetPT), uplinePT.IsAutoPresetPT, ToDouble(uplinePT.MinPT), ToDouble(uplinePT.MaxPT), IntervalPT, true, true, "PTChanging(this)");
            uiPT.SuperAutoPresetPT_vl = new PT_Selection(SettingPage, "pt_2", ToDouble(uplinePT.AutoPresetPT), !uplinePT.IsAutoPresetPT, 0, ToDouble(uplinePT.MaxPT), IntervalPT, true, true, string.Empty);
            uiPT.SuperIsAutoPresetPT_vl = new PT_AutoCheckBox_Control(SettingPage, "chkAutoPT", uplinePT.IsAutoPresetPT, false, "checkPT(this)");

            uiPT.MasterMaxPT_vl = new PT_Selection(SettingPage, "pt_3", ToDouble(pt.MaxPT), false, 0, ToDouble(uplinePT.MaxPT - uplinePT.PresetPT), IntervalPT, true, true, string.Empty);
            uiPT.MasterMinPT_vl = new PT_Selection(SettingPage, "pt_4", ToDouble(pt.MinPT), false, 0, ToDouble(pt.MaxPT), IntervalPT, true, true, string.Empty);
            uiPT.MasterCommission_vl = new PT_Selection(SettingPage, "comm_5", ToDouble(pt.Comm), false, 0, ToDouble(uplinePT.Comm), IntervalComm, true, true, string.Empty);

            SettingPage.PTCommDetail = uiPT;
            SettingPage.mainDiv = "mainDiv";

            writer.Write(this.SettingPage);
        }

        /// <summary>
        /// Registers variables for javascript to render combobox
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