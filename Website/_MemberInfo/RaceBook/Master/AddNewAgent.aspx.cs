// --------------------------------------------------------------------------
// <copyright file="AddNewAgent.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.06.01">Improve performance, check StypeCop</change>
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

    public partial class AddNewAgent_Page : OnAgent_Page
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            this.JavaScriptRegister("isEdit", false);
            AddNewAgent_Template tpl = new AddNewAgent_Template();

            DataTable racingData = (View as Dictionary<string, DataTable>)["RacingData"];
            DataRow dr = racingData.Rows[0];
            tpl.errorMessage = new ErrorMsg_Control(TemplateMain);
            this.RenderStatus(dr, tpl.PositionTaking);
            this.RenderCustomerInfo(dr, tpl.UserInfo);
            this.RenderCommission(dr, tpl.PositionTaking);
            this.RenderBetSetting(dr, tpl.PositionTaking);

            this.RenderPositionTaking(dr, tpl.PositionTaking);

            this.RegisterClientVariables();
            this.RegisterClientHiddenControl();

            TemplateMain.actionFrm = "UpdateAgent.ashx";
            TemplateMain.RaceBookJs.extJs = "AddNewAgent.js" + "?" + Sunplus.Agent.Model.BL.Definitions.AppSettings.JsVersion;
            TemplateMain.RaceBookJs.ExtJs_More.extJs_More = "ConfirmAutoPT.js" + "?" + Sunplus.Agent.Model.BL.Definitions.AppSettings.JsVersion;

            tpl.UserInfo.CopyAll.copy_all = Resources.Racing.copyallfromuktotewin;
            tpl.button_text = Resources.Racing.submit;

            TemplateMain.content = tpl;
            writer.Write(this.TemplateMain);
        }

        public override object InvokeData(Hashtable p)
        {
            Dictionary<string, DataTable> viewData = new Dictionary<string, DataTable>();

            RacingBO bo = new RacingBO(CurrentUserSession.NetSessionId);

            Hashtable outputs = null;
            DataSet ds = bo.GetHRSetting_Agent(CustId, out outputs);

            viewData.Add("RacingData", ds.Tables[0]);

            return viewData;
        }

        public override void RenderStatus(DataRow dr, TplCore.Element.IElement e)
        {
            OnAgent_Template tpl = e as OnAgent_Template;

            RacingStatus_Control_Template[] ts = new RacingStatus_Control_Template[6];
            this.RenderStatus(dr, ref ts[0], ref ts[1], ref ts[2], ref ts[3], ref ts[4], ref ts[5]);

            for (int i = 0; i < 6; i++)
            {
                ts[i].upline_text = Resources.Racing.agent;
                ts[i].Downline.donwline_text = Resources.Racing.agentdownline;
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
            OnAgent_Template tpl = e as OnAgent_Template;

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
            OnAgent_Template tpl = e as OnAgent_Template;

            PT_Selection[] ts = new PT_Selection[6];

            tpl.Section_151.Section_151_31.commission_text = tpl.Section_151.Section_151_41.commission_text = tpl.Section_151.Section_151_51.commission_text
                = tpl.Section_152.Section_152_41.commission_text = tpl.Section_152.Section_152_51.commission_text
                = tpl.Section_153.Section_153_51.commission_text = Resources.Racing.commission;

            this.RenderCommission(dr, tpl, out ts[0], out ts[1], out ts[2], out ts[3], out ts[4], out ts[5]);

            tpl.Section_151.Section_151_41.commission_val = ts[0];
            tpl.Section_151.Section_151_31.commission_val = ts[1];
            tpl.Section_151.Section_151_51.commission_val = ts[2];
            tpl.Section_152.Section_152_41.commission_val = ts[3];
            tpl.Section_152.Section_152_51.commission_val = ts[4];
            tpl.Section_153.Section_153_51.commission_val = ts[5];

            tpl.Section_151.Section_151_41.commission_allowupdate_checkbox = null;
            tpl.Section_151.Section_151_31.commission_allowupdate_checkbox = null;
            tpl.Section_151.Section_151_51.commission_allowupdate_checkbox = null;

            tpl.Section_152.Section_152_41.commission_allowupdate_checkbox = null;
            tpl.Section_152.Section_152_51.commission_allowupdate_checkbox = null;

            tpl.Section_153.Section_153_51.commission_allowupdate_checkbox = null;
        }

        public void RenderPositionTakingInfo(OnAgent_Template tpl)
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
            tpl.Section_151.Section_151_31.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_31.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_151.Section_151_41.agent_text = Resources.Racing.agent;
            tpl.Section_151.Section_151_41.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_41.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_151.Section_151_51.agent_text = Resources.Racing.agent;
            tpl.Section_151.Section_151_51.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_51.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_151.Section_151_31.menu_text = Resources.Racing.satote;
            tpl.Section_151.Section_151_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_151.Section_151_51.menu_text = Resources.Racing.autote;

            tpl.Section_151.Section_151_31.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_31.apply_all_member = Resources.Racing.applymastertoallmember;
            tpl.Section_151.Section_151_41.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_41.apply_all_member = Resources.Racing.applymastertoallmember;
            tpl.Section_151.Section_151_51.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_51.apply_all_member = Resources.Racing.applymastertoallmember;

            // 152
            tpl.Section_152.Section_152_41.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_41.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_51.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_51.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_41.agent_text = Resources.Racing.agent;
            tpl.Section_152.Section_152_41.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_152.Section_152_41.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_152.Section_152_51.agent_text = Resources.Racing.agent;
            tpl.Section_152.Section_152_51.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_152.Section_152_51.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_152.Section_152_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_152.Section_152_51.menu_text = Resources.Racing.autote;

            tpl.Section_152.Section_152_41.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_152.Section_152_41.apply_all_member = Resources.Racing.applymastertoallmember;
            tpl.Section_152.Section_152_51.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_152.Section_152_51.apply_all_member = Resources.Racing.applymastertoallmember;

            // 153
            tpl.Section_153.Section_153_51.win_text = Resources.Racing.win;
            tpl.Section_153.Section_153_51.place_text = Resources.Racing.place;

            tpl.Section_153.Section_153_51.agent_text = Resources.Racing.agent;
            tpl.Section_153.Section_153_51.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_153.Section_153_51.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_153.Section_153_51.menu_text = Resources.Racing.autote;

            tpl.Section_153.Section_153_51.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_153.Section_153_51.apply_all_member = Resources.Racing.applymastertoallmember;
        }

        public void RenderPositionTaking(DataRow dr, OnAgent_Template tpl)
        {
            this.RenderPositionTakingInfo(tpl);

            this.RenderPT_151_41(dr, tpl.Section_151.Section_151_41);
            this.RenderPT_151_31(dr, tpl.Section_151.Section_151_31);
            this.RenderPT_151_51(dr, tpl.Section_151.Section_151_51);
            this.RenderPT_152_41(dr, tpl.Section_152.Section_152_41);
            this.RenderPT_152_51(dr, tpl.Section_152.Section_152_51);
            this.RenderPT_153_51(dr, tpl.Section_153.Section_153_51);
        }
    }
}