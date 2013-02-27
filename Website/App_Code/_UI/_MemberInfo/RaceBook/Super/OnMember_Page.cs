// --------------------------------------------------------------------------
// <copyright file="AllowUpdatePT.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.06.01">Refactor code and check StyleCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook.Super
{
    using System;
    using System.Data;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.UI._MemberInfo.RaceBook.AllowUpdatePT_Control;
    using Sunplus.Agent.UI.PositionTaking.RaceBook.Super;

    public abstract class OnMember_Page : RaceBookSettingBase_Page
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

        public void RenderPT_151_41(DataRow dr, OnMember_Section_151_Section_151_41_Block tpl, ref bool isDisabled_CopyWin)
        {
            double v151spositiontaking_41 = 0, v151spositiontaking_42 = 0;
            double auto_v151spositiontaking_mem_41 = 0, auto_v151spositiontaking_mem_42 = 0;
            double v151apositiontaking_41 = 0, v151apositiontaking_42 = 0;
            double v151mpositiontaking_41 = 0, v151mpositiontaking_42 = 0;
            double v151pos_41_min = 0, v151pos_42_min = 0;
            double v151pos_41_max = 0, v151pos_42_max = 0;
            double auto_v151pos_41_max = 0, auto_v151pos_42_max = 0;
            bool isauto_v151spositiontaking_mem_41 = false, isauto_v151spositiontaking_mem_42 = false;
            bool isauto_v151spositiontaking_41 = false, isauto_v151spositiontaking_42 = false;

            double.TryParse(dr["v151spositiontaking_41"].ToString(), out v151spositiontaking_41);
            double.TryParse(dr["v151spositiontaking_42"].ToString(), out v151spositiontaking_42);

            double.TryParse(dr["auto_v151spositiontaking_mem_41"].ToString(), out auto_v151spositiontaking_mem_41);
            double.TryParse(dr["auto_v151spositiontaking_mem_42"].ToString(), out auto_v151spositiontaking_mem_42);

            double.TryParse(dr["v151apositiontaking_41"].ToString(), out v151apositiontaking_41);
            double.TryParse(dr["v151apositiontaking_42"].ToString(), out v151apositiontaking_42);

            double.TryParse(dr["v151mpositiontaking_41"].ToString(), out v151mpositiontaking_41);
            double.TryParse(dr["v151mpositiontaking_42"].ToString(), out v151mpositiontaking_42);

            double.TryParse(dr["v151pos_41_min"].ToString(), out v151pos_41_min);
            double.TryParse(dr["v151pos_42_min"].ToString(), out v151pos_42_min);

            double.TryParse(dr["v151pos_41_max"].ToString(), out v151pos_41_max);
            double.TryParse(dr["v151pos_42_max"].ToString(), out v151pos_42_max);

            double.TryParse(dr["auto_v151pos_41_max"].ToString(), out auto_v151pos_41_max);
            double.TryParse(dr["auto_v151pos_42_max"].ToString(), out auto_v151pos_42_max);

            isauto_v151spositiontaking_mem_41 = Convert.ToBoolean(dr["isauto_v151spositiontaking_mem_41"]);
            isauto_v151spositiontaking_mem_42 = Convert.ToBoolean(dr["isauto_v151spositiontaking_mem_42"]);

            isauto_v151spositiontaking_41 = Convert.ToBoolean(dr["isauto_v151spositiontaking_41"]);
            isauto_v151spositiontaking_42 = Convert.ToBoolean(dr["isauto_v151spositiontaking_42"]);

            this.JavaScriptRegister("IsAutoSuper_151_41", isauto_v151spositiontaking_mem_41);
            this.JavaScriptRegister("IsAutoSuper_151_42", isauto_v151spositiontaking_mem_42);

            this.HashTableRegister("chkAutoSuper_Master_151_41", isauto_v151spositiontaking_41);
            this.HashTableRegister("chkAutoSuper_Master_151_42", isauto_v151spositiontaking_42);

            tpl.agent_win_val = v151apositiontaking_41.ToString();
            tpl.agent_place_val = v151apositiontaking_42.ToString();
            tpl.master_win_val = v151mpositiontaking_41.ToString();
            tpl.master_place_val = v151mpositiontaking_42.ToString();

            isDisabled_CopyWin = !isauto_v151spositiontaking_mem_41 ? isauto_v151spositiontaking_41 ? true : false : false;

            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_41)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_41", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.super_win_val = isauto_v151spositiontaking_41 ? v151spositiontaking_41.ToString() : new PT_Selection(TemplateMain, "listSuper_151_41", v151spositiontaking_41, isauto_v151spositiontaking_mem_41, v151pos_41_min, v151pos_41_max, 0.1, true, false);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_151_41", auto_v151spositiontaking_mem_41, !isauto_v151spositiontaking_mem_41, 0, auto_v151pos_41_max, 0.1, true, false);

            tpl.super_place_val = isauto_v151spositiontaking_42 ? v151spositiontaking_42.ToString() : new PT_Selection(TemplateMain, "listSuper_151_42", v151spositiontaking_42, isauto_v151spositiontaking_mem_42, v151pos_42_min, v151pos_42_max, 0.1);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_151_42", auto_v151spositiontaking_mem_42, !isauto_v151spositiontaking_mem_42, 0, auto_v151pos_42_max, 0.1);

            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_41", isauto_v151spositiontaking_mem_41, "CheckAuto(this)");
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_42", isauto_v151spositiontaking_mem_42, "CheckAuto(this)");

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_151_41", "151_41");
            }
        }

        public void RenderPT_151_31(DataRow dr, OnMember_Section_151_Section_151_31_Block tpl)
        {
            double v151spositiontaking_31 = 0, v151spositiontaking_32 = 0;
            double auto_v151spositiontaking_mem_31 = 0, auto_v151spositiontaking_mem_32 = 0;
            double v151apositiontaking_31 = 0, v151apositiontaking_32 = 0;
            double v151mpositiontaking_31 = 0, v151mpositiontaking_32 = 0;
            double v151pos_31_min = 0, v151pos_32_min = 0;
            double v151pos_31_max = 0, v151pos_32_max = 0;
            double auto_v151pos_31_max = 0, auto_v151pos_32_max = 0;
            bool isauto_v151spositiontaking_mem_31 = false, isauto_v151spositiontaking_mem_32 = false;
            bool isauto_v151spositiontaking_31 = false, isauto_v151spositiontaking_32 = false;

            double.TryParse(dr["v151spositiontaking_31"].ToString(), out v151spositiontaking_31);
            double.TryParse(dr["v151spositiontaking_32"].ToString(), out v151spositiontaking_32);

            double.TryParse(dr["auto_v151spositiontaking_mem_31"].ToString(), out auto_v151spositiontaking_mem_31);
            double.TryParse(dr["auto_v151spositiontaking_mem_32"].ToString(), out auto_v151spositiontaking_mem_32);

            double.TryParse(dr["v151apositiontaking_31"].ToString(), out v151apositiontaking_31);
            double.TryParse(dr["v151apositiontaking_32"].ToString(), out v151apositiontaking_32);

            double.TryParse(dr["v151mpositiontaking_31"].ToString(), out v151mpositiontaking_31);
            double.TryParse(dr["v151mpositiontaking_32"].ToString(), out v151mpositiontaking_32);

            double.TryParse(dr["v151pos_31_min"].ToString(), out v151pos_31_min);
            double.TryParse(dr["v151pos_32_min"].ToString(), out v151pos_32_min);

            double.TryParse(dr["v151pos_31_max"].ToString(), out v151pos_31_max);
            double.TryParse(dr["v151pos_32_max"].ToString(), out v151pos_32_max);

            double.TryParse(dr["auto_v151pos_31_max"].ToString(), out auto_v151pos_31_max);
            double.TryParse(dr["auto_v151pos_32_max"].ToString(), out auto_v151pos_32_max);

            isauto_v151spositiontaking_mem_31 = Convert.ToBoolean(dr["isauto_v151spositiontaking_mem_31"]);
            isauto_v151spositiontaking_mem_32 = Convert.ToBoolean(dr["isauto_v151spositiontaking_mem_32"]);

            isauto_v151spositiontaking_31 = Convert.ToBoolean(dr["isauto_v151spositiontaking_31"]);
            isauto_v151spositiontaking_32 = Convert.ToBoolean(dr["isauto_v151spositiontaking_32"]);

            this.JavaScriptRegister("IsAutoSuper_151_31", isauto_v151spositiontaking_mem_31);
            this.JavaScriptRegister("IsAutoSuper_151_32", isauto_v151spositiontaking_mem_32);

            this.HashTableRegister("chkAutoSuper_Master_151_31", isauto_v151spositiontaking_31);
            this.HashTableRegister("chkAutoSuper_Master_151_32", isauto_v151spositiontaking_32);

            tpl.agent_win_val = v151apositiontaking_31.ToString();
            tpl.agent_place_val = v151apositiontaking_32.ToString();
            tpl.master_win_val = v151mpositiontaking_31.ToString();
            tpl.master_place_val = v151mpositiontaking_32.ToString();

            bool isDisabled_CopyWin = !isauto_v151spositiontaking_mem_31 ? isauto_v151spositiontaking_31 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_31)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_31", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.super_win_val = isauto_v151spositiontaking_31 ? v151spositiontaking_31.ToString() : new PT_Selection(TemplateMain, "listSuper_151_31", v151spositiontaking_31, isauto_v151spositiontaking_mem_31, v151pos_31_min, v151pos_31_max, 0.1, true, false);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_151_31", auto_v151spositiontaking_mem_31, !isauto_v151spositiontaking_mem_31, 0, auto_v151pos_31_max, 0.1, true, false);

            tpl.super_place_val = isauto_v151spositiontaking_32 ? v151spositiontaking_32.ToString() : new PT_Selection(TemplateMain, "listSuper_151_32", v151spositiontaking_32, isauto_v151spositiontaking_mem_32, v151pos_32_min, v151pos_32_max, 0.1);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_151_32", auto_v151spositiontaking_mem_32, !isauto_v151spositiontaking_mem_32, 0, auto_v151pos_32_max, 0.1);

            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_31", isauto_v151spositiontaking_mem_31, "CheckAuto(this)");
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_32", isauto_v151spositiontaking_mem_32, "CheckAuto(this)");

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_151_31", "151_31");
            }
        }

        public void RenderPT_151_51(DataRow dr, OnMember_Section_151_Section_151_51_Block tpl)
        {
            double v151spositiontaking_51 = 0, v151spositiontaking_52 = 0;
            double auto_v151spositiontaking_mem_51 = 0, auto_v151spositiontaking_mem_52 = 0;
            double v151apositiontaking_51 = 0, v151apositiontaking_52 = 0;
            double v151mpositiontaking_51 = 0, v151mpositiontaking_52 = 0;
            double v151pos_51_min = 0, v151pos_52_min = 0;
            double v151pos_51_max = 0, v151pos_52_max = 0;
            double auto_v151pos_51_max = 0, auto_v151pos_52_max = 0;
            bool isauto_v151spositiontaking_mem_51 = false, isauto_v151spositiontaking_mem_52 = false;
            bool isauto_v151spositiontaking_51 = false, isauto_v151spositiontaking_52 = false;

            double.TryParse(dr["v151spositiontaking_51"].ToString(), out v151spositiontaking_51);
            double.TryParse(dr["v151spositiontaking_52"].ToString(), out v151spositiontaking_52);

            double.TryParse(dr["auto_v151spositiontaking_mem_51"].ToString(), out auto_v151spositiontaking_mem_51);
            double.TryParse(dr["auto_v151spositiontaking_mem_52"].ToString(), out auto_v151spositiontaking_mem_52);

            double.TryParse(dr["v151apositiontaking_51"].ToString(), out v151apositiontaking_51);
            double.TryParse(dr["v151apositiontaking_52"].ToString(), out v151apositiontaking_52);

            double.TryParse(dr["v151mpositiontaking_51"].ToString(), out v151mpositiontaking_51);
            double.TryParse(dr["v151mpositiontaking_52"].ToString(), out v151mpositiontaking_52);

            double.TryParse(dr["v151pos_51_min"].ToString(), out v151pos_51_min);
            double.TryParse(dr["v151pos_52_min"].ToString(), out v151pos_52_min);

            double.TryParse(dr["v151pos_51_max"].ToString(), out v151pos_51_max);
            double.TryParse(dr["v151pos_52_max"].ToString(), out v151pos_52_max);

            double.TryParse(dr["auto_v151pos_51_max"].ToString(), out auto_v151pos_51_max);
            double.TryParse(dr["auto_v151pos_52_max"].ToString(), out auto_v151pos_52_max);

            isauto_v151spositiontaking_mem_51 = Convert.ToBoolean(dr["isauto_v151spositiontaking_mem_51"]);
            isauto_v151spositiontaking_mem_52 = Convert.ToBoolean(dr["isauto_v151spositiontaking_mem_52"]);

            isauto_v151spositiontaking_51 = Convert.ToBoolean(dr["isauto_v151spositiontaking_51"]);
            isauto_v151spositiontaking_52 = Convert.ToBoolean(dr["isauto_v151spositiontaking_52"]);

            this.JavaScriptRegister("IsAutoSuper_151_51", isauto_v151spositiontaking_mem_51);
            this.JavaScriptRegister("IsAutoSuper_151_52", isauto_v151spositiontaking_mem_52);

            this.HashTableRegister("chkAutoSuper_Master_151_51", isauto_v151spositiontaking_51);
            this.HashTableRegister("chkAutoSuper_Master_151_52", isauto_v151spositiontaking_52);

            tpl.agent_win_val = v151apositiontaking_51.ToString();
            tpl.agent_place_val = v151apositiontaking_52.ToString();
            tpl.master_win_val = v151mpositiontaking_51.ToString();
            tpl.master_place_val = v151mpositiontaking_52.ToString();

            bool isDisabled_CopyWin = !isauto_v151spositiontaking_mem_51 ? isauto_v151spositiontaking_51 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_51)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_51", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.super_win_val = isauto_v151spositiontaking_51 ? v151spositiontaking_51.ToString() : new PT_Selection(TemplateMain, "listSuper_151_51", v151spositiontaking_51, isauto_v151spositiontaking_mem_51, v151pos_51_min, v151pos_51_max, 0.1, true, false);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_151_51", auto_v151spositiontaking_mem_51, !isauto_v151spositiontaking_mem_51, 0, auto_v151pos_51_max, 0.1, true, false);

            tpl.super_place_val = isauto_v151spositiontaking_52 ? v151spositiontaking_52.ToString() : new PT_Selection(TemplateMain, "listSuper_151_52", v151spositiontaking_52, isauto_v151spositiontaking_mem_52, v151pos_52_min, v151pos_52_max, 0.1);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_151_52", auto_v151spositiontaking_mem_52, !isauto_v151spositiontaking_mem_52, 0, auto_v151pos_52_max, 0.1);

            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_51", isauto_v151spositiontaking_mem_51, "CheckAuto(this)");
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_151_52", isauto_v151spositiontaking_mem_52, "CheckAuto(this)");

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_151_51", "151_51");
            }
        }

        public void RenderPT_152_41(DataRow dr, OnMember_Section_152_Section_152_41_Block tpl)
        {
            double v152spositiontaking_41 = 0, v152spositiontaking_42 = 0;
            double auto_v152spositiontaking_mem_41 = 0, auto_v152spositiontaking_mem_42 = 0;
            double v152apositiontaking_41 = 0, v152apositiontaking_42 = 0;
            double v152mpositiontaking_41 = 0, v152mpositiontaking_42 = 0;
            double v152pos_41_min = 0, v152pos_42_min = 0;
            double v152pos_41_max = 0, v152pos_42_max = 0;
            double auto_v152pos_41_max = 0, auto_v152pos_42_max = 0;
            bool isauto_v152spositiontaking_mem_41 = false, isauto_v152spositiontaking_mem_42 = false;
            bool isauto_v152spositiontaking_41 = false, isauto_v152spositiontaking_42 = false;

            double.TryParse(dr["v152spositiontaking_41"].ToString(), out v152spositiontaking_41);
            double.TryParse(dr["v152spositiontaking_42"].ToString(), out v152spositiontaking_42);

            double.TryParse(dr["auto_v152spositiontaking_mem_41"].ToString(), out auto_v152spositiontaking_mem_41);
            double.TryParse(dr["auto_v152spositiontaking_mem_42"].ToString(), out auto_v152spositiontaking_mem_42);

            double.TryParse(dr["v152apositiontaking_41"].ToString(), out v152apositiontaking_41);
            double.TryParse(dr["v152apositiontaking_42"].ToString(), out v152apositiontaking_42);

            double.TryParse(dr["v152mpositiontaking_41"].ToString(), out v152mpositiontaking_41);
            double.TryParse(dr["v152mpositiontaking_42"].ToString(), out v152mpositiontaking_42);

            double.TryParse(dr["v152pos_41_min"].ToString(), out v152pos_41_min);
            double.TryParse(dr["v152pos_42_min"].ToString(), out v152pos_42_min);

            double.TryParse(dr["v152pos_41_max"].ToString(), out v152pos_41_max);
            double.TryParse(dr["v152pos_42_max"].ToString(), out v152pos_42_max);

            double.TryParse(dr["auto_v152pos_41_max"].ToString(), out auto_v152pos_41_max);
            double.TryParse(dr["auto_v152pos_42_max"].ToString(), out auto_v152pos_42_max);

            isauto_v152spositiontaking_mem_41 = Convert.ToBoolean(dr["isauto_v152spositiontaking_mem_41"]);
            isauto_v152spositiontaking_mem_42 = Convert.ToBoolean(dr["isauto_v152spositiontaking_mem_42"]);

            isauto_v152spositiontaking_41 = Convert.ToBoolean(dr["isauto_v152spositiontaking_41"]);
            isauto_v152spositiontaking_42 = Convert.ToBoolean(dr["isauto_v152spositiontaking_42"]);

            this.JavaScriptRegister("IsAutoSuper_152_41", isauto_v152spositiontaking_mem_41);
            this.JavaScriptRegister("IsAutoSuper_152_42", isauto_v152spositiontaking_mem_42);

            this.HashTableRegister("chkAutoSuper_Master_152_41", isauto_v152spositiontaking_41);
            this.HashTableRegister("chkAutoSuper_Master_152_42", isauto_v152spositiontaking_42);

            tpl.agent_win_val = v152apositiontaking_41.ToString();
            tpl.agent_place_val = v152apositiontaking_42.ToString();
            tpl.master_win_val = v152mpositiontaking_41.ToString();
            tpl.master_place_val = v152mpositiontaking_42.ToString();

            bool isDisabled_CopyWin = !isauto_v152spositiontaking_mem_41 ? isauto_v152spositiontaking_41 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_152_41)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_152_41", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.super_win_val = isauto_v152spositiontaking_41 ? v152spositiontaking_41.ToString() : new PT_Selection(TemplateMain, "listSuper_152_41", v152spositiontaking_41, isauto_v152spositiontaking_mem_41, v152pos_41_min, v152pos_41_max, 0.1, true, false);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_152_41", auto_v152spositiontaking_mem_41, !isauto_v152spositiontaking_mem_41, 0, auto_v152pos_41_max, 0.1, true, false);

            tpl.super_place_val = isauto_v152spositiontaking_42 ? v152spositiontaking_42.ToString() : new PT_Selection(TemplateMain, "listSuper_152_42", v152spositiontaking_42, isauto_v152spositiontaking_mem_42, v152pos_42_min, v152pos_42_max, 0.1);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_152_42", auto_v152spositiontaking_mem_42, !isauto_v152spositiontaking_mem_42, 0, auto_v152pos_42_max, 0.1);

            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_152_41", isauto_v152spositiontaking_mem_41, "CheckAuto(this)");
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_152_42", isauto_v152spositiontaking_mem_42, "CheckAuto(this)");

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_152_41", "152_41");
            }
        }

        public void RenderPT_152_51(DataRow dr, OnMember_Section_152_Section_152_51_Block tpl)
        {
            double v152spositiontaking_51 = 0, v152spositiontaking_52 = 0;
            double auto_v152spositiontaking_mem_51 = 0, auto_v152spositiontaking_mem_52 = 0;
            double v152apositiontaking_51 = 0, v152apositiontaking_52 = 0;
            double v152mpositiontaking_51 = 0, v152mpositiontaking_52 = 0;
            double v152pos_51_min = 0, v152pos_52_min = 0;
            double v152pos_51_max = 0, v152pos_52_max = 0;
            double auto_v152pos_51_max = 0, auto_v152pos_52_max = 0;
            bool isauto_v152spositiontaking_mem_51 = false, isauto_v152spositiontaking_mem_52 = false;
            bool isauto_v152spositiontaking_51 = false, isauto_v152spositiontaking_52 = false;

            double.TryParse(dr["v152spositiontaking_51"].ToString(), out v152spositiontaking_51);
            double.TryParse(dr["v152spositiontaking_52"].ToString(), out v152spositiontaking_52);

            double.TryParse(dr["auto_v152spositiontaking_mem_51"].ToString(), out auto_v152spositiontaking_mem_51);
            double.TryParse(dr["auto_v152spositiontaking_mem_52"].ToString(), out auto_v152spositiontaking_mem_52);

            double.TryParse(dr["v152apositiontaking_51"].ToString(), out v152apositiontaking_51);
            double.TryParse(dr["v152apositiontaking_52"].ToString(), out v152apositiontaking_52);

            double.TryParse(dr["v152mpositiontaking_51"].ToString(), out v152mpositiontaking_51);
            double.TryParse(dr["v152mpositiontaking_52"].ToString(), out v152mpositiontaking_52);

            double.TryParse(dr["v152pos_51_min"].ToString(), out v152pos_51_min);
            double.TryParse(dr["v152pos_52_min"].ToString(), out v152pos_52_min);

            double.TryParse(dr["v152pos_51_max"].ToString(), out v152pos_51_max);
            double.TryParse(dr["v152pos_52_max"].ToString(), out v152pos_52_max);

            double.TryParse(dr["auto_v152pos_51_max"].ToString(), out auto_v152pos_51_max);
            double.TryParse(dr["auto_v152pos_52_max"].ToString(), out auto_v152pos_52_max);

            isauto_v152spositiontaking_mem_51 = Convert.ToBoolean(dr["isauto_v152spositiontaking_mem_51"]);
            isauto_v152spositiontaking_mem_52 = Convert.ToBoolean(dr["isauto_v152spositiontaking_mem_52"]);

            isauto_v152spositiontaking_51 = Convert.ToBoolean(dr["isauto_v152spositiontaking_51"]);
            isauto_v152spositiontaking_52 = Convert.ToBoolean(dr["isauto_v152spositiontaking_52"]);

            this.JavaScriptRegister("IsAutoSuper_152_51", isauto_v152spositiontaking_mem_51);
            this.JavaScriptRegister("IsAutoSuper_152_52", isauto_v152spositiontaking_mem_52);

            this.HashTableRegister("chkAutoSuper_Master_152_51", isauto_v152spositiontaking_51);
            this.HashTableRegister("chkAutoSuper_Master_152_52", isauto_v152spositiontaking_52);

            tpl.agent_win_val = v152apositiontaking_51.ToString();
            tpl.agent_place_val = v152apositiontaking_52.ToString();
            tpl.master_win_val = v152mpositiontaking_51.ToString();
            tpl.master_place_val = v152mpositiontaking_52.ToString();

            bool isDisabled_CopyWin = !isauto_v152spositiontaking_mem_51 ? isauto_v152spositiontaking_51 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_152_51)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_152_51", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.super_win_val = isauto_v152spositiontaking_51 ? v152spositiontaking_51.ToString() : new PT_Selection(TemplateMain, "listSuper_152_51", v152spositiontaking_51, isauto_v152spositiontaking_mem_51, v152pos_51_min, v152pos_51_max, 0.1, true, false);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_152_51", auto_v152spositiontaking_mem_51, !isauto_v152spositiontaking_mem_51, 0, auto_v152pos_51_max, 0.1, true, false);

            tpl.super_place_val = isauto_v152spositiontaking_52 ? v152spositiontaking_52.ToString() : new PT_Selection(TemplateMain, "listSuper_152_52", v152spositiontaking_52, isauto_v152spositiontaking_mem_52, v152pos_52_min, v152pos_52_max, 0.1);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_152_52", auto_v152spositiontaking_mem_52, !isauto_v152spositiontaking_mem_52, 0, auto_v152pos_52_max, 0.1);

            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_152_51", isauto_v152spositiontaking_mem_51, "CheckAuto(this)");
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_152_52", isauto_v152spositiontaking_mem_52, "CheckAuto(this)");

            if (!this.IsMultiple)
            {
                tpl.positiontaking_allowupdate_checkbox = string.Empty;
            }
            else
            {
                tpl.positiontaking_allowupdate_checkbox = new AllowUpdatePT(tpl, "ChkUpdate_152_51", "152_51");
            }
        }

        public void RenderPT_153_51(DataRow dr, OnMember_Section_153_Section_153_51_Block tpl)
        {
            double v153spositiontaking_51 = 0, v153spositiontaking_52 = 0;
            double auto_v153spositiontaking_mem_51 = 0, auto_v153spositiontaking_mem_52 = 0;
            double v153apositiontaking_51 = 0, v153apositiontaking_52 = 0;
            double v153mpositiontaking_51 = 0, v153mpositiontaking_52 = 0;
            double v153pos_51_min = 0, v153pos_52_min = 0;
            double v153pos_51_max = 0, v153pos_52_max = 0;
            double auto_v153pos_51_max = 0, auto_v153pos_52_max = 0;
            bool isauto_v153spositiontaking_mem_51 = false, isauto_v153spositiontaking_mem_52 = false;
            bool isauto_v153spositiontaking_51 = false, isauto_v153spositiontaking_52 = false;

            double.TryParse(dr["v153spositiontaking_51"].ToString(), out v153spositiontaking_51);
            double.TryParse(dr["v153spositiontaking_52"].ToString(), out v153spositiontaking_52);

            double.TryParse(dr["auto_v153spositiontaking_mem_51"].ToString(), out auto_v153spositiontaking_mem_51);
            double.TryParse(dr["auto_v153spositiontaking_mem_52"].ToString(), out auto_v153spositiontaking_mem_52);

            double.TryParse(dr["v153apositiontaking_51"].ToString(), out v153apositiontaking_51);
            double.TryParse(dr["v153apositiontaking_52"].ToString(), out v153apositiontaking_52);

            double.TryParse(dr["v153mpositiontaking_51"].ToString(), out v153mpositiontaking_51);
            double.TryParse(dr["v153mpositiontaking_52"].ToString(), out v153mpositiontaking_52);

            double.TryParse(dr["v153pos_51_min"].ToString(), out v153pos_51_min);
            double.TryParse(dr["v153pos_52_min"].ToString(), out v153pos_52_min);

            double.TryParse(dr["v153pos_51_max"].ToString(), out v153pos_51_max);
            double.TryParse(dr["v153pos_52_max"].ToString(), out v153pos_52_max);

            double.TryParse(dr["auto_v153pos_51_max"].ToString(), out auto_v153pos_51_max);
            double.TryParse(dr["auto_v153pos_52_max"].ToString(), out auto_v153pos_52_max);

            isauto_v153spositiontaking_mem_51 = Convert.ToBoolean(dr["isauto_v153spositiontaking_mem_51"]);
            isauto_v153spositiontaking_mem_52 = Convert.ToBoolean(dr["isauto_v153spositiontaking_mem_52"]);

            isauto_v153spositiontaking_51 = Convert.ToBoolean(dr["isauto_v153spositiontaking_51"]);
            isauto_v153spositiontaking_52 = Convert.ToBoolean(dr["isauto_v153spositiontaking_52"]);

            this.JavaScriptRegister("IsAutoSuper_153_51", isauto_v153spositiontaking_mem_51);
            this.JavaScriptRegister("IsAutoSuper_153_52", isauto_v153spositiontaking_mem_52);

            this.HashTableRegister("chkAutoSuper_Master_153_51", isauto_v153spositiontaking_51);
            this.HashTableRegister("chkAutoSuper_Master_153_52", isauto_v153spositiontaking_52);

            tpl.agent_win_val = v153apositiontaking_51.ToString();
            tpl.agent_place_val = v153apositiontaking_52.ToString();
            tpl.master_win_val = v153mpositiontaking_51.ToString();
            tpl.master_place_val = v153mpositiontaking_52.ToString();

            bool isDisabled_CopyWin = !isauto_v153spositiontaking_mem_51 ? isauto_v153spositiontaking_51 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_153_51)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_153_51", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.super_win_val = isauto_v153spositiontaking_51 ? v153spositiontaking_51.ToString() : new PT_Selection(TemplateMain, "listSuper_153_51", v153spositiontaking_51, isauto_v153spositiontaking_mem_51, v153pos_51_min, v153pos_51_max, 0.1, true, false);
            tpl.super_auto_win_val = new PT_Selection(TemplateMain, "listAutoSuper_153_51", auto_v153spositiontaking_mem_51, !isauto_v153spositiontaking_mem_51, 0, auto_v153pos_51_max, 0.1, true, false);

            tpl.super_place_val = isauto_v153spositiontaking_52 ? v153spositiontaking_52.ToString() : new PT_Selection(TemplateMain, "listSuper_153_52", v153spositiontaking_52, isauto_v153spositiontaking_mem_52, v153pos_52_min, v153pos_52_max, 0.1);
            tpl.super_auto_place_val = new PT_Selection(TemplateMain, "listAutoSuper_153_52", auto_v153spositiontaking_mem_52, !isauto_v153spositiontaking_mem_52, 0, auto_v153pos_52_max, 0.1);

            tpl.super_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_153_51", isauto_v153spositiontaking_mem_51, "CheckAuto(this)");
            tpl.super_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoSuper_153_52", isauto_v153spositiontaking_mem_52, "CheckAuto(this)");

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