using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.UI;
using Fanex.Age.UI.Components;
using Sunplus.Agent.Model.BL.Casino;
using System.Data;
using Resources;
using Sunplus.Agent.Utilities;

namespace Sunplus.Agent.UI._BetList
{
    public partial class CasinoResult_Keno : TplPage
    {
        private int userBetId = 0;
        private string gameTypeNo = null;

        public override void RenderContent(HtmlTextWriter writer)
        {
            var tpl = new CasinoResult_Keno_Template();

            if (this.TableDetail != null && this.TableDetail.Rows.Count > 0)
            {
                var rows = TableDetail.Rows;
                var row = rows[0];

                this.RenderBasicInfo(tpl, row);
                this.RenderTotal(tpl, this.TableTotal.Rows[0]);
            }

            writer.Write(tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var casino = new CasinoBO(CurrentUserSession.NetSessionId);
            var tables = casino.Report.WinLossDetail(userBetId);

            return tables;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.gameTypeNo = Request["gametype"];
            int.TryParse(Request["userbetid"], out this.userBetId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (userBetId == 0)
            {
                return new ErrorPage_Page("Invalid user bet id!");
            }

            return null;
        }

        private DataTable TableDetail
        {
            get
            {
                return ((DataTable[])View)[0];
            }
        }

        private DataTable TableTotal
        {
            get
            {
                return ((DataTable[])View)[1];
            }
        }

        private void RenderBasicInfo(CasinoResult_Keno_Template tpl, DataRow dr)
        {
            tpl.tablename = dr[CasinoBO.CasinoLang("TableName")].ToString();
            tpl.datetime = dr["BetTime"].ToString();
            tpl.Player = Lang.player;
            tpl.UserName = dr["UserName"].ToString();
            tpl.Result = dr[CasinoBO.CasinoLang("ResultName")].ToString();

            tpl.Number = dr[CasinoBO.CasinoLang("Number")].ToString();

            var results = dr["Result"].ToString().Split(',');
            tpl.Val1 = results[0];
            tpl.Val2 = results[1];
            tpl.Val3 = results[2];
            tpl.Val4 = results[3];
            tpl.Val5 = results[4];
            tpl.Val6 = results[5];
            tpl.Val7 = results[6];
            tpl.Val8 = results[7];
            tpl.Val9 = results[8];
            tpl.Val10 = results[9];
            tpl.Val11 = results[10];
            tpl.Val12 = results[11];
            tpl.Val13 = results[12];
            tpl.Val14 = results[13];
            tpl.Val15 = results[14];
            tpl.Val16 = results[15];
            tpl.Val17 = results[16];
            tpl.Val18 = results[17];
            tpl.Val19 = results[18];
            tpl.Val20 = results[19];

            tpl.Pick = dr[CasinoBO.CasinoLang("Pick")].ToString();
            tpl.Pick_vl = dr["Pick_No"].ToString();
            tpl.Pick_count = dr["Pick_Count"].ToString();
            
            tpl.Hit = dr[CasinoBO.CasinoLang("Hit")].ToString();
            tpl.Hit_vl = dr["Hit_No"].ToString();
            tpl.Hit_count = dr["Hit_Count"].ToString();

            tpl.PlayerHistoryNo = dr[CasinoBO.CasinoLang("PlayerHistoryNo")].ToString();
            tpl.PlayerHistoryNo_vl = dr["PlayerHistoryNo_Result"].ToString();

            tpl.AutoPlay = dr[CasinoBO.CasinoLang("AutoPlay")].ToString();
            tpl.AutoPlay_vl = dr[CasinoBO.CasinoLang("AutoPlayResult")].ToString();
        }

        private void RenderTotal(CasinoResult_Keno_Template tpl, DataRow dr)
        {
            tpl.Odds = Lang.odds;
            tpl.Odds_vl = decimal.Round((decimal)dr["Odds"], 2).ToString();

            tpl.bet = Resources.Casino.Bet;
            tpl.bet_vl = string.Join(" ", new string[] { dr["Currency"].ToString(), this.NumberReport(dr["Bet"]) });
            tpl.winloss = Lang.win;

            var dec = decimal.Round((decimal)dr["WinLoss"], 2);
            tpl.winloss_vl = string.Join(" ", new string[] { dr["Currency"].ToString(), this.NumberReport(dec) });
        }

        private string NumberReport(object number)
        {
            decimal dcNumber = Convert.IsDBNull(number) ? 0 : (decimal)number;
            string resultText = Formater.DecFormat(number, 2);
            string spanClass = dcNumber >= 0 ? "bl_underdog" : "bl_favorite";
            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
        }
    }
}