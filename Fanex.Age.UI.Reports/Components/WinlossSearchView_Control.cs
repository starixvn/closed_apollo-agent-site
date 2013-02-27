using System;
using System.Collections.Generic;
using Fanex.Age.UI.Reports;
using Sunplus.Agent.Model.BL.Definitions;
using Fanex.Age.UI.Reports.Components;
using Sunplus.Agent.Utilities;
using Fanex.Age.UI.Components;

/*
 * Created 20100716@Don - Winloss Search View Panel
 * Revision:
 * 20121226@Kaden: Modify function RenderControl() when _isHistoryReport is true
 *
 */

namespace Fanex.Age.UI.Reports.Components
{
    public class WinlossSearchView_Control : TplCore.Element.IComponent
    {
        #region Members Variables

        private bool _isShowed = true;
        private DateTime _dateFrom = DateTime.Now;
        private DateTime _dateTo = DateTime.Now;

        private bool _isHistoryReport = false;
        private UserTypes _userType;
        private string _boldExportExcel = string.Empty;
        
        public WinlossSearchView_Template _template = new WinlossSearchView_Template();

        private Filter_Control _filterControl = null;

        #endregion



        #region Properties

        public bool IsHistoryReport
        {
            get { return _isHistoryReport; }
            set { _isHistoryReport = value; }
        }

        public UserTypes UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        public string BoldExportExcel
        {
            get { return _boldExportExcel; }
            set { _boldExportExcel = value; }
        }

        public Filter_Control FilterControl
        {
            get { return _filterControl; }
            set { _filterControl = value; }
        }

        #endregion



        #region Constructors

        public WinlossSearchView_Control(Filter_Control filterControl, DateTime dateFrom, DateTime dateTo) : this(filterControl, dateFrom, dateTo, true) { }

        public WinlossSearchView_Control(Filter_Control filterControl, DateTime dateFrom, DateTime dateTo, bool isShowed)
        {
            _filterControl = filterControl;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _isShowed = isShowed;
        }              

        #endregion



        #region Processes

        private void RenderControl()
        {
            _template.BoldExportExcel.Checked = _boldExportExcel;
            
            //Render date layout for history report or non-history report
            if (_isHistoryReport)
            {
                DateTime start = DateTime.Now;
                DateTime end = Sunplus.Agent.Model.BL.Definitions.AppSettings.Min_Report_Date;
                int totalMonths = (start.Year * 12 + start.Month) - (end.Year * 12 + end.Month);
                _template.DateRangeSelect.InitializeDropdownList(_dateFrom, _dateTo, totalMonths);
            }
            else
            {
                _template.DateRangeSelect.DateFrom = _dateFrom;
                _template.DateRangeSelect.DateTo = _dateTo;
                
            }

            _template.DateRangeSelect.SearchByDateFunction = "Winloss_SearchByDate(null, null, null, null)"; //Winloss_SearchByDate(fdate, tdate, targetCustId, actionPage)
            _template.DateRangeSelect.SearchOneDateFunction = "Winloss_SearchOneDay(this)";

            //Render winloss report filter
            RenderFilter();

            RenderHistoryMessage(_template.HistoryMessage);
        }

        #endregion



        #region Helpers

        private void RenderFilter()
        {
            _template.reportFilter = _filterControl.ToString();
            //_template.analysisFilter = new AnalysisReportFilter_Control(this, 
        }

        /// <summary>
        /// Render History Message
        /// </summary>
        /// <param name="messageTemplate"></param>
        private void RenderHistoryMessage(HistoryMessage_Template messageTemplate)
        {
            //ViewHistory(targetCustId, actionPage)
            string message = string.Empty;
            if (_isHistoryReport)
            {
                message = "<a href ='#' onclick='ViewHistory();'>" + Resources.Lang.CurrentReport + "</a>";
            }
            else
            {
                message = Resources.Lang.ViewHistoryReport1.Replace("{viewhistory}", "<a href ='#' onclick='ViewHistory();'>" + Resources.Lang.ViewHistoryReport2 + "</a>");
            }
            messageTemplate.ContentMessage.ContentMessage = message;

            //RenderOptionMessage(messageTemplate);
            messageTemplate.GrossCommMsg.Hide();
            messageTemplate.OptionMsg.Hide();
        }

        /// <summary>
        /// Show message about filtering data for SB, HR, FI
        /// </summary>
        /// <param name="messageTemplate"></param>
        private void RenderOptionMessage(HistoryMessage_Template messageTemplate)
        {
            messageTemplate.OptionMsg.Hide();
            if (_isHistoryReport)
            {
                string shortDatePattern = "yyyy/MM";
                DateTime lowerDate = DateTime.Now.AddMonths(-18);
                DateTime crossDate = new DateTime(2009, 6, 1);
                int result = String.Compare(lowerDate.ToString(shortDatePattern), crossDate.ToString(shortDatePattern));

                if (result <= 0)
                {
                    messageTemplate.OptionMsg.Show();
                    if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
                    {
                        messageTemplate.OptionMsg.OptionMsg = Resources.Lang.filterhr;
                    }
                    else
                    {
                        messageTemplate.OptionMsg.Hide();
                    }
                }
            }
        }

        #endregion



        #region IComponent Members

        public void HideLinkedResources()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            if (_isShowed)
            {
                RenderControl();
                return _template.ToString();
            }
            return string.Empty;
        }

        #endregion
    }
}
