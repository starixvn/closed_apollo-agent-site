using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using Sunplus.Agent.Utilities;
using Fanex.Age.UI.Reports.Components;

/*
 * Created 20100529@Jacob - Render MatchResultEntered page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.Reports.Common
{

    public partial class MatchResultEntered_Page : Reports_Page
    {
        #region Variable
        const string CACHE_ENTERNONENTER = "CACHE_ENTERNONENTER";
        /// <summary>
        /// If column count == TODAY_COLUMN_COUNT, show today enter in UI
        /// </summary>
        const int TODAY_COLUMN_COUNT = 7;
        private DateTime _Today = DateTime.Today;
        private DateTime _Yesterday = DateTime.Today ;

        #endregion
       
        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
         
            Dictionary<String, object> viewData = View as Dictionary<String, object>;
            MatchResultEntered_Template tpl = new MatchResultEntered_Template();
            RenderDateRange(tpl.ReportSearchHeader); 
            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)viewData["ReportData"];
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Common.MatchResultEnteredReportBuilder(_Yesterday, _Today, IsBold, reportData, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "MatchResultEntered.xls");
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportHeader(tpl);
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "MatchResultEntered");
            }
            else
            {
                // Render Report Header and history message
                RenderReportHeader(tpl);
                RenderHistoryMessage(tpl.HistoryMessage);

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
            Dictionary<String, object> viewData = new Dictionary<string, object>();

            String winlossTitle = GetWinlossTitle(); //_ReportBO.GetWinlossTitle();
            viewData.Add("WinlossTitle", winlossTitle);
            // Report data
            DataTable reportData = _ReportBO.GetMatchResultEntered(out _Today);
            _Yesterday = _Today.AddDays(-1);
            
            //  Sort data by _sortingColumn 
            reportData = SortData(reportData);
            viewData.Add("ReportData", reportData);
            return viewData;
        }
        #endregion

        #region Render Utility

        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(MatchResultEntered_Template template)
        {
            Dictionary<String, object> viewData = View as Dictionary<String, object>;
            template.WinlossName = viewData["WinlossTitle"].ToString();
            template.ExcelContents.BoldRow = this.BoldRowClass;            
            template.ReportSearchHeader.SearchByDateFunction = "MatchResultEntered_SearchByDate()";
            template.ReportSearchHeader.SearchOneDateFunction = "MatchResultEntered_SearchOneDay(this.value)";

            DataTable reportData = viewData["ReportData"] as DataTable;
            int columnCount = reportData.Columns.Count;
            int rowCount = reportData.Rows.Count;
            // Content Header
            template.ExcelContents.YesterdayHeader1.Yesterday = Formater.ShortDateFormat(_Yesterday);
            template.ExcelContents.TodayHeader1.Today = Formater.ShortDateFormat(_Today);
           

        }
        /// <summary>
        /// Render History message
        /// </summary>
        /// <param name="messageTemplate"></param>
        private void RenderHistoryMessage(HistoryMessage_Template messageTemplate)
        {
            string message = Resources.Lang.ViewHistoryReport1.Replace("{viewhistory}", "<a href ='#' onclick='MatchResultEntered_ViewHistory();'>" + Resources.Lang.ViewHistoryReport2 + "</a>");
            messageTemplate.ContentMessage.ContentMessage = message;
            messageTemplate.GrossCommMsg.Hide();
            messageTemplate.OptionMsg.Hide();
        }
        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(MatchResultEntered_ExcelContents_Block excelBlock)
        {
            Dictionary<String, object> viewData = View as Dictionary<String, object>;
            DataTable reportData = viewData["ReportData"] as DataTable;
            int columnCount = reportData.Columns.Count;
            
            excelBlock.Content.Clear();
            int rowCount = 0;
            // Report content
            MatchResultEntered_ExcelContents_Content_Block contentBlock = new MatchResultEntered_ExcelContents_Content_Block();
            for (int i = 0; i < reportData.Rows.Count; i++)
            {
                DataRow dr = reportData.Rows[i];
                String sportTypeName = dr["sportname"] as string;
                if (AllowRenderBet(sportTypeName))
                {
                    rowCount++;
                    contentBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    contentBlock.TypeName = sportTypeName;
                    contentBlock.YesterdayContent.YesterdayEntered = dr["YesterdayEntered"].ToString();
                    contentBlock.YesterdayContent.YesterdayNonEntered = dr["YesterdayNonEntered"].ToString();
                    contentBlock.TodayContent.TodayEntered = dr["TodayEntered"].ToString();
                    contentBlock.TodayContent.TodayNonEntered = dr["TodayNonEntered"].ToString();                   
                    excelBlock.Content.Append(contentBlock);
                }
            }
            
            if (rowCount == 0)
            {
                excelBlock.TodayHeader1.Today = Formater.ShortDateFormat(DateTime.Today);
                excelBlock.NoInfo.NoInfoRow.NumCols = "5";
                excelBlock.Content.Hide();
            }
            else
            {
                excelBlock.NoInfo.Hide();
            }

        }
        /// <summary>
        /// Render Hidden Field
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(MatchResultEntered_Template template)
        {
            template.CustId = CurrentUserSession.CustId.ToString();
            template.RoleId = CurrentUserSession.RoleId.ToString();
        }
        #endregion

        #region Utility

        private bool AllowRenderBet(String sportTypeName)
        {
            if (string.Compare(sportTypeName, Resources.Lang.hr) == 0)
            {
                if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
                {
                    return false;
                }
            }

            if (string.Compare(sportTypeName, Resources.Lang.VirtualSoccer) == 0)
            {
                if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowVirtualSports)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
