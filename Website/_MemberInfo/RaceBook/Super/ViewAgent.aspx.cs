// --------------------------------------------------------------------------
// <copyright file="ViewAgent.aspx.cs" company="Nexcel Solutions Vietnam">
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
    using Sunplus.Agent.UI._MemberInfo.RaceBook.MinMaxBet_Control;
    using Sunplus.Agent.UI.PositionTaking.RaceBook.Super;
    using Sunplus.Agent.Utilities;

    public partial class ViewAgent_Page : RaceBookSettingBase_Page
    {
        public override void RenderStatus(System.Data.DataRow dr, TplCore.Element.IElement e)
        {
            ViewAgent_Template tpl = e as ViewAgent_Template;

            RacingStatus_Control_Template[] ts = new RacingStatus_Control_Template[6];
            this.RenderStatus(dr, ref ts[0], ref ts[1], ref ts[2], ref ts[3], ref ts[4], ref ts[5], true);

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
        }

        public override void RenderBetSetting(System.Data.DataRow dr, TplCore.Element.IElement e)
        {
            ViewAgent_Template tpl = e as ViewAgent_Template;

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

        public override void RenderCommission(System.Data.DataRow dr, TplCore.Element.IElement e)
        {
            ViewAgent_Template tpl = e as ViewAgent_Template;

            double[] ts = new double[6];

            tpl.Section_151.Section_151_31.commission_text = tpl.Section_151.Section_151_41.commission_text = tpl.Section_151.Section_151_51.commission_text
                = tpl.Section_152.Section_152_41.commission_text = tpl.Section_152.Section_152_51.commission_text
                = tpl.Section_153.Section_153_51.commission_text = Resources.Racing.commission;

            this.RenderCommission(dr, tpl, out ts[0], out ts[1], out ts[2], out ts[3], out ts[4], out ts[5]);

            tpl.Section_151.Section_151_41.commission_val = Formater.FormatNumber4(ts[0]);
            tpl.Section_151.Section_151_31.commission_val = Formater.FormatNumber4(ts[1]);
            tpl.Section_151.Section_151_51.commission_val = Formater.FormatNumber4(ts[2]);
            tpl.Section_152.Section_152_41.commission_val = Formater.FormatNumber4(ts[3]);
            tpl.Section_152.Section_152_51.commission_val = Formater.FormatNumber4(ts[4]);
            tpl.Section_153.Section_153_51.commission_val = Formater.FormatNumber4(ts[5]);
        }

        public void RenderPositionTaking(DataRow dr, ViewAgent_Template tpl)
        {
            this.RenderPositionTakingInfo(tpl);
            this.RenderPT_151_41(dr, tpl.Section_151.Section_151_41);
            this.RenderPT_151_31(dr, tpl.Section_151.Section_151_31);
            this.RenderPT_151_51(dr, tpl.Section_151.Section_151_51);
            this.RenderPT_152_41(dr, tpl.Section_152.Section_152_41);
            this.RenderPT_152_51(dr, tpl.Section_152.Section_152_51);
            this.RenderPT_153_51(dr, tpl.Section_153.Section_153_51);
        }

        public void RenderPositionTakingInfo(ViewAgent_Template tpl)
        {
            tpl.UserInfo.CopyAll = null;

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

            tpl.Section_151.Section_151_31.agent_preset_text = Resources.Racing.agentpositiontaking;
            tpl.Section_151.Section_151_31.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_31.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_151.Section_151_41.agent_preset_text = Resources.Racing.agentpositiontaking;
            tpl.Section_151.Section_151_41.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_41.master_auto_preset_text = Resources.Racing.automasterpreset;
            tpl.Section_151.Section_151_41.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_151.Section_151_51.agent_preset_text = Resources.Racing.agentpositiontaking;
            tpl.Section_151.Section_151_51.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_151.Section_151_51.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_151.Section_151_31.menu_text = Resources.Racing.satote;
            tpl.Section_151.Section_151_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_151.Section_151_51.menu_text = Resources.Racing.autote;

            // 152
            tpl.Section_152.Section_152_41.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_41.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_51.win_text = Resources.Racing.win;
            tpl.Section_152.Section_152_51.place_text = Resources.Racing.place;

            tpl.Section_152.Section_152_41.agent_preset_text = Resources.Racing.agentpositiontaking;
            tpl.Section_152.Section_152_41.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_152.Section_152_41.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_152.Section_152_51.agent_preset_text = Resources.Racing.agentpositiontaking;
            tpl.Section_152.Section_152_51.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_152.Section_152_51.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_152.Section_152_41.menu_text = Resources.Racing.ukirishtote;
            tpl.Section_152.Section_152_51.menu_text = Resources.Racing.autote;

            // 153
            tpl.Section_153.Section_153_51.win_text = Resources.Racing.win;
            tpl.Section_153.Section_153_51.place_text = Resources.Racing.place;

            tpl.Section_153.Section_153_51.agent_preset_text = Resources.Racing.agentpositiontaking;
            tpl.Section_153.Section_153_51.master_preset_text = Resources.Racing.masterpreset;
            tpl.Section_153.Section_153_51.master_auto_preset_text = Resources.Racing.automasterpreset;

            tpl.Section_153.Section_153_51.menu_text = Resources.Racing.autote;
        }

        public void RenderPT_151_41(DataRow dr, ViewAgent_Section_151_Section_151_41_Block tpl)
        {
            double v151mpositiontaking_41 = 0, v151mpositiontaking_42 = 0;

            double v151apositiontaking_41 = 0, v151apositiontaking_42 = 0;

            double auto_v151mpositiontaking_41 = 0, auto_v151mpositiontaking_42 = 0;

            bool isauto_v151mpositiontaking_41 = false, isauto_v151mpositiontaking_42 = false;

            double.TryParse(dr["v151mpositiontaking_41"].ToString(), out v151mpositiontaking_41);
            double.TryParse(dr["v151mpositiontaking_42"].ToString(), out v151mpositiontaking_42);

            double.TryParse(dr["v151apositiontaking_41"].ToString(), out v151apositiontaking_41);
            double.TryParse(dr["v151apositiontaking_42"].ToString(), out v151apositiontaking_42);

            double.TryParse(dr["auto_v151mpositiontaking_41"].ToString(), out auto_v151mpositiontaking_41);
            double.TryParse(dr["auto_v151mpositiontaking_42"].ToString(), out auto_v151mpositiontaking_42);

            isauto_v151mpositiontaking_41 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_41"]);
            isauto_v151mpositiontaking_42 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_42"]);

            tpl.agent_win_val = v151apositiontaking_41.ToString();
            tpl.agent_place_val = v151apositiontaking_42.ToString();
            tpl.master_win_val = v151mpositiontaking_41.ToString();
            tpl.master_place_val = v151mpositiontaking_42.ToString();
            tpl.master_auto_win_val = auto_v151mpositiontaking_41.ToString();
            tpl.master_auto_place_val = auto_v151mpositiontaking_42.ToString();
            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v151mpositiontaking_41", isauto_v151mpositiontaking_41, true);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v151mpositiontaking_42", isauto_v151mpositiontaking_42, true);
        }

        public void RenderPT_151_31(DataRow dr, ViewAgent_Section_151_Section_151_31_Block tpl)
        {
            double v151mpositiontaking_31 = 0, v151mpositiontaking_32 = 0;

            double v151apositiontaking_31 = 0, v151apositiontaking_32 = 0;

            double auto_v151mpositiontaking_31 = 0, auto_v151mpositiontaking_32 = 0;

            bool isauto_v151mpositiontaking_31 = false, isauto_v151mpositiontaking_32 = false;

            double.TryParse(dr["v151mpositiontaking_31"].ToString(), out v151mpositiontaking_31);
            double.TryParse(dr["v151mpositiontaking_32"].ToString(), out v151mpositiontaking_32);

            double.TryParse(dr["v151apositiontaking_31"].ToString(), out v151apositiontaking_31);
            double.TryParse(dr["v151apositiontaking_32"].ToString(), out v151apositiontaking_32);

            double.TryParse(dr["auto_v151mpositiontaking_31"].ToString(), out auto_v151mpositiontaking_31);
            double.TryParse(dr["auto_v151mpositiontaking_32"].ToString(), out auto_v151mpositiontaking_32);

            isauto_v151mpositiontaking_31 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_31"]);
            isauto_v151mpositiontaking_32 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_32"]);

            tpl.agent_win_val = v151apositiontaking_31.ToString();
            tpl.agent_place_val = v151apositiontaking_32.ToString();
            tpl.master_win_val = v151mpositiontaking_31.ToString();
            tpl.master_place_val = v151mpositiontaking_32.ToString();
            tpl.master_auto_win_val = auto_v151mpositiontaking_31.ToString();
            tpl.master_auto_place_val = auto_v151mpositiontaking_32.ToString();
            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v151mpositiontaking_31", isauto_v151mpositiontaking_31, true);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v151mpositiontaking_32", isauto_v151mpositiontaking_32, true);
        }

        public void RenderPT_151_51(DataRow dr, ViewAgent_Section_151_Section_151_51_Block tpl)
        {
            double v151mpositiontaking_51 = 0, v151mpositiontaking_52 = 0;

            double v151apositiontaking_51 = 0, v151apositiontaking_52 = 0;

            double auto_v151mpositiontaking_51 = 0, auto_v151mpositiontaking_52 = 0;

            bool isauto_v151mpositiontaking_51 = false, isauto_v151mpositiontaking_52 = false;

            double.TryParse(dr["v151mpositiontaking_51"].ToString(), out v151mpositiontaking_51);
            double.TryParse(dr["v151mpositiontaking_52"].ToString(), out v151mpositiontaking_52);

            double.TryParse(dr["v151apositiontaking_51"].ToString(), out v151apositiontaking_51);
            double.TryParse(dr["v151apositiontaking_52"].ToString(), out v151apositiontaking_52);

            double.TryParse(dr["auto_v151mpositiontaking_51"].ToString(), out auto_v151mpositiontaking_51);
            double.TryParse(dr["auto_v151mpositiontaking_52"].ToString(), out auto_v151mpositiontaking_52);

            isauto_v151mpositiontaking_51 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_51"]);
            isauto_v151mpositiontaking_52 = Convert.ToBoolean(dr["isauto_v151mpositiontaking_52"]);

            tpl.agent_win_val = v151apositiontaking_51.ToString();
            tpl.agent_place_val = v151apositiontaking_52.ToString();
            tpl.master_win_val = v151mpositiontaking_51.ToString();
            tpl.master_place_val = v151mpositiontaking_52.ToString();
            tpl.master_auto_win_val = auto_v151mpositiontaking_51.ToString();
            tpl.master_auto_place_val = auto_v151mpositiontaking_52.ToString();
            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v151mpositiontaking_51", isauto_v151mpositiontaking_51, true);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v151mpositiontaking_52", isauto_v151mpositiontaking_52, true);
        }

        public void RenderPT_152_41(DataRow dr, ViewAgent_Section_152_Section_152_41_Block tpl)
        {
            double v152mpositiontaking_41 = 0, v152mpositiontaking_42 = 0;

            double v152apositiontaking_41 = 0, v152apositiontaking_42 = 0;

            double auto_v152mpositiontaking_41 = 0, auto_v152mpositiontaking_42 = 0;

            bool isauto_v152mpositiontaking_41 = false, isauto_v152mpositiontaking_42 = false;

            double.TryParse(dr["v152mpositiontaking_41"].ToString(), out v152mpositiontaking_41);
            double.TryParse(dr["v152mpositiontaking_42"].ToString(), out v152mpositiontaking_42);

            double.TryParse(dr["v152apositiontaking_41"].ToString(), out v152apositiontaking_41);
            double.TryParse(dr["v152apositiontaking_42"].ToString(), out v152apositiontaking_42);

            double.TryParse(dr["auto_v152mpositiontaking_41"].ToString(), out auto_v152mpositiontaking_41);
            double.TryParse(dr["auto_v152mpositiontaking_42"].ToString(), out auto_v152mpositiontaking_42);

            isauto_v152mpositiontaking_41 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_41"]);
            isauto_v152mpositiontaking_42 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_42"]);

            tpl.agent_win_val = v152apositiontaking_41.ToString();
            tpl.agent_place_val = v152apositiontaking_42.ToString();
            tpl.master_win_val = v152mpositiontaking_41.ToString();
            tpl.master_place_val = v152mpositiontaking_42.ToString();
            tpl.master_auto_win_val = auto_v152mpositiontaking_41.ToString();
            tpl.master_auto_place_val = auto_v152mpositiontaking_42.ToString();
            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v152mpositiontaking_41", isauto_v152mpositiontaking_41, true);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v152mpositiontaking_42", isauto_v152mpositiontaking_42, true);
        }

        public void RenderPT_152_51(DataRow dr, ViewAgent_Section_152_Section_152_51_Block tpl)
        {
            double v152mpositiontaking_51 = 0, v152mpositiontaking_52 = 0;

            double v152apositiontaking_51 = 0, v152apositiontaking_52 = 0;

            double auto_v152mpositiontaking_51 = 0, auto_v152mpositiontaking_52 = 0;

            bool isauto_v152mpositiontaking_51 = false, isauto_v152mpositiontaking_52 = false;

            double.TryParse(dr["v152mpositiontaking_51"].ToString(), out v152mpositiontaking_51);
            double.TryParse(dr["v152mpositiontaking_52"].ToString(), out v152mpositiontaking_52);

            double.TryParse(dr["v152apositiontaking_51"].ToString(), out v152apositiontaking_51);
            double.TryParse(dr["v152apositiontaking_52"].ToString(), out v152apositiontaking_52);

            double.TryParse(dr["auto_v152mpositiontaking_51"].ToString(), out auto_v152mpositiontaking_51);
            double.TryParse(dr["auto_v152mpositiontaking_52"].ToString(), out auto_v152mpositiontaking_52);

            isauto_v152mpositiontaking_51 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_51"]);
            isauto_v152mpositiontaking_52 = Convert.ToBoolean(dr["isauto_v152mpositiontaking_52"]);

            tpl.agent_win_val = v152apositiontaking_51.ToString();
            tpl.agent_place_val = v152apositiontaking_52.ToString();
            tpl.master_win_val = v152mpositiontaking_51.ToString();
            tpl.master_place_val = v152mpositiontaking_52.ToString();
            tpl.master_auto_win_val = auto_v152mpositiontaking_51.ToString();
            tpl.master_auto_place_val = auto_v152mpositiontaking_52.ToString();
            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v152mpositiontaking_51", isauto_v152mpositiontaking_51, true);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v152mpositiontaking_52", isauto_v152mpositiontaking_52, true);
        }

        public void RenderPT_153_51(DataRow dr, ViewAgent_Section_153_Section_153_51_Block tpl)
        {
            double v153mpositiontaking_51 = 0, v153mpositiontaking_52 = 0;

            double v153apositiontaking_51 = 0, v153apositiontaking_52 = 0;

            double auto_v153mpositiontaking_51 = 0, auto_v153mpositiontaking_52 = 0;

            bool isauto_v153mpositiontaking_51 = false, isauto_v153mpositiontaking_52 = false;

            double.TryParse(dr["v153mpositiontaking_51"].ToString(), out v153mpositiontaking_51);
            double.TryParse(dr["v153mpositiontaking_52"].ToString(), out v153mpositiontaking_52);

            double.TryParse(dr["v153apositiontaking_51"].ToString(), out v153apositiontaking_51);
            double.TryParse(dr["v153apositiontaking_52"].ToString(), out v153apositiontaking_52);

            double.TryParse(dr["auto_v153mpositiontaking_51"].ToString(), out auto_v153mpositiontaking_51);
            double.TryParse(dr["auto_v153mpositiontaking_52"].ToString(), out auto_v153mpositiontaking_52);

            isauto_v153mpositiontaking_51 = Convert.ToBoolean(dr["isauto_v153mpositiontaking_51"]);
            isauto_v153mpositiontaking_52 = Convert.ToBoolean(dr["isauto_v153mpositiontaking_52"]);

            tpl.agent_win_val = v153apositiontaking_51.ToString();
            tpl.agent_place_val = v153apositiontaking_52.ToString();
            tpl.master_win_val = v153mpositiontaking_51.ToString();
            tpl.master_place_val = v153mpositiontaking_52.ToString();
            tpl.master_auto_win_val = auto_v153mpositiontaking_51.ToString();
            tpl.master_auto_place_val = auto_v153mpositiontaking_52.ToString();
            tpl.master_auto_win_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v153mpositiontaking_51", isauto_v153mpositiontaking_51, true);
            tpl.master_auto_place_checkbox = new PT_AutoCheckBox_Control(TemplateMain, "isauto_v153mpositiontaking_52", isauto_v153mpositiontaking_52, true);
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            ViewAgent_Template tpl = new ViewAgent_Template();
            PT_Control_Template tplcontrol = new PT_Control_Template();
            DataTable racingData = (View as Dictionary<string, DataTable>)["RacingData"];
            DataRow dr = racingData.Rows[0];

            this.RenderStatus(dr, tpl);
            this.RenderCustomerInfo(dr, tpl.UserInfo);
            this.RenderCommission(dr, tpl);
            this.RenderBetSetting(dr, tpl);
            this.RenderPositionTaking(dr, tpl);

            TemplateMain.RaceBookJs = null;
            TemplateMain.actionFrm = "ViewAgent.aspx";

            this.RegisterClientVariables();

            this.RegisterJavascriptObject("isView", true);
            this.RegisterClientHiddenControl();
            TemplateMain.content = tpl;
            writer.Write(this.TemplateMain);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Dictionary<string, DataTable> viewData = new Dictionary<string, DataTable>();

            RacingBO bo = new RacingBO(CurrentUserSession.NetSessionId);

            Hashtable outputs = null;
            DataSet ds = bo.SuperGetHRSetting_Agent(CustId, out outputs);

            viewData.Add("RacingData", ds.Tables[0]);

            return viewData;
        }
    }
}