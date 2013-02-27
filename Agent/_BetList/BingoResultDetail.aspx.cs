using System.Web.UI;
using System.Collections;
using System;
using System.Data;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Bingo;
using Sunplus.Bingo.Services;
using Sunplus.Bingo.Core;
using Microsoft.Practices.Unity;


/*
 * Created 20100529@Jacob - Render BetList page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Sunplus.Agent.UI._BetList
{

    public partial class BingoResultDetail_Page : Fanex.Age.UI.TplPage
    {

        private int _userBetId = 0;
        private BingoResultDetail_Template _template = new BingoResultDetail_Template();

        #region Override


        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable[] handHistory = (DataTable[])View;

            //Response.Write(RenderTable(handHistory[0]));
            //Response.Write(RenderTable(handHistory[1]));

            //return;

            DataTable detailData = handHistory[1];
             if (handHistory == null || handHistory[0] == null || handHistory[1] == null ||
                handHistory[0].Rows.Count == 0 || handHistory[1].Rows.Count == 0) 
            {
                _template.NoInfo.Show();
                _template.WinlossDetail.Hide();
            }
            else
            {
                _template.WinlossDetail.TableName = handHistory[0].Rows[0][BingoBO.BingoLang("TableName")].ToString();
                _template.WinlossDetail.DateTime = handHistory[0].Rows[0]["BetTime"].ToString();
                _template.WinlossDetail.UserName = handHistory[0].Rows[0]["UserName"].ToString();
                _template.WinlossDetail.ResultName = handHistory[0].Rows[0][BingoBO.BingoLang("ResultName")].ToString();
                _template.WinlossDetail.ResultText = handHistory[0].Rows[0]["Result"].ToString();
                _template.WinlossDetail.PatternName = handHistory[0].Rows[0][BingoBO.BingoLang("PatternName")].ToString();
                _template.WinlossDetail.PatternText = handHistory[0].Rows[0]["Pattern"].ToString();
                 
                string bingoLiveBallNo = handHistory[0].Rows[0]["BingoLiveBallNo"] as string;
                string[] arrBallNo = bingoLiveBallNo.Split(new char[] { ',' });
                _template.WinlossDetail.kickOffSeq.kickOffSeq = RenderKickOffSeq(arrBallNo);

                _template.WinlossDetail.BetTotal = detailData.Rows[0]["Currency"].ToString() + " " + NumberReport(detailData.Rows[0]["Bet"]);
                _template.WinlossDetail.WinTotal = detailData.Rows[0]["Currency"].ToString() + " " + NumberReport(detailData.Rows[0]["WinLoss"]);

                _template.NoInfo.Hide();
                _template.WinlossDetail.Show();

                RenderHandHistoryDetail();
            }

            writer.Write(_template);
        }

        public string RenderKickOffSeq(string[] arrBallNo)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append("<table class=\"tblPop\" border=\"0\" cellpadding=\"0\" cellspacing=\"1\" width=\"450px\">");

            int c = arrBallNo.Length;
            builder.Append("<tr class=\"bg_eb2\">");
            for (int i = 0; i < 75; i++)
            {
                builder.Append("<td class='c'>");
                builder.Append(i >= c ? "N/A" : arrBallNo[i]);
                builder.Append("</td>");

                if (i == 14 || i == 29 || i == 44 || i == 59)
                {
                    builder.Append("</tr><tr class=\"bg_eb2\">");
                }
            }

            builder.Append("</tr></table><br />");

            return builder.ToString();
        }

        public string RenderTable(DataTable dt)
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


        public override void ProcessRequest(ref Hashtable p)
        {
            _userBetId = GetIntRequest("userbetid", 0);
        }

        public override object ValidateRequest(Hashtable p)
        {

            return null;
        }


        public override object InvokeData(Hashtable p)
        {
            IReportService bingoReportService = BingoContainer.Current.Resolve<IReportService>();

            LiveCasinoLibrary.SBAgent.Report report = new LiveCasinoLibrary.SBAgent.Report();

            return report.WinLossDetail(_userBetId, "01");
        }

        #endregion

        #region RenderUtility



        private void RenderHandHistoryDetail()
        {
            DataTable[] handHistory = (DataTable[])View;
            DataTable detailData = handHistory[0];

            _template.WinlossDetail.HandHistoryDetail.Clear();
            BingoResultDetail_WinlossDetail_HandHistoryDetail_Block block = new BingoResultDetail_WinlossDetail_HandHistoryDetail_Block();
            int i = 0;
            foreach (DataRow dr in detailData.Rows)
            {
                if (dr["ShowType"].ToString() == "Result_HitBetLine")
                {
                    block.BetType = dr[BingoBO.BingoLang("CardNo")].ToString();
                    block.playCardList = dr["PlayerCardList"] as string;
                    block.ballNoId = (i++).ToString();
                    block.BetValue = (dr["Bet"]).ToString();
                    block.WinValue = (dr["Win"]).ToString();
                    _template.WinlossDetail.HandHistoryDetail.Append(block);
                }
            }

            // Total
            //object betTotal = detailData.Compute("SUM(Bet)", "");
            //object winlossTotal = detailData.Compute("SUM(WinLoss)", "");
            //_template.WinlossDetail.BetTotal = NumberReport(betTotal);
            //_template.WinlossDetail.WinTotal = NumberReport(winlossTotal);
        }

        /// <summary>
        /// Reformat text for number in this report
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string NumberReport(object number)
        {
            decimal dcNumber = Convert.IsDBNull(number) ? 0 : (decimal)number;
            string resultText = Formater.DecFormat(number, 2);
            string spanClass = dcNumber >= 0 ? "bl_underdog" : "bl_favorite";
            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
        }
        #endregion
    }


}
