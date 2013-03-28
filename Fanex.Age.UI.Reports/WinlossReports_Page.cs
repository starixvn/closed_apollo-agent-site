/*
 * Created 20100703@Don - Support winloss report filter
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.UI.Reports
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Reports.Components;
    using Sunplus.Agent.Model.BL.Definitions;

    public abstract class WinlossReports_Page : Reports_Page
    {
        #region Member Variables

        protected Dictionary<string, FilterElement> _filterCollection = null;

        protected ICollection<KeyValuePair<string, bool>> _permission = null; // permission to call data

        protected Filter_Control _filterControl = null;

        protected bool _isHistoryReport = false;
        protected bool _isSwitched = false;
        protected UserTypes _userType;

        //paging
        private const int DEFAULT_PAGE_SIZE = 1000;
        private const string PAGE_INDEX_TEXT = "pageindex";
        private const string PAGE_SIZE_TEXT = "pagesize";
        private const string TOTAL_ROWS_TEXT = "totalrows";

        /// <summary>
        /// Configuration for using paging or not
        /// </summary>
        protected bool _isPaging = true;

        protected int _pageIndex = 1;
        protected int _pageSize = DEFAULT_PAGE_SIZE;
        protected int _rowCount = 0;
        protected List<object> PAGE_SIZE_COLLECTION = new List<object>(new object[] { 1000, 2000, 3000, 4000, 5000 });

        //Analysis Filter
        protected string[] optionId = new string[] { "rdbDaily", "rdbMonthly", "rdbSport" };

        protected string[] optionVal = new string[] { "", "", "" };
        protected string[] optionOnClick = new string[] { "Daily_Onclick()", "Monthly_Onclick()", "Sport_Onclick()" };
        protected bool[] optionIsChecked = new bool[] { true, false, false };

        protected int CurrentSelectDate = 0;

        #endregion Member Variables

        public WinlossReports_Page()
        {
        }

        #region Override

        protected virtual void InitCheckBox()
        {
            _filterCollection = new Dictionary<string, FilterElement>()
                {
                    {Filter_Control.SHOWALL, new FilterElement(Filter_Control.SHOWALL, Resources.Lang.all, "CheckAll()")},
                    {Filter_Control.SHOWSB, new FilterElement(Filter_Control.SHOWSB, Resources.Lang.SB, "IsCheck()")},
                    {Filter_Control.SHOWCASINO, new FilterElement(Filter_Control.SHOWCASINO, Resources.Lang.casino, "IsCheck()")},
                    {Filter_Control.SHOWRB, new FilterElement(Filter_Control.SHOWRB, Resources.Lang.Racebook, "IsCheck()")},
                    {Filter_Control.SHOW_NUMBER_GAME, new FilterElement(Filter_Control.SHOW_NUMBER_GAME, Resources.Lang.numbergame, "IsCheck()")},
                
                    {Filter_Control.SHOW_P2P, new FilterElement(Filter_Control.SHOW_P2P, Resources.Lang.P2P, "IsCheck()")},
                    {Filter_Control.SHOW_LIVECASINO, new FilterElement(Filter_Control.SHOW_LIVECASINO, Resources.Lang.LiveCasino, "IsCheck()")},
                    {Filter_Control.SHOW_VIRTUALSPORTS, new FilterElement(Filter_Control.SHOW_VIRTUALSPORTS, Resources.Lang.VirtualSports, "IsCheck()")},
                };
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            InitCheckBox();

            _isHistoryReport = GetBooleanRequest("IsHistoryReport", false);

            if (_isHistoryReport) MIN_REPORT_DATE = Sunplus.Agent.Model.BL.Definitions.AppSettings.Min_Report_Date;

            base.ProcessRequest(ref p);

            _isSwitched = GetBooleanRequest("IsSwitch", false);

            if (_isPaging)
            {
                _pageIndex = GetIntRequest("pageIndex", 1);
                _pageSize = GetIntRequest("pageSize", DEFAULT_PAGE_SIZE);
            }

            if (_isSwitched)
            {
                _dateFrom = DateTime.Today;
                _dateTo = DateTime.Today;
                if (_isHistoryReport)
                {
                    _dateFrom = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                }
            }

            _filterControl = new Filter_Control(_filterCollection, Request, _dateFrom);
            _permission = _filterControl.AsCollection();
        }

        #endregion Override

        #region Helper

        protected string GetSportName(object sportId)
        {
            switch (Convert.ToInt32(sportId))
            {
                case 0:
                    return Resources.Lang.sportBook;
                case 251:
                    return Resources.Lang.casino;
                case 151:
                    return Resources.Lang.Racebook;
                case 161:
                    return Resources.Lang.numbergame;
                case 253:
                    return Resources.Lang.P2P;
                case 162:
                    return Resources.Lang.LiveCasino;
                case 180:
                    return Resources.Lang.VirtualSports;
            }
            return string.Empty;
        }

        protected string RenderOptionText(int currency)
        {
            string textConvert = string.Empty;
            switch (currency)
            {
                case 0:
                    textConvert = Resources.Lang.allcurrencies;
                    break;
                case -2:
                case -3:
                    textConvert = Resources.Lang.baseon + " " + RemotingRef.RefData.GetExchangeData(currency * -1).Currency;
                    break;
                default:
                    textConvert = Resources.Lang.viewunder + " " + RemotingRef.RefData.GetExchangeData(currency).Currency;
                    break;
            }
            return textConvert;
        }

        protected WinlossSearchView_Control GenSearchPanel(bool isShowed, bool isShow_HistoryMessage, bool version2 = false)
        {
            WinlossSearchView_Control searchControl = new WinlossSearchView_Control(_filterControl, _dateFrom, _dateTo, isShowed);
            if (!isShow_HistoryMessage) searchControl._template.HistoryMessage.Hide();
            searchControl.IsHistoryReport = _isHistoryReport;
            searchControl.UserType = _userType;
            searchControl.BoldExportExcel = this.BoldChecked;

            if (version2)
            {
                RenderDateRangeV2(searchControl._template.DateRangeSelect, 18);
            }
            else
            {
                RenderDateRange(searchControl._template.DateRangeSelect);
            }

            return searchControl;
        }

        #region Render Filter search panel

        protected string RenderSearchPanel(bool isShowed)
        {
            WinlossSearchView_Control searchControl = GenSearchPanel(isShowed, true);
            return searchControl.ToString();
        }

        protected string RenderSearchPanel(bool isShowed, bool isShow_HistoryMessage, bool isShow_AnalysisFilter)
        {
            WinlossSearchView_Control searchControl = GenSearchPanel(isShowed, isShow_HistoryMessage);
            if (!isShow_AnalysisFilter)
            {
                HistoryMessage_Template hisTpl = new HistoryMessage_Template();
                hisTpl.GrossCommMsg = null;
                hisTpl.OptionMsg = null;
                hisTpl.ContentMessage.ContentMessage = Resources.Lang.historicalMessage18months;
                searchControl._template.AdditionalContent.Append(hisTpl);

                searchControl._template.AdditionalContent.Append(new AnalysisReportFilter_Control(searchControl._template, optionId, optionVal, optionOnClick, optionIsChecked));
            }
            return searchControl.ToString();
        }

        //Analysis Filter on chart
        protected string[] chartOptionId = new string[] { "rdbMonthly", "rdbProduct" };

        protected string[] chartOptionVal = new string[] { "", "" };
        protected string[] chartOptionOnClick = new string[] { "Monthly_Onclick()", "Product_Onclick()" };
        protected bool[] chartOptionIsChecked = new bool[] { true, false };

        protected string RenderSearchPanelOnChart()
        {
            WinlossSearchView_Control searchControl = GenSearchPanel(true, false, true);

            HistoryMessage_Template hisTpl = new HistoryMessage_Template();
            hisTpl.GrossCommMsg = null;
            hisTpl.OptionMsg = null;
            hisTpl.ContentMessage.ContentMessage = Resources.Lang.viewingDataUpTo18Months;
            searchControl._template.AdditionalContent.Append(hisTpl);
            //This is commented. Because hide report product
            searchControl._template.DateRangeSelect.HideTodayYesterdayButton = true;
            searchControl._template.BoldExportExcel.Hide();
            return searchControl.ToString();
        }

        #endregion Render Filter search panel

        protected string RenderSearchPanel_ConsolidatedReport(DataTable dtbCurrency, string currencyId)
        {
            WinlossSearchView_Control searchControl = GenSearchPanel(true, false);
            HistoryMessage_Template hisTpl = new HistoryMessage_Template();
            hisTpl.GrossCommMsg = null;
            hisTpl.OptionMsg.OptionMsg = Resources.Lang.historicalMessage18months;
            hisTpl.ContentMessage.ContentMessage = Resources.Lang.takenotebasecurrency;
            searchControl._template.AdditionalContent.Append(hisTpl);
            searchControl._template.AdditionalContent.Append(new AnalysisReportFilter_Control(searchControl._template, optionId, optionVal, optionOnClick, optionIsChecked));
            searchControl._template.DateRangeSelect._template.DateFilter.Append("<td>" + new DateTimeFilter_Control(CurrentSelectDate) + "</td>");
            searchControl._template.DateRangeSelect.HideTodayYesterdayButton = true;
            searchControl._template.DateRangeSelect._template.CurrencyFilter.Append("<td>" + new CurrencyFilter_Control(dtbCurrency, currencyId) + "</td>");
            return searchControl.ToString();
        }

        protected void ValidateSelectDateTime()
        {
            switch (CurrentSelectDate)
            {
                case 0: break;
                case 8: break;
                case 1:
                    _dateFrom = _dateTo = DateTime.Today;
                    break;
                case 2:
                    _dateFrom = DateTime.Today.AddDays(-1);
                    _dateTo = _dateFrom;
                    break;
                case 3:
                    _dateFrom = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).AddDays(1);
                    _dateTo = _dateFrom.AddDays(6);
                    break;
                case 4:
                    DateTime pastDate = DateTime.Now.AddDays(-7);
                    _dateFrom = pastDate.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).AddDays(1);
                    _dateTo = _dateFrom.AddDays(6);
                    break;
                case 5:
                    _dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    _dateTo = DateTime.Now;
                    break;
                case 6:
                    _dateTo = DateTime.Today.AddDays(0 - DateTime.Today.Day);
                    _dateFrom = _dateTo.AddDays(1 - _dateTo.Day);
                    break;
                case 7:
                    DateTime lastmonth = DateTime.Today.AddDays(0 - DateTime.Today.Day);
                    _dateTo = lastmonth.AddDays(0 - lastmonth.Day);
                    _dateFrom = _dateTo.AddDays(1 - _dateTo.Day);
                    break;
            }
        }

        protected Hashtable GetPagingParams()
        {
            Hashtable pagingParams = new Hashtable();
            if (_isPaging)
            {
                pagingParams.Add(PAGE_SIZE_TEXT, _pageSize);
                pagingParams.Add(PAGE_INDEX_TEXT, _pageIndex);
                pagingParams.Add(TOTAL_ROWS_TEXT, 0);
            }

            return pagingParams;
        }

        protected void UpdateRowCount(Hashtable pagingParams)
        {
            _rowCount = 0;
            if (_isPaging && (pagingParams != null) && pagingParams.ContainsKey(TOTAL_ROWS_TEXT))
            {
                _rowCount = (int)pagingParams[TOTAL_ROWS_TEXT];
            }
        }

        protected void RenderFilterElementNames()
        {
            List<object> result = new List<object>(_filterCollection.Count);
            List<object> status = new List<object>(_filterCollection.Count);
            foreach (string elementName in _filterCollection.Keys)
            {
                if (_filterCollection[elementName].Visible)
                {
                    result.Add(FilterElement.PREFIX_ELEMENT + elementName);
                    status.Add((_filterCollection[elementName].Checked ? "on" : string.Empty));
                }
            }
            RegisterJavascriptObject("FilterCollection", result.ToArray());
            RegisterJavascriptObject("FilterStatusCollection", status.ToArray());
            RegisterJavascriptObject("CustId", _custId);
        }

        /// <summary>
        /// Sort data
        /// Regisger pagecount object to javascript
        /// </summary>
        /// <param name="reportData"></param>
        protected override DataTable SortData(DataTable reportData)
        {
            int pageCount = _rowCount % _pageSize == 0 ? _rowCount / _pageSize : _rowCount / _pageSize + 1;
            if (pageCount <= 1)
            {
                reportData = base.SortData(reportData);
            }
            else _sortingEnabled = false;

            return reportData;
        }

        #endregion Helper
    }
}