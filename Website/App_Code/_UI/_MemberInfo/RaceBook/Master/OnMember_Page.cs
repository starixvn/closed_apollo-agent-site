// --------------------------------------------------------------------------
// <copyright file="OnMember.aspx.cs" company="Nexcel Solutions Vietnam">
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

    public abstract class OnMember_Page : RaceBookSettingBase_Page
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

        public void RenderPT_151_41(DataRow dr, OnMember_Section_151_Section_151_41_Block tpl, ref bool isDisabled_CopyWin)
        {
            double v151mpositiontaking_41 = 0, v151mpositiontaking_42 = 0;

            double auto_v151mpositiontaking_mem_41 = 0, auto_v151mpositiontaking_mem_42 = 0;

            double v151apositiontaking_41 = 0, v151apositiontaking_42 = 0;

            double v151pos_41_min = 0, v151pos_42_min = 0;

            double v151pos_41_max = 0, v151pos_42_max = 0;

            double auto_v151mpositiontaking_mem_41_max = 0, auto_v151mpositiontaking_mem_42_max = 0;

            bool isauto_v151mpositiontaking_mem_41 = false, isauto_v151mpositiontaking_mem_42 = false;

            bool isauto_v151mpositiontaking_41 = false, isauto_v151mpositiontaking_42 = false;

            double.TryParse(dr["v151mpositiontaking_41"].ToString(), out v151mpositiontaking_41);
            double.TryParse(dr["v151mpositiontaking_42"].ToString(), out v151mpositiontaking_42);

            double.TryParse(dr["auto_v151mpositiontaking_mem_41"].ToString(), out auto_v151mpositiontaking_mem_41);
            double.TryParse(dr["auto_v151mpositiontaking_mem_42"].ToString(), out auto_v151mpositiontaking_mem_42);

            double.TryParse(dr["v151apositiontaking_41"].ToString(), out v151apositiontaking_41);
            double.TryParse(dr["v151apositiontaking_42"].ToString(), out v151apositiontaking_42);

            double.TryParse(dr["v151pos_41_min"].ToString(), out v151pos_41_min);
            double.TryParse(dr["v151pos_42_min"].ToString(), out v151pos_42_min);

            double.TryParse(dr["v151pos_41_max"].ToString(), out v151pos_41_max);
            double.TryParse(dr["v151pos_42_max"].ToString(), out v151pos_42_max);

            double.TryParse(dr["auto_v151mpositiontaking_mem_41_max"].ToString(), out auto_v151mpositiontaking_mem_41_max);
            double.TryParse(dr["auto_v151mpositiontaking_mem_42_max"].ToString(), out auto_v151mpositiontaking_mem_42_max);

            isauto_v151mpositiontaking_mem_41 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_mem_41"]);
            isauto_v151mpositiontaking_mem_42 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_mem_42"]);

            isauto_v151mpositiontaking_41 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_41"]);
            isauto_v151mpositiontaking_42 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_42"]);

            tpl.agent_win_val = Convert.ToDouble(dr["v151apositiontaking_41"]).ToString();
            tpl.agent_place_val = Convert.ToDouble(dr["v151apositiontaking_42"]).ToString();

            this.JavaScriptRegister("v151pos_41_max", v151pos_41_max);
            this.JavaScriptRegister("v151pos_42_max", v151pos_42_max);

            this.JavaScriptRegister("IsAutoMaster_151_41", isauto_v151mpositiontaking_mem_41);
            this.JavaScriptRegister("IsAutoMaster_151_42", isauto_v151mpositiontaking_mem_42);

            this.HashTableRegister("chkAutoMaster_Agent_151_41", isauto_v151mpositiontaking_41);
            this.HashTableRegister("chkAutoMaster_Agent_151_42", isauto_v151mpositiontaking_42);

            isDisabled_CopyWin = !isauto_v151mpositiontaking_mem_41 ? isauto_v151mpositiontaking_41 ? true : false : false;

            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_41)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_41", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.master_win_val = isauto_v151mpositiontaking_41 ? v151mpositiontaking_41.ToString() : new PT_Selection(TemplateMain, "listMaster_151_41", v151mpositiontaking_41, isauto_v151mpositiontaking_mem_41, v151pos_41_min, v151pos_41_max, 0.1, true, false);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_151_41", auto_v151mpositiontaking_mem_41, !isauto_v151mpositiontaking_mem_41, 0, auto_v151mpositiontaking_mem_41_max, 0.1, true, false);

            tpl.master_place_val = isauto_v151mpositiontaking_42 ? v151mpositiontaking_42.ToString() : new PT_Selection(TemplateMain, "listMaster_151_42", v151mpositiontaking_42, isauto_v151mpositiontaking_mem_42, v151pos_42_min, v151pos_42_max, 0.1);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_151_42", auto_v151mpositiontaking_mem_42, !isauto_v151mpositiontaking_mem_42, 0, auto_v151mpositiontaking_mem_42_max, 0.1);

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_41", isauto_v151mpositiontaking_mem_41, "CheckAuto(this)");
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_42", isauto_v151mpositiontaking_mem_42, "CheckAuto(this)");

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
            double v151mpositiontaking_31 = 0, v151mpositiontaking_32 = 0;

            double auto_v151mpositiontaking_mem_31 = 0, auto_v151mpositiontaking_mem_32 = 0;

            double v151apositiontaking_31 = 0, v151apositiontaking_32 = 0;

            double v151pos_31_min = 0, v151pos_32_min = 0;

            double v151pos_31_max = 0, v151pos_32_max = 0;

            double auto_v151mpositiontaking_mem_31_max = 0, auto_v151mpositiontaking_mem_32_max = 0;

            bool isauto_v151mpositiontaking_mem_31 = false, isauto_v151mpositiontaking_mem_32 = false;

            bool isauto_v151mpositiontaking_31 = false, isauto_v151mpositiontaking_32 = false;

            double.TryParse(dr["v151mpositiontaking_31"].ToString(), out v151mpositiontaking_31);
            double.TryParse(dr["v151mpositiontaking_32"].ToString(), out v151mpositiontaking_32);

            double.TryParse(dr["auto_v151mpositiontaking_mem_31"].ToString(), out auto_v151mpositiontaking_mem_31);
            double.TryParse(dr["auto_v151mpositiontaking_mem_32"].ToString(), out auto_v151mpositiontaking_mem_32);

            double.TryParse(dr["v151apositiontaking_31"].ToString(), out v151apositiontaking_31);
            double.TryParse(dr["v151apositiontaking_32"].ToString(), out v151apositiontaking_32);

            double.TryParse(dr["v151pos_31_min"].ToString(), out v151pos_31_min);
            double.TryParse(dr["v151pos_32_min"].ToString(), out v151pos_32_min);

            double.TryParse(dr["v151pos_31_max"].ToString(), out v151pos_31_max);
            double.TryParse(dr["v151pos_32_max"].ToString(), out v151pos_32_max);

            double.TryParse(dr["auto_v151mpositiontaking_mem_31_max"].ToString(), out auto_v151mpositiontaking_mem_31_max);
            double.TryParse(dr["auto_v151mpositiontaking_mem_32_max"].ToString(), out auto_v151mpositiontaking_mem_32_max);

            isauto_v151mpositiontaking_mem_31 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_mem_31"]);
            isauto_v151mpositiontaking_mem_32 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_mem_32"]);

            isauto_v151mpositiontaking_31 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_31"]);
            isauto_v151mpositiontaking_32 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_32"]);

            tpl.agent_win_val = Convert.ToDouble(dr["v151apositiontaking_31"]).ToString();
            tpl.agent_place_val = Convert.ToDouble(dr["v151apositiontaking_32"]).ToString();

            this.JavaScriptRegister("v151pos_31_max", v151pos_31_max);
            this.JavaScriptRegister("v151pos_32_max", v151pos_32_max);

            this.JavaScriptRegister("IsAutoMaster_151_31", isauto_v151mpositiontaking_mem_31);
            this.JavaScriptRegister("IsAutoMaster_151_32", isauto_v151mpositiontaking_mem_32);

            this.HashTableRegister("chkAutoMaster_Agent_151_31", isauto_v151mpositiontaking_31);
            this.HashTableRegister("chkAutoMaster_Agent_151_32", isauto_v151mpositiontaking_32);

            bool isDisabled_CopyWin = !isauto_v151mpositiontaking_mem_31 ? isauto_v151mpositiontaking_31 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_31)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_31", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.master_win_val = isauto_v151mpositiontaking_31 ? v151mpositiontaking_31.ToString() : new PT_Selection(TemplateMain, "listMaster_151_31", v151mpositiontaking_31, isauto_v151mpositiontaking_mem_31, v151pos_31_min, v151pos_31_max, 0.1, true, false);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_151_31", auto_v151mpositiontaking_mem_31, !isauto_v151mpositiontaking_mem_31, 0, auto_v151mpositiontaking_mem_31_max, 0.1, true, false);

            tpl.master_place_val = isauto_v151mpositiontaking_32 ? v151mpositiontaking_32.ToString() : new PT_Selection(TemplateMain, "listMaster_151_32", v151mpositiontaking_32, isauto_v151mpositiontaking_mem_32, v151pos_32_min, v151pos_32_max, 0.1, true, false);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_151_32", auto_v151mpositiontaking_mem_32, !isauto_v151mpositiontaking_mem_32, 0, auto_v151mpositiontaking_mem_32_max, 0.1, true, false);

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_31", isauto_v151mpositiontaking_mem_31, "CheckAuto(this)");
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_32", isauto_v151mpositiontaking_mem_32, "CheckAuto(this)");

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
            double v151mpositiontaking_51 = 0, v151mpositiontaking_52 = 0;

            double auto_v151mpositiontaking_mem_51 = 0, auto_v151mpositiontaking_mem_52 = 0;

            double v151apositiontaking_51 = 0, v151apositiontaking_52 = 0;

            double v151pos_51_min = 0, v151pos_52_min = 0;

            double v151pos_51_max = 0, v151pos_52_max = 0;

            double auto_v151mpositiontaking_mem_51_max = 0, auto_v151mpositiontaking_mem_52_max = 0;

            bool isauto_v151mpositiontaking_mem_51 = false, isauto_v151mpositiontaking_mem_52 = false;

            bool isauto_v151mpositiontaking_51 = false, isauto_v151mpositiontaking_52 = false;

            double.TryParse(dr["v151mpositiontaking_51"].ToString(), out v151mpositiontaking_51);
            double.TryParse(dr["v151mpositiontaking_52"].ToString(), out v151mpositiontaking_52);

            double.TryParse(dr["auto_v151mpositiontaking_mem_51"].ToString(), out auto_v151mpositiontaking_mem_51);
            double.TryParse(dr["auto_v151mpositiontaking_mem_52"].ToString(), out auto_v151mpositiontaking_mem_52);

            double.TryParse(dr["v151apositiontaking_51"].ToString(), out v151apositiontaking_51);
            double.TryParse(dr["v151apositiontaking_52"].ToString(), out v151apositiontaking_52);

            double.TryParse(dr["v151pos_51_min"].ToString(), out v151pos_51_min);
            double.TryParse(dr["v151pos_52_min"].ToString(), out v151pos_52_min);

            double.TryParse(dr["v151pos_51_max"].ToString(), out v151pos_51_max);
            double.TryParse(dr["v151pos_52_max"].ToString(), out v151pos_52_max);

            double.TryParse(dr["auto_v151mpositiontaking_mem_51_max"].ToString(), out auto_v151mpositiontaking_mem_51_max);
            double.TryParse(dr["auto_v151mpositiontaking_mem_52_max"].ToString(), out auto_v151mpositiontaking_mem_52_max);

            isauto_v151mpositiontaking_mem_51 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_mem_51"]);
            isauto_v151mpositiontaking_mem_52 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_mem_52"]);

            isauto_v151mpositiontaking_51 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_51"]);
            isauto_v151mpositiontaking_52 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_52"]);

            tpl.agent_win_val = Convert.ToDouble(dr["v151apositiontaking_51"]).ToString();
            tpl.agent_place_val = Convert.ToDouble(dr["v151apositiontaking_52"]).ToString();

            this.JavaScriptRegister("v151pos_51_max", v151pos_51_max);
            this.JavaScriptRegister("v151pos_52_max", v151pos_52_max);

            this.JavaScriptRegister("IsAutoMaster_151_51", isauto_v151mpositiontaking_mem_51);
            this.JavaScriptRegister("IsAutoMaster_151_52", isauto_v151mpositiontaking_mem_52);

            this.HashTableRegister("chkAutoMaster_Agent_151_51", isauto_v151mpositiontaking_51);
            this.HashTableRegister("chkAutoMaster_Agent_151_52", isauto_v151mpositiontaking_52);

            bool isDisabled_CopyWin = !isauto_v151mpositiontaking_mem_51 ? isauto_v151mpositiontaking_51 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_151_51)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_151_51", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.master_win_val = isauto_v151mpositiontaking_51 ? v151mpositiontaking_51.ToString() : new PT_Selection(TemplateMain, "listMaster_151_51", v151mpositiontaking_51, isauto_v151mpositiontaking_mem_51, v151pos_51_min, v151pos_51_max, 0.1, true, false);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_151_51", auto_v151mpositiontaking_mem_51, !isauto_v151mpositiontaking_mem_51, 0, auto_v151mpositiontaking_mem_51_max, 0.1, true, false);

            tpl.master_place_val = isauto_v151mpositiontaking_52 ? v151mpositiontaking_52.ToString() : new PT_Selection(TemplateMain, "listMaster_151_52", v151mpositiontaking_52, isauto_v151mpositiontaking_mem_52, v151pos_52_min, v151pos_52_max, 0.1);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_151_52", auto_v151mpositiontaking_mem_52, !isauto_v151mpositiontaking_mem_52, 0, auto_v151mpositiontaking_mem_52_max, 0.1);

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_51", isauto_v151mpositiontaking_mem_51, "CheckAuto(this)");
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_151_52", isauto_v151mpositiontaking_mem_52, "CheckAuto(this)");

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
            double v152mpositiontaking_41 = 0, v152mpositiontaking_42 = 0;

            double auto_v152mpositiontaking_mem_41 = 0, auto_v152mpositiontaking_mem_42 = 0;

            double v152apositiontaking_41 = 0, v152apositiontaking_42 = 0;

            double v152pos_41_min = 0, v152pos_42_min = 0;

            double v152pos_41_max = 0, v152pos_42_max = 0;

            double auto_v152mpositiontaking_mem_41_max = 0, auto_v152mpositiontaking_mem_42_max = 0;

            bool isauto_v152mpositiontaking_mem_41 = false, isauto_v152mpositiontaking_mem_42 = false;

            bool isauto_v152mpositiontaking_41 = false, isauto_v152mpositiontaking_42 = false;

            double.TryParse(dr["v152mpositiontaking_41"].ToString(), out v152mpositiontaking_41);
            double.TryParse(dr["v152mpositiontaking_42"].ToString(), out v152mpositiontaking_42);

            double.TryParse(dr["auto_v152mpositiontaking_mem_41"].ToString(), out auto_v152mpositiontaking_mem_41);
            double.TryParse(dr["auto_v152mpositiontaking_mem_42"].ToString(), out auto_v152mpositiontaking_mem_42);

            double.TryParse(dr["v152apositiontaking_41"].ToString(), out v152apositiontaking_41);
            double.TryParse(dr["v152apositiontaking_42"].ToString(), out v152apositiontaking_42);

            double.TryParse(dr["v152pos_41_min"].ToString(), out v152pos_41_min);
            double.TryParse(dr["v152pos_42_min"].ToString(), out v152pos_42_min);

            double.TryParse(dr["v152pos_41_max"].ToString(), out v152pos_41_max);
            double.TryParse(dr["v152pos_42_max"].ToString(), out v152pos_42_max);

            double.TryParse(dr["auto_v152mpositiontaking_mem_41_max"].ToString(), out auto_v152mpositiontaking_mem_41_max);
            double.TryParse(dr["auto_v152mpositiontaking_mem_42_max"].ToString(), out auto_v152mpositiontaking_mem_42_max);

            isauto_v152mpositiontaking_mem_41 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_mem_41"]);
            isauto_v152mpositiontaking_mem_42 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_mem_42"]);

            isauto_v152mpositiontaking_41 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_41"]);
            isauto_v152mpositiontaking_42 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_42"]);

            tpl.agent_win_val = Convert.ToDouble(dr["v152apositiontaking_41"]).ToString();
            tpl.agent_place_val = Convert.ToDouble(dr["v152apositiontaking_42"]).ToString();

            this.JavaScriptRegister("v152pos_41_max", v152pos_41_max);
            this.JavaScriptRegister("v152pos_42_max", v152pos_42_max);

            this.JavaScriptRegister("IsAutoMaster_152_41", isauto_v152mpositiontaking_mem_41);
            this.JavaScriptRegister("IsAutoMaster_152_42", isauto_v152mpositiontaking_mem_42);

            this.HashTableRegister("chkAutoMaster_Agent_152_41", isauto_v152mpositiontaking_41);
            this.HashTableRegister("chkAutoMaster_Agent_152_42", isauto_v152mpositiontaking_42);

            bool isDisabled_CopyWin = !isauto_v152mpositiontaking_mem_41 ? isauto_v152mpositiontaking_41 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_152_41)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_152_41", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.master_win_val = isauto_v152mpositiontaking_41 ? v152mpositiontaking_41.ToString() : new PT_Selection(TemplateMain, "listMaster_152_41", v152mpositiontaking_41, isauto_v152mpositiontaking_mem_41, v152pos_41_min, v152pos_41_max, 0.1, true, false);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_152_41", auto_v152mpositiontaking_mem_41, !isauto_v152mpositiontaking_mem_41, 0, auto_v152mpositiontaking_mem_41_max, 0.1, true, false);

            tpl.master_place_val = isauto_v152mpositiontaking_42 ? v152mpositiontaking_42.ToString() : new PT_Selection(TemplateMain, "listMaster_152_42", v152mpositiontaking_42, isauto_v152mpositiontaking_mem_42, v152pos_42_min, v152pos_42_max, 0.1);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_152_42", auto_v152mpositiontaking_mem_42, !isauto_v152mpositiontaking_mem_42, 0, auto_v152mpositiontaking_mem_42_max, 0.1);

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_152_41", isauto_v152mpositiontaking_mem_41, "CheckAuto(this)");
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_152_42", isauto_v152mpositiontaking_mem_42, "CheckAuto(this)");

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
            double v152mpositiontaking_51 = 0, v152mpositiontaking_52 = 0;

            double auto_v152mpositiontaking_mem_51 = 0, auto_v152mpositiontaking_mem_52 = 0;

            double v152apositiontaking_51 = 0, v152apositiontaking_52 = 0;

            double v152pos_51_min = 0, v152pos_52_min = 0;

            double v152pos_51_max = 0, v152pos_52_max = 0;

            double auto_v152mpositiontaking_mem_51_max = 0, auto_v152mpositiontaking_mem_52_max = 0;

            bool isauto_v152mpositiontaking_mem_51 = false, isauto_v152mpositiontaking_mem_52 = false;

            bool isauto_v152mpositiontaking_51 = false, isauto_v152mpositiontaking_52 = false;

            double.TryParse(dr["v152mpositiontaking_51"].ToString(), out v152mpositiontaking_51);
            double.TryParse(dr["v152mpositiontaking_52"].ToString(), out v152mpositiontaking_52);

            double.TryParse(dr["auto_v152mpositiontaking_mem_51"].ToString(), out auto_v152mpositiontaking_mem_51);
            double.TryParse(dr["auto_v152mpositiontaking_mem_52"].ToString(), out auto_v152mpositiontaking_mem_52);

            double.TryParse(dr["v152apositiontaking_51"].ToString(), out v152apositiontaking_51);
            double.TryParse(dr["v152apositiontaking_52"].ToString(), out v152apositiontaking_52);

            double.TryParse(dr["v152pos_51_min"].ToString(), out v152pos_51_min);
            double.TryParse(dr["v152pos_52_min"].ToString(), out v152pos_52_min);

            double.TryParse(dr["v152pos_51_max"].ToString(), out v152pos_51_max);
            double.TryParse(dr["v152pos_52_max"].ToString(), out v152pos_52_max);

            double.TryParse(dr["auto_v152mpositiontaking_mem_51_max"].ToString(), out auto_v152mpositiontaking_mem_51_max);
            double.TryParse(dr["auto_v152mpositiontaking_mem_52_max"].ToString(), out auto_v152mpositiontaking_mem_52_max);

            isauto_v152mpositiontaking_mem_51 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_mem_51"]);
            isauto_v152mpositiontaking_mem_52 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_mem_52"]);

            isauto_v152mpositiontaking_51 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_51"]);
            isauto_v152mpositiontaking_52 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_52"]);

            tpl.agent_win_val = Convert.ToDouble(dr["v152apositiontaking_51"]).ToString();
            tpl.agent_place_val = Convert.ToDouble(dr["v152apositiontaking_52"]).ToString();

            this.JavaScriptRegister("v152pos_51_max", v152pos_51_max);
            this.JavaScriptRegister("v152pos_52_max", v152pos_52_max);

            this.JavaScriptRegister("IsAutoMaster_152_51", isauto_v152mpositiontaking_mem_51);
            this.JavaScriptRegister("IsAutoMaster_152_52", isauto_v152mpositiontaking_mem_52);

            this.HashTableRegister("chkAutoMaster_Agent_152_51", isauto_v152mpositiontaking_51);
            this.HashTableRegister("chkAutoMaster_Agent_152_52", isauto_v152mpositiontaking_52);

            bool isDisabled_CopyWin = !isauto_v152mpositiontaking_mem_51 ? isauto_v152mpositiontaking_51 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_152_51)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_152_51", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.master_win_val = isauto_v152mpositiontaking_51 ? v152mpositiontaking_51.ToString() : new PT_Selection(TemplateMain, "listMaster_152_51", v152mpositiontaking_51, isauto_v152mpositiontaking_mem_51, v152pos_51_min, v152pos_51_max, 0.1, true, false);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_152_51", auto_v152mpositiontaking_mem_51, !isauto_v152mpositiontaking_mem_51, 0, auto_v152mpositiontaking_mem_51_max, 0.1, true, false);

            tpl.master_place_val = isauto_v152mpositiontaking_52 ? v152mpositiontaking_52.ToString() : new PT_Selection(TemplateMain, "listMaster_152_52", v152mpositiontaking_52, isauto_v152mpositiontaking_mem_52, v152pos_52_min, v152pos_52_max, 0.1);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_152_52", auto_v152mpositiontaking_mem_52, !isauto_v152mpositiontaking_mem_52, 0, auto_v152mpositiontaking_mem_52_max, 0.1);

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_152_51", isauto_v152mpositiontaking_mem_51, "CheckAuto(this)");
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_152_52", isauto_v152mpositiontaking_mem_52, "CheckAuto(this)");

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
            double v153mpositiontaking_51 = 0, v153mpositiontaking_52 = 0;

            double auto_v153mpositiontaking_mem_51 = 0, auto_v153mpositiontaking_mem_52 = 0;

            double v153apositiontaking_51 = 0, v153apositiontaking_52 = 0;

            double v153pos_51_min = 0, v153pos_52_min = 0;

            double v153pos_51_max = 0, v153pos_52_max = 0;

            double auto_v153mpositiontaking_mem_51_max = 0, auto_v153mpositiontaking_mem_52_max = 0;

            bool isauto_v153mpositiontaking_mem_51 = false, isauto_v153mpositiontaking_mem_52 = false;

            bool isauto_v153mpositiontaking_51 = false, isauto_v153mpositiontaking_52 = false;

            double.TryParse(dr["v153mpositiontaking_51"].ToString(), out v153mpositiontaking_51);
            double.TryParse(dr["v153mpositiontaking_52"].ToString(), out v153mpositiontaking_52);

            double.TryParse(dr["auto_v153mpositiontaking_mem_51"].ToString(), out auto_v153mpositiontaking_mem_51);
            double.TryParse(dr["auto_v153mpositiontaking_mem_52"].ToString(), out auto_v153mpositiontaking_mem_52);

            double.TryParse(dr["v153apositiontaking_51"].ToString(), out v153apositiontaking_51);
            double.TryParse(dr["v153apositiontaking_52"].ToString(), out v153apositiontaking_52);

            double.TryParse(dr["v153pos_51_min"].ToString(), out v153pos_51_min);
            double.TryParse(dr["v153pos_52_min"].ToString(), out v153pos_52_min);

            double.TryParse(dr["v153pos_51_max"].ToString(), out v153pos_51_max);
            double.TryParse(dr["v153pos_52_max"].ToString(), out v153pos_52_max);

            double.TryParse(dr["auto_v153mpositiontaking_mem_51_max"].ToString(), out auto_v153mpositiontaking_mem_51_max);
            double.TryParse(dr["auto_v153mpositiontaking_mem_52_max"].ToString(), out auto_v153mpositiontaking_mem_52_max);

            isauto_v153mpositiontaking_mem_51 = Convert.ToBoolean(dr["isauto_v153mpositiontaking_mem_51"]);
            isauto_v153mpositiontaking_mem_52 = Convert.ToBoolean(dr["isauto_v153mpositiontaking_mem_52"]);

            isauto_v153mpositiontaking_51 = Convert.ToBoolean(dr["isauto_v153mpositiontaking_51"]);
            isauto_v153mpositiontaking_52 = Convert.ToBoolean(dr["isauto_v153mpositiontaking_52"]);

            tpl.agent_win_val = Convert.ToDouble(dr["v153apositiontaking_51"]).ToString();
            tpl.agent_place_val = Convert.ToDouble(dr["v153apositiontaking_52"]).ToString();

            this.JavaScriptRegister("v153pos_51_max", v153pos_51_max);
            this.JavaScriptRegister("v153pos_52_max", v153pos_52_max);

            this.JavaScriptRegister("IsAutoMaster_153_51", isauto_v153mpositiontaking_mem_51);
            this.JavaScriptRegister("IsAutoMaster_153_52", isauto_v153mpositiontaking_mem_52);

            this.HashTableRegister("chkAutoMaster_Agent_153_51", isauto_v153mpositiontaking_51);
            this.HashTableRegister("chkAutoMaster_Agent_153_52", isauto_v153mpositiontaking_52);

            bool isDisabled_CopyWin = !isauto_v153mpositiontaking_mem_51 ? isauto_v153mpositiontaking_51 ? true : false : false;
            string checkCopy_Onclick = "CheckCopy_Onclick(this, Arr_153_51)";
            tpl.copy_from_win = new PT_CopyFromCheckBox_Control(tpl, "chkCopyFromWin_153_51", checkCopy_Onclick, Resources.Racing.copyfromwin, isDisabled_CopyWin);

            tpl.master_win_val = isauto_v153mpositiontaking_51 ? v153mpositiontaking_51.ToString() : new PT_Selection(TemplateMain, "listMaster_153_51", v153mpositiontaking_51, isauto_v153mpositiontaking_mem_51, v153pos_51_min, v153pos_51_max, 0.1, true, false);
            tpl.master_auto_win_val = new PT_Selection(TemplateMain, "listAutoMaster_153_51", auto_v153mpositiontaking_mem_51, !isauto_v153mpositiontaking_mem_51, 0, auto_v153mpositiontaking_mem_51_max, 0.1, true, false);

            tpl.master_place_val = isauto_v153mpositiontaking_52 ? v153mpositiontaking_52.ToString() : new PT_Selection(TemplateMain, "listMaster_153_52", v153mpositiontaking_52, isauto_v153mpositiontaking_mem_52, v153pos_52_min, v153pos_52_max, 0.1);
            tpl.master_auto_place_val = new PT_Selection(TemplateMain, "listAutoMaster_153_52", auto_v153mpositiontaking_mem_52, !isauto_v153mpositiontaking_mem_52, 0, auto_v153mpositiontaking_mem_52_max, 0.1);

            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_153_51", isauto_v153mpositiontaking_mem_51, "CheckAuto(this)");
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(tpl, "chkAutoMaster_153_52", isauto_v153mpositiontaking_mem_52, "CheckAuto(this)");

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