// --------------------------------------------------------------------------
// <copyright file="EditMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.4.13">Improve styles</change>
//   <change who="Kaden.Trinh" date="2012.05.31">Refactor code and check StyleCop</change>
// </history>
namespace Fanex.Age.UI.LiveCasino.Agent
{
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.LiveCasinoSetting.Model;
    using Resources;

    /// <summary>
    /// UI for agent edits member of Live Casino
    /// </summary>
    public partial class EditMember_Page : LiveCasinoBase
    {
        /// <summary>
        /// Render UI html
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            base.RenderContent(writer);

            var pt = Data[PtCommSetting] as PTSetting;
            var uplinePT = pt.Upline as PTSetting;

            var uiPT = CasinoUI.AgentEditMemberPT;

            var agentPT = CasinoUI.PositionTakingCtrl;
            var agentPT_items = CasinoUI.PositionTakingCtrl_Items;

            var memberComm = CasinoUI.CommControl;
            var memberComm_items = CasinoUI.CommControl_Items;

            uiPT.MemberCommission = Lang.lblMemberCommission;
            uiPT.AgentPresetPT = Lang.lblPresetPTAgent;

            uiPT.AgentPresetPT_vl = new PT_Selection(SettingPage, "pt_1", ToDouble(uplinePT.PresetPT), uplinePT.IsAutoPresetPT, ToDouble(uplinePT.MinPT), ToDouble(uplinePT.MaxPT), ToDouble(IntervalPT), true, true, string.Empty);
            uiPT.MemberCommission_vl = new PT_Selection(SettingPage, "comm_2", ToDouble(pt.Comm), false, 0, ToDouble(uplinePT.Comm), ToDouble(IntervalComm), true, true, string.Empty);

            SettingPage.PTCommDetail = uiPT;
            SettingPage.mainDiv = "Div850px";

            writer.Write(this.SettingPage);
        }

        /// <summary>
        /// Register variables for javascript to calculate PT values
        /// </summary>
        /// <param name="pt">PT of current upline and downline</param>
        protected override void RegisterJsPT(PTSetting pt)
        {
            var uplinePT = pt.Upline;

            this.RegisterJavascriptObject("PresetMaxPT", this.ToDecimal(uplinePT.MaxPT));
            this.RegisterJavascriptObject("PresetMinPT", this.ToDecimal(uplinePT.MinPT));
        }
    }
}