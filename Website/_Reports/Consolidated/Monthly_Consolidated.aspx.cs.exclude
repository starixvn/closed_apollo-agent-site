using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;



using Fanex.Age.UI.Components;
using System.Data;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Definitions;


namespace Fanex.Age.UI.Reports.Consolidated
{
    public partial class Monthly_Consolidated_Page : Consolidated_Page
    {
        Dictionary<String, DataTable> viewData = new Dictionary<string, DataTable>();
        int currencyId = 20;
        public Monthly_Consolidated_Page()
        {
            DateTime newdate = DateTime.Today.AddMonths(-18);
            MIN_REPORT_DATE = new DateTime(newdate.Year, newdate.Month, 1);
        }
        public override void RenderContent(HtmlTextWriter writer)
        {
            Consolidated_Template tpl = new Consolidated_Template();
            tpl.actionForm = "action='Monthly_Consolidated.aspx?" + Request.QueryString.ToString() + "'";
            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)viewData["reportData"];
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Consolidated.ConsolidatedReportBuilder(Fanex.Age.DTO.eReportType.Monthly,
                    currencyId, reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "Consolidated.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = string.Empty;
                RenderReportHeader(tpl);
                RenderReportContent(tpl);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "Consolidated Report");
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

            DataTable reportCurrency = _ReportBO.GetConsolidateCurrency(CurrentUserSession.CustId);

            DataTable reportData = _ReportBO.GetConsolidate(
                   CurrentUserSession.CustId,
                   _dateFrom,
                   _dateTo,
                   currencyId,
                   2,
                   _permission);
            reportData = SortData(reportData);

            viewData.Add("reportCurrency", reportCurrency);
            viewData.Add("reportData", reportData);

            return viewData;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _isPaging = true;
            base.ProcessRequest(ref p);
            _userType = UserTypes.Super;
            currencyId = GetIntRequest("ddlcurrency", 0);
            CurrentSelectDate = GetIntRequest("ddlSelectDate", 1);
           // ValidateSelectDateTime();
        }
                
        private void RenderReportHeader(Consolidated_Template tpl)
        {
            optionIsChecked = new bool[] { false, true, false };
            tpl.SearchPanel = RenderSearchPanel_ConsolidatedReport(viewData["reportCurrency"], currencyId.ToString());
            tpl.ExcelContents.BoldRow = this.BoldRowClass;
            tpl.title = tpl.ExcelContents.title = Resources.Lang.monthlyconsolidatedreport;
            tpl.ExcelContents.SelectOption = RenderOptionText(currencyId);
        }

        private void RenderReportContentHeader(Consolidated_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
            excelBlock.option = Resources.Lang.monthly;
        }

        private void RenderHiddenField(Consolidated_Template template)
        {
            template.CustId = _custId.ToString();
            template.IsSwitch = "0";
            template.OldView = string.Empty;
            template.UserName = string.Empty;
            template.BetType = string.Empty;
            RenderFilterElementNames();
        }

      

        private Dictionary<string, int> RenderGroupByContent(DataTable dtb)
        {
            Dictionary<string, int> Dic = new Dictionary<string, int>();
            if (dtb.Rows.Count > 0)
            {
                for (int i = 0; i < dtb.Rows.Count; i++)
                {
                    string key = dtb.Rows[i]["breakdown"].ToString();

                    if (Dic.ContainsKey(key))
                    {
                        int value = Convert.ToInt32(Dic[key]);
                        Dic[key] = value + 1;
                    }
                    else
                    {
                        Dic.Add(key, 0);
                    }
                }                
            }
            return Dic;
        }               

        public static ArrayList SortHashtable(Hashtable originalList)
        {
            ArrayList sortedList = new ArrayList();
            sortedList.AddRange(originalList.Keys);
            sortedList.Sort();
            return sortedList;
        }

        public void RenderReportContent(Consolidated_Template tpl)
        {
            DataTable reportData = viewData["reportData"];
            Dictionary<string, int> htb = RenderGroupByContent(reportData);//GroupByTable(reportData);

            var items = from k in htb.Keys orderby k ascending select k;

            Consolidated_ExcelContents_Block excelBlock = tpl.ExcelContents;
            excelBlock.Content.Clear();
            excelBlock.ContentTotalBaseCurrency.Clear();
            excelBlock.ContentTotalCurrency.Clear();
            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            int numRow = reportData.Rows.Count;
            if (numRow > 0)
            {
                Consolidated_ExcelContents_Content_Block contentBlock = new Consolidated_ExcelContents_Content_Block();
                foreach (string k in items)
                {
                    DataRow[] Rows = reportData.Select("breakdown =" + k);
                    RenderGroupContent(currencyId,Rows, excelBlock,false,false );
                }

                //RenderTotalCurrency(reportData, tpl);
                RenderTotalBaseOnCurrency(reportData, tpl);
                switch (currencyId)
                {
                    case -2:
                    case -3:
                        RenderTotalCurrencyBaseOnCurrency(reportData,currencyId, tpl);
                        break;
                    default:
                        RenderTotalCurrency(reportData, tpl);
                        break;
                }

                excelBlock.NoInfo.Hide();

            }
            else
            {
                excelBlock.NoInfo.NoInfoRow.NumCols = "12";

                if (_isSwitched)
                {
                    excelBlock.Clear();
                }

            }
            tpl.PagingControl = string.Empty;
        }

    }
}
