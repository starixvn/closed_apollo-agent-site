// --------------------------------------------------------------------------
// <copyright file="EditMinPT.aspx.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Age.UI.Components.PT_Control;
    using Sunplus.Agent.Model.BL.Racing;
    using Sunplus.Agent.UI.PositionTaking.RaceBook.Super;

    public partial class EditMinPT_Page : RaceBookSettingBase_Page
    {
        public override void RenderStatus(DataRow dr, TplCore.Element.IElement e)
        {
        }

        public override void RenderBetSetting(DataRow dr, TplCore.Element.IElement e)
        {
        }

        public override void RenderCommission(DataRow dr, TplCore.Element.IElement e)
        {
        }

        public void RenderPT_151_41(DataRow dr, EditMinPT_Section_151_Section_151_41_Block tpl)
        {
            double v151apositiontaking_41 = 0, v151apositiontaking_42 = 0;
            double v151apositiontaking_41_min = 0, v151apositiontaking_42_min = 0;

            double.TryParse(dr["v151apositiontaking_41"].ToString(), out v151apositiontaking_41);
            double.TryParse(dr["v151apositiontaking_42"].ToString(), out v151apositiontaking_42);

            double.TryParse(dr["v151apositiontaking_41_min"].ToString(), out v151apositiontaking_41_min);
            double.TryParse(dr["v151apositiontaking_42_min"].ToString(), out v151apositiontaking_42_min);

            tpl.win_val = new PT_Selection(TemplateMain, "listMinPT_151_41", v151apositiontaking_41_min, 0, v151apositiontaking_41, 0.1, true, false);
            tpl.place_val = new PT_Selection(TemplateMain, "listMinPT_151_42", v151apositiontaking_42_min, 0, v151apositiontaking_42, 0.1);
        }

        public void RenderPT_151_31(DataRow dr, EditMinPT_Section_151_Section_151_31_Block tpl)
        {
            double v151apositiontaking_31 = 0, v151apositiontaking_32 = 0;
            double v151apositiontaking_31_min = 0, v151apositiontaking_32_min = 0;

            double.TryParse(dr["v151apositiontaking_31"].ToString(), out v151apositiontaking_31);
            double.TryParse(dr["v151apositiontaking_32"].ToString(), out v151apositiontaking_32);

            double.TryParse(dr["v151apositiontaking_31_min"].ToString(), out v151apositiontaking_31_min);
            double.TryParse(dr["v151apositiontaking_32_min"].ToString(), out v151apositiontaking_32_min);

            tpl.win_val = new PT_Selection(TemplateMain, "listMinPT_151_31", v151apositiontaking_31_min, 0, v151apositiontaking_31, 0.1, true, false);
            tpl.place_val = new PT_Selection(TemplateMain, "listMinPT_151_32", v151apositiontaking_32_min, 0, v151apositiontaking_32, 0.1);
        }

        public void RenderPT_151_51(DataRow dr, EditMinPT_Section_151_Section_151_51_Block tpl)
        {
            double v151apositiontaking_51 = 0, v151apositiontaking_52 = 0;
            double v151apositiontaking_51_min = 0, v151apositiontaking_52_min = 0;

            double.TryParse(dr["v151apositiontaking_51"].ToString(), out v151apositiontaking_51);
            double.TryParse(dr["v151apositiontaking_52"].ToString(), out v151apositiontaking_52);

            double.TryParse(dr["v151apositiontaking_51_min"].ToString(), out v151apositiontaking_51_min);
            double.TryParse(dr["v151apositiontaking_52_min"].ToString(), out v151apositiontaking_52_min);

            tpl.win_val = new PT_Selection(TemplateMain, "listMinPT_151_51", v151apositiontaking_51_min, 0, v151apositiontaking_51, 0.1, true, false);
            tpl.place_val = new PT_Selection(TemplateMain, "listMinPT_151_52", v151apositiontaking_52_min, 0, v151apositiontaking_52, 0.1);
        }

        public void RenderPT_152_41(DataRow dr, EditMinPT_Section_152_Section_152_41_Block tpl)
        {
            double v152apositiontaking_41 = 0, v152apositiontaking_42 = 0;
            double v152apositiontaking_41_min = 0, v152apositiontaking_42_min = 0;

            double.TryParse(dr["v152apositiontaking_41"].ToString(), out v152apositiontaking_41);
            double.TryParse(dr["v152apositiontaking_42"].ToString(), out v152apositiontaking_42);

            double.TryParse(dr["v152apositiontaking_41_min"].ToString(), out v152apositiontaking_41_min);
            double.TryParse(dr["v152apositiontaking_42_min"].ToString(), out v152apositiontaking_42_min);

            tpl.win_val = new PT_Selection(TemplateMain, "listMinPT_152_41", v152apositiontaking_41_min, 0, v152apositiontaking_41, 0.1, true, false);
            tpl.place_val = new PT_Selection(TemplateMain, "listMinPT_152_42", v152apositiontaking_42_min, 0, v152apositiontaking_42, 0.1);
        }

        public void RenderPT_152_51(DataRow dr, EditMinPT_Section_152_Section_152_51_Block tpl)
        {
            double v152apositiontaking_51 = 0, v152apositiontaking_52 = 0;
            double v152apositiontaking_51_min = 0, v152apositiontaking_52_min = 0;

            double.TryParse(dr["v152apositiontaking_51"].ToString(), out v152apositiontaking_51);
            double.TryParse(dr["v152apositiontaking_52"].ToString(), out v152apositiontaking_52);

            double.TryParse(dr["v152apositiontaking_51_min"].ToString(), out v152apositiontaking_51_min);
            double.TryParse(dr["v152apositiontaking_52_min"].ToString(), out v152apositiontaking_52_min);

            tpl.win_val = new PT_Selection(TemplateMain, "listMinPT_152_51", v152apositiontaking_51_min, 0, v152apositiontaking_51, 0.1, true, false);
            tpl.place_val = new PT_Selection(TemplateMain, "listMinPT_152_52", v152apositiontaking_52_min, 0, v152apositiontaking_52, 0.1);
        }

        public void RenderPT_153_51(DataRow dr, EditMinPT_Section_153_Section_153_51_Block tpl)
        {
            double v153apositiontaking_51 = 0, v153apositiontaking_52 = 0;
            double v153apositiontaking_51_min = 0, v153apositiontaking_52_min = 0;

            double.TryParse(dr["v153apositiontaking_51"].ToString(), out v153apositiontaking_51);
            double.TryParse(dr["v153apositiontaking_52"].ToString(), out v153apositiontaking_52);

            double.TryParse(dr["v153apositiontaking_51_min"].ToString(), out v153apositiontaking_51_min);
            double.TryParse(dr["v153apositiontaking_52_min"].ToString(), out v153apositiontaking_52_min);

            tpl.win_val = new PT_Selection(TemplateMain, "listMinPT_153_51", v153apositiontaking_51_min, 0, v153apositiontaking_51, 0.1, true, false);
            tpl.place_val = new PT_Selection(TemplateMain, "listMinPT_153_52", v153apositiontaking_52_min, 0, v153apositiontaking_52, 0.1);
        }

        public void RenderPositionTakingInfo(EditMinPT_Template tpl)
        {
            string checkCopy_Onclick = string.Empty;
            string copy_from_win_text = Resources.Racing.copyfromwin;
            tpl.UserInfo.CopyAll.copy_all = Resources.Racing.copyallptfromuktotewin;

            tpl.Section_151.title = Resources.Racing.horseracing;
            tpl.Section_152.title = Resources.Racing.greyhound;
            tpl.Section_153.title = Resources.Racing.harness;

            tpl.Section_151.Section_151_31.positiontaking_text = tpl.Section_151.Section_151_41.positiontaking_text = tpl.Section_151.Section_151_51.positiontaking_text
                = tpl.Section_152.Section_152_41.positiontaking_text = tpl.Section_152.Section_152_51.positiontaking_text
                = tpl.Section_153.Section_153_51.positiontaking_text = Resources.Racing.positiontaking;

            // 151
            tpl.Section_151.Section_151_31.win_text = Resources.Racing.win;
            tpl.Section_151.Section_151_31.place_text = Resources.Racing.place;
            checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_31)";
            tpl.Section_151.Section_151_31.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_31", checkCopy_Onclick, copy_from_win_text);

            tpl.Section_151.Section_151_41.win_text = Resources.Racing.win;
            tpl.Section_151.Section_151_41.place_text = Resources.Racing.place;
            checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_41)";
            tpl.Section_151.Section_151_41.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_41", checkCopy_Onclick, copy_from_win_text);

            tpl.Section_151.Section_151_51.win_text = Resources.Racing.win;
            tpl.Section_151.Section_151_51.place_text = Resources.Racing.place;
            checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_51)";
            tpl.Section_151.Section_151_51.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_51", checkCopy_Onclick, copy_from_win_text);

            tpl.Section_151.Section_151_31.menu_text = Resources.Racing.satote;
            tpl.Section_151.Section_151_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_151.Section_151_51.menu_text = Resources.Racing.autote;

            // 152
            tpl.Section_152.Section_152_41.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_41.place_text = Resources.Racing.place;
            checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_152_41)";
            tpl.Section_152.Section_152_41.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_152_41", checkCopy_Onclick, copy_from_win_text);

            tpl.Section_152.Section_152_51.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_51.place_text = Resources.Racing.place;
            checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_152_51)";
            tpl.Section_152.Section_152_51.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_152_51", checkCopy_Onclick, copy_from_win_text);

            tpl.Section_152.Section_152_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_152.Section_152_51.menu_text = Resources.Racing.autote;

            // 153
            tpl.Section_153.Section_153_51.win_text = Resources.Racing.win;
            tpl.Section_153.Section_153_51.place_text = Resources.Racing.place;
            checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_153_51)";
            tpl.Section_153.Section_153_51.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_153_51", checkCopy_Onclick, copy_from_win_text);

            tpl.Section_153.Section_153_51.menu_text = Resources.Racing.autote;

            tpl.button_text = Resources.Racing.submit;
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            EditMinPT_Template tpl = new EditMinPT_Template();
            PT_Control_Template tplcontrol = new PT_Control_Template();
            DataTable racingData = (View as Dictionary<string, DataTable>)["RacingData"];
            DataRow dr = racingData.Rows[0];
            tpl.errorMessage = new ErrorMsg_Control(TemplateMain);

            this.RenderCustomerInfo(dr, tpl.UserInfo, Resources.Racing.editminpt);
            this.RenderPositionTakingInfo(tpl);
            this.RenderPT_151_41(dr, tpl.Section_151.Section_151_41);
            this.RenderPT_151_31(dr, tpl.Section_151.Section_151_31);
            this.RenderPT_151_51(dr, tpl.Section_151.Section_151_51);
            this.RenderPT_152_41(dr, tpl.Section_152.Section_152_41);
            this.RenderPT_152_51(dr, tpl.Section_152.Section_152_51);
            this.RenderPT_153_51(dr, tpl.Section_153.Section_153_51);

            this.RegisterClientVariables();

            TemplateMain.actionFrm = "../Handlers/UpdateMinPT.ashx";
            TemplateMain.RaceBookJs.extJs = "EditMinPT.js" + "?" + Sunplus.Agent.Model.BL.Definitions.AppSettings.JsVersion;
            TemplateMain.RaceBookJs.ExtJs_More = null;

            TemplateMain.content = tpl;
            TemplateMain.hidden_PT = string.Empty;

            writer.Write(this.TemplateMain);
        }

        public override object InvokeData(Hashtable p)
        {
            Dictionary<string, DataTable> viewData = new Dictionary<string, DataTable>();

            RacingBO bo = new RacingBO(CurrentUserSession.NetSessionId);

            Hashtable outputs = null;
            DataSet ds = bo.MasterGetMinPTHR(CustId, out outputs);

            viewData.Add("RacingData", ds.Tables[0]);

            return viewData;
        }
    }
}