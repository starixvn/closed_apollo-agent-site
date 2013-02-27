using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using Fanex.Age.UI.Components;
using Sunplus.Agent.Utilities;
using System.Collections;
using System.IO;


/*
 * Created 20100529@Jacob - Render Statement page
 * Revision:
 * 20100916@Don - Support hiding Today and Yesterday buttons 
 * 20110524@Andy - Extend 1 more week of [To date]
 */
namespace Fanex.Age.UI.Reports.Common
{

    public partial class Statement_Page : Reports_Page
    {
        public Statement_Page()
        {
            MIN_REPORT_DATE = new DateTime(1900, 1, 1);
            MAX_REPORT_DATE = DateTime.Now.AddDays(7);
        }

        #region Variable
        //paging
        private const int DEFAULT_PAGE_SIZE = 100;
        private const string PAGE_INDEX_TEXT = "pageindex";
        private const string PAGE_SIZE_TEXT = "pagesize";
        private const string TOTAL_ROWS_TEXT = "totalrows";
        private int _pageIndex = 1;
        private int _pageSize = DEFAULT_PAGE_SIZE;
        private int _rowCount = 0;

        private const string SUMOF_TOTAL_AMT_TEXT = "sumoftotalamt";
        private decimal _sumOfTotalAmt = 0;
        #endregion

        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            Statement_Template tpl = new Statement_Template();

            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Common.StatementReportBuilder(IsAgentSite, reportData, _dateFrom, _dateTo);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "Statement.xls");
                    return;
                }

                // Need to render report content only
                if (IsAgentSite) RenderAgentReportContent(tpl);
                else RenderSuperReportContent(tpl);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "Statement");
            }
            else
            {
                // Render Report Header and history g
                RenderHeader(tpl);
                RenderDateRange(tpl.AgentHeader.DateRangeSelect);

                if (IsAgentSite) RenderAgentReportContent(tpl);
                else RenderSuperReportContent(tpl);
                RenderHiddenField(tpl);

                writer.Write(tpl);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            base.ProcessRequest(ref p);
            if (String.IsNullOrEmpty(Request["fdate"]))
            {
                _dateFrom = Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate;
                _dateTo = DateTime.Today.AddDays(7);
            }
            if (_exportToExcel)
            {
                _pageIndex = _pageSize = 0;
            }
            else
            {
                _pageIndex = GetIntRequest("pageIndex", 1);
                _pageSize = GetIntRequest("pageSize", DEFAULT_PAGE_SIZE);
            }
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Hashtable pagingParams = GetPagingParams();
            // Report data
            DataTable reportData = null;            
            if (_dateFrom <= _dateTo)
            {
                reportData = _ReportBO.GetDailyStatement(_custId, _roleId, _dateFrom, _dateTo, pagingParams);
            }
            else
            {
                reportData = new DataTable();
            }
           
            UpdateRowCount(pagingParams);

            return reportData;
        }
        #endregion

        #region Render Utility
        /// <summary>
        /// Render header
        /// </summary>
        /// <param name="template"></param>
        private void RenderHeader(Statement_Template template)
        {
            template.SuperHeader.Hide();
            template.AgentHeader.Show();
            template.AgentHeader.DateRangeSelect.HideTodayYesterdayButton = true;
            template.AgentHeader.DateRangeSelect.SetDateRange(DateTime.MinValue, DateTime.Now.AddDays(7));            
        }
        /// <summary>
        /// Rnder report content for agent
        /// </summary>
        /// <param name="template"></param>
        private void RenderAgentReportContent(Statement_Template template)
        {
           
            DataTable reportData = View as DataTable;

            Statement_ExcelContents_Block excelBlock = template.ExcelContents;
            //excelBlock.TotalAMTHeader.Hide();
            int numRow = reportData.Rows.Count;
            excelBlock.ContentRow.Clear();
            if (numRow > 0)
            {
                int startIndex = _exportToExcel ? 0 : (_pageIndex - 1) * _pageSize;
                int endIndex = _exportToExcel ? numRow : _pageIndex * _pageSize;
                if (endIndex > numRow) endIndex = numRow;
                decimal total = 0;
                Statement_ExcelContents_ContentRow_Block contentRow = new Statement_ExcelContents_ContentRow_Block();

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];

                    #region AssignValue
                    String transType = dr["transtype"].ToString().ToLower();
                    if (transType == "pos" || transType == "com" || transType == "bal")
                    {
                        contentRow.DateValue = Convert.ToDateTime(dr["accdate"]).ToString("ddddd") + "<br>" + Formater.ShortDateFormat(dr["accdate"]);
                    }
                    else
                    {
                        contentRow.DateValue = Convert.ToDateTime(dr["accdate"]).ToString("ddddd") + "<br>" + Formater.FullDateFormat(dr["accdate"]);
                    }
                    total = (decimal)dr["totalamt"];

                    contentRow.RemarkValue = dr["transdesc"].ToString();
                    contentRow.TypeValue = dr["transtype"].ToString();
                    if (dr["transdesc"].ToString().ToUpper().Equals("OPENING BALANCE"))
                    {
                        _sumOfTotalAmt = total;
                        contentRow.TotalAMTContent.TotalAMTValue = "--";
                    }
                    else
                    {
                        _sumOfTotalAmt += total;
                        contentRow.TotalAMTContent.TotalAMTValue = DecorateMoneyNumber(dr["totalamt"]);
                    }
                    contentRow.TotalValue = DecorateMoneyNumber(_sumOfTotalAmt);
                    contentRow.NoValue = dr["RowNum"].ToString(); 
                    contentRow.ColorClass = (i - startIndex) % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                    #endregion

                    excelBlock.ContentRow.Append(contentRow);
                }
                
                // Paging
                string pagingControlName = "Statement" + CurrentUserSession.SiteName + "_Paging";
                template.PagingControl = string.Empty;
                Paging_Control pagingControl = new Paging_Control(template, pagingControlName, _pageSize, Request.Url.AbsoluteUri, _rowCount);
                template.PagingControl = pagingControl.ToString();

                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.NoInfo.NoInfoRow.NumCols = "5";
                template.PagingControl = "";
            }

        }

        /// <summary>
        /// Rnder report content for master and super
        /// </summary>
        /// <param name="template"></param>
        private void RenderSuperReportContent(Statement_Template template)
        {
            DataTable reportData = View as DataTable;

            Statement_ExcelContents_Block excelBlock = template.ExcelContents;

            int numRow = reportData.Rows.Count;
            excelBlock.ContentRow.Clear();
            if (numRow > 0)
            {
                int startIndex = _exportToExcel ? 0 : (_pageIndex - 1) * _pageSize;
                int endIndex = _exportToExcel ? numRow : _pageIndex * _pageSize;
                if (endIndex > numRow) endIndex = numRow;
                decimal total = 0;
                Statement_ExcelContents_ContentRow_Block contentRow = new Statement_ExcelContents_ContentRow_Block();

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];

                    #region AssignValue
                    String transType = dr["transtype"].ToString().ToLower();
                    if (transType == "pos" || transType == "com" || transType == "bal")
                    {
                        contentRow.DateValue = Convert.ToDateTime(dr["matchdate"]).ToString("ddddd") + "<br>" + Formater.ShortDateFormat(dr["matchdate"]);
                    }
                    else
                    {
                        contentRow.DateValue = Convert.ToDateTime(dr["matchdate"]).ToString("ddddd") + "<br>" + Formater.FullDateFormat(dr["matchdate"]);
                    }
                    total = (decimal)dr["totalamt"];

                    contentRow.RemarkValue = dr["transdesc"].ToString();
                    contentRow.TypeValue = dr["transtype"].ToString();
                    if (dr["transdesc"].ToString().ToUpper().Equals("OPENING BALANCE"))
                    {
                        _sumOfTotalAmt = total;
                        contentRow.TotalAMTContent.TotalAMTValue = "--";
                    }
                    else
                    {
                        _sumOfTotalAmt += total;
                        contentRow.TotalAMTContent.TotalAMTValue = DecorateMoneyNumber(dr["totalamt"]);
                    }
                    contentRow.TotalValue = DecorateMoneyNumber(_sumOfTotalAmt);
                    contentRow.NoValue = dr["RowNum"].ToString();
                    contentRow.ColorClass = (i - startIndex) % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                    #endregion

                    excelBlock.ContentRow.Append(contentRow);
                }

                // Paging
                string pagingControlName = "Statement" + CurrentUserSession.SiteName + "_Paging";
                template.PagingControl = string.Empty;
                Paging_Control pagingControl = new Paging_Control(template, pagingControlName, _pageSize, Request.Url.AbsoluteUri, _rowCount);
                template.PagingControl = pagingControl.ToString();
                
                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.NoInfo.NoInfoRow.NumCols = "5";
                template.PagingControl = "";
            }

        }
        /// <summary>
        /// Render Hidden Field
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(Statement_Template template)
        {
            template.CustId = _custId.ToString();
            template.RoleId = _roleId.ToString();
        }
        /// <summary>
        /// Decorate money number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string DecorateMoneyNumber(object value)
        {
            decimal numberMoney = Convert.ToDecimal(value);
            numberMoney = Math.Round(numberMoney, 2, MidpointRounding.AwayFromZero);
            if (numberMoney >= 0)
                return "<span class='bl_under'>" + Formater.FormatNumber3(numberMoney, CurrentUserSession.Language) + "</span>";
            else
                return "<span class='bl_favorite'>" + Formater.FormatNumber3(numberMoney, CurrentUserSession.Language) + "</span>";
        }
        /// <summary>
        /// Return true if current user is agent or current role id = agent
        /// </summary>
        private bool IsAgentSite
        {
            get
            {
                return CurrentUserSession.IsAgent || _roleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent ? true : false;
            }
        }
        /// <summary>
        /// Return true if current user is master or current role id = master
        /// </summary>
        private bool IsMasterSite
        {
            get
            {
                return CurrentUserSession.IsMaster || _roleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Master ? true : false;
            }
        }
        /// <summary>
        /// Get paging and sum of total amount params
        /// </summary>
        /// <returns></returns>
        private Hashtable GetPagingParams()
        {
            Hashtable pagingParams = new Hashtable();
            pagingParams.Add(PAGE_SIZE_TEXT, _pageSize);
            pagingParams.Add(PAGE_INDEX_TEXT, _pageIndex);
            pagingParams.Add(TOTAL_ROWS_TEXT, 0);
            pagingParams.Add(SUMOF_TOTAL_AMT_TEXT, 0);

            return pagingParams;
        }
        /// <summary>
        /// Update _rowCount and _sumOfTotalAmt variables
        /// </summary>
        /// <param name="pagingParams"></param>
        private void UpdateRowCount(Hashtable pagingParams)
        {
            _rowCount = 0;
            _sumOfTotalAmt = 0;
            if (pagingParams != null)
            {
                if (pagingParams.ContainsKey(TOTAL_ROWS_TEXT))
                {
                    _rowCount = (int)pagingParams[TOTAL_ROWS_TEXT];
                }
                if (pagingParams.ContainsKey(SUMOF_TOTAL_AMT_TEXT))
                {
                    string temp = pagingParams[SUMOF_TOTAL_AMT_TEXT].ToString();
                    decimal.TryParse(temp, out _sumOfTotalAmt);
                }
            }
        }
        #endregion
    }

}
