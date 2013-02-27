// --------------------------------------------------------------------------
// <copyright file="EditMultMaster.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.06.01">Refactor code and check StyleCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook.Super
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Components.PT_Control;
    using Sunplus.Agent.Model.BL.Racing;
    using Sunplus.Agent.UI._Components.RacingStatus_Control;
    using Sunplus.Agent.UI._MemberInfo.RaceBook.AllowUpdatePT_Control;
    using Sunplus.Agent.UI._MemberInfo.RaceBook.MinMaxBet_Control;
    using Sunplus.Agent.UI.PositionTaking.RaceBook.Super;

    public partial class EditMultMaster_Page : OnMaster_Page
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            this.IsMultiple = true;
            EditMultipleMaster_Template tpl = new EditMultipleMaster_Template();
            PT_Control_Template tplcontrol = new PT_Control_Template();
            DataTable racingData = (View as Dictionary<string, DataTable>)["RacingData"];
            DataRow dr = racingData.Rows[0];
            tpl.errorMessage = new ErrorMsg_Control(TemplateMain);
            this.RenderStatus(dr, tpl.PositionTaking);

            this.RenderCommission(dr, tpl.PositionTaking);
            this.RenderBetSetting(dr, tpl.PositionTaking);
            this.RenderPositionTaking(dr, tpl.PositionTaking);

            this.JavaScriptRegister("isMultiple", "1");

            this.RegisterClientVariables();
            this.RegisterClientHiddenControl();

            TemplateMain.actionFrm = "UpdateMaster.ashx";
            TemplateMain.RaceBookJs.extJs = "EditMultMaster.js" + "?" + Sunplus.Agent.Model.BL.Definitions.AppSettings.JsVersion;
            TemplateMain.RaceBookJs.ExtJs_More.extJs_More = "ConfirmAutoPT.js" + "?" + Sunplus.Agent.Model.BL.Definitions.AppSettings.JsVersion;
            TemplateMain.title = tpl.title = Resources.Racing.editmultpt;
            tpl.button_text = Resources.Racing.submit;
            tpl.title = Resources.Racing.edithorseracing;
            tpl.CopyAll.copy_all = Resources.Racing.copyallfromuktotewin;

            TemplateMain.content = tpl;
            writer.Write(this.TemplateMain);
        }

        public override void RenderStatus(DataRow dr, TplCore.Element.IElement e)
        {
            OnMaster_Template tpl = e as OnMaster_Template;

            RacingStatus_Control_Template[] ts = new RacingStatus_Control_Template[6];
            this.RenderStatus(dr, ref ts[0], ref ts[1], ref ts[2], ref ts[3], ref ts[4], ref ts[5]);

            for (int i = 0; i < 6; i++)
            {
                ts[i].upline_text = Resources.Racing.master;
                ts[i].Downline.donwline_text = Resources.Racing.masterdownline;
            }

            tpl.Section_151.Section_151_31.status.status_text = tpl.Section_151.Section_151_41.status.status_text = tpl.Section_151.Section_151_51.status.status_text = tpl.Section_152.Section_152_41.status.status_text = tpl.Section_152.Section_152_51.status.status_text = tpl.Section_153.Section_153_51.status.status_text = Resources.Lang.status;

            tpl.Section_151.Section_151_41.status_disable = ts[0];
            tpl.Section_151.Section_151_31.status_disable = ts[1];
            tpl.Section_151.Section_151_51.status_disable = ts[2];
            tpl.Section_152.Section_152_41.status_disable = ts[3];
            tpl.Section_152.Section_152_51.status_disable = ts[4];
            tpl.Section_153.Section_153_51.status_disable = ts[5];

            tpl.Section_151.Section_151_41.status.status_disable_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateStatus_151_41", "151_41");
            tpl.Section_151.Section_151_31.status.status_disable_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateStatus_151_31", "151_31");
            tpl.Section_151.Section_151_51.status.status_disable_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateStatus_151_51", "151_51");

            tpl.Section_152.Section_152_41.status.status_disable_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateStatus_152_41", "152_41");
            tpl.Section_152.Section_152_51.status.status_disable_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateStatus_152_51", "152_51");

            tpl.Section_153.Section_153_51.status.status_disable_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateStatus_153_51", "153_51");
        }

        public override void RenderBetSetting(DataRow dr, TplCore.Element.IElement e)
        {
            OnMaster_Template tpl = e as OnMaster_Template;

            tpl.Section_151.Section_151_31.betsetting_text = tpl.Section_151.Section_151_41.betsetting_text = tpl.Section_151.Section_151_51.betsetting_text
            = tpl.Section_152.Section_152_41.betsetting_text = tpl.Section_152.Section_152_51.betsetting_text
            = tpl.Section_153.Section_153_51.betsetting_text = Resources.Racing.betsetting;

            tpl.Section_151.Section_151_41.betsetting_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateBetSetting_151_41", "151_41");
            tpl.Section_151.Section_151_31.betsetting_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateBetSetting_151_31", "151_31");
            tpl.Section_151.Section_151_51.betsetting_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateBetSetting_151_51", "151_51");

            tpl.Section_152.Section_152_41.betsetting_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateBetSetting_152_41", "152_41");
            tpl.Section_152.Section_152_51.betsetting_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateBetSetting_152_51", "152_51");

            tpl.Section_153.Section_153_51.betsetting_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateBetSetting_153_51", "153_51");

            MinMaxBet_Control_Template[] ts = new MinMaxBet_Control_Template[6];
            this.RenderBetSetting(dr, ref ts[0], ref ts[1], ref ts[2], ref ts[3], ref ts[4], ref ts[5]);

            tpl.Section_151.Section_151_41.betsetting_val = ts[0];
            tpl.Section_151.Section_151_31.betsetting_val = ts[1];
            tpl.Section_151.Section_151_51.betsetting_val = ts[2];
            tpl.Section_152.Section_152_41.betsetting_val = ts[3];
            tpl.Section_152.Section_152_51.betsetting_val = ts[4];
            tpl.Section_153.Section_153_51.betsetting_val = ts[5];
        }

        public override void RenderCommission(DataRow dr, TplCore.Element.IElement e)
        {
            OnMaster_Template tpl = e as OnMaster_Template;
            AllowUpdatePT_Control_Template allowUpdate = new AllowUpdatePT_Control_Template();

            PT_Selection[] ts = new PT_Selection[6];

            tpl.Section_151.Section_151_31.commission_text = tpl.Section_151.Section_151_41.commission_text = tpl.Section_151.Section_151_51.commission_text
                = tpl.Section_152.Section_152_41.commission_text = tpl.Section_152.Section_152_51.commission_text
                = tpl.Section_153.Section_153_51.commission_text = Resources.Racing.commission;

            tpl.Section_151.Section_151_41.commission_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateCommission_151_41", "151_41");
            tpl.Section_151.Section_151_31.commission_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateCommission_151_31", "151_31");
            tpl.Section_151.Section_151_51.commission_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateCommission_151_51", "151_51");

            tpl.Section_152.Section_152_41.commission_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateCommission_152_41", "152_41");
            tpl.Section_152.Section_152_51.commission_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateCommission_152_51", "152_51");

            tpl.Section_153.Section_153_51.commission_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdateCommission_153_51", "153_51");

            this.RenderCommission(dr, tpl, out ts[0], out ts[1], out ts[2], out ts[3], out ts[4], out ts[5]);

            tpl.Section_151.Section_151_41.commission_val = ts[0];
            tpl.Section_151.Section_151_31.commission_val = ts[1];
            tpl.Section_151.Section_151_51.commission_val = ts[2];

            tpl.Section_152.Section_152_41.commission_val = ts[3];
            tpl.Section_152.Section_152_51.commission_val = ts[4];

            tpl.Section_153.Section_153_51.commission_val = ts[5];
        }

        public void RenderPositionTakingInfo(OnMaster_Template tpl)
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

            tpl.Section_151.Section_151_31.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_31.super_preset_text = Resources.Racing.superpreset;
            tpl.Section_151.Section_151_31.super_auto_preset_text = Resources.Racing.autosuperpreset;

            tpl.Section_151.Section_151_41.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_41.super_preset_text = Resources.Racing.superpreset;
            tpl.Section_151.Section_151_41.super_auto_preset_text = Resources.Racing.autosuperpreset;

            tpl.Section_151.Section_151_51.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_51.super_preset_text = Resources.Racing.superpreset;
            tpl.Section_151.Section_151_51.super_auto_preset_text = Resources.Racing.autosuperpreset;

            tpl.Section_151.Section_151_31.menu_text = Resources.Racing.satote;
            tpl.Section_151.Section_151_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_151.Section_151_51.menu_text = Resources.Racing.autote;

            tpl.Section_151.Section_151_31.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_31.apply_all_member = Resources.Racing.applysupertoallmember;
            tpl.Section_151.Section_151_41.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_41.apply_all_member = Resources.Racing.applysupertoallmember;
            tpl.Section_151.Section_151_51.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_151.Section_151_51.apply_all_member = Resources.Racing.applysupertoallmember;

            // 152
            tpl.Section_152.Section_152_41.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_41.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_51.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_51.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_41.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_152.Section_152_41.super_preset_text = Resources.Racing.superpreset;
            tpl.Section_152.Section_152_41.super_auto_preset_text = Resources.Racing.autosuperpreset;

            tpl.Section_152.Section_152_51.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_152.Section_152_51.super_preset_text = Resources.Racing.superpreset;
            tpl.Section_152.Section_152_51.super_auto_preset_text = Resources.Racing.autosuperpreset;

            tpl.Section_152.Section_152_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_152.Section_152_51.menu_text = Resources.Racing.autote;

            tpl.Section_152.Section_152_41.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_152.Section_152_41.apply_all_member = Resources.Racing.applysupertoallmember;
            tpl.Section_152.Section_152_51.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_152.Section_152_51.apply_all_member = Resources.Racing.applysupertoallmember;

            // 153
            tpl.Section_153.Section_153_51.win_text = Resources.Racing.win;
            tpl.Section_153.Section_153_51.place_text = Resources.Racing.place;

            tpl.Section_153.Section_153_51.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_153.Section_153_51.super_preset_text = Resources.Racing.superpreset;
            tpl.Section_153.Section_153_51.super_auto_preset_text = Resources.Racing.autosuperpreset;

            tpl.Section_153.Section_153_51.menu_text = Resources.Racing.autote;

            tpl.Section_153.Section_153_51.copy_from_win = Resources.Racing.copyfromwin;
            tpl.Section_153.Section_153_51.apply_all_member = Resources.Racing.applysupertoallmember;
        }

        public void RenderPositionTaking(DataRow dr, OnMaster_Template tpl)
        {
            this.RenderPositionTakingInfo(tpl);

            this.RenderPT_151_41(dr, tpl.Section_151.Section_151_41);
            this.RenderPT_151_31(dr, tpl.Section_151.Section_151_31);
            this.RenderPT_151_51(dr, tpl.Section_151.Section_151_51);
            this.RenderPT_152_41(dr, tpl.Section_152.Section_152_41);
            this.RenderPT_152_51(dr, tpl.Section_152.Section_152_51);
            this.RenderPT_153_51(dr, tpl.Section_153.Section_153_51);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Dictionary<string, DataTable> viewData = new Dictionary<string, DataTable>();

            RacingBO bo = new RacingBO(CurrentUserSession.NetSessionId);

            Hashtable outputs = null;
            DataSet ds = bo.GetHRSetting_Master(CustId, out outputs);

            viewData.Add("RacingData", ds.Tables[0]);

            return viewData;
        }
    }
}