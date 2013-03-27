using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;


using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.Entities.Permission;

/*
 * Created 20100529@Jacob - Render Outstanding page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.Reports.Common
{

    public partial class Outstanding_Page : Reports_Page
    {
        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            Dictionary<String, object> viewData = View as Dictionary<String, object>;

            Outstanding_Template tpl = new Outstanding_Template();
            // Render Report Header and history message
            RenderReportHeader(viewData["OutstandingTitle"].ToString(), tpl);
            RenderContentHeader(viewData["PositionTitle"].ToString(), tpl);

            RenderReportContent(tpl);

            writer.Write(tpl);

            RegisterSortingJavascriptObject();
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            base.ProcessRequest(ref p);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            //if (Request.Headers["User-Agent"] == "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-GB; rv:1.9.0.19) Gecko/2010031422 Firefox/3.0.19 GTB7.0 (.NET CLR 3.5.30729)")
            //{
            //    Response.Redirect("~/Error.aspx");
            //}

            return base.ValidateRequest(p);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Dictionary<String, object> viewData = new Dictionary<string, object>();

            String positionTitle = "";
            String outstandingTitle = GetOutstandingTilte(_roleId, out positionTitle);
            viewData.Add("OutstandingTitle", outstandingTitle);
            viewData.Add("PositionTitle", positionTitle);
            // Report data
            DataTable reportData = _ReportBO.GetMemberOutstanding(_custId, _roleId);
            // Sort data
            reportData = SortData(reportData);
            viewData.Add("ReportData", reportData);


            return viewData;
        }
        #endregion

        #region Render Utility

        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="outstandingTitle"></param>
        /// <param name="template"></param>
        private void RenderReportHeader(String outstandingTitle, Outstanding_Template template)
        {
            template.MemberOutStanding = outstandingTitle;
            template.BoldChecked = this.BoldChecked;
            template.BoldRow = this.BoldRowClass;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="positionTitle"></param>
        /// <param name="headerBlock"></param>
        private void RenderContentHeader(String positionTitle, Outstanding_Template template)
        {
            template.AgentPosition = positionTitle;
            template.SportBookTitle = UserPermission.IsShowNumberGame ? Resources.Lang.SportHorseNumberGame : Resources.Lang.sportBook;
            template.SportBookTitle += UserPermission.IsShowLiveCasino ? " - " + Resources.Lang.LiveCasino : string.Empty;
            template.SportBookTitle += UserPermission.IsShowVirtualSports ? " - " + Resources.Lang.VirtualSports : string.Empty;

            // For Casino
            if (!UserPermission.IsShowCasino)
            {
                template.Casino1.Hide();
                template.Casino2.Hide();
            }

            //if (!UserPermission.IsShowBingo)
            //{
                template.Bingo1.Hide();
                template.Bingo2.Hide();
            //}

            if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
            {
                template.P2P1.Hide();
                template.P2P2.Hide();
            }
        }

        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(Outstanding_Template template)
        {
            Dictionary<String, object> viewData = View as Dictionary<String, object>;
            DataTable reportData = viewData["ReportData"] as DataTable;
            int rowCount = reportData.Rows.Count;
            template.ContentRow.Clear();
            if (rowCount > 0)
            {
                template.NotFoundContent.Hide();
                Outstanding_ContentRow_Block contentBlock = new Outstanding_ContentRow_Block();
                contentBlock.Bingo.Hide();
                if (!UserPermission.IsShowCasino) contentBlock.Casino.Hide();
                if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P) contentBlock.P2P.Hide();

                int index = 0;

                foreach (DataRow dr in reportData.Rows)
                {
                    index++;
                    RenderReportRow(dr, index, contentBlock);
                    template.ContentRow.Append(contentBlock);

                }

                // Calculate total 
                decimal totalOutstanding = 0, totalPT = 0, totalCSOutstanding = 0, totalCSTransfer = 0, totalBingoOutstanding = 0, totalBingoTransfer = 0
                    ,totalP2POutstanding = 0, totalP2PTransfer = 0;
                totalOutstanding = GetDBDecimalValue(reportData.Compute("SUM(outstanding)", ""));
                totalPT = GetDBDecimalValue(reportData.Compute("SUM(positiontaking)", ""));
                totalCSOutstanding = GetDBDecimalValue(reportData.Compute("SUM(casinooutstanding)", ""));
                totalCSTransfer = GetDBDecimalValue(reportData.Compute("SUM(casinotransfer)", ""));
                
                if (reportData.Columns.Contains("bingooutstanding"))
                {
                    totalBingoOutstanding = GetDBDecimalValue(reportData.Compute("SUM(bingooutstanding)", ""));
                    totalBingoTransfer = GetDBDecimalValue(reportData.Compute("SUM(bingotransfer)", ""));
                }

                if (reportData.Columns.Contains("p2poutstanding"))
                {
                    totalP2POutstanding = GetDBDecimalValue(reportData.Compute("SUM(p2poutstanding)", ""));
                    totalP2PTransfer = GetDBDecimalValue(reportData.Compute("SUM(p2ptransfer)", ""));
                }

                RenderTotalRow(template.TotalContent, totalOutstanding, totalPT, totalCSOutstanding, totalCSTransfer, totalBingoOutstanding, totalBingoTransfer, totalP2POutstanding, totalP2PTransfer);
            }
            else
            {
                template.TotalContent.Hide();
                template.NotFoundContent.NoInfoRow.NumCols = ReportColumnCount.ToString();
            }

        }
        /// <summary>
        /// Render report row
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="rowIndex"></param>
        /// <param name="rowBlock"></param>
        private void RenderReportRow(DataRow dr, int rowIndex, Outstanding_ContentRow_Block rowBlock)
        {
            // Render casino content
            if (UserPermission.IsShowCasino) RenderCasinoContentRow(dr, rowBlock.Casino);
            //if (UserPermission.IsShowBingo) RenderBingoContentRow(dr, rowBlock.Bingo);
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P) RenderP2PContentRow(dr, rowBlock.P2P);
            // Decide to show outstanding downline link or show user name only
            RenderUserNameContent(dr, rowBlock);

            // rowBlock.
            rowBlock.PositionValue = (Convert.IsDBNull(dr["positiontaking"]) || (decimal)dr["positiontaking"] == 0) ? "" : Formater.DecFormat(dr["positiontaking"], 2);
            // Color
            if (dr.Table.Columns.Contains("closed") && Convert.ToInt32(dr["closed"]) != 0)
            {
                rowBlock.ColorClass = CLOSED_ROW_CLASS;
            }
            else
            {
                rowBlock.ColorClass = rowIndex % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
            }

        }
        /// <summary>
        /// Render casino content
        /// Decide to show view downline link or not 
        /// Decide to show casino content or not
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="casinoBlock"></param>
        private void RenderCasinoContentRow(DataRow dr, Outstanding_ContentRow_Casino_Block casinoBlock)
        {
            casinoBlock.TransferValue = DecimalFormat(dr["CasinoTransfer"]);
            string casinoOutstanding = DecimalFormat(dr["casinooutstanding"]);
            if (AllowCasino)
            {
                if (CurrentUserSession.IsAgent || _roleId == 2)
                {
                    casinoBlock.OutstandingLink.CustId = dr["custid"].ToString();
                    casinoBlock.OutstandingLink.RoleId = _roleId.ToString();
                    casinoBlock.OutstandingLink.BetType = Fanex.Age.UI.Common.BetListRunningByDate_Member;
                    casinoBlock.OutstandingLink.OutstandingValue = casinoOutstanding;
                    casinoBlock.OutstandingLink.UserNameValue = dr["username"].ToString();
                }
                else
                {
                    casinoBlock.OutstandingLink.SetValue(casinoOutstanding);
                }
                casinoBlock.Show();
            }
            else
            {
                casinoBlock.Hide();
            }
        }
        private void RenderP2PContentRow(DataRow dr, Outstanding_ContentRow_P2P_Block p2pBlock)
        {
            p2pBlock.TransferValue = DecimalFormat(dr["P2PTransfer"]);
            p2pBlock.OutstandingValue = DecimalFormat(dr["p2poutstanding"]);

            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
            {
                p2pBlock.Show();
            }
            else
            {
                p2pBlock.Hide();
            }
        }
        /// <summary>
        /// Render casino content
        /// Decide to show view downline link or not 
        /// Decide to show casino content or not
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="casinoBlock"></param>
        //private void RenderBingoContentRow(DataRow dr, Outstanding_ContentRow_Bingo_Block bingoBlock)
        //{
        //    bingoBlock.TransferValue = DecimalFormat(dr["BingoTransfer"]);
        //    string bingoOutstanding = DecimalFormat(dr["bingooutstanding"]);
        //    if (CurrentUserSession.IsAgent || _roleId == 2)
        //    {
        //        bingoBlock.OutstandingLink.CustId = dr["custid"].ToString();
        //        bingoBlock.OutstandingLink.RoleId = _roleId.ToString();
        //        bingoBlock.OutstandingLink.BetType = Fanex.Age.UI.Common.BetListRunningByDate_Member;
        //        bingoBlock.OutstandingLink.OutstandingValue = bingoOutstanding;
        //        bingoBlock.OutstandingLink.UserNameValue = dr["username"].ToString();
        //    }
        //    else
        //    {
        //        bingoBlock.OutstandingLink.SetValue(bingoOutstanding);
        //    }
        //}
        /// <summary>
        /// Render total row
        /// </summary>
        /// <param name="totalBlock"></param>
        /// <param name="totalOutstanding"></param>
        /// <param name="totalPT"></param>
        /// <param name="totalCSOutstanding"></param>
        /// <param name="totalCSTransfer"></param>
        /// <param name="totalBingoOutstanding"></param>
        /// <param name="totalBingoTransfer"></param>
        private void RenderTotalRow(Outstanding_TotalContent_Block totalBlock, decimal totalOutstanding, decimal totalPT, decimal totalCSOutstanding, decimal totalCSTransfer, decimal totalBingoOutstanding, decimal totalBingoTransfer, decimal totalP2POutstanding, decimal totalP2PTransfer)
        {
            totalBlock.AgentPosition = DecimalFormat(totalPT);
            totalBlock.Outstanding = DecimalFormat(totalOutstanding);

            if (AllowCasino)
            {
                totalBlock.Casino.Outstanding = DecimalFormat(totalCSOutstanding);
                totalBlock.Casino.Transfer = DecimalFormat(totalCSTransfer);
            }
            else totalBlock.Casino.Hide();

            //if (UserPermission.IsShowBingo)
            //{
            //    totalBlock.Bingo.Outstanding = DecimalFormat(totalBingoOutstanding);
            //    totalBlock.Bingo.Transfer = DecimalFormat(totalBingoTransfer);
            //}
            //else 
            totalBlock.Bingo.Hide();

            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
            {
                totalBlock.P2P.Outstanding = DecimalFormat(totalP2POutstanding);
                totalBlock.P2P.Transfer = DecimalFormat(totalP2PTransfer);
            }
            else totalBlock.P2P.Hide();
        }


        /// <summary>
        /// Decide to show outstanding downline link or show user name only
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="rowBlock"></param>
        private void RenderUserNameContent(DataRow dr, Outstanding_ContentRow_Block rowBlock)
        {
            string outstanding = (Convert.IsDBNull(dr["outstanding"]) || (decimal)dr["outstanding"] == 0) ? "" : Formater.DecFormat(dr["outstanding"], 2);
            // If user is agent, only show the value, not show the link to downline
            if (CurrentUserSession.IsAgent || _roleId == 2)
            {
                rowBlock.UserNameLink.SetValue(dr["username"].ToString());
                rowBlock.OutstandingLink.OutstandingValue = outstanding;
                rowBlock.OutstandingLink.UserNameValue = dr["username"].ToString();
                rowBlock.OutstandingLink.CustId = dr["custid"].ToString();
                rowBlock.OutstandingLink.BetType = Fanex.Age.UI.Common.BetListRunningByDate_Member;

            }
            else
            {
                rowBlock.UserNameLink.RoleId = (_roleId - 1).ToString();
                rowBlock.UserNameLink.CustId = dr["custid"].ToString();
                rowBlock.UserNameLink.UserNameValue = dr["username"].ToString();
                rowBlock.OutstandingLink.SetValue(outstanding);
            }
        }
        #endregion

        #region Utility
        /// <summary>
        /// Reformat decimal value
        /// Show blank if value = 0 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private string DecimalFormat(object value)
        {            
            if (Convert.IsDBNull(value)) return string.Empty;
            decimal temp = (decimal)value;
            return temp == 0 ? "" : Formater.DecFormat(temp, 2);
        }
        /// <summary>
        /// Get number of report column
        /// </summary>
        private int ReportColumnCount
        {
            get
            {
                int numCols = 3;
                if (UserPermission.IsShowCasino) numCols += 2;
                //if (UserPermission.IsShowBingo) numCols += 2;
                if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P) numCols += 2;
                return numCols;
            }
        }      

        private decimal GetDBDecimalValue(object value)
        {
            return Convert.IsDBNull(value) ? 0 : (decimal)value;
        }
        #endregion

    }

}
