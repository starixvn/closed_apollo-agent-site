using System;
using System.Collections.Generic;
using System.Web;
using TplCore.Element;

/// <summary>
/// Generate refresh count down control
/// </summary>
namespace Fanex.Age.UI.Reports.Components
{
    public class ReportSearchHeader_Control : IComponent
    {
        #region Variable
        private SearchHeader_Template _template = new SearchHeader_Template();

        /// <summary>
        /// Only use for normal date range select, not for dropdownlist
        /// </summary>
        public DateTime DateFrom
        {
            set
            {
                _template.DateRangeSelect.DateFrom = value;

            }
        }
        /// <summary>
        /// Only use for normal date range select, not for dropdownlist
        /// </summary>
        public DateTime DateTo
        {
            set
            {
                _template.DateRangeSelect.DateTo = value;
            }
        }
        /// <summary>
        /// This is used to override javascript function SearchByDate
        /// </summary>
        public string SearchByDateFunction
        {
            set { _template.DateRangeSelect.SearchByDateFunction = value; }
        }
        /// <summary>
        /// This is used to override javascript function SearchOneDate
        /// </summary>
        public string SearchOneDateFunction
        {
            set { _template.DateRangeSelect.SearchOneDateFunction = value; }
        }
        #endregion

        #region Cookies
        /// <summary>
        /// CookieRptBold
        /// </summary>
        public bool CookieRptBold
        {
            get
            {
                String value = HttpContext.Current.Request.Cookies["rpt_bold"] == null ? "0" : HttpContext.Current.Request.Cookies["rpt_bold"].Value;
                return value == "1" ? true : false;
            }
        }
        #endregion

        #region Contructor
        /// <summary>
        /// Create the empty DateRangeSelect
        /// </summary>
        /// <param name="parent"></param>
        public ReportSearchHeader_Control(IElement parent)
        {
            if (parent != null) parent.RegisterComponent("ReportSearchHeader", this);
            _template.BoldCheckbox.Checked = this.CookieRptBold ? "checked='checked'" : "";
        }

        #endregion


        #region IComponent Members

        public void HideLinkedResources()
        {
        }

        public override string ToString()
        {
            return _template.ToString();
        }
        #endregion

        #region DateRange
        /// <summary>
        /// Set Date range
        /// </summary>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        public void SetDateRange(DateTime minDate, DateTime maxDate)
        {
            _template.DateRangeSelect.SetDateRange(minDate, maxDate);

        }
        #endregion

        #region ShowDateTo
        /// <summary>
        /// 
        /// </summary>
        public bool ShowDateTo
        {
            set
            {
                _template.DateRangeSelect.ShowDateTo(value);
            }
        }
        #endregion

        #region ShowExportExcel
        /// <summary>
        /// 
        /// </summary>
        public bool ShowExportExcel
        {
            set
            {
                if (value) _template.ExportExcel.Show();
                else _template.ExportExcel.Hide();
            }
        }
        #endregion

        #region ShowExportExcel
        /// <summary>
        /// 
        /// </summary>
        public bool ShowBoldCheckbox
        {
            set
            {
                if (value) _template.BoldCheckbox.Show();
                else _template.BoldCheckbox.Hide();
            }
        }
        #endregion
    }
}