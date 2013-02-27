// --------------------------------------------------------------------------
// <copyright file="AddNewMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.06.01">Improve performance, Check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook.Agent
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
    using Sunplus.Agent.UI.PositionTaking.RaceBook.Agent;
    using Sunplus.Agent.Utilities;

    public partial class AddNewMember_Page : OnMember_Page
    {
        public override void RenderStatus(DataRow dr, TplCore.Element.IElement e)
        {
            OnMember_Template tpl = e as OnMember_Template;

            RacingStatus_Control_Template[] ts = new RacingStatus_Control_Template[6];
            this.RenderStatus(dr, ref ts[0], ref ts[1], ref ts[2], ref ts[3], ref ts[4], ref ts[5]);

            for (int i = 0; i < 6; i++)
            {
                ts[i].upline_text = null;
                ts[i].Downline = null;
            }

            tpl.Section_151.Section_151_41.status_disable = ts[0];
            tpl.Section_151.Section_151_31.status_disable = ts[1];
            tpl.Section_151.Section_151_51.status_disable = ts[2];
            tpl.Section_152.Section_152_41.status_disable = ts[3];
            tpl.Section_152.Section_152_51.status_disable = ts[4];
            tpl.Section_153.Section_153_51.status_disable = ts[5];

            tpl.Section_151.Section_151_41.status = null;
            tpl.Section_151.Section_151_31.status = null;
            tpl.Section_151.Section_151_51.status = null;

            tpl.Section_152.Section_152_41.status = null;
            tpl.Section_152.Section_152_51.status = null;

            tpl.Section_153.Section_153_51.status = null;
        }

        public override void RenderBetSetting(DataRow dr, TplCore.Element.IElement e)
        {
            OnMember_Template tpl = e as OnMember_Template;

            tpl.Section_151.Section_151_31.betsetting_text = tpl.Section_151.Section_151_41.betsetting_text = tpl.Section_151.Section_151_51.betsetting_text
            = tpl.Section_152.Section_152_41.betsetting_text = tpl.Section_152.Section_152_51.betsetting_text
            = tpl.Section_153.Section_153_51.betsetting_text = Resources.Racing.betsetting;

            MinMaxBet_Control_Template[] ts = new MinMaxBet_Control_Template[6];
            this.RenderBetSetting(dr, ref ts[0], ref ts[1], ref ts[2], ref ts[3], ref ts[4], ref ts[5]);

            tpl.Section_151.Section_151_41.betsetting_val = ts[0];
            tpl.Section_151.Section_151_31.betsetting_val = ts[1];
            tpl.Section_151.Section_151_51.betsetting_val = ts[2];
            tpl.Section_152.Section_152_41.betsetting_val = ts[3];
            tpl.Section_152.Section_152_51.betsetting_val = ts[4];
            tpl.Section_153.Section_153_51.betsetting_val = ts[5];

            tpl.Section_151.Section_151_41.betsetting_allowupdate_checkbox = null;
            tpl.Section_151.Section_151_31.betsetting_allowupdate_checkbox = null;
            tpl.Section_151.Section_151_51.betsetting_allowupdate_checkbox = null;

            tpl.Section_152.Section_152_41.betsetting_allowupdate_checkbox = null;
            tpl.Section_152.Section_152_51.betsetting_allowupdate_checkbox = null;

            tpl.Section_153.Section_153_51.betsetting_allowupdate_checkbox = null;
        }

        public override void RenderCommission(DataRow dr, TplCore.Element.IElement e)
        {
            OnMember_Template tpl = e as OnMember_Template;

            PT_Selection[] ts = new PT_Selection[6];

            tpl.Section_151.Section_151_31.commission_text = tpl.Section_151.Section_151_41.commission_text = tpl.Section_151.Section_151_51.commission_text
                = tpl.Section_152.Section_152_41.commission_text = tpl.Section_152.Section_152_51.commission_text
                = tpl.Section_153.Section_153_51.commission_text = Resources.Racing.commission;

            tpl.Section_151.Section_151_31.agent_com = tpl.Section_151.Section_151_41.agent_com = tpl.Section_151.Section_151_51.agent_com
                = tpl.Section_152.Section_152_41.agent_com = tpl.Section_152.Section_152_51.agent_com
                = tpl.Section_153.Section_153_51.agent_com = Resources.Racing.agentcomm;

            tpl.Section_151.Section_151_31.member_com = tpl.Section_151.Section_151_41.member_com = tpl.Section_151.Section_151_51.member_com
                = tpl.Section_152.Section_152_41.member_com = tpl.Section_152.Section_152_51.member_com
                = tpl.Section_153.Section_153_51.member_com = Resources.Racing.membercomm;

            this.RenderCommission(dr, tpl, out ts[0], out ts[1], out ts[2], out ts[3], out ts[4], out ts[5]);

            tpl.Section_151.Section_151_41.member_com_val = ts[0];
            tpl.Section_151.Section_151_31.member_com_val = ts[1];
            tpl.Section_151.Section_151_51.member_com_val = ts[2];
            tpl.Section_152.Section_152_41.member_com_val = ts[3];
            tpl.Section_152.Section_152_51.member_com_val = ts[4];
            tpl.Section_153.Section_153_51.member_com_val = ts[5];

            tpl.Section_151.Section_151_41.agent_com_val = Formater.FormatNumber4(dr["v151discount_41_max"]);
            tpl.Section_151.Section_151_31.agent_com_val = Formater.FormatNumber4(dr["v151discount_31_max"]);
            tpl.Section_151.Section_151_51.agent_com_val = Formater.FormatNumber4(dr["v151discount_51_max"]);

            tpl.Section_152.Section_152_41.agent_com_val = Formater.FormatNumber4(dr["v152discount_41_max"]);
            tpl.Section_152.Section_152_51.agent_com_val = Formater.FormatNumber4(dr["v152discount_51_max"]);

            tpl.Section_153.Section_153_51.agent_com_val = Formater.FormatNumber4(dr["v153discount_51_max"]);

            tpl.Section_151.Section_151_41.commission_allowupdate_checkbox = null;
            tpl.Section_151.Section_151_31.commission_allowupdate_checkbox = null;
            tpl.Section_151.Section_151_51.commission_allowupdate_checkbox = null;

            tpl.Section_152.Section_152_41.commission_allowupdate_checkbox = null;
            tpl.Section_152.Section_152_51.commission_allowupdate_checkbox = null;

            tpl.Section_153.Section_153_51.commission_allowupdate_checkbox = null;
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            this.JavaScriptRegister("isEdit", false);
            AddNewMember_Template tpl = new AddNewMember_Template();

            DataTable racingData = (View as Dictionary<string, DataTable>)["RacingData"];
            DataRow dr = racingData.Rows[0];
            tpl.errorMessage = new ErrorMsg_Control(TemplateMain);
            this.RenderCustomerInfo(dr, tpl.UserInfo);
            this.RenderPositionTaking(dr, tpl.PositionTaking);

            this.RenderStatus(dr, tpl.PositionTaking);

            this.RenderCommission(dr, tpl.PositionTaking);
            this.RenderBetSetting(dr, tpl.PositionTaking);

            this.RegisterClientVariables();

            TemplateMain.actionFrm = "UpdateMember.ashx";
            TemplateMain.RaceBookJs.extJs = "EditMember.js" + "?" + Sunplus.Agent.Model.BL.Definitions.AppSettings.JsVersion;
            TemplateMain.RaceBookJs.ExtJs_More.extJs_More = "LimitMinMaxBet.js" + "?" + Sunplus.Agent.Model.BL.Definitions.AppSettings.JsVersion;

            tpl.UserInfo.CopyAll.copy_all = Resources.Racing.copyallfromuktotewin;
            tpl.button_text = Resources.Racing.submit;

            TemplateMain.content = tpl;
            TemplateMain.hidden_PT = string.Empty;

            writer.Write(this.TemplateMain);
        }

        public override object InvokeData(Hashtable p)
        {
            Dictionary<string, DataTable> viewData = new Dictionary<string, DataTable>();

            RacingBO bo = new RacingBO(CurrentUserSession.NetSessionId);

            Hashtable outputs = null;
            DataSet ds = bo.GetHRSetting_Member(CustId, out outputs);

            viewData.Add("RacingData", ds.Tables[0]);

            return viewData;
        }

        public void RenderPositionTaking(DataRow dr, OnMember_Template tpl)
        {
            this.RenderPositionTakingInfo(tpl);

            this.RenderPT_151_41(dr, tpl.Section_151.Section_151_41);
            this.RenderPT_151_31(dr, tpl.Section_151.Section_151_31);
            this.RenderPT_151_51(dr, tpl.Section_151.Section_151_51);
            this.RenderPT_152_41(dr, tpl.Section_152.Section_152_41);
            this.RenderPT_152_51(dr, tpl.Section_152.Section_152_51);
            this.RenderPT_153_51(dr, tpl.Section_153.Section_153_51);
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

            tpl.Section_151.Section_151_31.menu_text = Resources.Racing.satote;
            tpl.Section_151.Section_151_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_151.Section_151_51.menu_text = Resources.Racing.autote;

            tpl.Section_151.Section_151_31.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_41.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_51.copy_from_win = Resources.Racing.copyfromwin;

            // 152
            tpl.Section_152.Section_152_41.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_41.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_51.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_51.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_152.Section_152_51.menu_text = Resources.Racing.autote;

            tpl.Section_152.Section_152_41.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_152.Section_152_51.copy_from_win = Resources.Racing.copyfromwin;

            // 153
            tpl.Section_153.Section_153_51.win_text = Resources.Racing.win;
            tpl.Section_153.Section_153_51.place_text = Resources.Racing.place;
            tpl.Section_153.Section_153_51.menu_text = Resources.Racing.autote;

            tpl.Section_153.Section_153_51.copy_from_win = Resources.Racing.copyfromwin;
        }
    }
}