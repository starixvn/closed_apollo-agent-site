using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using Sunplus.Agent.Utilities;



/*
 * Created 20100529@Jacob - Render HRCommission page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.Reports.Commission
{

    public partial class HRCommission_Page : Reports_Page
    {
        public HRCommission_Page()
        {
            MIN_REPORT_DATE = Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate;
        }

        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            HRCommission_Template tpl = new HRCommission_Template();

            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Commission.HRCommissionReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "RacingCommission.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "RacingCommission");
            }
            else
            {
                // Render Report Header and history message
                RenderDateRange(tpl.ReportSearchHeader);
                RenderReportHeader(tpl);

                RenderReportContent(tpl.ExcelContents);
                RenderHiddenField(tpl);

                writer.Write(tpl);
            }

            RegisterSortingJavascriptObject();
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            base.ProcessRequest(ref p);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            DataTable reportData=  _ReportBO.GetHRCommision(_custId, _roleId, _dateFrom, _dateTo);
            reportData = SortData(reportData);
            return reportData;
        }
        #endregion

        #region Render Utility
        
        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(HRCommission_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }
        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(HRCommission_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }
        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(HRCommission_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;

            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            excelBlock.ContentRow.Clear();
            int numRow = reportData.Rows.Count;
            if (numRow > 0)
            {
                HRCommission_ExcelContents_ContentRow_Block contentRow = new HRCommission_ExcelContents_ContentRow_Block();
                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];

                    RenderUserNameField(dr, contentRow);

                    // Assign Value
                    #region Assign value
                    contentRow.ColorClass = (i % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                    contentRow.WinUKComm = Formater.DecFormat(dr["tote_win"], 2);
                    contentRow.PlaceUKComm = Formater.DecFormat(dr["tote_palce"], 2);
                    contentRow.WinPlaceUKComm = Formater.DecFormat(dr["tote_winplace"], 2);
                    contentRow.TotalUKComm = Formater.DecFormat(dr["tote_total"], 2);

                    #endregion

                    excelBlock.ContentRow.Append(contentRow);
                    
                }
                #region Assign TotalValue
                excelBlock.TotalContent.WinUKComm = Formater.DecFormat(reportData.Compute("SUM(tote_win)", ""), 2);
                excelBlock.TotalContent.PlaceUKComm = Formater.DecFormat(reportData.Compute("SUM(tote_palce)", ""), 2);
                excelBlock.TotalContent.WinPlaceUKComm = Formater.DecFormat(reportData.Compute("SUM(tote_winplace)", ""), 2);
                excelBlock.TotalContent.TotalUKComm = Formater.DecFormat(reportData.Compute("SUM(tote_total)", ""), 2);

                #endregion

                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.TotalContent.Hide();
                excelBlock.NoInfo.NoInfoRow.NumCols = "5";
            }

        }
        /// <summary>
        /// Render user name field
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="contentRow"></param>
        private void RenderUserNameField(DataRow dr, HRCommission_ExcelContents_ContentRow_Block contentRow)
        {
            // UserName
            if (_roleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent)
            {
                // If user is agent, not show downline link
                contentRow.DownlineLink.Hide();
                contentRow.BetListLink.Show();
                contentRow.BetListLink.UserName = dr["username"].ToString();
                contentRow.BetListLink.CustId = dr["custid"].ToString();
            }
            else
            {
                // show downline link
                contentRow.DownlineLink.Show();
                contentRow.BetListLink.Hide();
                contentRow.DownlineLink.UserName = dr["username"].ToString();
                contentRow.DownlineLink.CustId = dr["custid"].ToString();
                if (_roleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Master)
                {
                    contentRow.DownlineLink.RoleId = ((int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent).ToString();
                }
                else
                {
                    contentRow.DownlineLink.RoleId = ((int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Master).ToString();
                }
            }
        }
        /// <summary>
        /// Render Hidden Field
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(HRCommission_Template template)
        {
            template.CustId = _custId.ToString();
            template.RoleId = _roleId.ToString();
        }
        #endregion
    }

}
