using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fanex.Age.UI.Components;
using System.Data;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Definitions;

namespace Fanex.Age.UI.Reports.WinlossAnalysis
{
    public partial class Monthly_WinlossAnalysisSuper_Page : WinlossReports_Page
    {
        public Monthly_WinlossAnalysisSuper_Page()
        {
            DateTime newdate = DateTime.Today.AddMonths(-18);
            MIN_REPORT_DATE = new DateTime(newdate.Year, newdate.Month, 1); 
        }


        public override void RenderContent(HtmlTextWriter writer)
        {
            WinlossAnalysisSuper_Template tpl = new WinlossAnalysisSuper_Template();
            tpl.actionForm = "action='Monthly_WinlossAnalysisSuper.aspx?" + Request.QueryString.ToString() + "'";
            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.WinlossAnalysis.WinlossAnalysisSuperReportBuilder(Fanex.Age.DTO.eReportType.Monthly,
                    reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "WinlossAnalysis.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = string.Empty;
                RenderReportHeader(tpl);
                RenderReportContent(tpl);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "WinlossAnalysis");
            }
            else
            {
                // Render Report Header
                RenderReportHeader(tpl);
                RenderReportContent(tpl);
                RenderHiddenField(tpl);

                writer.Write(tpl);
            }

            RegisterSortingJavascriptObject();
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            DataTable reportData = null;
            reportData = _ReportBO.GetWinlossAnalysisSuper(
                    CurrentUserSession.CustId,
                    _dateFrom,
                    _dateTo,
                    2,
                    _permission);
            reportData = SortData(reportData);

            return reportData != null ? reportData : new DataTable();
        }
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _isPaging = true;
            _isHistoryReport = true;
            p.Add("IsAnanlysis", true); 
            base.ProcessRequest(ref p);
            _userType = UserTypes.Super;
        }
        private void RenderReportHeader(WinlossAnalysisSuper_Template tpl)
        {
            optionIsChecked = new bool[] { false, true, false };

            tpl.SearchPanel = RenderSearchPanel(true, false,false);
            tpl.ExcelContents.BoldRow = this.BoldRowClass;
            tpl.title = tpl.ExcelContents.title = Resources.Lang.monthlywinlossanalysisreport;
            
        }

        private void RenderReportContentHeader(WinlossAnalysisSuper_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
            excelBlock.option = Resources.Lang.monthly;
        }

        private void RenderHiddenField(WinlossAnalysisSuper_Template template)
        {
            template.CustId = _custId.ToString();
            template.IsSwitch = "0";
            template.OldView = string.Empty;
            template.UserName = string.Empty;
            template.BetType = string.Empty;
            RenderFilterElementNames();
        }

        private string FormatDateTime(string DateTimeString)
        {
            int year = Convert.ToInt32(DateTimeString.Substring(0, 4));
            int month = Convert.ToInt32(DateTimeString.Substring(4, 2));
            int day = 01; //Convert.ToInt32(DateTimeString.Substring(6, 2));
            DateTime dt = new DateTime(year, month, day);
            return dt.Month + "/" + dt.Year;
        }

        private void RenderReportContent(WinlossAnalysisSuper_Template tpl)
        {
            DataTable reportData = View as DataTable;

            WinlossAnalysisSuper_ExcelContents_Block excelBlock = tpl.ExcelContents;
            excelBlock.Content.Clear();
            excelBlock.ContentTotal.Clear();
            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            int numRow = reportData.Rows.Count;
            if (numRow > 0)
            {
                WinlossAnalysisSuper_ExcelContents_Content_Block contentBlock = new WinlossAnalysisSuper_ExcelContents_Content_Block();
                foreach (DataRow row in reportData.Rows)
                {
                    contentBlock.ColorClass = (numRow % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS; ;
                    numRow--;

                    contentBlock.option = FormatDateTime(row["breakdown"].ToString());
                    contentBlock.turnover = Formater.DecFormat(row["turnover"], 2);

                    contentBlock.mem_winloss = Formater.DecFormat(row["member_winlost"], 2);
                    contentBlock.mem_comm = Formater.DecFormat(row["member_comm"], 2);
                    contentBlock.mem_total = Formater.DecFormat(row["member_total"], 2);

                    contentBlock.age_winloss = Formater.DecFormat(row["agent_winlost"], 2);
                    contentBlock.age_comm = Formater.DecFormat(row["agent_comm"], 2);
                    contentBlock.age_total = Formater.DecFormat(row["agent_total"], 2);

                    contentBlock.ma_winloss = Formater.DecFormat(row["master_winlost"], 2);
                    contentBlock.ma_comm = Formater.DecFormat(row["master_comm"], 2);
                    contentBlock.ma_total = Formater.DecFormat(row["master_total"], 2);

                    contentBlock.sup_winloss = Formater.DecFormat(row["super_winlost"], 2);
                    contentBlock.sup_comm = Formater.DecFormat(row["super_comm"], 2);
                    contentBlock.sup_total = Formater.DecFormat(row["super_total"], 2);

                    contentBlock.company = Formater.DecFormat(row["company"], 2);
                    contentBlock.betcount = Formater.DecFormat(row["betcount"], 0);

                    excelBlock.Content.Append(contentBlock);
                }
                // Render Total Summary
                WinlossAnalysisSuper_ExcelContents_ContentTotal_Block totalBlock = new WinlossAnalysisSuper_ExcelContents_ContentTotal_Block();
                totalBlock.turnover = Formater.DecFormat(reportData.Compute("SUM(turnover)", ""), 2);
                totalBlock.mem_winloss = Formater.DecFormat(reportData.Compute("SUM(member_winlost)", ""), 2);
                totalBlock.mem_comm = Formater.DecFormat(reportData.Compute("SUM(member_comm)", ""), 2);
                totalBlock.mem_total = Formater.DecFormat(reportData.Compute("SUM(member_total)", ""), 2);

                totalBlock.age_winloss = Formater.DecFormat(reportData.Compute("SUM(agent_winlost)", ""), 2);
                totalBlock.age_comm = Formater.DecFormat(reportData.Compute("SUM(agent_comm)", ""), 2);
                totalBlock.age_total = Formater.DecFormat(reportData.Compute("SUM(agent_total)", ""), 2);

                totalBlock.ma_winloss = Formater.DecFormat(reportData.Compute("SUM(master_winlost)", ""), 2);
                totalBlock.ma_comm = Formater.DecFormat(reportData.Compute("SUM(master_comm)", ""), 2);
                totalBlock.ma_total = Formater.DecFormat(reportData.Compute("SUM(master_total)", ""), 2);

                totalBlock.sup_winloss = Formater.DecFormat(reportData.Compute("SUM(super_winlost)", ""), 2);
                totalBlock.sup_comm = Formater.DecFormat(reportData.Compute("SUM(super_comm)", ""), 2);
                totalBlock.sup_total = Formater.DecFormat(reportData.Compute("SUM(super_total)", ""), 2);

                totalBlock.company = Formater.DecFormat(reportData.Compute("SUM(company)", ""), 2);
                totalBlock.betcount = Formater.DecFormat(reportData.Compute("SUM(betcount)", ""), 0);

                excelBlock.Content.Append(totalBlock);
                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.NoInfo.NoInfoRow.NumCols = "16";
                if (_isSwitched)
                {
                    excelBlock.Clear();
                }
                
            }
            tpl.PagingControl = string.Empty;
        }

        
    }
}
