using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using Sunplus.Agent.Utilities;



/*
 * Created 20100529@Jacob - Render FinanceCommission page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.Reports.Commission
{

    public partial class FinanceCommission_Page : Reports_Page
    {
        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            FinanceCommission_Template tpl = new FinanceCommission_Template();

            if (_exportToExcel)
            {
                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "FinanceCommision");
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
            DataTable reportData=_ReportBO.GetFinanceCommision(_custId, _roleId, _dateFrom, _dateTo);
            reportData = SortData(reportData);
            return reportData;
        }
        #endregion

        #region Render Utility

        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(FinanceCommission_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }
        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(FinanceCommission_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }
        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(FinanceCommission_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;

            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            int numRow = reportData.Rows.Count;
            excelBlock.ContentRow.Clear();
            if (numRow > 0)
            {

                decimal ou5Total = 0, ou15Total = 0, ou30Total = 0, ou45Total = 0, ou60Total = 0, ouEndOfDayTotal = 0, ouTotalTotal = 0;
                decimal oe5Total = 0, oe15Total = 0, oe30Total = 0, oe45Total = 0, oe60Total = 0, oeEndOfDayTotal = 0, oeTotalTotal = 0;

                FinanceCommission_ExcelContents_ContentRow_Block contentRow = new FinanceCommission_ExcelContents_ContentRow_Block();
                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];

                    RenderUserNameField(dr, contentRow);

                    // Assign Value
                    #region Assign value
                    contentRow.ColorClass = (i % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                    contentRow.OU5 = Formater.DecFormat(dr["fa51_com"], 2);
                    contentRow.OU15 = Formater.DecFormat(dr["fa52_com"], 2);
                    contentRow.OU30 = Formater.DecFormat(dr["fa53_com"], 2);
                    contentRow.OU45 = Formater.DecFormat(dr["fa54_com"], 2);
                    contentRow.OU60 = Formater.DecFormat(dr["fa55_com"], 2);
                    contentRow.OUEndOfDay = Formater.DecFormat(dr["fa56_com"], 2);
                    contentRow.OUTotal = Formater.DecFormat(dr["totalcomm_ou"], 2);

                    contentRow.OE5 = Formater.DecFormat(dr["fa61_com"], 2);
                    contentRow.OE15 = Formater.DecFormat(dr["fa62_com"], 2);
                    contentRow.OE30 = Formater.DecFormat(dr["fa63_com"], 2);
                    contentRow.OE45 = Formater.DecFormat(dr["fa64_com"], 2);
                    contentRow.OE60 = Formater.DecFormat(dr["fa65_com"], 2);
                    contentRow.OEEndOfDay = Formater.DecFormat(dr["fa66_com"], 2);
                    contentRow.OETotal = Formater.DecFormat(dr["totalcomm_oe"], 2);

                    #endregion

                    excelBlock.ContentRow.Append(contentRow);
                    // Calculate total value
                    #region Calculate TotalValue

                    ou5Total += Math.Round((decimal)dr["fa51_com"], 2, MidpointRounding.AwayFromZero);
                    ou15Total += Math.Round((decimal)dr["fa52_com"], 2, MidpointRounding.AwayFromZero);
                    ou30Total += Math.Round((decimal)dr["fa53_com"], 2, MidpointRounding.AwayFromZero);
                    ou45Total += Math.Round((decimal)dr["fa54_com"], 2, MidpointRounding.AwayFromZero);
                    ou60Total += Math.Round((decimal)dr["fa55_com"], 2, MidpointRounding.AwayFromZero);
                    ouEndOfDayTotal += Math.Round((decimal)dr["fa56_com"], 2, MidpointRounding.AwayFromZero);
                    ouTotalTotal += Math.Round((decimal)dr["totalcomm_ou"], 2, MidpointRounding.AwayFromZero);

                    oe5Total += Math.Round((decimal)dr["fa51_com"], 2, MidpointRounding.AwayFromZero);
                    oe15Total += Math.Round((decimal)dr["fa62_com"], 2, MidpointRounding.AwayFromZero);
                    oe30Total += Math.Round((decimal)dr["fa63_com"], 2, MidpointRounding.AwayFromZero);
                    oe45Total += Math.Round((decimal)dr["fa64_com"], 2, MidpointRounding.AwayFromZero);
                    oe60Total += Math.Round((decimal)dr["fa65_com"], 2, MidpointRounding.AwayFromZero);
                    oeEndOfDayTotal += Math.Round((decimal)dr["fa66_com"], 2, MidpointRounding.AwayFromZero);
                    oeTotalTotal += Math.Round((decimal)dr["totalcomm_oe"], 2, MidpointRounding.AwayFromZero);

                    #endregion

                }
                #region Assign TotalValue
                excelBlock.TotalContent.OU5Total = Formater.DecFormat(ou5Total, 2);
                excelBlock.TotalContent.OU15Total = Formater.DecFormat(ou15Total, 2);
                excelBlock.TotalContent.OU30Total = Formater.DecFormat(ou30Total, 2);
                excelBlock.TotalContent.OU45Total = Formater.DecFormat(ou45Total, 2);
                excelBlock.TotalContent.OU60Total = Formater.DecFormat(ou60Total, 2);
                excelBlock.TotalContent.OUEndOfDayTotal = Formater.DecFormat(ouEndOfDayTotal, 2);
                excelBlock.TotalContent.OUTotal = Formater.DecFormat(ouTotalTotal, 2);

                excelBlock.TotalContent.OE5Total = Formater.DecFormat(oe5Total, 2);
                excelBlock.TotalContent.OE15Total = Formater.DecFormat(oe15Total, 2);
                excelBlock.TotalContent.OE30Total = Formater.DecFormat(oe30Total, 2);
                excelBlock.TotalContent.OE45Total = Formater.DecFormat(oe45Total, 2);
                excelBlock.TotalContent.OE60Total = Formater.DecFormat(oe60Total, 2);
                excelBlock.TotalContent.OEEndOfDayTotal = Formater.DecFormat(oeEndOfDayTotal, 2);
                excelBlock.TotalContent.OETotal = Formater.DecFormat(oeTotalTotal, 2);
                #endregion



                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.TotalContent.Hide();
                excelBlock.NoInfo.NoInfoRow.NumCols = "15";
            }

        }
        /// <summary>
        /// Render user name field
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="contentRow"></param>
        private void RenderUserNameField(DataRow dr, FinanceCommission_ExcelContents_ContentRow_Block contentRow)
        {
            // UserName
            if (_roleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent)
            {
                // If user is agent, not show downline link
                contentRow.DownlineLink.Hide();
                contentRow.UserName = dr["username"].ToString();
            }
            else
            {
                // show downline link
                contentRow.DownlineLink.Show();
                contentRow.DownlineLink.UserName = dr["username"].ToString();
                contentRow.UserName = "";
                if (_roleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Master)
                {
                    contentRow.DownlineLink.RoleId = ((int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent).ToString();
                    contentRow.DownlineLink.CustId = dr["recommend"].ToString();
                }
                else
                {
                    contentRow.DownlineLink.RoleId = ((int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Master).ToString();
                    contentRow.DownlineLink.CustId = dr["mrecommend"].ToString();
                }
            }
        }
        /// <summary>
        /// Render Hidden Field
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(FinanceCommission_Template template)
        {
            template.CustId = _custId.ToString();
        }
        #endregion
    }

}
