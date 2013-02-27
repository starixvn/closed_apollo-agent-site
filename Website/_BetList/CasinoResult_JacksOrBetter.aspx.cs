namespace Sunplus.Agent.UI._BetList
{
    using System.Data;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Resources;
    using Sunplus.Agent.Model.BL.Casino;
    using TplCore.Element;
    using DNAide.Core;

    public partial class CasinoResult_JacksOrBetter : TplPage
    {
        private int userBetId = 0;

        private string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            var tpl = new CasinoResult_JacksOrBetter_Template();
            var rows = this.TableDetail.Rows;

            //Clear values
            tpl.Details.Clear();

            // render result
            if (rows != null && rows.Count > 0)
            {
                // render info
                tpl.tableName.tablename = rows[0][this.CasinoLang("TableName")] as string;
                tpl.tableName.datetime = rows[0]["BetTime"].ToString();
                tpl.userName.Player = Lang.player;
                tpl.userName.UserName = rows[0]["UserName"] as string;

                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var b = new CasinoResult_JacksOrBetter_Details_resultValue_Block();
                    b.result_vl = row["Result"].ToString();
                    tpl.Details.Append(b);
                }

                //Renders total region
                tpl.total.bet = Resources.Lang.lblBet;
                tpl.total.winloss = Resources.Lang.win;

                rows = this.TableTotal.Rows;
                if (rows != null && rows.Count > 0)
                {
                    decimal dcBet = 0, dcWin = 0;
                    foreach (DataRow r in rows)
                    {
                        dcBet += CastAide.AsDecimal(r["Bet"]);
                        dcWin += CastAide.AsDecimal(r["WinLoss"]);
                    }

                    tpl.total.bet_vl = decimal.Round(dcBet, 2).ToString();
                    tpl.total.winloss_vl = decimal.Round(dcWin, 2).ToString();
                }

                //Outputs to HTML
                writer.Write(tpl);
            }
            else
            {
                writer.Write(new ErrorPage_Page(Resources.Lang.noInfo));
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["userbetid"], out this.userBetId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (this.userBetId == 0)
            {
                return new ErrorPage_Page("Invalid user bet id!");
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var casino = new CasinoBO(CurrentUserSession.NetSessionId);
            var tables = casino.Report.WinLossDetail(this.userBetId);

            return tables;
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
    }
}