// --------------------------------------------------------------------------
// <copyright file="OnMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.06.01">Improve performance, Check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook.Agent
{
    using System.Data;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.UI.PositionTaking.RaceBook.Agent;

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

        public void RenderPT_151_41(DataRow dr, OnMember_Section_151_Section_151_41_Block tpl)
        {
            double maxv151pos_41 = 0, maxv151pos_42 = 0;
            double v151apositiontaking_41 = 0, v151apositiontaking_42 = 0;
            double minv151pos_41 = 0, minv151pos_42 = 0;

            double.TryParse(dr["v151pos_41_max"].ToString(), out maxv151pos_41);
            double.TryParse(dr["v151pos_42_max"].ToString(), out maxv151pos_42);

            double.TryParse(dr["v151apositiontaking_41"].ToString(), out v151apositiontaking_41);
            double.TryParse(dr["v151apositiontaking_42"].ToString(), out v151apositiontaking_42);

            double.TryParse(dr["v151pos_41_min"].ToString(), out minv151pos_41);
            double.TryParse(dr["v151pos_42_min"].ToString(), out minv151pos_42);

            tpl.member_win_val = new PT_Selection(TemplateMain, "listMember_151_41", v151apositiontaking_41, minv151pos_41, maxv151pos_41, 0.1, true, false);
            tpl.member_place_val = new PT_Selection(TemplateMain, "listMember_151_42", v151apositiontaking_42, minv151pos_42, maxv151pos_42, 0.1);

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

        public void RenderPT_151_31(DataRow dr, OnMember_Section_151_Section_151_31_Block tpl)
        {
            double maxv151pos_31 = 0, maxv151pos_32 = 0;
            double v151apositiontaking_31 = 0, v151apositiontaking_32 = 0;
            double minv151pos_31 = 0, minv151pos_32 = 0;

            double.TryParse(dr["v151pos_31_max"].ToString(), out maxv151pos_31);
            double.TryParse(dr["v151pos_32_max"].ToString(), out maxv151pos_32);

            double.TryParse(dr["v151apositiontaking_31"].ToString(), out v151apositiontaking_31);
            double.TryParse(dr["v151apositiontaking_32"].ToString(), out v151apositiontaking_32);

            double.TryParse(dr["v151pos_31_min"].ToString(), out minv151pos_31);
            double.TryParse(dr["v151pos_32_min"].ToString(), out minv151pos_32);

            tpl.member_win_val = new PT_Selection(TemplateMain, "listMember_151_31", v151apositiontaking_31, minv151pos_31, maxv151pos_31, 0.1, true, false);
            tpl.member_place_val = new PT_Selection(TemplateMain, "listMember_151_32", v151apositiontaking_32, minv151pos_32, maxv151pos_32, 0.1);

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

        public void RenderPT_151_51(DataRow dr, OnMember_Section_151_Section_151_51_Block tpl)
        {
            double maxv151pos_51 = 0, maxv151pos_52 = 0;
            double v151apositiontaking_51 = 0, v151apositiontaking_52 = 0;
            double minv151pos_51 = 0, minv151pos_52 = 0;

            double.TryParse(dr["v151pos_51_max"].ToString(), out maxv151pos_51);
            double.TryParse(dr["v151pos_52_max"].ToString(), out maxv151pos_52);

            double.TryParse(dr["v151apositiontaking_51"].ToString(), out v151apositiontaking_51);
            double.TryParse(dr["v151apositiontaking_52"].ToString(), out v151apositiontaking_52);

            double.TryParse(dr["v151pos_51_min"].ToString(), out minv151pos_51);
            double.TryParse(dr["v151pos_52_min"].ToString(), out minv151pos_52);

            tpl.member_win_val = new PT_Selection(TemplateMain, "listMember_151_51", v151apositiontaking_51, minv151pos_51, maxv151pos_51, 0.1, true, false);
            tpl.member_place_val = new PT_Selection(TemplateMain, "listMember_151_52", v151apositiontaking_52, minv151pos_52, maxv151pos_52, 0.1);

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

        public void RenderPT_152_41(DataRow dr, OnMember_Section_152_Section_152_41_Block tpl)
        {
            double maxv152pos_41 = 0, maxv152pos_42 = 0;
            double v152apositiontaking_41 = 0, v152apositiontaking_42 = 0;
            double minv152pos_41 = 0, minv152pos_42 = 0;

            double.TryParse(dr["v152pos_41_max"].ToString(), out maxv152pos_41);
            double.TryParse(dr["v152pos_42_max"].ToString(), out maxv152pos_42);

            double.TryParse(dr["v152apositiontaking_41"].ToString(), out v152apositiontaking_41);
            double.TryParse(dr["v152apositiontaking_42"].ToString(), out v152apositiontaking_42);

            double.TryParse(dr["v152pos_41_min"].ToString(), out minv152pos_41);
            double.TryParse(dr["v152pos_42_min"].ToString(), out minv152pos_42);

            tpl.member_win_val = new PT_Selection(TemplateMain, "listMember_152_41", v152apositiontaking_41, minv152pos_41, maxv152pos_41, 0.1, true, false);
            tpl.member_place_val = new PT_Selection(TemplateMain, "listMember_152_42", v152apositiontaking_42, minv152pos_42, maxv152pos_42, 0.1);

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

        public void RenderPT_152_51(DataRow dr, OnMember_Section_152_Section_152_51_Block tpl)
        {
            double maxv152pos_51 = 0, maxv152pos_52 = 0;
            double v152apositiontaking_51 = 0, v152apositiontaking_52 = 0;
            double minv152pos_51 = 0, minv152pos_52 = 0;

            double.TryParse(dr["v152pos_51_max"].ToString(), out maxv152pos_51);
            double.TryParse(dr["v152pos_52_max"].ToString(), out maxv152pos_52);

            double.TryParse(dr["v152apositiontaking_51"].ToString(), out v152apositiontaking_51);
            double.TryParse(dr["v152apositiontaking_52"].ToString(), out v152apositiontaking_52);

            double.TryParse(dr["v152pos_51_min"].ToString(), out minv152pos_51);
            double.TryParse(dr["v152pos_52_min"].ToString(), out minv152pos_52);

            tpl.member_win_val = new PT_Selection(TemplateMain, "listMember_152_51", v152apositiontaking_51, minv152pos_51, maxv152pos_51, 0.1, true, false);
            tpl.member_place_val = new PT_Selection(TemplateMain, "listMember_152_52", v152apositiontaking_52, minv152pos_52, maxv152pos_52, 0.1);

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

        public void RenderPT_153_51(DataRow dr, OnMember_Section_153_Section_153_51_Block tpl)
        {
            double maxv153pos_51 = 0, maxv153pos_52 = 0;
            double v153apositiontaking_51 = 0, v153apositiontaking_52 = 0;
            double minv153pos_51 = 0, minv153pos_52 = 0;

            double.TryParse(dr["v153pos_51_max"].ToString(), out maxv153pos_51);
            double.TryParse(dr["v153pos_52_max"].ToString(), out maxv153pos_52);

            double.TryParse(dr["v153apositiontaking_51"].ToString(), out v153apositiontaking_51);
            double.TryParse(dr["v153apositiontaking_52"].ToString(), out v153apositiontaking_52);

            double.TryParse(dr["v153pos_51_min"].ToString(), out minv153pos_51);
            double.TryParse(dr["v153pos_52_min"].ToString(), out minv153pos_52);

            tpl.member_win_val = new PT_Selection(TemplateMain, "listMember_153_51", v153apositiontaking_51, minv153pos_51, maxv153pos_51, 0.1, true, false);
            tpl.member_place_val = new PT_Selection(TemplateMain, "listMember_153_52", v153apositiontaking_52, minv153pos_52, maxv153pos_52, 0.1);

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