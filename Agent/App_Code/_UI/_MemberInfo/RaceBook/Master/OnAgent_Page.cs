// --------------------------------------------------------------------------
// <copyright file="OnAgent.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.06.01">Improve performance, Check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook.Master
{
    using System;
    using System.Data;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.UI.PositionTaking.RaceBook.Master;

    public abstract class OnAgent_Page : RaceBookSettingBase_Page
    {
        #region Variable

        private bool _isMultiple = false;

        #endregion Variable

        #region Properties

        public bool IsMultiple
        {
            get
            {
                return this._isMultiple;
            }

            set
            {
                this._isMultiple = value;
            }
        }

        #endregion Properties

        public void RenderPT_151_41(DataRow dr, OnAgent_Section_151_Section_151_41_Block tpl)
        {
            double v151apositiontaking_41_max = 0, v151apositiontaking_42_max = 0; // aMax

            double v151mpositiontaking_41_max = 0, v151mpositiontaking_42_max = 0; // mMax

            double v151apositiontaking_41_max_2 = 0, v151apositiontaking_42_max_2 = 0; // aMax2

            double v151apositiontaking_41_min = 0, v151apositiontaking_42_min = 0; // aMin

            double v151mpositiontaking_41 = 0, v151mpositiontaking_42 = 0; // m_3141

            double v151apositiontaking_41 = 0, v151apositiontaking_42 = 0; // a_3141

            double v151mapositiontaking_41_max = 0, v151mapositiontaking_42_max = 0; // maMax

            double v151mapositiontaking_41_min = 0, v151mapositiontaking_42_min = 0; // maMin

            double auto_v151mpositiontaking_41 = 0, auto_v151mpositiontaking_42 = 0; // mAuto

            double auto_v151mpositiontaking_41_max = 0, auto_v151mpositiontaking_42_max = 0; // mAutoMax

            bool isauto_v151mpositiontaking_41 = false, isauto_v151mpositiontaking_42 = false; // isAuto

            // v151apositiontaking_
            double.TryParse(dr["v151apositiontaking_41_max"].ToString(), out v151apositiontaking_41_max_2); // aMax2_3141
            double.TryParse(dr["v151apositiontaking_42_max"].ToString(), out v151apositiontaking_42_max_2);

            double.TryParse(dr["v151apositiontaking_41_min"].ToString(), out v151apositiontaking_41_min); // aMin_3141
            double.TryParse(dr["v151apositiontaking_42_min"].ToString(), out v151apositiontaking_42_min);

            double.TryParse(dr["v151mpositiontaking_41"].ToString(), out v151mpositiontaking_41); // m_3141
            double.TryParse(dr["v151mpositiontaking_42"].ToString(), out v151mpositiontaking_42);

            double.TryParse(dr["v151apositiontaking_41"].ToString(), out v151apositiontaking_41); // a_3141
            double.TryParse(dr["v151apositiontaking_42"].ToString(), out v151apositiontaking_42);

            double.TryParse(dr["v151mapositiontaking_41_max"].ToString(), out v151mapositiontaking_41_max); // maMax_3141
            double.TryParse(dr["v151mapositiontaking_42_max"].ToString(), out v151mapositiontaking_42_max);

            double.TryParse(dr["v151mapositiontaking_41_min"].ToString(), out v151mapositiontaking_41_min);
            double.TryParse(dr["v151mapositiontaking_42_min"].ToString(), out v151mapositiontaking_42_min);

            double.TryParse(dr["auto_v151mpositiontaking_41"].ToString(), out auto_v151mpositiontaking_41); // mAuto_3141
            double.TryParse(dr["auto_v151mpositiontaking_42"].ToString(), out auto_v151mpositiontaking_42);

            double.TryParse(dr["auto_v151mpositiontaking_41_max"].ToString(), out auto_v151mpositiontaking_41_max); // mAutoMax_3141
            double.TryParse(dr["auto_v151mpositiontaking_42_max"].ToString(), out auto_v151mpositiontaking_42_max);

            isauto_v151mpositiontaking_41 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_41"]); // isAuto_3141
            isauto_v151mpositiontaking_42 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_42"]);

            v151mpositiontaking_41_max = v151mapositiontaking_41_max - v151apositiontaking_41_min;
            v151mpositiontaking_42_max = v151mapositiontaking_42_max - v151apositiontaking_42_min;

            // max(a2) = min(a4, m3-m2, l2)
            v151apositiontaking_41_max = (v151mapositiontaking_41_max - v151mpositiontaking_41 > v151apositiontaking_41_max_2) ? v151apositiontaking_41_max_2 : v151mapositiontaking_41_max - v151mpositiontaking_41;
            v151apositiontaking_42_max = (v151mapositiontaking_42_max - v151mpositiontaking_42 > v151apositiontaking_42_max_2) ? v151apositiontaking_42_max_2 : v151mapositiontaking_42_max - v151mpositiontaking_42;

            this.JavaScriptRegister("v151apositiontaking_41_max", v151apositiontaking_41_max_2 > v151mapositiontaking_41_max ? v151apositiontaking_41_max_2 : v151mapositiontaking_41_max);
            this.JavaScriptRegister("v151apositiontaking_42_max", v151apositiontaking_42_max_2 > v151mapositiontaking_42_max ? v151apositiontaking_42_max_2 : v151mapositiontaking_42_max);

            this.JavaScriptRegister("v151apositiontaking_41_min", v151apositiontaking_41_min);
            this.JavaScriptRegister("v151apositiontaking_42_min", v151apositiontaking_42_min);

            this.JavaScriptRegister("v151mapositiontaking_41_max", v151mapositiontaking_41_max);
            this.JavaScriptRegister("v151mapositiontaking_42_max", v151mapositiontaking_42_max);

            this.JavaScriptRegister("IsAutoMaster_151_41", isauto_v151mpositiontaking_41);
            this.JavaScriptRegister("IsAutoMaster_151_42", isauto_v151mpositiontaking_42);

            tpl.agent_win_val = new PT_Selection(TemplateMain, "listAgent_151_41", v151apositiontaking_41, v151apositiontaking_41_min, v151apositiontaking_41_max, 0.1, true, false);
            tpl.agent_place_val = new PT_Selection(TemplateMain, "listAgent_151_42", v151apositiontaking_42, v151apositiontaking_42_min, v151apositiontaking_42_max, 0.1);

            string selectOnChange_41 = "SelectOnChangePT(this,'listAgent_151_41','v151mapositiontaking_41_max','v151apositiontaking_41_max','v151apositiontaking_41_min');";
            string selectOnChange_42 = "SelectOnChangePT(this,'listAgent_151_42','v151mapositiontaking_42_max','v151apositiontaking_42_max','v151apositiontaking_42_min');";

            tpl.master_preset_win_val = new PT_Selection(TemplateMain, "listMaster_151_41", v151mpositiontaking_41, isauto_v151mpositiontaking_41, 0, v151mpositiontaking_41_max, 0.1, true, false, selectOnChange_41);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_151_41", auto_v151mpositiontaking_41, !isauto_v151mpositiontaking_41, 0, auto_v151mpositiontaking_41_max, 0.1, true, false);

            tpl.master_preset_place_val = new PT_Selection(TemplateMain, "listMaster_151_42", v151mpositiontaking_42, isauto_v151mpositiontaking_42, 0, v151mpositiontaking_42_max, 0.1, selectOnChange_42);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_151_42", auto_v151mpositiontaking_42, !isauto_v151mpositiontaking_42, 0, auto_v151mpositiontaking_42_max, 0.1);

            string autoOnCheck_41 = "CheckAuto(this,'listMaster_151_41','listAgent_151_41','v151mapositiontaking_41_max','v151apositiontaking_41_max','v151apositiontaking_41_min');";
            string autoOnCheck_42 = "CheckAuto(this,'listMaster_151_42','listAgent_151_42','v151mapositiontaking_42_max','v151apositiontaking_42_max','v151apositiontaking_42_min');";

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_41", isauto_v151mpositiontaking_41, autoOnCheck_41);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_42", isauto_v151mpositiontaking_42, autoOnCheck_42);

            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_41)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_41", checkCopy_Onclick, Resources.Racing.copyfromwin);

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_151_41", "151_41");
            }
        }

        public void RenderPT_151_31(DataRow dr, OnAgent_Section_151_Section_151_31_Block tpl)
        {
            double v151apositiontaking_31_max = 0, v151apositiontaking_32_max = 0; // aMax

            double v151mpositiontaking_31_max = 0, v151mpositiontaking_32_max = 0; // mMax

            double v151apositiontaking_31_max_2 = 0, v151apositiontaking_32_max_2 = 0; // aMax2

            double v151apositiontaking_31_min = 0, v151apositiontaking_32_min = 0; // aMin

            double v151mpositiontaking_31 = 0, v151mpositiontaking_32 = 0; // m_3141

            double v151apositiontaking_31 = 0, v151apositiontaking_32 = 0; // a_3141

            double v151mapositiontaking_31_max = 0, v151mapositiontaking_32_max = 0; // maMax

            double v151mapositiontaking_31_min = 0, v151mapositiontaking_32_min = 0; // maMin

            double auto_v151mpositiontaking_31 = 0, auto_v151mpositiontaking_32 = 0; // mAuto

            double auto_v151mpositiontaking_31_max = 0, auto_v151mpositiontaking_32_max = 0; // mAutoMax

            bool isauto_v151mpositiontaking_31 = false, isauto_v151mpositiontaking_32 = false; // isAuto

            // v151apositiontaking_
            double.TryParse(dr["v151apositiontaking_31_max"].ToString(), out v151apositiontaking_31_max_2); // aMax2_3141
            double.TryParse(dr["v151apositiontaking_32_max"].ToString(), out v151apositiontaking_32_max_2);

            double.TryParse(dr["v151apositiontaking_31_min"].ToString(), out v151apositiontaking_31_min); // aMin_3141
            double.TryParse(dr["v151apositiontaking_32_min"].ToString(), out v151apositiontaking_32_min);

            double.TryParse(dr["v151mpositiontaking_31"].ToString(), out v151mpositiontaking_31); // m_3141
            double.TryParse(dr["v151mpositiontaking_32"].ToString(), out v151mpositiontaking_32);

            double.TryParse(dr["v151apositiontaking_31"].ToString(), out v151apositiontaking_31); // a_3141
            double.TryParse(dr["v151apositiontaking_32"].ToString(), out v151apositiontaking_32);

            double.TryParse(dr["v151mapositiontaking_31_max"].ToString(), out v151mapositiontaking_31_max); // maMax_3141
            double.TryParse(dr["v151mapositiontaking_32_max"].ToString(), out v151mapositiontaking_32_max);

            double.TryParse(dr["v151mapositiontaking_31_min"].ToString(), out v151mapositiontaking_31_min);
            double.TryParse(dr["v151mapositiontaking_32_min"].ToString(), out v151mapositiontaking_32_min);

            double.TryParse(dr["auto_v151mpositiontaking_31"].ToString(), out auto_v151mpositiontaking_31); // mAuto_3141
            double.TryParse(dr["auto_v151mpositiontaking_32"].ToString(), out auto_v151mpositiontaking_32);

            double.TryParse(dr["auto_v151mpositiontaking_31_max"].ToString(), out auto_v151mpositiontaking_31_max); // mAutoMax_3141
            double.TryParse(dr["auto_v151mpositiontaking_32_max"].ToString(), out auto_v151mpositiontaking_32_max);

            isauto_v151mpositiontaking_31 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_31"]); // isAuto_3141
            isauto_v151mpositiontaking_32 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_32"]);

            v151mpositiontaking_31_max = v151mapositiontaking_31_max - v151apositiontaking_31_min;
            v151mpositiontaking_32_max = v151mapositiontaking_32_max - v151apositiontaking_32_min;

            v151apositiontaking_31_max = (v151mapositiontaking_31_max - v151mpositiontaking_31 > v151apositiontaking_31_max_2) ? v151apositiontaking_31_max_2 : v151mapositiontaking_31_max - v151mpositiontaking_31;
            v151apositiontaking_32_max = (v151mapositiontaking_32_max - v151mpositiontaking_32 > v151apositiontaking_32_max_2) ? v151apositiontaking_32_max_2 : v151mapositiontaking_32_max - v151mpositiontaking_32;

            this.JavaScriptRegister("v151apositiontaking_31_max", v151apositiontaking_31_max_2 > v151mapositiontaking_31_max ? v151apositiontaking_31_max_2 : v151mapositiontaking_31_max);
            this.JavaScriptRegister("v151apositiontaking_32_max", v151apositiontaking_32_max_2 > v151mapositiontaking_32_max ? v151apositiontaking_32_max_2 : v151mapositiontaking_32_max);

            this.JavaScriptRegister("v151apositiontaking_31_min", v151apositiontaking_31_min);
            this.JavaScriptRegister("v151apositiontaking_32_min", v151apositiontaking_32_min);

            this.JavaScriptRegister("v151mapositiontaking_31_max", v151mapositiontaking_31_max);
            this.JavaScriptRegister("v151mapositiontaking_32_max", v151mapositiontaking_32_max);

            this.JavaScriptRegister("IsAutoMaster_151_31", isauto_v151mpositiontaking_31);
            this.JavaScriptRegister("IsAutoMaster_151_32", isauto_v151mpositiontaking_32);

            tpl.agent_win_val = new PT_Selection(TemplateMain, "listAgent_151_31", v151apositiontaking_31, v151apositiontaking_31_min, v151apositiontaking_31_max, 0.1, true, false);
            tpl.agent_place_val = new PT_Selection(TemplateMain, "listAgent_151_32", v151apositiontaking_32, v151apositiontaking_32_min, v151apositiontaking_32_max, 0.1);

            string selectOnChange_31 = "SelectOnChangePT(this,'listAgent_151_31','v151mapositiontaking_31_max','v151apositiontaking_31_max','v151apositiontaking_31_min');";
            string selectOnChange_32 = "SelectOnChangePT(this,'listAgent_151_32','v151mapositiontaking_32_max','v151apositiontaking_32_max','v151apositiontaking_32_min');";

            tpl.master_preset_win_val = new PT_Selection(TemplateMain, "listMaster_151_31", v151mpositiontaking_31, isauto_v151mpositiontaking_31, 0, v151mpositiontaking_31_max, 0.1, true, false, selectOnChange_31);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_151_31", auto_v151mpositiontaking_31, !isauto_v151mpositiontaking_31, 0, auto_v151mpositiontaking_31_max, 0.1, true, false);

            tpl.master_preset_place_val = new PT_Selection(TemplateMain, "listMaster_151_32", v151mpositiontaking_32, isauto_v151mpositiontaking_32, 0, v151mpositiontaking_32_max, 0.1, selectOnChange_32);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_151_32", auto_v151mpositiontaking_32, !isauto_v151mpositiontaking_32, 0, auto_v151mpositiontaking_32_max, 0.1);

            string autoOnCheck_31 = "CheckAuto(this,'listMaster_151_31','listAgent_151_31','v151mapositiontaking_31_max','v151apositiontaking_31_max','v151apositiontaking_31_min');";
            string autoOnCheck_32 = "CheckAuto(this,'listMaster_151_32','listAgent_151_32','v151mapositiontaking_32_max','v151apositiontaking_32_max','v151apositiontaking_32_min');";

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_31", isauto_v151mpositiontaking_31, autoOnCheck_31);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_32", isauto_v151mpositiontaking_32, autoOnCheck_32);

            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_31)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_31", checkCopy_Onclick, Resources.Racing.copyfromwin);

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_151_31", "151_31");
            }
        }

        public void RenderPT_151_51(DataRow dr, OnAgent_Section_151_Section_151_51_Block tpl)
        {
            double v151apositiontaking_51_max = 0, v151apositiontaking_52_max = 0; // aMax

            double v151mpositiontaking_51_max = 0, v151mpositiontaking_52_max = 0; // mMax

            double v151apositiontaking_51_max_2 = 0, v151apositiontaking_52_max_2 = 0; // aMax2

            double v151apositiontaking_51_min = 0, v151apositiontaking_52_min = 0; // aMin

            double v151mpositiontaking_51 = 0, v151mpositiontaking_52 = 0; // m_5141

            double v151apositiontaking_51 = 0, v151apositiontaking_52 = 0; // a_5141

            double v151mapositiontaking_51_max = 0, v151mapositiontaking_52_max = 0; // maMax

            double v151mapositiontaking_51_min = 0, v151mapositiontaking_52_min = 0; // maMin

            double auto_v151mpositiontaking_51 = 0, auto_v151mpositiontaking_52 = 0; // mAuto

            double auto_v151mpositiontaking_51_max = 0, auto_v151mpositiontaking_52_max = 0; // mAutoMax

            bool isauto_v151mpositiontaking_51 = false, isauto_v151mpositiontaking_52 = false; // isAuto

            // v151apositiontaking_
            double.TryParse(dr["v151apositiontaking_51_max"].ToString(), out v151apositiontaking_51_max_2); // aMax2_5141
            double.TryParse(dr["v151apositiontaking_52_max"].ToString(), out v151apositiontaking_52_max_2);

            double.TryParse(dr["v151apositiontaking_51_min"].ToString(), out v151apositiontaking_51_min); // aMin_5141
            double.TryParse(dr["v151apositiontaking_52_min"].ToString(), out v151apositiontaking_52_min);

            double.TryParse(dr["v151mpositiontaking_51"].ToString(), out v151mpositiontaking_51); // m_5141
            double.TryParse(dr["v151mpositiontaking_52"].ToString(), out v151mpositiontaking_52);

            double.TryParse(dr["v151apositiontaking_51"].ToString(), out v151apositiontaking_51); // a_5141
            double.TryParse(dr["v151apositiontaking_52"].ToString(), out v151apositiontaking_52);

            double.TryParse(dr["v151mapositiontaking_51_max"].ToString(), out v151mapositiontaking_51_max); // maMax_5141
            double.TryParse(dr["v151mapositiontaking_52_max"].ToString(), out v151mapositiontaking_52_max);

            double.TryParse(dr["v151mapositiontaking_51_min"].ToString(), out v151mapositiontaking_51_min);
            double.TryParse(dr["v151mapositiontaking_52_min"].ToString(), out v151mapositiontaking_52_min);

            double.TryParse(dr["auto_v151mpositiontaking_51"].ToString(), out auto_v151mpositiontaking_51); // mAuto_5141
            double.TryParse(dr["auto_v151mpositiontaking_52"].ToString(), out auto_v151mpositiontaking_52);

            double.TryParse(dr["auto_v151mpositiontaking_51_max"].ToString(), out auto_v151mpositiontaking_51_max); // mAutoMax_5141
            double.TryParse(dr["auto_v151mpositiontaking_52_max"].ToString(), out auto_v151mpositiontaking_52_max);

            isauto_v151mpositiontaking_51 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_51"]); // isAuto_5141
            isauto_v151mpositiontaking_52 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_52"]);

            v151mpositiontaking_51_max = v151mapositiontaking_51_max - v151apositiontaking_51_min;
            v151mpositiontaking_52_max = v151mapositiontaking_52_max - v151apositiontaking_52_min;

            v151apositiontaking_51_max = (v151mapositiontaking_51_max - v151mpositiontaking_51 > v151apositiontaking_51_max_2) ? v151apositiontaking_51_max_2 : v151mapositiontaking_51_max - v151mpositiontaking_51;
            v151apositiontaking_52_max = (v151mapositiontaking_52_max - v151mpositiontaking_52 > v151apositiontaking_52_max_2) ? v151apositiontaking_52_max_2 : v151mapositiontaking_52_max - v151mpositiontaking_52;

            this.JavaScriptRegister("v151apositiontaking_51_max", v151apositiontaking_51_max_2 > v151mapositiontaking_51_max ? v151apositiontaking_51_max_2 : v151mapositiontaking_51_max);
            this.JavaScriptRegister("v151apositiontaking_52_max", v151apositiontaking_52_max_2 > v151mapositiontaking_52_max ? v151apositiontaking_52_max_2 : v151mapositiontaking_52_max);

            this.JavaScriptRegister("v151apositiontaking_51_min", v151apositiontaking_51_min);
            this.JavaScriptRegister("v151apositiontaking_52_min", v151apositiontaking_52_min);

            this.JavaScriptRegister("v151mapositiontaking_51_max", v151mapositiontaking_51_max);
            this.JavaScriptRegister("v151mapositiontaking_52_max", v151mapositiontaking_52_max);

            this.JavaScriptRegister("IsAutoMaster_151_51", isauto_v151mpositiontaking_51);
            this.JavaScriptRegister("IsAutoMaster_151_52", isauto_v151mpositiontaking_52);

            tpl.agent_win_val = new PT_Selection(TemplateMain, "listAgent_151_51", v151apositiontaking_51, v151apositiontaking_51_min, v151apositiontaking_51_max, 0.1, true, false);
            tpl.agent_place_val = new PT_Selection(TemplateMain, "listAgent_151_52", v151apositiontaking_52, v151apositiontaking_52_min, v151apositiontaking_52_max, 0.1);

            string selectOnChange_51 = "SelectOnChangePT(this,'listAgent_151_51','v151mapositiontaking_51_max','v151apositiontaking_51_max','v151apositiontaking_51_min');";
            string selectOnChange_52 = "SelectOnChangePT(this,'listAgent_151_52','v151mapositiontaking_52_max','v151apositiontaking_52_max','v151apositiontaking_52_min');";

            tpl.master_preset_win_val = new PT_Selection(TemplateMain, "listMaster_151_51", v151mpositiontaking_51, isauto_v151mpositiontaking_51, 0, v151mpositiontaking_51_max, 0.1, true, false, selectOnChange_51);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_151_51", auto_v151mpositiontaking_51, !isauto_v151mpositiontaking_51, 0, auto_v151mpositiontaking_51_max, 0.1, true, false);

            tpl.master_preset_place_val = new PT_Selection(TemplateMain, "listMaster_151_52", v151mpositiontaking_52, isauto_v151mpositiontaking_52, 0, v151mpositiontaking_52_max, 0.1, selectOnChange_52);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_151_52", auto_v151mpositiontaking_52, !isauto_v151mpositiontaking_52, 0, auto_v151mpositiontaking_52_max, 0.1);

            string autoOnCheck_51 = "CheckAuto(this,'listMaster_151_51','listAgent_151_51','v151mapositiontaking_51_max','v151apositiontaking_51_max','v151apositiontaking_51_min');";
            string autoOnCheck_52 = "CheckAuto(this,'listMaster_151_52','listAgent_151_52','v151mapositiontaking_52_max','v151apositiontaking_52_max','v151apositiontaking_52_min');";

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_51", isauto_v151mpositiontaking_51, autoOnCheck_51);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_52", isauto_v151mpositiontaking_52, autoOnCheck_52);

            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_51)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_51", checkCopy_Onclick, Resources.Racing.copyfromwin);

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_151_51", "151_51");
            }
        }

        public void RenderPT_152_41(DataRow dr, OnAgent_Section_152_Section_152_41_Block tpl)
        {
            double v152apositiontaking_41_max = 0, v152apositiontaking_42_max = 0; // aMax

            double v152mpositiontaking_41_max = 0, v152mpositiontaking_42_max = 0; // mMax

            double v152apositiontaking_41_max_2 = 0, v152apositiontaking_42_max_2 = 0; // aMax2

            double v152apositiontaking_41_min = 0, v152apositiontaking_42_min = 0; // aMin

            double v152mpositiontaking_41 = 0, v152mpositiontaking_42 = 0; // m_3141

            double v152apositiontaking_41 = 0, v152apositiontaking_42 = 0; // a_3141

            double v152mapositiontaking_41_max = 0, v152mapositiontaking_42_max = 0; // maMax

            double v152mapositiontaking_41_min = 0, v152mapositiontaking_42_min = 0; // maMin

            double auto_v152mpositiontaking_41 = 0, auto_v152mpositiontaking_42 = 0; // mAuto

            double auto_v152mpositiontaking_41_max = 0, auto_v152mpositiontaking_42_max = 0; // mAutoMax

            bool isauto_v152mpositiontaking_41 = false, isauto_v152mpositiontaking_42 = false; // isAuto

            // v151apositiontaking_
            double.TryParse(dr["v152apositiontaking_41_max"].ToString(), out v152apositiontaking_41_max_2); // aMax2_3141
            double.TryParse(dr["v152apositiontaking_42_max"].ToString(), out v152apositiontaking_42_max_2);

            double.TryParse(dr["v152apositiontaking_41_min"].ToString(), out v152apositiontaking_41_min); // aMin_3141
            double.TryParse(dr["v152apositiontaking_42_min"].ToString(), out v152apositiontaking_42_min);

            double.TryParse(dr["v152mpositiontaking_41"].ToString(), out v152mpositiontaking_41); // m_3141
            double.TryParse(dr["v152mpositiontaking_42"].ToString(), out v152mpositiontaking_42);

            double.TryParse(dr["v152apositiontaking_41"].ToString(), out v152apositiontaking_41); // a_3141
            double.TryParse(dr["v152apositiontaking_42"].ToString(), out v152apositiontaking_42);

            double.TryParse(dr["v152mapositiontaking_41_max"].ToString(), out v152mapositiontaking_41_max); // maMax_3141
            double.TryParse(dr["v152mapositiontaking_42_max"].ToString(), out v152mapositiontaking_42_max);

            double.TryParse(dr["v152mapositiontaking_41_min"].ToString(), out v152mapositiontaking_41_min);
            double.TryParse(dr["v152mapositiontaking_42_min"].ToString(), out v152mapositiontaking_42_min);

            double.TryParse(dr["auto_v152mpositiontaking_41"].ToString(), out auto_v152mpositiontaking_41); // mAuto_3141
            double.TryParse(dr["auto_v152mpositiontaking_42"].ToString(), out auto_v152mpositiontaking_42);

            double.TryParse(dr["auto_v152mpositiontaking_41_max"].ToString(), out auto_v152mpositiontaking_41_max); // mAutoMax_3141
            double.TryParse(dr["auto_v152mpositiontaking_42_max"].ToString(), out auto_v152mpositiontaking_42_max);

            isauto_v152mpositiontaking_41 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_41"]); // isAuto_3141
            isauto_v152mpositiontaking_42 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_42"]);

            v152mpositiontaking_41_max = v152mapositiontaking_41_max - v152apositiontaking_41_min;
            v152mpositiontaking_42_max = v152mapositiontaking_42_max - v152apositiontaking_42_min;

            v152apositiontaking_41_max = (v152mapositiontaking_41_max - v152mpositiontaking_41 > v152apositiontaking_41_max_2) ? v152apositiontaking_41_max_2 : v152mapositiontaking_41_max - v152mpositiontaking_41;
            v152apositiontaking_42_max = (v152mapositiontaking_42_max - v152mpositiontaking_42 > v152apositiontaking_42_max_2) ? v152apositiontaking_42_max_2 : v152mapositiontaking_42_max - v152mpositiontaking_42;

            this.JavaScriptRegister("v152apositiontaking_41_max", v152apositiontaking_41_max_2 > v152mapositiontaking_41_max ? v152apositiontaking_41_max_2 : v152mapositiontaking_41_max);
            this.JavaScriptRegister("v152apositiontaking_42_max", v152apositiontaking_42_max_2 > v152mapositiontaking_42_max ? v152apositiontaking_42_max_2 : v152mapositiontaking_42_max);

            this.JavaScriptRegister("v152apositiontaking_41_min", v152apositiontaking_41_min);
            this.JavaScriptRegister("v152apositiontaking_42_min", v152apositiontaking_42_min);

            this.JavaScriptRegister("v152mapositiontaking_41_max", v152mapositiontaking_41_max);
            this.JavaScriptRegister("v152mapositiontaking_42_max", v152mapositiontaking_42_max);

            this.JavaScriptRegister("IsAutoMaster_152_41", isauto_v152mpositiontaking_41);
            this.JavaScriptRegister("IsAutoMaster_152_42", isauto_v152mpositiontaking_42);

            tpl.agent_win_val = new PT_Selection(TemplateMain, "listAgent_152_41", v152apositiontaking_41, v152apositiontaking_41_min, v152apositiontaking_41_max, 0.1, true, false);
            tpl.agent_place_val = new PT_Selection(TemplateMain, "listAgent_152_42", v152apositiontaking_42, v152apositiontaking_42_min, v152apositiontaking_42_max, 0.1);

            string selectOnChange_41 = "SelectOnChangePT(this,'listAgent_152_41','v152mapositiontaking_41_max','v152apositiontaking_41_max','v152apositiontaking_41_min');";
            string selectOnChange_42 = "SelectOnChangePT(this,'listAgent_152_42','v152mapositiontaking_42_max','v152apositiontaking_42_max','v152apositiontaking_42_min');";

            tpl.master_preset_win_val = new PT_Selection(TemplateMain, "listMaster_152_41", v152mpositiontaking_41, isauto_v152mpositiontaking_41, 0, v152mpositiontaking_41_max, 0.1, true, false, selectOnChange_41);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_152_41", auto_v152mpositiontaking_41, !isauto_v152mpositiontaking_41, 0, auto_v152mpositiontaking_41_max, 0.1, true, false);

            tpl.master_preset_place_val = new PT_Selection(TemplateMain, "listMaster_152_42", v152mpositiontaking_42, isauto_v152mpositiontaking_42, 0, v152mpositiontaking_42_max, 0.1, selectOnChange_42);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_152_42", auto_v152mpositiontaking_42, !isauto_v152mpositiontaking_42, 0, auto_v152mpositiontaking_42_max, 0.1);

            string autoOnCheck_41 = "CheckAuto(this,'listMaster_152_41','listAgent_152_41','v152mapositiontaking_41_max','v152apositiontaking_41_max','v152apositiontaking_41_min');";
            string autoOnCheck_42 = "CheckAuto(this,'listMaster_152_42','listAgent_152_42','v152mapositiontaking_42_max','v152apositiontaking_42_max','v152apositiontaking_42_min');";

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_152_41", isauto_v152mpositiontaking_41, autoOnCheck_41);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_152_42", isauto_v152mpositiontaking_42, autoOnCheck_42);

            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_152_41)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_152_41", checkCopy_Onclick, Resources.Racing.copyfromwin);

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_152_41", "152_41");
            }
        }

        public void RenderPT_152_51(DataRow dr, OnAgent_Section_152_Section_152_51_Block tpl)
        {
            double v152apositiontaking_51_max = 0, v152apositiontaking_52_max = 0; // aMax

            double v152mpositiontaking_51_max = 0, v152mpositiontaking_52_max = 0; // mMax

            double v152apositiontaking_51_max_2 = 0, v152apositiontaking_52_max_2 = 0; // aMax2

            double v152apositiontaking_51_min = 0, v152apositiontaking_52_min = 0; // aMin

            double v152mpositiontaking_51 = 0, v152mpositiontaking_52 = 0; // m_3141

            double v152apositiontaking_51 = 0, v152apositiontaking_52 = 0; // a_3141

            double v152mapositiontaking_51_max = 0, v152mapositiontaking_52_max = 0; // maMax

            double v152mapositiontaking_51_min = 0, v152mapositiontaking_52_min = 0; // maMin

            double auto_v152mpositiontaking_51 = 0, auto_v152mpositiontaking_52 = 0; // mAuto

            double auto_v152mpositiontaking_51_max = 0, auto_v152mpositiontaking_52_max = 0; // mAutoMax

            bool isauto_v152mpositiontaking_51 = false, isauto_v152mpositiontaking_52 = false; // isAuto

            // v151apositiontaking_
            double.TryParse(dr["v152apositiontaking_51_max"].ToString(), out v152apositiontaking_51_max_2); // aMax2_3141
            double.TryParse(dr["v152apositiontaking_52_max"].ToString(), out v152apositiontaking_52_max_2);

            double.TryParse(dr["v152apositiontaking_51_min"].ToString(), out v152apositiontaking_51_min); // aMin_3141
            double.TryParse(dr["v152apositiontaking_52_min"].ToString(), out v152apositiontaking_52_min);

            double.TryParse(dr["v152mpositiontaking_51"].ToString(), out v152mpositiontaking_51); // m_3141
            double.TryParse(dr["v152mpositiontaking_52"].ToString(), out v152mpositiontaking_52);

            double.TryParse(dr["v152apositiontaking_51"].ToString(), out v152apositiontaking_51); // a_3141
            double.TryParse(dr["v152apositiontaking_52"].ToString(), out v152apositiontaking_52);

            double.TryParse(dr["v152mapositiontaking_51_max"].ToString(), out v152mapositiontaking_51_max); // maMax_3141
            double.TryParse(dr["v152mapositiontaking_52_max"].ToString(), out v152mapositiontaking_52_max);

            double.TryParse(dr["v152mapositiontaking_51_min"].ToString(), out v152mapositiontaking_51_min);
            double.TryParse(dr["v152mapositiontaking_52_min"].ToString(), out v152mapositiontaking_52_min);

            double.TryParse(dr["auto_v152mpositiontaking_51"].ToString(), out auto_v152mpositiontaking_51); // mAuto_3141
            double.TryParse(dr["auto_v152mpositiontaking_52"].ToString(), out auto_v152mpositiontaking_52);

            double.TryParse(dr["auto_v152mpositiontaking_51_max"].ToString(), out auto_v152mpositiontaking_51_max); // mAutoMax_3141
            double.TryParse(dr["auto_v152mpositiontaking_52_max"].ToString(), out auto_v152mpositiontaking_52_max);

            isauto_v152mpositiontaking_51 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_51"]); // isAuto_3141
            isauto_v152mpositiontaking_52 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_52"]);

            v152mpositiontaking_51_max = v152mapositiontaking_51_max - v152apositiontaking_51_min;
            v152mpositiontaking_52_max = v152mapositiontaking_52_max - v152apositiontaking_52_min;

            v152apositiontaking_51_max = (v152mapositiontaking_51_max - v152mpositiontaking_51 > v152apositiontaking_51_max_2) ? v152apositiontaking_51_max_2 : v152mapositiontaking_51_max - v152mpositiontaking_51;
            v152apositiontaking_52_max = (v152mapositiontaking_52_max - v152mpositiontaking_52 > v152apositiontaking_52_max_2) ? v152apositiontaking_52_max_2 : v152mapositiontaking_52_max - v152mpositiontaking_52;

            this.JavaScriptRegister("v152apositiontaking_51_max", v152apositiontaking_51_max_2 > v152mapositiontaking_51_max ? v152apositiontaking_51_max_2 : v152mapositiontaking_51_max);
            this.JavaScriptRegister("v152apositiontaking_52_max", v152apositiontaking_52_max_2 > v152mapositiontaking_52_max ? v152apositiontaking_52_max_2 : v152mapositiontaking_52_max);

            this.JavaScriptRegister("v152apositiontaking_51_min", v152apositiontaking_51_min);
            this.JavaScriptRegister("v152apositiontaking_52_min", v152apositiontaking_52_min);

            this.JavaScriptRegister("v152mapositiontaking_51_max", v152mapositiontaking_51_max);
            this.JavaScriptRegister("v152mapositiontaking_52_max", v152mapositiontaking_52_max);

            this.JavaScriptRegister("IsAutoMaster_152_51", isauto_v152mpositiontaking_51);
            this.JavaScriptRegister("IsAutoMaster_152_52", isauto_v152mpositiontaking_52);

            tpl.agent_win_val = new PT_Selection(TemplateMain, "listAgent_152_51", v152apositiontaking_51, v152apositiontaking_51_min, v152apositiontaking_51_max, 0.1, true, false);
            tpl.agent_place_val = new PT_Selection(TemplateMain, "listAgent_152_52", v152apositiontaking_52, v152apositiontaking_52_min, v152apositiontaking_52_max, 0.1);

            string selectOnChange_41 = "SelectOnChangePT(this,'listAgent_152_51','v152mapositiontaking_51_max','v152apositiontaking_51_max','v152apositiontaking_51_min');";
            string selectOnChange_42 = "SelectOnChangePT(this,'listAgent_152_52','v152mapositiontaking_52_max','v152apositiontaking_52_max','v152apositiontaking_51_min');";

            tpl.master_preset_win_val = new PT_Selection(TemplateMain, "listMaster_152_51", v152mpositiontaking_51, isauto_v152mpositiontaking_51, 0, v152mpositiontaking_51_max, 0.1, true, false, selectOnChange_41);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_152_51", auto_v152mpositiontaking_51, !isauto_v152mpositiontaking_51, 0, auto_v152mpositiontaking_51_max, 0.1, true, false);

            tpl.master_preset_place_val = new PT_Selection(TemplateMain, "listMaster_152_52", v152mpositiontaking_52, isauto_v152mpositiontaking_52, 0, v152mpositiontaking_52_max, 0.1, selectOnChange_42);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_152_52", auto_v152mpositiontaking_52, !isauto_v152mpositiontaking_52, 0, auto_v152mpositiontaking_52_max, 0.1);

            string autoOnCheck_51 = "CheckAuto(this,'listMaster_152_51','listAgent_152_51','v152mapositiontaking_51_max','v152apositiontaking_51_max','v152apositiontaking_51_min');";
            string autoOnCheck_52 = "CheckAuto(this,'listMaster_152_52','listAgent_152_52','v152mapositiontaking_52_max','v152apositiontaking_52_max','v152apositiontaking_52_min');";

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_152_51", isauto_v152mpositiontaking_51, autoOnCheck_51);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_152_52", isauto_v152mpositiontaking_52, autoOnCheck_52);

            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_152_51)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_152_51", checkCopy_Onclick, Resources.Racing.copyfromwin);

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_152_51", "152_51");
            }
        }

        public void RenderPT_153_51(DataRow dr, OnAgent_Section_153_Section_153_51_Block tpl)
        {
            double v153apositiontaking_51_max = 0, v153apositiontaking_52_max = 0; // aMax

            double v153mpositiontaking_51_max = 0, v153mpositiontaking_52_max = 0; // mMax

            double v153apositiontaking_51_max_2 = 0, v153apositiontaking_52_max_2 = 0; // aMax2

            double v153apositiontaking_51_min = 0, v153apositiontaking_52_min = 0; // aMin

            double v153mpositiontaking_51 = 0, v153mpositiontaking_52 = 0; // m_3141

            double v153apositiontaking_51 = 0, v153apositiontaking_52 = 0; // a_3141

            double v153mapositiontaking_51_max = 0, v153mapositiontaking_52_max = 0; // maMax

            double v153mapositiontaking_51_min = 0, v153mapositiontaking_52_min = 0; // maMin

            double auto_v153mpositiontaking_51 = 0, auto_v153mpositiontaking_52 = 0; // mAuto

            double auto_v153mpositiontaking_51_max = 0, auto_v153mpositiontaking_52_max = 0; // mAutoMax

            bool isauto_v153mpositiontaking_51 = false, isauto_v153mpositiontaking_52 = false; // isAuto

            // v151apositiontaking_
            double.TryParse(dr["v153apositiontaking_51_max"].ToString(), out v153apositiontaking_51_max_2); // aMax2_3141
            double.TryParse(dr["v153apositiontaking_52_max"].ToString(), out v153apositiontaking_52_max_2);

            double.TryParse(dr["v153apositiontaking_51_min"].ToString(), out v153apositiontaking_51_min); // aMin_3141
            double.TryParse(dr["v153apositiontaking_52_min"].ToString(), out v153apositiontaking_52_min);

            double.TryParse(dr["v153mpositiontaking_51"].ToString(), out v153mpositiontaking_51); // m_3141
            double.TryParse(dr["v153mpositiontaking_52"].ToString(), out v153mpositiontaking_52);

            double.TryParse(dr["v153apositiontaking_51"].ToString(), out v153apositiontaking_51); // a_3141
            double.TryParse(dr["v153apositiontaking_52"].ToString(), out v153apositiontaking_52);

            double.TryParse(dr["v153mapositiontaking_51_max"].ToString(), out v153mapositiontaking_51_max); // maMax_3141
            double.TryParse(dr["v153mapositiontaking_52_max"].ToString(), out v153mapositiontaking_52_max);

            double.TryParse(dr["v153mapositiontaking_51_min"].ToString(), out v153mapositiontaking_51_min);
            double.TryParse(dr["v153mapositiontaking_52_min"].ToString(), out v153mapositiontaking_52_min);

            double.TryParse(dr["auto_v153mpositiontaking_51"].ToString(), out auto_v153mpositiontaking_51); // mAuto_3141
            double.TryParse(dr["auto_v153mpositiontaking_52"].ToString(), out auto_v153mpositiontaking_52);

            double.TryParse(dr["auto_v153mpositiontaking_51_max"].ToString(), out auto_v153mpositiontaking_51_max); // mAutoMax_3141
            double.TryParse(dr["auto_v153mpositiontaking_52_max"].ToString(), out auto_v153mpositiontaking_52_max);

            isauto_v153mpositiontaking_51 = Convert.ToBoolean(dr["isauto_v153mpositiontaking_51"]); // isAuto_3141
            isauto_v153mpositiontaking_52 = Convert.ToBoolean(dr["isauto_v153mpositiontaking_52"]);

            v153mpositiontaking_51_max = v153mapositiontaking_51_max - v153apositiontaking_51_min;
            v153mpositiontaking_52_max = v153mapositiontaking_52_max - v153apositiontaking_52_min;

            v153apositiontaking_51_max = (v153mapositiontaking_51_max - v153mpositiontaking_51 > v153apositiontaking_51_max_2) ? v153apositiontaking_51_max_2 : v153mapositiontaking_51_max - v153mpositiontaking_51;
            v153apositiontaking_52_max = (v153mapositiontaking_52_max - v153mpositiontaking_52 > v153apositiontaking_52_max_2) ? v153apositiontaking_52_max_2 : v153mapositiontaking_52_max - v153mpositiontaking_52;

            this.JavaScriptRegister("v153apositiontaking_51_max", v153apositiontaking_51_max_2 > v153mapositiontaking_51_max ? v153apositiontaking_51_max_2 : v153mapositiontaking_51_max);
            this.JavaScriptRegister("v153apositiontaking_52_max", v153apositiontaking_52_max_2 > v153mapositiontaking_52_max ? v153apositiontaking_52_max_2 : v153mapositiontaking_52_max);

            this.JavaScriptRegister("v153apositiontaking_51_min", v153apositiontaking_51_min);
            this.JavaScriptRegister("v153apositiontaking_52_min", v153apositiontaking_52_min);

            this.JavaScriptRegister("v153mapositiontaking_51_max", v153mapositiontaking_51_max);
            this.JavaScriptRegister("v153mapositiontaking_52_max", v153mapositiontaking_52_max);

            this.JavaScriptRegister("IsAutoMaster_153_51", isauto_v153mpositiontaking_51);
            this.JavaScriptRegister("IsAutoMaster_153_52", isauto_v153mpositiontaking_52);

            tpl.agent_win_val = new PT_Selection(TemplateMain, "listAgent_153_51", v153apositiontaking_51, v153apositiontaking_51_min, v153apositiontaking_51_max, 0.1, true, false);
            tpl.agent_place_val = new PT_Selection(TemplateMain, "listAgent_153_52", v153apositiontaking_52, v153apositiontaking_52_min, v153apositiontaking_52_max, 0.1);

            string selectOnChange_41 = "SelectOnChangePT(this,'listAgent_153_51','v153mapositiontaking_51_max','v153apositiontaking_51_max','v153apositiontaking_51_min');";
            string selectOnChange_42 = "SelectOnChangePT(this,'listAgent_153_52','v153mapositiontaking_52_max','v153apositiontaking_52_max','v153apositiontaking_52_min');";

            tpl.master_preset_win_val = new PT_Selection(TemplateMain, "listMaster_153_51", v153mpositiontaking_51, isauto_v153mpositiontaking_51, 0, v153mpositiontaking_51_max, 0.1, true, false, selectOnChange_41);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_153_51", auto_v153mpositiontaking_51, !isauto_v153mpositiontaking_51, 0, auto_v153mpositiontaking_51_max, 0.1, true, false);

            tpl.master_preset_place_val = new PT_Selection(TemplateMain, "listMaster_153_52", v153mpositiontaking_52, isauto_v153mpositiontaking_52, 0, v153mpositiontaking_52_max, 0.1, selectOnChange_42);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_153_52", auto_v153mpositiontaking_52, !isauto_v153mpositiontaking_52, 0, auto_v153mpositiontaking_52_max, 0.1);

            string autoOnCheck_51 = "CheckAuto(this,'listMaster_153_51','listAgent_153_51','v153mapositiontaking_51_max','v153apositiontaking_51_max','v153apositiontaking_51_min');";
            string autoOnCheck_52 = "CheckAuto(this,'listMaster_153_52','listAgent_153_52','v153mapositiontaking_52_max','v153apositiontaking_52_max','v153apositiontaking_52_min');";

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_153_51", isauto_v153mpositiontaking_51, autoOnCheck_51);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_153_52", isauto_v153mpositiontaking_52, autoOnCheck_52);

            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_153_51)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_153_51", checkCopy_Onclick, Resources.Racing.copyfromwin);

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_153_51", "153_51");
            }
        }
    }
}