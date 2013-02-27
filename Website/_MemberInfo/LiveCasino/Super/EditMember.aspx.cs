// --------------------------------------------------------------------------
// <copyright file="EditMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.4.13">Format styles</change>
// </history>

namespace Fanex.Age.UI.LiveCasino
{
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.LiveCasinoSetting.Model;
    using Resources;

    /// <summary>
    /// Super edits member pt-comm
    /// </summary>
    public partial class Super_EditMember : LiveCasinoBase
    {
        /// <summary>
        /// Render UI
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public override void RenderContent(HtmlTextWriter writer)
        {            
            base.RenderContent(writer);

            var pt = Data[PtCommSetting] as PTSetting;
            var uplinePT = pt.Upline.Upline.Upline as PTSetting;
            var superpt = CasinoUI.PositionTakingCtrl;
            var superptItems = CasinoUI.PositionTakingCtrl_Items;
            var superautopt = CasinoUI.PositionTakingCtrl;
            var superautoptItems = CasinoUI.PositionTakingCtrl_Items;

            var masterpt = pt.Upline.Upline as PTSetting;
            var agentpt = pt.Upline as PTSetting;
            var isautomasterpt = pt.Upline.Upline as PTSetting;

            var uiPT = CasinoUI.SuperEditMemberPT;

            uiPT.SuperPT = Lang.lblPositionTakingSuper;
            uiPT.MasterPT = Lang.lblPositionTakingMaster;
            uiPT.AgentPT = Lang.lblPositionTakingAgent;
            uiPT.SuperAutoPT = Lang.lblAutoPTSuper;

            if (!isautomasterpt.IsAutoPresetPT)
            {
                if (uplinePT.MinPT > uplinePT.MaxPT)
                {
                    uiPT.SuperPT_vl = new PT_Selection(SettingPage, "pt_1", ToDouble(uplinePT.PresetPT), uplinePT.IsAutoPresetPT, ToDouble(uplinePT.PresetPT), ToDouble(uplinePT.PresetPT), ToDouble(IntervalPT), true, true, string.Empty);
                }
                else
                {
                    uiPT.SuperPT_vl = new PT_Selection(SettingPage, "pt_1", ToDouble(uplinePT.PresetPT), uplinePT.IsAutoPresetPT, ToDouble(uplinePT.MinPT), ToDouble(uplinePT.MaxPT), ToDouble(IntervalPT), true, true, string.Empty);
                }
            }
            else
            {
                uiPT.SuperPT_vl = ToDecimal(uplinePT.PresetPT).ToString();
            }

            uiPT.SuperAutoPT_vl = new PT_Selection(SettingPage, "pt_2", ToDouble(uplinePT.AutoPresetPT), !uplinePT.IsAutoPresetPT, 0, ToDouble(uplinePT.MaxAutoPresetPT), ToDouble(IntervalPT), true, true, string.Empty);
            uiPT.SuperIsAutoPT_vl = new PT_AutoCheckBox_Control(SettingPage, "chkAutoPT", uplinePT.IsAutoPresetPT, false, "checkPT(this)");

            uiPT.MasterPT_vl = ToDecimal(masterpt.PresetPT).ToString();
            uiPT.AgentPT_vl = ToDecimal(agentpt.PresetPT).ToString();
                      
            SettingPage.PTCommDetail = uiPT;
            SettingPage.mainDiv = "Div850px";

            writer.Write(this.SettingPage);
        }

        /// <summary>
        /// Registers PT of upline and downline
        /// </summary>
        /// <param name="pt">PT of upline and downline</param>
        protected override void RegisterJsPT(PTSetting pt)
        {
            var uplinePT = pt.Upline.Upline.Upline;

            this.RegisterJavascriptObject("PresetMaxPT", this.ToDecimal(uplinePT.MaxPT));
            this.RegisterJavascriptObject("PresetMinPT", this.ToDecimal(uplinePT.MinPT));
        }
    }
}