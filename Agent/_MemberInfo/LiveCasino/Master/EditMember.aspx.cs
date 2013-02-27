// --------------------------------------------------------------------------
// <copyright file="EditMember.aspx.cs" company="Nexcel Solutions Vietnam">
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
    /// Master edits member pt - comm
    /// </summary>
    public partial class EditMember_Page : LiveCasinoBase
    {
        /// <summary>
        /// Renders UI
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            base.RenderContent(writer);

            var pt = (Data[PtCommSetting] as PTSetting).Upline as PTSetting;
            var uplinePT = pt.Upline as PTSetting;
            var uiPT = CasinoUI.MasterEditMemberPT;
            var isautoagentpt = pt as PTSetting;

            if (uplinePT.IsDisabledAutoPresetPT)
            {
                uiPT.hiden = "Hiden";
            }
            else
            {
                uiPT.hiden = string.Empty;
            }

            uiPT.AgentPT = Lang.lblPositionTakingAgent;
            uiPT.MasterAutoPT = Lang.lblMasterAuto + " " + Lang.positiontaking;
            uiPT.MasterPT = Lang.lblPositionTakingMaster;

            uiPT.MasterAutoPT_vl = new PT_Selection(SettingPage, "pt_2", ToDouble(uplinePT.AutoPresetPT), !uplinePT.IsAutoPresetPT, 0, ToDouble(uplinePT.MaxAutoPresetPT), ToDouble(IntervalPT), true, true, string.Empty);
            
            if (!isautoagentpt.IsAutoPresetPT)
            {
                if (uplinePT.MinPT > uplinePT.MaxPT)
                {
                    uiPT.MasterPT_vl = new PT_Selection(SettingPage, "pt_1", ToDouble(uplinePT.PresetPT), uplinePT.IsAutoPresetPT, ToDouble(uplinePT.PresetPT), ToDouble(uplinePT.PresetPT), ToDouble(IntervalPT), true, true, string.Empty);
                }
                else
                {
                    uiPT.MasterPT_vl = new PT_Selection(SettingPage, "pt_1", ToDouble(uplinePT.PresetPT), uplinePT.IsAutoPresetPT, ToDouble(uplinePT.MinPT), ToDouble(uplinePT.MaxPT), ToDouble(IntervalPT), true, true, string.Empty);
                }
            }
            else
            {
                uiPT.MasterPT_vl = ToDecimal(uplinePT.PresetPT).ToString();
            }

            uiPT.MasterIsAutoPT_vl = new PT_AutoCheckBox_Control(SettingPage, "chkAutoPT", uplinePT.IsAutoPresetPT, false, "checkPT(this)");
            uiPT.AgentPT_vl = ToDecimal(pt.PresetPT).ToString();
            
            SettingPage.PTCommDetail = uiPT;
            SettingPage.mainDiv = "Div850px";

            writer.Write(this.SettingPage);
        }

        /// <summary>
        /// Registers variables for javascript to render combobox
        /// </summary>
        /// <param name="pt">Upline and downline pt</param>
        protected override void RegisterJsPT(PTSetting pt)
        {
            var uplinePT = pt.Upline;

            this.RegisterJavascriptObject("PresetMaxPT", this.ToDecimal(uplinePT.MaxPT));
            this.RegisterJavascriptObject("PresetMinPT", this.ToDecimal(uplinePT.MinPT));
        }
    }
}