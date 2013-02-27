using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.Casino;
using Fanex.Age.UI.Components;
using System.Data;
using Resources;
using Sunplus.Agent.Utilities;

namespace Sunplus.Agent.UI._BetList
{
    public partial class CasinoResult_Scratch : TplPage
    {
        private int userBetId = 0;
        private string gameTypeNo = null;

        private string RenderTable(DataTable dt)
        {
            string strRet = "<table border=1>";
            strRet += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strRet += "<td>" + dt.Columns[i].ColumnName + "</td>";
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strRet += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    strRet += "<td>" + (dt.Rows[i][j] == null ? "null" : dt.Rows[i][j].ToString()) + "</td>";
                }
                strRet += "</tr>";
            }

            strRet += "</tr>";
            strRet += "</table><br />";

            return strRet;
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            var tpl = new CasinoResult_Scratch_Template();

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
                return new ErrorPage_Page(Lang.invalidLogin);
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

        private void RenderBasicInfo(CasinoResult_Scratch_Template tpl, DataRow dr)
        {
            tpl.tablename = dr[CasinoBO.CasinoLang("TableName")].ToString();
            tpl.datetime = dr["BetTime"].ToString();
            tpl.Player = Lang.player;
            tpl.UserName = dr["UserName"].ToString();
            tpl.Result = dr[CasinoBO.CasinoLang("ResultName")].ToString();

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

            tpl.PlayerHistoryNo = dr[CasinoBO.CasinoLang("PlayerHistoryNo")].ToString();
            tpl.PlayerHistoryNo_vl = dr["PlayerHistoryNo_Result"].ToString();

            tpl.AutoPlay = dr[CasinoBO.CasinoLang("AutoPlay")].ToString();
            tpl.AutoPlay_vl = dr[CasinoBO.CasinoLang("AutoPlayResult")].ToString();

            if (gameTypeNo == "RH011")
            {
                var blog = new CasinoResult_Scratch_MultiplierBlog_Block();
                blog.Multiplier = dr[CasinoBO.CasinoLang("Multiplier")].ToString();
                blog.Multiplier_vl = Convert.ToInt32(dr["Multiplier"]).ToString();

                tpl.MultiplierBlog = blog;
            }
            else
                tpl.MultiplierBlog.Clear();
        }

        private void RenderTotal(CasinoResult_Scratch_Template tpl, DataRow dr)
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
            decimal dcNumber = (Convert.IsDBNull(number) || string.IsNullOrEmpty(number.ToString())) ? 0m : Convert.ToDecimal(number);
            string resultText = Formater.DecFormat(dcNumber, 2);
            string spanClass = dcNumber >= 0 ? "bl_underdog" : "bl_favorite";
            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
        }
    }
}