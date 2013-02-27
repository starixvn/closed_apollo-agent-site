// --------------------------------------------------------------------------
// <copyright file="OnMaster.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.06.01">Improve performance, Check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook.Super
{
    using System;
    using System.Data;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.UI._MemberInfo.RaceBook.AllowUpdatePT_Control;
    using Sunplus.Agent.UI.PositionTaking.RaceBook.Super;

    public abstract class OnMaster_Page : RaceBookSettingBase_Page
    {
        #region Variable

        private AllowUpdatePT_Control_Template allowcheckbox = new AllowUpdatePT_Control_Template();
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

        public void RenderPT_151_41(DataRow dr, OnMaster_Section_151_Section_151_41_Block tpl)
        {
            double v151mpositiontaking_41_max = 0, v151mpositiontaking_42_max = 0; // mMax

            double v151mpositiontaking_41_max_2 = 0, v151mpositiontaking_42_max_2 = 0; // mMax2

            double v151mpositiontaking_41_min = 0, v151mpositiontaking_42_min = 0; // mMin

            double v151spositiontaking_41_max = 0, v151spositiontaking_42_max = 0; // sMax

            double v151spositiontaking_41 = 0, v151spositiontaking_42 = 0; // s_3141

            double v151mpositiontaking_41 = 0, v151mpositiontaking_42 = 0; // m_3141

            double v151mspositiontaking_41_max = 0, v151mspositiontaking_42_max = 0; // msMax

            double v151mspositiontaking_41_min = 0, v151mspositiontaking_42_min = 0; // msMin

            double auto_v151spositiontaking_41 = 0, auto_v151spositiontaking_42 = 0; // sAuto

            double auto_v151spositiontaking_41_max = 0, auto_v151spositiontaking_42_max = 0; // sAutoMax

            bool isauto_v151spositiontaking_41 = false, isauto_v151spositiontaking_42 = false; // isAuto

            double.TryParse(dr["v151mpositiontaking_41_max"].ToString(), out v151mpositiontaking_41_max_2);
            double.TryParse(dr["v151mpositiontaking_42_max"].ToString(), out v151mpositiontaking_42_max_2);

            double.TryParse(dr["v151mpositiontaking_41_min"].ToString(), out v151mpositiontaking_41_min);
            double.TryParse(dr["v151mpositiontaking_42_min"].ToString(), out v151mpositiontaking_42_min);

            double.TryParse(dr["v151spositiontaking_41_max"].ToString(), out v151spositiontaking_41_max);
            double.TryParse(dr["v151spositiontaking_42_max"].ToString(), out v151spositiontaking_42_max);

            double.TryParse(dr["v151spositiontaking_41"].ToString(), out v151spositiontaking_41);
            double.TryParse(dr["v151spositiontaking_42"].ToString(), out v151spositiontaking_42);

            double.TryParse(dr["v151mpositiontaking_41"].ToString(), out v151mpositiontaking_41);
            double.TryParse(dr["v151mpositiontaking_42"].ToString(), out v151mpositiontaking_42);

            double.TryParse(dr["v151mspositiontaking_41_max"].ToString(), out v151mspositiontaking_41_max);
            double.TryParse(dr["v151mspositiontaking_42_max"].ToString(), out v151mspositiontaking_42_max);

            double.TryParse(dr["v151mspositiontaking_41_min"].ToString(), out v151mspositiontaking_41_min);
            double.TryParse(dr["v151mspositiontaking_42_min"].ToString(), out v151mspositiontaking_42_min);

            double.TryParse(dr["auto_v151spositiontaking_41"].ToString(), out auto_v151spositiontaking_41);
            double.TryParse(dr["auto_v151spositiontaking_42"].ToString(), out auto_v151spositiontaking_42);

            double.TryParse(dr["auto_v151spositiontaking_41_max"].ToString(), out auto_v151spositiontaking_41_max);
            double.TryParse(dr["auto_v151spositiontaking_42_max"].ToString(), out auto_v151spositiontaking_42_max);

            isauto_v151spositiontaking_41 = Convert.ToBoolean(dr["isauto_v151spositiontaking_41"]);
            isauto_v151spositiontaking_42 = Convert.ToBoolean(dr["isauto_v151spositiontaking_42"]);

            v151spositiontaking_41_max = v151spositiontaking_41_max - v151mpositiontaking_41_min;
            v151spositiontaking_42_max = v151spositiontaking_42_max - v151mpositiontaking_42_min;

            v151mpositiontaking_41_max = (isauto_v151spositiontaking_41 == true) ? v151mspositiontaking_41_max : v151mspositiontaking_41_max - v151spositiontaking_41;
            v151mpositiontaking_41_max = (v151mpositiontaking_41_max > v151mpositiontaking_41_max_2) ? v151mpositiontaking_41_max_2 : v151mpositiontaking_41_max;

            v151mpositiontaking_42_max = (isauto_v151spositiontaking_42 == true) ? v151mspositiontaking_42_max : v151mspositiontaking_42_max - v151spositiontaking_42;
            v151mpositiontaking_42_max = (v151mpositiontaking_42_max > v151mpositiontaking_42_max_2) ? v151mpositiontaking_42_max_2 : v151mpositiontaking_42_max;

            this.JavaScriptRegister("v151mpositiontaking_41_max", v151mpositiontaking_41_max_2);
            this.JavaScriptRegister("v151mpositiontaking_42_max", v151mpositiontaking_42_max_2);

            this.JavaScriptRegister("v151mspositiontaking_41_max", v151mspositiontaking_41_max);
            this.JavaScriptRegister("v151mspositiontaking_42_max", v151mspositiontaking_42_max);

            this.JavaScriptRegister("v151mpositiontaking_41_min", v151mpositiontaking_41_min);
            this.JavaScriptRegister("v151mpositiontaking_42_min", v151mpositiontaking_42_min);

            this.JavaScriptRegister("IsAutoSuper_151_41", isauto_v151spositiontaking_41);
            this.JavaScriptRegister("IsAutoSuper_151_42", isauto_v151spositiontaking_42);

            tpl.master_win_val = new PT_Selection(TemplateMain, "listMaster_151_41", v151mpositiontaking_41, v151mpositiontaking_41_min, v151mpositiontaking_41_max, 0.1, true, false);
            tpl.master_place_val = new PT_Selection(TemplateMain, "listMaster_151_42", v151mpositiontaking_42, v151mpositiontaking_42_min, v151mpositiontaking_42_max, 0.1);

            string selectOnChange_41 = "SelectOnChangePT(this,'listMaster_151_41','v151mspositiontaking_41_max','v151mpositiontaking_41_max','v151mpositiontaking_41_min');";
            string selectOnChange_42 = "SelectOnChangePT(this,'listMaster_151_42','v151mspositiontaking_42_max','v151mpositiontaking_42_max','v151mpositiontaking_42_min');";

            tpl.super_win_val = new PT_Selection(TemplateMain, "listSuper_151_41", v151spositiontaking_41, isauto_v151spositiontaking_41, 0, v151spositiontaking_41_max, 0.1, true, false, selectOnChange_41);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_151_41", auto_v151spositiontaking_41, !isauto_v151spositiontaking_41, 0, auto_v151spositiontaking_41_max, 0.1, true, false);

            tpl.super_place_val = new PT_Selection(TemplateMain, "listSuper_151_42", v151spositiontaking_42, isauto_v151spositiontaking_42, 0, v151spositiontaking_42_max, 0.1, selectOnChange_42);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_151_42", auto_v151spositiontaking_42, !isauto_v151spositiontaking_42, 0, auto_v151spositiontaking_42_max, 0.1);

            string autoOnCheck_41 = "CheckAuto(this,'listSuper_151_41','listMaster_151_41','v151mspositiontaking_41_max','v151mpositiontaking_41_max','v151mpositiontaking_41_min');";
            string autoOnCheck_42 = "CheckAuto(this,'listSuper_151_42','listMaster_151_42','v151mspositiontaking_42_max','v151mpositiontaking_42_max','v151mpositiontaking_42_min');";
            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_41", isauto_v151spositiontaking_41, autoOnCheck_41);
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_42", isauto_v151spositiontaking_42, autoOnCheck_42);

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

        public void RenderPT_151_31(DataRow dr, OnMaster_Section_151_Section_151_31_Block tpl)
        {
            double v151mpositiontaking_31_max = 0, v151mpositiontaking_32_max = 0; // mMax

            double v151mpositiontaking_31_max_2 = 0, v151mpositiontaking_32_max_2 = 0; // mMax2

            double v151mpositiontaking_31_min = 0, v151mpositiontaking_32_min = 0; // mMin

            double v151spositiontaking_31_max = 0, v151spositiontaking_32_max = 0; // sMax

            double v151spositiontaking_31 = 0, v151spositiontaking_32 = 0; // s_3141

            double v151mpositiontaking_31 = 0, v151mpositiontaking_32 = 0; // m_3141

            double v151mspositiontaking_31_max = 0, v151mspositiontaking_32_max = 0; // msMax

            double v151mspositiontaking_31_min = 0, v151mspositiontaking_32_min = 0; // msMin

            double auto_v151spositiontaking_31 = 0, auto_v151spositiontaking_32 = 0; // sAuto

            double auto_v151spositiontaking_31_max = 0, auto_v151spositiontaking_32_max = 0; // sAutoMax

            bool isauto_v151spositiontaking_31 = false, isauto_v151spositiontaking_32 = false; // isAuto

            double.TryParse(dr["v151mpositiontaking_31_max"].ToString(), out v151mpositiontaking_31_max_2);
            double.TryParse(dr["v151mpositiontaking_32_max"].ToString(), out v151mpositiontaking_32_max_2);

            double.TryParse(dr["v151mpositiontaking_31_min"].ToString(), out v151mpositiontaking_31_min);
            double.TryParse(dr["v151mpositiontaking_32_min"].ToString(), out v151mpositiontaking_32_min);

            double.TryParse(dr["v151spositiontaking_31_max"].ToString(), out v151spositiontaking_31_max);
            double.TryParse(dr["v151spositiontaking_32_max"].ToString(), out v151spositiontaking_32_max);

            double.TryParse(dr["v151spositiontaking_31"].ToString(), out v151spositiontaking_31);
            double.TryParse(dr["v151spositiontaking_32"].ToString(), out v151spositiontaking_32);

            double.TryParse(dr["v151mpositiontaking_31"].ToString(), out v151mpositiontaking_31);
            double.TryParse(dr["v151mpositiontaking_32"].ToString(), out v151mpositiontaking_32);

            double.TryParse(dr["v151mspositiontaking_31_max"].ToString(), out v151mspositiontaking_31_max);
            double.TryParse(dr["v151mspositiontaking_32_max"].ToString(), out v151mspositiontaking_32_max);

            double.TryParse(dr["v151mspositiontaking_31_min"].ToString(), out v151mspositiontaking_31_min);
            double.TryParse(dr["v151mspositiontaking_32_min"].ToString(), out v151mspositiontaking_32_min);

            double.TryParse(dr["auto_v151spositiontaking_31"].ToString(), out auto_v151spositiontaking_31);
            double.TryParse(dr["auto_v151spositiontaking_32"].ToString(), out auto_v151spositiontaking_32);

            double.TryParse(dr["auto_v151spositiontaking_31_max"].ToString(), out auto_v151spositiontaking_31_max);
            double.TryParse(dr["auto_v151spositiontaking_32_max"].ToString(), out auto_v151spositiontaking_32_max);

            isauto_v151spositiontaking_31 = Convert.ToBoolean(dr["isauto_v151spositiontaking_31"]);
            isauto_v151spositiontaking_32 = Convert.ToBoolean(dr["isauto_v151spositiontaking_32"]);

            v151spositiontaking_31_max = v151spositiontaking_31_max - v151mpositiontaking_31_min;
            v151spositiontaking_32_max = v151spositiontaking_32_max - v151mpositiontaking_32_min;

            v151mpositiontaking_31_max = (isauto_v151spositiontaking_31 == true) ? v151mspositiontaking_31_max : v151mspositiontaking_31_max - v151spositiontaking_31;
            v151mpositiontaking_31_max = (v151mpositiontaking_31_max > v151mpositiontaking_31_max_2) ? v151mpositiontaking_31_max_2 : v151mpositiontaking_31_max;

            v151mpositiontaking_32_max = (isauto_v151spositiontaking_32 == true) ? v151mspositiontaking_32_max : v151mspositiontaking_32_max - v151spositiontaking_32;
            v151mpositiontaking_32_max = (v151mpositiontaking_32_max > v151mpositiontaking_32_max_2) ? v151mpositiontaking_32_max_2 : v151mpositiontaking_32_max;

            this.JavaScriptRegister("v151mpositiontaking_31_max", v151mpositiontaking_31_max_2);
            this.JavaScriptRegister("v151mpositiontaking_32_max", v151mpositiontaking_32_max_2);

            this.JavaScriptRegister("v151mspositiontaking_31_max", v151mspositiontaking_31_max);
            this.JavaScriptRegister("v151mspositiontaking_32_max", v151mspositiontaking_32_max);

            this.JavaScriptRegister("v151mpositiontaking_31_min", v151mpositiontaking_31_min);
            this.JavaScriptRegister("v151mpositiontaking_32_min", v151mpositiontaking_32_min);

            this.JavaScriptRegister("IsAutoSuper_151_31", isauto_v151spositiontaking_31);
            this.JavaScriptRegister("IsAutoSuper_151_32", isauto_v151spositiontaking_32);

            tpl.master_win_val = new PT_Selection(TemplateMain, "listMaster_151_31", v151mpositiontaking_31, v151mpositiontaking_31_min, v151mpositiontaking_31_max, 0.1, true, false);
            tpl.master_place_val = new PT_Selection(TemplateMain, "listMaster_151_32", v151mpositiontaking_32, v151mpositiontaking_32_min, v151mpositiontaking_32_max, 0.1);

            string selectOnChange_31 = "SelectOnChangePT(this,'listMaster_151_31','v151mspositiontaking_31_max','v151mpositiontaking_31_max','v151mpositiontaking_31_min');";
            string selectOnChange_32 = "SelectOnChangePT(this,'listMaster_151_32','v151mspositiontaking_32_max','v151mpositiontaking_32_max','v151mpositiontaking_32_min');";

            tpl.super_win_val = new PT_Selection(TemplateMain, "listSuper_151_31", v151spositiontaking_31, isauto_v151spositiontaking_31, 0, v151spositiontaking_31_max, 0.1, true, false, selectOnChange_31);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_151_31", auto_v151spositiontaking_31, !isauto_v151spositiontaking_31, 0, auto_v151spositiontaking_31_max, 0.1, true, false);

            tpl.super_place_val = new PT_Selection(TemplateMain, "listSuper_151_32", v151spositiontaking_32, isauto_v151spositiontaking_32, 0, v151spositiontaking_32_max, 0.1, selectOnChange_32);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_151_32", auto_v151spositiontaking_32, !isauto_v151spositiontaking_32, 0, auto_v151spositiontaking_32_max, 0.1);

            string autoOnCheck_31 = "CheckAuto(this,'listSuper_151_31','listMaster_151_31','v151mspositiontaking_31_max','v151mpositiontaking_31_max','v151mpositiontaking_31_min');";
            string autoOnCheck_32 = "CheckAuto(this,'listSuper_151_32','listMaster_151_32','v151mspositiontaking_32_max','v151mpositiontaking_32_max','v151mpositiontaking_32_min');";
            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_31", isauto_v151spositiontaking_31, autoOnCheck_31);
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_32", isauto_v151spositiontaking_32, autoOnCheck_32);

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

        public void RenderPT_151_51(DataRow dr, OnMaster_Section_151_Section_151_51_Block tpl)
        {
            double v151mpositiontaking_51_max = 0, v151mpositiontaking_52_max = 0; // mMax

            double v151mpositiontaking_51_max_2 = 0, v151mpositiontaking_52_max_2 = 0; // mMax2

            double v151mpositiontaking_51_min = 0, v151mpositiontaking_52_min = 0; // mMin

            double v151spositiontaking_51_max = 0, v151spositiontaking_52_max = 0; // sMax

            double v151spositiontaking_51 = 0, v151spositiontaking_52 = 0; // s_5141

            double v151mpositiontaking_51 = 0, v151mpositiontaking_52 = 0; // m_5141

            double v151mspositiontaking_51_max = 0, v151mspositiontaking_52_max = 0; // msMax

            double v151mspositiontaking_51_min = 0, v151mspositiontaking_52_min = 0; // msMin

            double auto_v151spositiontaking_51 = 0, auto_v151spositiontaking_52 = 0; // sAuto

            double auto_v151spositiontaking_51_max = 0, auto_v151spositiontaking_52_max = 0; // sAutoMax

            bool isauto_v151spositiontaking_51 = false, isauto_v151spositiontaking_52 = false; // isAuto

            double.TryParse(dr["v151mpositiontaking_51_max"].ToString(), out v151mpositiontaking_51_max_2);
            double.TryParse(dr["v151mpositiontaking_52_max"].ToString(), out v151mpositiontaking_52_max_2);

            double.TryParse(dr["v151mpositiontaking_51_min"].ToString(), out v151mpositiontaking_51_min);
            double.TryParse(dr["v151mpositiontaking_52_min"].ToString(), out v151mpositiontaking_52_min);

            double.TryParse(dr["v151spositiontaking_51_max"].ToString(), out v151spositiontaking_51_max);
            double.TryParse(dr["v151spositiontaking_52_max"].ToString(), out v151spositiontaking_52_max);

            double.TryParse(dr["v151spositiontaking_51"].ToString(), out v151spositiontaking_51);
            double.TryParse(dr["v151spositiontaking_52"].ToString(), out v151spositiontaking_52);

            double.TryParse(dr["v151mpositiontaking_51"].ToString(), out v151mpositiontaking_51);
            double.TryParse(dr["v151mpositiontaking_52"].ToString(), out v151mpositiontaking_52);

            double.TryParse(dr["v151mspositiontaking_51_max"].ToString(), out v151mspositiontaking_51_max);
            double.TryParse(dr["v151mspositiontaking_52_max"].ToString(), out v151mspositiontaking_52_max);

            double.TryParse(dr["v151mspositiontaking_51_min"].ToString(), out v151mspositiontaking_51_min);
            double.TryParse(dr["v151mspositiontaking_52_min"].ToString(), out v151mspositiontaking_52_min);

            double.TryParse(dr["auto_v151spositiontaking_51"].ToString(), out auto_v151spositiontaking_51);
            double.TryParse(dr["auto_v151spositiontaking_52"].ToString(), out auto_v151spositiontaking_52);

            double.TryParse(dr["auto_v151spositiontaking_51_max"].ToString(), out auto_v151spositiontaking_51_max);
            double.TryParse(dr["auto_v151spositiontaking_52_max"].ToString(), out auto_v151spositiontaking_52_max);

            isauto_v151spositiontaking_51 = Convert.ToBoolean(dr["isauto_v151spositiontaking_51"]);
            isauto_v151spositiontaking_52 = Convert.ToBoolean(dr["isauto_v151spositiontaking_52"]);

            v151spositiontaking_51_max = v151spositiontaking_51_max - v151mpositiontaking_51_min;
            v151spositiontaking_52_max = v151spositiontaking_52_max - v151mpositiontaking_52_min;

            v151mpositiontaking_51_max = (isauto_v151spositiontaking_51 == true) ? v151mspositiontaking_51_max : v151mspositiontaking_51_max - v151spositiontaking_51;
            v151mpositiontaking_51_max = (v151mpositiontaking_51_max > v151mpositiontaking_51_max_2) ? v151mpositiontaking_51_max_2 : v151mpositiontaking_51_max;

            v151mpositiontaking_52_max = (isauto_v151spositiontaking_52 == true) ? v151mspositiontaking_52_max : v151mspositiontaking_52_max - v151spositiontaking_52;
            v151mpositiontaking_52_max = (v151mpositiontaking_52_max > v151mpositiontaking_52_max_2) ? v151mpositiontaking_52_max_2 : v151mpositiontaking_52_max;

            this.JavaScriptRegister("v151mpositiontaking_51_max", v151mpositiontaking_51_max_2);
            this.JavaScriptRegister("v151mpositiontaking_52_max", v151mpositiontaking_52_max_2);

            this.JavaScriptRegister("v151mspositiontaking_51_max", v151mspositiontaking_51_max);
            this.JavaScriptRegister("v151mspositiontaking_52_max", v151mspositiontaking_52_max);

            this.JavaScriptRegister("v151mpositiontaking_51_min", v151mpositiontaking_51_min);
            this.JavaScriptRegister("v151mpositiontaking_52_min", v151mpositiontaking_52_min);

            this.JavaScriptRegister("IsAutoSuper_151_51", isauto_v151spositiontaking_51);
            this.JavaScriptRegister("IsAutoSuper_151_52", isauto_v151spositiontaking_52);

            tpl.master_win_val = new PT_Selection(TemplateMain, "listMaster_151_51", v151mpositiontaking_51, v151mpositiontaking_51_min, v151mpositiontaking_51_max, 0.1, true, false);
            tpl.master_place_val = new PT_Selection(TemplateMain, "listMaster_151_52", v151mpositiontaking_52, v151mpositiontaking_52_min, v151mpositiontaking_52_max, 0.1);

            string selectOnChange_51 = "SelectOnChangePT(this,'listMaster_151_51','v151mspositiontaking_51_max','v151mpositiontaking_51_max','v151mpositiontaking_51_min');";
            string selectOnChange_52 = "SelectOnChangePT(this,'listMaster_151_52','v151mspositiontaking_52_max','v151mpositiontaking_52_max','v151mpositiontaking_52_min');";

            tpl.super_win_val = new PT_Selection(TemplateMain, "listSuper_151_51", v151spositiontaking_51, isauto_v151spositiontaking_51, 0, v151spositiontaking_51_max, 0.1, true, false, selectOnChange_51);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_151_51", auto_v151spositiontaking_51, !isauto_v151spositiontaking_51, 0, auto_v151spositiontaking_51_max, 0.1, true, false);

            tpl.super_place_val = new PT_Selection(TemplateMain, "listSuper_151_52", v151spositiontaking_52, isauto_v151spositiontaking_52, 0, v151spositiontaking_52_max, 0.1, selectOnChange_52);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_151_52", auto_v151spositiontaking_52, !isauto_v151spositiontaking_52, 0, auto_v151spositiontaking_52_max, 0.1);

            string autoOnCheck_51 = "CheckAuto(this,'listSuper_151_51','listMaster_151_51','v151mspositiontaking_51_max','v151mpositiontaking_51_max','v151mpositiontaking_51_min');";
            string autoOnCheck_52 = "CheckAuto(this,'listSuper_151_52','listMaster_151_52','v151mspositiontaking_52_max','v151mpositiontaking_52_max','v151mpositiontaking_52_min');";
            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_51", isauto_v151spositiontaking_51, autoOnCheck_51);
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_52", isauto_v151spositiontaking_52, autoOnCheck_52);

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

        public void RenderPT_152_41(DataRow dr, OnMaster_Section_152_Section_152_41_Block tpl)
        {
            double v152mpositiontaking_41_max = 0, v152mpositiontaking_42_max = 0; // mMax

            double v152mpositiontaking_41_max_2 = 0, v152mpositiontaking_42_max_2 = 0; // mMax2

            double v152mpositiontaking_41_min = 0, v152mpositiontaking_42_min = 0; // mMin

            double v152spositiontaking_41_max = 0, v152spositiontaking_42_max = 0; // sMax

            double v152spositiontaking_41 = 0, v152spositiontaking_42 = 0; // s_3141

            double v152mpositiontaking_41 = 0, v152mpositiontaking_42 = 0; // m_3141

            double v152mspositiontaking_41_max = 0, v152mspositiontaking_42_max = 0; // msMax

            double v152mspositiontaking_41_min = 0, v152mspositiontaking_42_min = 0; // msMin

            double auto_v152spositiontaking_41 = 0, auto_v152spositiontaking_42 = 0; // sAuto

            double auto_v152spositiontaking_41_max = 0, auto_v152spositiontaking_42_max = 0; // sAutoMax

            bool isauto_v152spositiontaking_41 = false, isauto_v152spositiontaking_42 = false; // isAuto

            double.TryParse(dr["v152mpositiontaking_41_max"].ToString(), out v152mpositiontaking_41_max_2);
            double.TryParse(dr["v152mpositiontaking_42_max"].ToString(), out v152mpositiontaking_42_max_2);

            double.TryParse(dr["v152mpositiontaking_41_min"].ToString(), out v152mpositiontaking_41_min);
            double.TryParse(dr["v152mpositiontaking_42_min"].ToString(), out v152mpositiontaking_42_min);

            double.TryParse(dr["v152spositiontaking_41_max"].ToString(), out v152spositiontaking_41_max);
            double.TryParse(dr["v152spositiontaking_42_max"].ToString(), out v152spositiontaking_42_max);

            double.TryParse(dr["v152spositiontaking_41"].ToString(), out v152spositiontaking_41);
            double.TryParse(dr["v152spositiontaking_42"].ToString(), out v152spositiontaking_42);

            double.TryParse(dr["v152mpositiontaking_41"].ToString(), out v152mpositiontaking_41);
            double.TryParse(dr["v152mpositiontaking_42"].ToString(), out v152mpositiontaking_42);

            double.TryParse(dr["v152mspositiontaking_41_max"].ToString(), out v152mspositiontaking_41_max);
            double.TryParse(dr["v152mspositiontaking_42_max"].ToString(), out v152mspositiontaking_42_max);

            double.TryParse(dr["v152mspositiontaking_41_min"].ToString(), out v152mspositiontaking_41_min);
            double.TryParse(dr["v152mspositiontaking_42_min"].ToString(), out v152mspositiontaking_42_min);

            double.TryParse(dr["auto_v152spositiontaking_41"].ToString(), out auto_v152spositiontaking_41);
            double.TryParse(dr["auto_v152spositiontaking_42"].ToString(), out auto_v152spositiontaking_42);

            double.TryParse(dr["auto_v152spositiontaking_41_max"].ToString(), out auto_v152spositiontaking_41_max);
            double.TryParse(dr["auto_v152spositiontaking_42_max"].ToString(), out auto_v152spositiontaking_42_max);

            isauto_v152spositiontaking_41 = Convert.ToBoolean(dr["isauto_v152spositiontaking_41"]);
            isauto_v152spositiontaking_42 = Convert.ToBoolean(dr["isauto_v152spositiontaking_42"]);

            v152spositiontaking_41_max = v152spositiontaking_41_max - v152mpositiontaking_41_min;
            v152spositiontaking_42_max = v152spositiontaking_42_max - v152mpositiontaking_42_min;

            v152mpositiontaking_41_max = (isauto_v152spositiontaking_41 == true) ? v152mspositiontaking_41_max : v152mspositiontaking_41_max - v152spositiontaking_41;
            v152mpositiontaking_41_max = (v152mpositiontaking_41_max > v152mpositiontaking_41_max_2) ? v152mpositiontaking_41_max_2 : v152mpositiontaking_41_max;

            v152mpositiontaking_42_max = (isauto_v152spositiontaking_42 == true) ? v152mspositiontaking_42_max : v152mspositiontaking_42_max - v152spositiontaking_42;
            v152mpositiontaking_42_max = (v152mpositiontaking_42_max > v152mpositiontaking_42_max_2) ? v152mpositiontaking_42_max_2 : v152mpositiontaking_42_max;

            this.JavaScriptRegister("v152mpositiontaking_41_max", v152mpositiontaking_41_max_2);
            this.JavaScriptRegister("v152mpositiontaking_42_max", v152mpositiontaking_42_max_2);

            this.JavaScriptRegister("v152mspositiontaking_41_max", v152mspositiontaking_41_max);
            this.JavaScriptRegister("v152mspositiontaking_42_max", v152mspositiontaking_42_max);

            this.JavaScriptRegister("v152mpositiontaking_41_min", v152mpositiontaking_41_min);
            this.JavaScriptRegister("v152mpositiontaking_42_min", v152mpositiontaking_42_min);

            this.JavaScriptRegister("IsAutoSuper_152_41", isauto_v152spositiontaking_41);
            this.JavaScriptRegister("IsAutoSuper_152_42", isauto_v152spositiontaking_42);

            tpl.master_win_val = new PT_Selection(TemplateMain, "listMaster_152_41", v152mpositiontaking_41, v152mpositiontaking_41_min, v152mpositiontaking_41_max, 0.1, true, false);
            tpl.master_place_val = new PT_Selection(TemplateMain, "listMaster_152_42", v152mpositiontaking_42, v152mpositiontaking_42_min, v152mpositiontaking_42_max, 0.1);

            string selectOnChange_41 = "SelectOnChangePT(this,'listMaster_152_41','v152mspositiontaking_41_max','v152mpositiontaking_41_max','v152mpositiontaking_41_min');";
            string selectOnChange_42 = "SelectOnChangePT(this,'listMaster_152_42','v152mspositiontaking_42_max','v152mpositiontaking_42_max','v152mpositiontaking_42_min');";

            tpl.super_win_val = new PT_Selection(TemplateMain, "listSuper_152_41", v152spositiontaking_41, isauto_v152spositiontaking_41, 0, v152spositiontaking_41_max, 0.1, true, false, selectOnChange_41);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_152_41", auto_v152spositiontaking_41, !isauto_v152spositiontaking_41, 0, auto_v152spositiontaking_41_max, 0.1, true, false);

            tpl.super_place_val = new PT_Selection(TemplateMain, "listSuper_152_42", v152spositiontaking_42, isauto_v152spositiontaking_42, 0, v152spositiontaking_42_max, 0.1, selectOnChange_42);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_152_42", auto_v152spositiontaking_42, !isauto_v152spositiontaking_42, 0, auto_v152spositiontaking_42_max, 0.1);

            string autoOnCheck_41 = "CheckAuto(this,'listSuper_152_41','listMaster_152_41','v152mspositiontaking_41_max','v152mpositiontaking_41_max','v152mpositiontaking_41_min');";
            string autoOnCheck_42 = "CheckAuto(this,'listSuper_152_42','listMaster_152_42','v152mspositiontaking_42_max','v152mpositiontaking_42_max','v152mpositiontaking_42_min');";
            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_152_41", isauto_v152spositiontaking_41, autoOnCheck_41);
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_152_42", isauto_v152spositiontaking_42, autoOnCheck_42);

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

        public void RenderPT_152_51(DataRow dr, OnMaster_Section_152_Section_152_51_Block tpl)
        {
            double v152mpositiontaking_51_max = 0, v152mpositiontaking_52_max = 0; // mMax

            double v152mpositiontaking_51_max_2 = 0, v152mpositiontaking_52_max_2 = 0; // mMax2

            double v152mpositiontaking_51_min = 0, v152mpositiontaking_52_min = 0; // mMin

            double v152spositiontaking_51_max = 0, v152spositiontaking_52_max = 0; // sMax

            double v152spositiontaking_51 = 0, v152spositiontaking_52 = 0; // s_3141

            double v152mpositiontaking_51 = 0, v152mpositiontaking_52 = 0; // m_3141

            double v152mspositiontaking_51_max = 0, v152mspositiontaking_52_max = 0; // msMax

            double v152mspositiontaking_51_min = 0, v152mspositiontaking_52_min = 0; // msMin

            double auto_v152spositiontaking_51 = 0, auto_v152spositiontaking_52 = 0; // sAuto

            double auto_v152spositiontaking_51_max = 0, auto_v152spositiontaking_52_max = 0; // sAutoMax

            bool isauto_v152spositiontaking_51 = false, isauto_v152spositiontaking_52 = false; // isAuto

            double.TryParse(dr["v152mpositiontaking_51_max"].ToString(), out v152mpositiontaking_51_max_2);
            double.TryParse(dr["v152mpositiontaking_52_max"].ToString(), out v152mpositiontaking_52_max_2);

            double.TryParse(dr["v152mpositiontaking_51_min"].ToString(), out v152mpositiontaking_51_min);
            double.TryParse(dr["v152mpositiontaking_52_min"].ToString(), out v152mpositiontaking_52_min);

            double.TryParse(dr["v152spositiontaking_51_max"].ToString(), out v152spositiontaking_51_max);
            double.TryParse(dr["v152spositiontaking_52_max"].ToString(), out v152spositiontaking_52_max);

            double.TryParse(dr["v152spositiontaking_51"].ToString(), out v152spositiontaking_51);
            double.TryParse(dr["v152spositiontaking_52"].ToString(), out v152spositiontaking_52);

            double.TryParse(dr["v152mpositiontaking_51"].ToString(), out v152mpositiontaking_51);
            double.TryParse(dr["v152mpositiontaking_52"].ToString(), out v152mpositiontaking_52);

            double.TryParse(dr["v152mspositiontaking_51_max"].ToString(), out v152mspositiontaking_51_max);
            double.TryParse(dr["v152mspositiontaking_52_max"].ToString(), out v152mspositiontaking_52_max);

            double.TryParse(dr["v152mspositiontaking_51_min"].ToString(), out v152mspositiontaking_51_min);
            double.TryParse(dr["v152mspositiontaking_52_min"].ToString(), out v152mspositiontaking_52_min);

            double.TryParse(dr["auto_v152spositiontaking_51"].ToString(), out auto_v152spositiontaking_51);
            double.TryParse(dr["auto_v152spositiontaking_52"].ToString(), out auto_v152spositiontaking_52);

            double.TryParse(dr["auto_v152spositiontaking_51_max"].ToString(), out auto_v152spositiontaking_51_max);
            double.TryParse(dr["auto_v152spositiontaking_52_max"].ToString(), out auto_v152spositiontaking_52_max);

            isauto_v152spositiontaking_51 = Convert.ToBoolean(dr["isauto_v152spositiontaking_51"]);
            isauto_v152spositiontaking_52 = Convert.ToBoolean(dr["isauto_v152spositiontaking_52"]);

            v152spositiontaking_51_max = v152spositiontaking_51_max - v152mpositiontaking_51_min;
            v152spositiontaking_52_max = v152spositiontaking_52_max - v152mpositiontaking_52_min;

            v152mpositiontaking_51_max = (isauto_v152spositiontaking_51 == true) ? v152mspositiontaking_51_max : v152mspositiontaking_51_max - v152spositiontaking_51;
            v152mpositiontaking_51_max = (v152mpositiontaking_51_max > v152mpositiontaking_51_max_2) ? v152mpositiontaking_51_max_2 : v152mpositiontaking_51_max;

            v152mpositiontaking_52_max = (isauto_v152spositiontaking_52 == true) ? v152mspositiontaking_52_max : v152mspositiontaking_52_max - v152spositiontaking_52;
            v152mpositiontaking_52_max = (v152mpositiontaking_52_max > v152mpositiontaking_52_max_2) ? v152mpositiontaking_52_max_2 : v152mpositiontaking_52_max;

            this.JavaScriptRegister("v152mpositiontaking_51_max", v152mpositiontaking_51_max_2);
            this.JavaScriptRegister("v152mpositiontaking_52_max", v152mpositiontaking_52_max_2);

            this.JavaScriptRegister("v152mspositiontaking_51_max", v152mspositiontaking_51_max);
            this.JavaScriptRegister("v152mspositiontaking_52_max", v152mspositiontaking_52_max);

            this.JavaScriptRegister("v152mpositiontaking_51_min", v152mspositiontaking_51_min);
            this.JavaScriptRegister("v152mpositiontaking_52_min", v152mspositiontaking_52_min);

            this.JavaScriptRegister("IsAutoSuper_152_51", isauto_v152spositiontaking_51);
            this.JavaScriptRegister("IsAutoSuper_152_52", isauto_v152spositiontaking_52);

            tpl.master_win_val = new PT_Selection(TemplateMain, "listMaster_152_51", v152mpositiontaking_51, v152mpositiontaking_51_min, v152mpositiontaking_51_max, 0.1, true, false);
            tpl.master_place_val = new PT_Selection(TemplateMain, "listMaster_152_52", v152mpositiontaking_52, v152mpositiontaking_52_min, v152mpositiontaking_52_max, 0.1);

            string selectOnChange_51 = "SelectOnChangePT(this,'listMaster_152_51','v152mspositiontaking_51_max','v152mpositiontaking_51_max','v152mpositiontaking_51_min');";
            string selectOnChange_52 = "SelectOnChangePT(this,'listMaster_152_52','v152mspositiontaking_52_max','v152mpositiontaking_52_max','v152mpositiontaking_52_min');";

            tpl.super_win_val = new PT_Selection(TemplateMain, "listSuper_152_51", v152spositiontaking_51, isauto_v152spositiontaking_51, 0, v152spositiontaking_51_max, 0.1, true, false, selectOnChange_51);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_152_51", auto_v152spositiontaking_51, !isauto_v152spositiontaking_51, 0, auto_v152spositiontaking_51_max, 0.1, true, false);

            tpl.super_place_val = new PT_Selection(TemplateMain, "listSuper_152_52", v152spositiontaking_52, isauto_v152spositiontaking_52, 0, v152spositiontaking_52_max, 0.1, selectOnChange_52);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_152_52", auto_v152spositiontaking_52, !isauto_v152spositiontaking_52, 0, auto_v152spositiontaking_52_max, 0.1);

            string autoOnCheck_51 = "CheckAuto(this,'listSuper_152_51','listMaster_152_51','v152mspositiontaking_51_max','v152mpositiontaking_51_max','v152mpositiontaking_51_min');";
            string autoOnCheck_52 = "CheckAuto(this,'listSuper_152_52','listMaster_152_52','v152mspositiontaking_52_max','v152mpositiontaking_52_max','v152mpositiontaking_52_min');";
            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_152_51", isauto_v152spositiontaking_51, autoOnCheck_51);
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_152_52", isauto_v152spositiontaking_52, autoOnCheck_52);

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

        public void RenderPT_153_51(DataRow dr, OnMaster_Section_153_Section_153_51_Block tpl)
        {
            double v153mpositiontaking_51_max = 0, v153mpositiontaking_52_max = 0; // mMax

            double v153mpositiontaking_51_max_2 = 0, v153mpositiontaking_52_max_2 = 0; // mMax2

            double v153mpositiontaking_51_min = 0, v153mpositiontaking_52_min = 0; // mMin

            double v153spositiontaking_51_max = 0, v153spositiontaking_52_max = 0; // sMax

            double v153spositiontaking_51 = 0, v153spositiontaking_52 = 0; // s_3141

            double v153mpositiontaking_51 = 0, v153mpositiontaking_52 = 0; // m_3141

            double v153mspositiontaking_51_max = 0, v153mspositiontaking_52_max = 0; // msMax

            double v153mspositiontaking_51_min = 0, v153mspositiontaking_52_min = 0; // msMin

            double auto_v153spositiontaking_51 = 0, auto_v153spositiontaking_52 = 0; // sAuto

            double auto_v153spositiontaking_51_max = 0, auto_v153spositiontaking_52_max = 0; // sAutoMax

            bool isauto_v153spositiontaking_51 = false, isauto_v153spositiontaking_52 = false; // isAuto

            double.TryParse(dr["v153mpositiontaking_51_max"].ToString(), out v153mpositiontaking_51_max_2);
            double.TryParse(dr["v153mpositiontaking_52_max"].ToString(), out v153mpositiontaking_52_max_2);

            double.TryParse(dr["v153mpositiontaking_51_min"].ToString(), out v153mpositiontaking_51_min);
            double.TryParse(dr["v153mpositiontaking_52_min"].ToString(), out v153mpositiontaking_52_min);

            double.TryParse(dr["v153spositiontaking_51_max"].ToString(), out v153spositiontaking_51_max);
            double.TryParse(dr["v153spositiontaking_52_max"].ToString(), out v153spositiontaking_52_max);

            double.TryParse(dr["v153spositiontaking_51"].ToString(), out v153spositiontaking_51);
            double.TryParse(dr["v153spositiontaking_52"].ToString(), out v153spositiontaking_52);

            double.TryParse(dr["v153mpositiontaking_51"].ToString(), out v153mpositiontaking_51);
            double.TryParse(dr["v153mpositiontaking_52"].ToString(), out v153mpositiontaking_52);

            double.TryParse(dr["v153mspositiontaking_51_max"].ToString(), out v153mspositiontaking_51_max);
            double.TryParse(dr["v153mspositiontaking_52_max"].ToString(), out v153mspositiontaking_52_max);

            double.TryParse(dr["v153mspositiontaking_51_min"].ToString(), out v153mspositiontaking_51_min);
            double.TryParse(dr["v153mspositiontaking_52_min"].ToString(), out v153mspositiontaking_52_min);

            double.TryParse(dr["auto_v153spositiontaking_51"].ToString(), out auto_v153spositiontaking_51);
            double.TryParse(dr["auto_v153spositiontaking_52"].ToString(), out auto_v153spositiontaking_52);

            double.TryParse(dr["auto_v153spositiontaking_51_max"].ToString(), out auto_v153spositiontaking_51_max);
            double.TryParse(dr["auto_v153spositiontaking_52_max"].ToString(), out auto_v153spositiontaking_52_max);

            isauto_v153spositiontaking_51 = Convert.ToBoolean(dr["isauto_v153spositiontaking_51"]);
            isauto_v153spositiontaking_52 = Convert.ToBoolean(dr["isauto_v153spositiontaking_52"]);

            v153spositiontaking_51_max = v153spositiontaking_51_max - v153mpositiontaking_51_min;
            v153spositiontaking_52_max = v153spositiontaking_52_max - v153mpositiontaking_52_min;

            v153mpositiontaking_51_max = (isauto_v153spositiontaking_51 == true) ? v153mspositiontaking_51_max : v153mspositiontaking_51_max - v153spositiontaking_51;
            v153mpositiontaking_51_max = (v153mpositiontaking_51_max > v153mpositiontaking_51_max_2) ? v153mpositiontaking_51_max_2 : v153mpositiontaking_51_max;

            v153mpositiontaking_52_max = (isauto_v153spositiontaking_52 == true) ? v153mspositiontaking_52_max : v153mspositiontaking_52_max - v153spositiontaking_52;
            v153mpositiontaking_52_max = (v153mpositiontaking_52_max > v153mpositiontaking_52_max_2) ? v153mpositiontaking_52_max_2 : v153mpositiontaking_52_max;

            this.JavaScriptRegister("v153mpositiontaking_51_max", v153mpositiontaking_51_max_2);
            this.JavaScriptRegister("v153mpositiontaking_52_max", v153mpositiontaking_52_max_2);

            this.JavaScriptRegister("v153mspositiontaking_51_max", v153mspositiontaking_51_max);
            this.JavaScriptRegister("v153mspositiontaking_52_max", v153mspositiontaking_52_max);

            this.JavaScriptRegister("v153mpositiontaking_51_min", v153mpositiontaking_51_min);
            this.JavaScriptRegister("v153mpositiontaking_52_min", v153mpositiontaking_52_min);

            this.JavaScriptRegister("IsAutoSuper_153_51", isauto_v153spositiontaking_51);
            this.JavaScriptRegister("IsAutoSuper_153_52", isauto_v153spositiontaking_52);

            tpl.master_win_val = new PT_Selection(TemplateMain, "listMaster_153_51", v153mpositiontaking_51, v153mpositiontaking_51_min, v153mpositiontaking_51_max, 0.1, true, false);
            tpl.master_place_val = new PT_Selection(TemplateMain, "listMaster_153_52", v153mpositiontaking_52, v153mpositiontaking_52_min, v153mpositiontaking_52_max, 0.1);

            string selectOnChange_51 = "SelectOnChangePT(this,'listMaster_153_51','v153mspositiontaking_51_max','v153mpositiontaking_51_max','v153mpositiontaking_51_min');";
            string selectOnChange_52 = "SelectOnChangePT(this,'listMaster_153_52','v153mspositiontaking_52_max','v153mpositiontaking_52_max','v153mpositiontaking_52_min');";

            tpl.super_win_val = new PT_Selection(TemplateMain, "listSuper_153_51", v153spositiontaking_51, isauto_v153spositiontaking_51, 0, v153spositiontaking_51_max, 0.1, true, false, selectOnChange_51);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_153_51", auto_v153spositiontaking_51, !isauto_v153spositiontaking_51, 0, auto_v153spositiontaking_51_max, 0.1, true, false);

            tpl.super_place_val = new PT_Selection(TemplateMain, "listSuper_153_52", v153spositiontaking_52, isauto_v153spositiontaking_52, 0, v153spositiontaking_52_max, 0.1, selectOnChange_52);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_153_52", auto_v153spositiontaking_52, !isauto_v153spositiontaking_52, 0, auto_v153spositiontaking_52_max, 0.1);

            string autoOnCheck_51 = "CheckAuto(this,'listSuper_153_51','listMaster_153_51','v153mspositiontaking_51_max','v153mpositiontaking_51_max','v153mpositiontaking_51_min');";
            string autoOnCheck_52 = "CheckAuto(this,'listSuper_153_52','listMaster_153_52','v153mspositiontaking_52_max','v153mpositiontaking_52_max','v153mpositiontaking_52_min');";
            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_153_51", isauto_v153spositiontaking_51, autoOnCheck_51);
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_153_52", isauto_v153spositiontaking_52, autoOnCheck_52);

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