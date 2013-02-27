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
    public partial class CasinoResult_HiLoMahjong : TplPage
    {
        private int userBetId = 0;
        private string gameTypeNo = null;

        public override void RenderContent(HtmlTextWriter writer)
        {
            var tpl = new CasinoResult_HiLoMahjong_Template();
            tpl.Rows.Clear();

            if (this.TableDetail != null && this.TableDetail.Rows.Count > 0)
            {
                var rows = TableDetail.Rows;

                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var showType = row["showType"] as string;

                    if (showType == "BasicInfo")
                    {
                        this.RenderBasicInfo(tpl, row);
                    }
                    else if (showType == "PlayHistoryNo")
                    {
                        this.RenderPlayHistoryNo(tpl, row);
                    }
                    else if (showType == "Result_CardInfo")
                    {
                        this.RenderResult_CardInfo(tpl, row);
                    }
                }

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

        private void RenderBasicInfo(CasinoResult_HiLoMahjong_Template tpl, DataRow dr)
        {
            tpl.tablename = dr[CasinoBO.CasinoLang("TableName")].ToString();
            tpl.datetime = dr["BetTime"].ToString();
            tpl.Player = Lang.player;
            tpl.UserName = dr["UserName"].ToString();
            tpl.Result = dr[CasinoBO.CasinoLang("ResultName")].ToString();
            tpl.Guess = dr[CasinoBO.CasinoLang("BetTypeName")].ToString();
        }

        private void RenderResult_CardInfo(CasinoResult_HiLoMahjong_Template tpl, DataRow dr)
        {
            var row = new CasinoResult_HiLoMahjong_Rows_Block();

            row.Result_vl = dr["Result"].ToString();
            row.Guess_vl = dr[CasinoBO.CasinoLang("ResultName")].ToString();
            tpl.Rows.Append(row);
        }

        private void RenderPlayHistoryNo(CasinoResult_HiLoMahjong_Template tpl, DataRow dr)
        {
            tpl.PlayerHistoryNo = dr[CasinoBO.CasinoLang("PlayerHistoryNo")].ToString();
            tpl.PlayerHistoryNo_vl = dr["PlayerHistoryNo_Result"].ToString();            
        }

        private void RenderTotal(CasinoResult_HiLoMahjong_Template tpl, DataRow dr)
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