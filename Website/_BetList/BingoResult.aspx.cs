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

    public partial class BingoResult_Page : Fanex.Age.UI.TplPage
    {


        #region Variable
        private long _refNo = 0;
        private string _userName = "";
        private bool _showPic = false;
        private int _custId = 0;
        private DateTime _dateFrom = DateTime.Now;
        private DateTime _dateTo = DateTime.Now;
        #endregion

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {

            BingoResult_Template template = new BingoResult_Template();
            RenderReportHeader(template);
            RenderReportRowContent(template);

            writer.Write(template);

            // writer.Write(RenderTable(View as DataTable));
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
            _refNo = GetLongRequest("ibcrefno", 0);
            _userName = GetStringRequest("uname", "");
            _showPic = GetBooleanRequest("showPic", false);
            _custId = GetIntRequest("ibccustid", 0);

            if (_custId == 0)
            {
                _custId = GetIntRequest("custid", 0);
            }

            _dateFrom = GetDateTimeRequest("fdate", DateTime.Now);
            _dateTo = GetDateTimeRequest("tdate", DateTime.Now);

        }

        

        public override object ValidateRequest(Hashtable p)
        {

            return null;
        }


        public override object InvokeData(Hashtable p)
        {            
            IReportService bingoReportService = BingoContainer.Current.Resolve<IReportService>();
            DataTable handHistory = new DataTable();

            if (_refNo != 0)
            {
                handHistory = bingoReportService.GetWinLossHandHistory(_refNo, BingoSettings.BingoGame.CodeGameNo, _showPic);
            }
            else
            {
                handHistory = bingoReportService.GetWinLossHistoryRealTime(_dateFrom, _dateTo, _custId, BingoSettings.BingoGame.CodeGameNo, _showPic);
            }

            //DataSet ds = new DataSet();
            //ds.ReadXml("Bingo.xml");
            //return ds.Tables["Bingo"];
            return handHistory == null ? new DataTable() : handHistory;

        }
        #endregion


        #region RenderUtility
        /// <summary>
        /// Render report content
        /// Group by SessionLogID
        /// Calculate total for each group SessionLogID
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportRowContent(BingoResult_Template template)
        {
            
            template.ContentRow.Clear();
            template.ContentTotal.Clear();
            DataTable reportData = View as DataTable;
            if (reportData.Rows.Count > 0)
            {
                BingoResult_ContentRow_Block rowBlock = new BingoResult_ContentRow_Block();
                BingoResult_ContentTotal_Block totalBlock = new BingoResult_ContentTotal_Block();
                string currentSessionId = reportData.Rows[0]["SessionLogID"].ToString();
                decimal stakeTotal = 0, betTotal = 0, commTotal = 0, winlossTotal = 0;
                rowBlock.ColorOver = Fanex.Age.UI.Common.RowBgOver;
                int counter = 1;
                for (int i = 0, rowCount = reportData.Rows.Count; i < rowCount; i++)
                {
                    DataRow dr = reportData.Rows[i];

                    // Render row
                    rowBlock.ColorClass = counter % 2 == 0 ? Fanex.Age.UI.Common.RowBgEven : Fanex.Age.UI.Common.RowBgOdd;
                    rowBlock.No = counter.ToString();
                    rowBlock.GameID = dr["GameID"].ToString();
                    rowBlock.GameNo = dr["GameNo"].ToString();
                    rowBlock.PlayerHistoryNo = dr["PlayerHistoryNo"].ToString();
                    rowBlock.GameType = GetGameTypeName(dr);
                    rowBlock.GameGroup = dr[BingoBO.BingoLang("GameGroupName")].ToString();
                    //rowBlock.BeginBalance = NumberReport(dr["BeginBalance"]);
                    //rowBlock.EndBalance = NumberReport(dr["EndBalance"]);

                    rowBlock.Bet = NumberReport(dr["Bet"]);
                    betTotal += ToDecimal(dr["Bet"]);
                    rowBlock.Stake = NumberReport(dr["Stake"]);
                    stakeTotal += ToDecimal(dr["Stake"]);
                    rowBlock.Win = NumberReport(dr["WinLoss"]);
                    winlossTotal += ToDecimal(dr["Winloss"]);
                    rowBlock.RollingComm = NumberReport(dr["RollComm"]);
                    commTotal += ToDecimal(dr["RollComm"]);

                    if (Convert.ToInt32(dr["G_Status"]) == 55 || Convert.ToInt32(dr["G_Status"]) == 50)
                    { // Cancel or Void
                        rowBlock.ResultNoDetail.Result = dr[BingoBO.BingoLang("Result")].ToString();
                        rowBlock.ResultDetail.Visible = false;
                        rowBlock.ResultNoDetail.Visible = true;
                    }
                    else
                    {
                        rowBlock.ResultDetail.UserBetID = dr["UserBetID"].ToString();
                        rowBlock.ResultDetail.Result = dr[BingoBO.BingoLang("Result")].ToString();
                        rowBlock.ResultNoDetail.Visible = false;
                        rowBlock.ResultDetail.Visible = true;
                    }

                    rowBlock.DateAndTime = dr["EndDate"].ToString();
                    // Show AG%, MAG%, SAG% in table header
                    decimal agentPT = Convert.IsDBNull(dr["AG%"]) ? 0 : (decimal)dr["AG%"];
                    decimal masterPT = Convert.IsDBNull(dr["MA%"]) ? 0 : (decimal)dr["MA%"];
                    decimal superPT = Convert.IsDBNull(dr["SH%"]) ? 0 : (decimal)dr["SH%"];
                    rowBlock.AG = NumberMoney(agentPT * 100);
                    if (CurrentUserSession.IsMaster)
                    {                       
                        rowBlock.MA = NumberMoney(masterPT * 100) + "<br/>";
                        rowBlock.SA = "";
                    }
                    else if (CurrentUserSession.IsSuper)
                    {
                        rowBlock.MA = NumberMoney(masterPT * 100) + "<br/>";
                        rowBlock.SA = NumberMoney(superPT * 100) + "<br/>";
                    }
                    else if(CurrentUserSession.IsAgent)
                    {
                        rowBlock.MA = null;
                        rowBlock.SA = null;
                    }
                    counter++;
                    string sessionId = dr["SessionLogID"].ToString();
                    // Render total row
                    if (i == rowCount - 1)
                    {
                        totalBlock.TotalStake = NumberReport(stakeTotal);
                        totalBlock.TotalBet = NumberReport(betTotal);
                        totalBlock.TotalWinloss = NumberReport(winlossTotal);
                        totalBlock.TotalComm = NumberReport(commTotal);
                        totalBlock.SessionLogID = currentSessionId.ToString();

                        stakeTotal = betTotal = commTotal = winlossTotal = 0;
                        counter = 1;
                        currentSessionId = sessionId;

                        template.ContentTotal = totalBlock;
                    }

                    template.ContentRow.Append(rowBlock);
                }
                template.NoInfo.Hide();
            }
            else
            {
                template.ContentTotal.Hide();
            }
        }

        /// <summary>
        /// Render report header
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportHeader(BingoResult_Template template)
        {
            template.UserName = _userName;
            template.RefNo = (_refNo == 0) ? string.Empty : _refNo.ToString();
            template.ShowPicChecked = _showPic ? "checked='checked'" : "";

            // Show AG%, MAG%, SAG% in table header
            template.AGPercent = Resources.Lang.lblAG + "%";
            if (CurrentUserSession.IsMaster)
            {
                template.MAPercent = Resources.Lang.lblMA + "%<br/>";
                template.SAPercent = "";
            }
            else if (CurrentUserSession.IsSuper)
            {
                template.MAPercent = Resources.Lang.lblMA + "%<br/>" ;
                template.SAPercent = Resources.Lang.lblSA + "%<br/>";
            }
            else if (CurrentUserSession.IsAgent)
            {
                template.MAPercent = null;
                template.SAPercent = null;
            }

        }
        #endregion

        #region Utility

        private decimal ToDecimal(object number)
        {
            return (Convert.IsDBNull(number) | number == null) ? 0 : (decimal)number;
        }

        /// <summary>
        /// Reformat text for number in this report
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string NumberReport(object number)
        {
            decimal dcNumber = ToDecimal(number);
            string resultText = Formater.DecFormat(dcNumber, 2);
            string spanClass = dcNumber >= 0 ? "bl_underdog" : "bl_favorite";
            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
        }
        private string NumberMoney(object number)
        {
            decimal dcNumber = Convert.IsDBNull(number) ? 0 : (decimal)number;
            string resultText = Formater.FormatNumber3(dcNumber, CurrentUserSession.Language);
            string spanClass = dcNumber > 0 ? "bl_underdog" : "bl_favorite";
            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
        }
        /// <summary>
        /// Get game type name
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetGameTypeName(DataRow dr)
        {
            // Tarzan game, God of prosperity, Chinese chess
            string gameTypeNo = dr["GameTypeNo"].ToString();
            string gameTypeName = dr[BingoBO.BingoLang("GameTypeName")].ToString();
            bool isSpecialGame = (gameTypeNo == "S0001") || (gameTypeNo == "S0004") || (gameTypeNo == "S0003") || (gameTypeNo == "S0006") || (gameTypeNo == "S0007") || (gameTypeNo == "S0005");

            string shoeName = string.Empty;
            if (isSpecialGame)
            {
                string shoeNo = dr["ShoeNo"].ToString();
                switch (shoeNo)
                {
                    case "N": shoeName = Resources.Lang.normal; break;
                    case "F": shoeName = Resources.Lang.freeSpin; break;
                    case "B": shoeName = Resources.Lang.bonusFreeSpin; break;
                    case "S": shoeName = (gameTypeNo == "S0004") ? Resources.Lang.supermode : Resources.Lang.jungleWildMode; break;
                }
                gameTypeName = gameTypeName + "<br />(" + shoeName + ")";
            }
            return gameTypeName;
        }
        #endregion
    }


}
