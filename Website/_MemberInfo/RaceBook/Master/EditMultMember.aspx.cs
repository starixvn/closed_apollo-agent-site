// --------------------------------------------------------------------------
// <copyright file="EditMultMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.06.01">Improve performance, Check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook.Master
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model.BL.Racing;
    using Sunplus.Agent.UI._Components.RacingStatus_Control;
    using Sunplus.Agent.UI._MemberInfo.RaceBook.MinMaxBet_Control;
    using Sunplus.Agent.UI.PositionTaking.RaceBook.Master;
    using Sunplus.Agent.Utilities;

    public partial class EditMultMember_Page : OnMember_Page
    {
        private EditMultipleMember_Template tplMain = new EditMultipleMember_Template();
        private bool isDisabled_CopyWin = false;

        public override void RenderContent(HtmlTextWriter writer)
        {
            this.IsMultiple = true;

            DataTable racingData = (View as Dictionary<string, DataTable>)["RacingData"];
            DataRow dr = racingData.Rows[0];
            this.tplMain.errorMessage = new ErrorMsg_Control(this.TemplateMain);
            this.RenderStatus(dr, this.tplMain.PositionTaking);

            this.RenderCommission(dr, this.tplMain.PositionTaking);
            this.RenderBetSetting(dr, this.tplMain.PositionTaking);

            this.RenderPositionTaking(dr, this.tplMain.PositionTaking);
            this.JavaScriptRegister("isMultiple", "1");
            this.RegisterClientVariables();
            this.RegisterClientHiddenControl();

            this.TemplateMain.actionFrm = "UpdateMember.ashx";
            this.TemplateMain.RaceBookJs.extJs = "EditMultMember.js" + "?" + Sunplus.Agent.Model.BL.Definitions.AppSettings.JsVersion;
            this.TemplateMain.RaceBookJs.ExtJs_More = null;

            this.TemplateMain.title = this.tplMain.title = Resources.Racing.editmultpt;

            this.tplMain.button_text = Resources.Racing.submit;
            this.tplMain.title = Resources.Racing.edithorseracing;
            this.tplMain.CopyAll.copy_all = Resources.Racing.copyallfromuktotewin;

            TemplateMain.content = this.tplMain;
            writer.Write(this.TemplateMain);
        }

        public override object InvokeData(Hashtable p)
        {
            Dictionary<string, DataTable> viewData = new Dictionary<string, DataTable>();

            RacingBO bo = new RacingBO(CurrentUserSession.NetSessionId);

            Hashtable outputs = null;
            DataSet ds = bo.Master_GetHRSetting_Member(CustId, out outputs);

            viewData.Add("RacingData", ds.Tables[0]);

            return viewData;
        }

        public override void RenderStatus(DataRow dr, TplCore.Element.IElement e)
        {
            OnMember_Template tpl = e as OnMember_Template;

            RacingStatus_Control_Template[] ts = new RacingStatus_Control_Template[6];
            this.RenderStatus(dr, ref ts[0], ref ts[1], ref ts[2], ref ts[3], ref ts[4], ref ts[5], true);

            for (int i = 0; i < 6; i++)
            {
                ts[i].upline_text = null;
                ts[i].Downline = null;
            }

            tpl.Section_151.Section_151_31.status.status_text = tpl.Section_151.Section_151_41.status.status_text = tpl.Section_151.Section_151_51.status.status_text = tpl.Section_152.Section_152_41.status.status_text = tpl.Section_152.Section_152_51.status.status_text = tpl.Section_153.Section_153_51.status.status_text = Resources.Lang.status;

            tpl.Section_151.Section_151_41.status_disable = ts[0];
            tpl.Section_151.Section_151_31.status_disable = ts[1];
            tpl.Section_151.Section_151_51.status_disable = ts[2];
            tpl.Section_152.Section_152_41.status_disable = ts[3];
            tpl.Section_152.Section_152_51.status_disable = ts[4];
            tpl.Section_153.Section_153_51.status_disable = ts[5];

            tpl.Section_151.Section_151_41.status.status_disable_allowupdate_checkbox = null;
            tpl.Section_151.Section_151_31.status.status_disable_allowupdate_checkbox = null;
            tpl.Section_151.Section_151_51.status.status_disable_allowupdate_checkbox = null;

            tpl.Section_152.Section_152_41.status.status_disable_allowupdate_checkbox = null;
            tpl.Section_152.Section_152_51.status.status_disable_allowupdate_checkbox = null;

            tpl.Section_153.Section_153_51.status.status_disable_allowupdate_checkbox = null;
        }

        public override void RenderBetSetting(DataRow dr, TplCore.Element.IElement e)
        {
            OnMember_Template tpl = e as OnMember_Template;

            tpl.Section_151.Section_151_31.betsetting_text = tpl.Section_151.Section_151_41.betsetting_text = tpl.Section_151.Section_151_51.betsetting_text
            = tpl.Section_152.Section_152_41.betsetting_text = tpl.Section_152.Section_152_51.betsetting_text
            = tpl.Section_153.Section_153_51.betsetting_text = Resources.Racing.betsetting;

            MinMaxBet_Control_Template[] ts = new MinMaxBet_Control_Template[6];
            this.RenderViewBetSetting(dr, ref ts[0], ref ts[1], ref ts[2], ref ts[3], ref ts[4], ref ts[5]);

            tpl.Section_151.Section_151_41.betsetting_val = ts[0];
            tpl.Section_151.Section_151_31.betsetting_val = ts[1];
            tpl.Section_151.Section_151_51.betsetting_val = ts[2];
            tpl.Section_152.Section_152_41.betsetting_val = ts[3];
            tpl.Section_152.Section_152_51.betsetting_val = ts[4];
            tpl.Section_153.Section_153_51.betsetting_val = ts[5];
        }

        public override void RenderCommission(DataRow dr, TplCore.Element.IElement e)
        {
            OnMember_Template tpl = e as OnMember_Template;

            tpl.Section_151.Section_151_31.commission_allowupdate_checkbox = tpl.Section_151.Section_151_41.commission_allowupdate_checkbox = tpl.Section_151.Section_151_51.commission_allowupdate_checkbox
                = tpl.Section_152.Section_152_41.commission_allowupdate_checkbox = tpl.Section_152.Section_152_51.commission_allowupdate_checkbox
                = tpl.Section_153.Section_153_51.commission_allowupdate_checkbox = null;

            tpl.Section_151.Section_151_31.commission_text = tpl.Section_151.Section_151_41.commission_text = tpl.Section_151.Section_151_51.commission_text
                = tpl.Section_152.Section_152_41.commission_text = tpl.Section_152.Section_152_51.commission_text
                = tpl.Section_153.Section_153_51.commission_text = Resources.Racing.commission;

            tpl.Section_151.Section_151_31.agent_com = tpl.Section_151.Section_151_41.agent_com = tpl.Section_151.Section_151_51.agent_com
                = tpl.Section_152.Section_152_41.agent_com = tpl.Section_152.Section_152_51.agent_com
                = tpl.Section_153.Section_153_51.agent_com = Resources.Racing.agentcomm;

            tpl.Section_151.Section_151_31.member_com = tpl.Section_151.Section_151_41.member_com = tpl.Section_151.Section_151_51.member_com
                = tpl.Section_152.Section_152_41.member_com = tpl.Section_152.Section_152_51.member_com
                = tpl.Section_153.Section_153_51.member_com = Resources.Racing.membercomm;

            tpl.Section_151.Section_151_41.member_com_val = Formater.FormatNumber4(dr["v151discount_41"]);
            tpl.Section_151.Section_151_31.member_com_val = Formater.FormatNumber4(dr["v151discount_31"]);
            tpl.Section_151.Section_151_51.member_com_val = Formater.FormatNumber4(dr["v151discount_51"]);
            tpl.Section_152.Section_152_41.member_com_val = Formater.FormatNumber4(dr["v152discount_41"]);
            tpl.Section_152.Section_152_51.member_com_val = Formater.FormatNumber4(dr["v152discount_51"]);
            tpl.Section_153.Section_153_51.member_com_val = Formater.FormatNumber4(dr["v153discount_51"]);

            tpl.Section_151.Section_151_41.agent_com_val = Formater.FormatNumber4(dr["v151discount_41_ag"]);
            tpl.Section_151.Section_151_31.agent_com_val = Formater.FormatNumber4(dr["v151discount_31_ag"]);
            tpl.Section_151.Section_151_51.agent_com_val = Formater.FormatNumber4(dr["v151discount_51_ag"]);
            tpl.Section_152.Section_152_41.agent_com_val = Formater.FormatNumber4(dr["v152discount_41_ag"]);
            tpl.Section_152.Section_152_51.agent_com_val = Formater.FormatNumber4(dr["v152discount_51_ag"]);
            tpl.Section_153.Section_153_51.agent_com_val = Formater.FormatNumber4(dr["v153discount_51_ag"]);
        }

        public void RenderPositionTakingInfo(OnMember_Template tpl)
        {
            tpl.Section_151.title = Resources.Racing.horseracing;
            tpl.Section_152.title = Resources.Racing.greyhound;
            tpl.Section_153.title = Resources.Racing.harness;

            tpl.Section_151.Section_151_31.positiontaking_text = tpl.Section_151.Section_151_41.positiontaking_text = tpl.Section_151.Section_151_51.positiontaking_text
                = tpl.Section_152.Section_152_41.positiontaking_text = tpl.Section_152.Section_152_51.positiontaking_text
                = tpl.Section_153.Section_153_51.positiontaking_text = Resources.Racing.positiontaking;

            // 151
            tpl.Section_151.Section_151_31.win_text = Resources.Racing.win;
            tpl.Section_151.Section_151_31.place_text = Resources.Racing.place;

            tpl.Section_151.Section_151_41.win_text = Resources.Racing.win;
            tpl.Section_151.Section_151_41.place_text = Resources.Racing.place;

            tpl.Section_151.Section_151_51.win_text = Resources.Racing.win;
            tpl.Section_151.Section_151_51.place_text = Resources.Racing.place;

            tpl.Section_151.Section_151_31.agent_text = Resources.Racing.agent;
            tpl.Section_151.Section_151_31.master_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_31.Automaster_text = Resources.Racing.automasterpreset;

            tpl.Section_151.Section_151_41.agent_text = Resources.Racing.agent;
            tpl.Section_151.Section_151_41.master_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_41.Automaster_text = Resources.Racing.automasterpreset;

            tpl.Section_151.Section_151_51.agent_text = Resources.Racing.agent;
            tpl.Section_151.Section_151_51.master_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_51.Automaster_text = Resources.Racing.automasterpreset;

            tpl.Section_151.Section_151_31.menu_text = Resources.Racing.satote;
            tpl.Section_151.Section_151_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_151.Section_151_51.menu_text = Resources.Racing.autote;

            tpl.Section_151.Section_151_31.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_41.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_51.copy_from_win = Resources.Racing.copyfromwin;

            tpl.Section_152.Section_152_41.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_41.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_51.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_51.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_41.agent_text = Resources.Racing.agent;
            tpl.Section_152.Section_152_41.master_text = Resources.Racing.masterpreset;
            tpl.Section_152.Section_152_41.Automaster_text = Resources.Racing.automasterpreset;

            tpl.Section_152.Section_152_51.agent_text = Resources.Racing.agent;
            tpl.Section_152.Section_152_51.master_text = Resources.Racing.masterpreset;
            tpl.Section_152.Section_152_51.Automaster_text = Resources.Racing.automasterpreset;

            tpl.Section_152.Section_152_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_152.Section_152_51.menu_text = Resources.Racing.autote;

            tpl.Section_152.Section_152_41.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_152.Section_152_51.copy_from_win = Resources.Racing.copyfromwin;

            tpl.Section_153.Section_153_51.win_text = Resources.Racing.win;
            tpl.Section_153.Section_153_51.place_text = Resources.Racing.place;

            tpl.Section_153.Section_153_51.agent_text = Resources.Racing.agent;
            tpl.Section_153.Section_153_51.master_text = Resources.Racing.masterpreset;
            tpl.Section_153.Section_153_51.Automaster_text = Resources.Racing.automasterpreset;

            tpl.Section_153.Section_153_51.menu_text = Resources.Racing.autote;

            tpl.Section_153.Section_153_51.copy_from_win = Resources.Racing.copyfromwin;
        }

        public void RenderPositionTaking(DataRow dr, OnMember_Template tpl)
        {
            this.RenderPositionTakingInfo(tpl);

            this.RenderPT_151_41(dr, tpl.Section_151.Section_151_41, ref this.isDisabled_CopyWin);
            this.tplMain.CopyAll.disabled_copyall = this.isDisabled_CopyWin ? "disabled='true'" : string.Empty;

            this.RenderPT_151_31(dr, tpl.Section_151.Section_151_31);
            this.RenderPT_151_51(dr, tpl.Section_151.Section_151_51);
            this.RenderPT_152_41(dr, tpl.Section_152.Section_152_41);
            this.RenderPT_152_51(dr, tpl.Section_152.Section_152_51);
            this.RenderPT_153_51(dr, tpl.Section_153.Section_153_51);

            bool isdisableAutoMaster = Convert.ToBoolean(dr["DisableAutoMPT"]);
            if (isdisableAutoMaster)
            {
                tpl.Section_151.Section_151_31.hideAuto = "hidden";
                tpl.Section_151.Section_151_41.hideAuto = "hidden";
                tpl.Section_151.Section_151_51.hideAuto = "hidden";

                tpl.Section_152.Section_152_41.hideAuto = "hidden";
                tpl.Section_152.Section_152_51.hideAuto = "hidden";
                tpl.Section_153.Section_153_51.hideAuto = "hidden";
            }
            else
            {
                tpl.Section_151.Section_151_31.hideAuto = string.Empty;
                tpl.Section_151.Section_151_41.hideAuto = string.Empty;
                tpl.Section_151.Section_151_51.hideAuto = string.Empty;

                tpl.Section_152.Section_152_41.hideAuto = string.Empty;
                tpl.Section_152.Section_152_51.hideAuto = string.Empty;
                tpl.Section_153.Section_153_51.hideAuto = string.Empty;
            }
        }
    }
}